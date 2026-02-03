namespace Contracts.enums
{
    public enum ErrorCatalog : ushort
    {
        noError = 200,
        Ok = 200,
        Unknown = 400,
        ObjectNotFound = 404,
        missingValues = 400,
        ConnectionLost = 503,
        VioleteConstrains = 500,
        DataBaseFauiler = 300
    }
}
