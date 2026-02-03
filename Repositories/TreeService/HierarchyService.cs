using System;
using Dapper;
using Entities.Models;
using Entities.Models.BaseTables;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Repositories.TreeService;

public class HierarchyService
{
    private readonly RepositoryContext _repositoryContext;

    public HierarchyService(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public async Task<List<T>> GetHierarchyAsync<T>(HierarchyQueryOptions options)
        where T : BaseTree
    {
        var sql = BuildHierarchySql<T>(options);
        var connection = _repositoryContext.Database.GetDbConnection();
        await connection.OpenAsync();

        try
        {
            var results = await connection.QueryAsync<T>(sql); 
            return results.ToList();
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }
    private string BuildHierarchySql<T>(HierarchyQueryOptions options)
    where T : BaseTree
    {
        var tableName = ReflectionHelpers.ReflectionHelpers.GetTableName<T>();
        var colMap = ReflectionHelpers.ReflectionHelpers.GetColumnMappings<T>();

        if (!colMap.TryGetValue(nameof(BaseTree.Id), out var idColumn))
            throw new InvalidOperationException($"No ColumnName for 'Id' on {typeof(T).Name}.");

        if (!colMap.TryGetValue(nameof(BaseTree.ParentId), out var parentColumn))
            throw new InvalidOperationException($"No ColumnName for 'ParentId' on {typeof(T).Name}.");

       
        string anchorWhere = options.StartFromRoot ? $"e.{parentColumn} IS NULL" : $"e.{idColumn} = {options.StartId}";
        
        string joinCondition = options.TopDown ? $"e.{parentColumn} = h.\"Id\"" : $"e.{idColumn} = h.\"ParentId\"";

        var extraCols = colMap
            .Where(kvp => kvp.Key != nameof(BaseTree.Id) && kvp.Key != nameof(BaseTree.ParentId) && kvp.Key != nameof(BaseTree.IsLeaf)
                && kvp.Key != nameof(BaseTree.Level) && kvp.Key != nameof(BaseTree.Path))
            .Select(kvp => $"e.{kvp.Value} AS \"{kvp.Key}\"")
            .ToList();

        string selectExtra = string.Join(", ", extraCols);


        string delimiter = options.Delimiter ?? " -> ";
        string anchorPath = $"CAST(e.{idColumn} AS TEXT) AS \"Path\"";
        string recursivePath = $"h.\"Path\" || '{delimiter}' || e.{idColumn} AS \"Path\"";

        string idListFilter = "";
        if (options.Ids != null && options.Ids.Count > 0)
        {
            string idList = string.Join(",", options.Ids);
            idListFilter = $"AND e.{idColumn} IN ({idList})";
        }

        string stringListFilter = "";
        if (options.StringValues != null && options.StringValues.Count > 0 && !string.IsNullOrEmpty(options.FilterColumn))
        {
            if (colMap.TryGetValue(options.FilterColumn, out var filterColumn))
            {
                string stringList = string.Join(",", options.StringValues.Select(value => $"'{value.Replace("'", "''")}'"));
                stringListFilter = $"AND e.{filterColumn} IN ({stringList})";
            }
            else
            {
                throw new InvalidOperationException($"Filter column '{options.FilterColumn}' does not exist in {typeof(T).Name}.");
            }
        }

        // ✅ Handling Single Column Filtering (Text Match)
        string filterCondition = "";
        if (!string.IsNullOrEmpty(options.FilterColumn) && !string.IsNullOrEmpty(options.FilterValue))
        {
            if (colMap.TryGetValue(options.FilterColumn, out var filterColumn))
            {
                switch (options?.FilterType?.ToLower())
                {
                    case "contains":
                        filterCondition = $"AND e.{filterColumn} ILIKE '%{options.FilterValue}%'";
                        break;
                    case "startswith":
                        filterCondition = $"AND e.{filterColumn} ILIKE '{options.FilterValue}%'";
                        break;
                    case "endswith":
                        filterCondition = $"AND e.{filterColumn} ILIKE '%{options.FilterValue}'";
                        break;
                    case "equals":
                        filterCondition = $"AND e.{filterColumn} = '{options.FilterValue}'";
                        break;
                    case "greaterthan":
                        filterCondition = $"AND e.{filterColumn} > '{options.FilterValue}'";
                        break;
                    case "lessthan":
                        filterCondition = $"AND e.{filterColumn} < '{options.FilterValue}'";
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported filter type: {options.FilterType}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Filter column '{options.FilterColumn}' does not exist in {typeof(T).Name}.");
            }
        }

        string visitedAnchor = options.PreventCycles ? $", ARRAY[e.{idColumn}] AS visited" : "";
        string visitedRecursive = options.PreventCycles ? $", h.visited || e.{idColumn}" : "";
        string preventCycleCondition = options.PreventCycles ? $"AND NOT (e.{idColumn} = ANY(h.visited))" : "";
        string maxDepthCondition = options.MaxDepth.HasValue ? $"AND h.\"Level\" < {options.MaxDepth.Value}" : "";

        return $@"
    WITH RECURSIVE h AS (
        SELECT e.{idColumn} AS ""Id"", e.{parentColumn} AS ""ParentId"", 1 AS ""Level"",
            {anchorPath}, FALSE AS ""IsLeaf"", {selectExtra} {visitedAnchor}
        FROM {tableName} e
        WHERE {anchorWhere} {idListFilter} {stringListFilter} {filterCondition}

        UNION ALL

        SELECT e.{idColumn} AS ""Id"", e.{parentColumn} AS ""ParentId"", h.""Level"" + 1 AS ""Level"",
            {recursivePath}, FALSE AS ""IsLeaf"", {selectExtra} {visitedRecursive}
        FROM {tableName} e
        JOIN h ON {joinCondition}
        WHERE 1=1 {preventCycleCondition} {maxDepthCondition}{idListFilter} {stringListFilter} {filterCondition}
    )
    SELECT h.""Id"", h.""ParentId"", h.""Level"", h.""Path"",
        CASE WHEN NOT EXISTS (SELECT 1 FROM {tableName} c WHERE c.{parentColumn} = h.""Id"") 
            THEN TRUE ELSE FALSE END AS ""IsLeaf"", {selectExtra}
            FROM h
	        JOIN {tableName} e ON e.id = h.""Id""  
            ORDER BY h.""Path"";
    ";
    }

}
