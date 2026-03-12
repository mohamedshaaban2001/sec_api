using Contracts.DTOs.User;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace Service_API.Services
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public KeycloakService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var keycloakSettings = _configuration.GetSection("KeycloakAdmin");
            var tokenUrl = $"{keycloakSettings["AuthServerUrl"]}/realms/{keycloakSettings["Realm"]}/protocol/openid-connect/token";

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", keycloakSettings["ClientId"]),
                new KeyValuePair<string, string>("client_secret", keycloakSettings["ClientSecret"]),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await _httpClient.PostAsync(tokenUrl, requestBody);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Keycloak Token Error ({response.StatusCode}): {errorContent}");
                throw new HttpRequestException($"Keycloak Token Error: {response.ReasonPhrase}. Details: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<bool> CreateUserAsync(UserCreateDto user, int personId)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                IConfigurationSection? keycloakSettings = _configuration.GetSection("KeycloakAdmin");
                var createUserUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/users";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var keycloakUser = new
                {
                    username = user.UserName,
                    enabled = true,
                    credentials = new[]
                    {
                        new { type = "password", value = user.UserPassword, temporary = false }
                    },
                    attributes = new Dictionary<string, object>
                    {
                        { "person_code", personId },
                        { "USER_CODE", personId },
                        { "universities", new[] { "{\"id\": 1}" } } // Static value as requested
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(createUserUrl, keycloakUser);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Keycloak Create Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDto user, int personId)
        {
            try 
            {
                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");
                
                // 1. Find user by username to get ID
                var userId = await GetUserIdByUsername(user.UserName, token, keycloakSettings["AuthServerUrl"], keycloakSettings["Realm"]);
                
                if (string.IsNullOrEmpty(userId)) return false;

                var updateUserUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/users/{userId}";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Note: Updating password usually requires a separate endpoint or 'credentials' array in update? 
                // Admin API allows updating attributes via PUT
                var updateUser = new
                {
                    // Update attributes if needed
                    attributes = new Dictionary<string, object>
                    {
                        { "person_code", personId },
                        { "USER_CODE", personId },
                        { "universities", new[] { "{\"id\": 1}" } }
                    }
                };

                var response = await _httpClient.PutAsJsonAsync(updateUserUrl, updateUser);
                
                // If password needs update coverage, we might need a separate call to /reset-password logic if changed
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Keycloak Update Error: {ex.Message}");
                 return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");

                var userId = await GetUserIdByUsername(username, token, keycloakSettings["AuthServerUrl"], keycloakSettings["Realm"]);
                if (string.IsNullOrEmpty(userId)) return false;

                var deleteUserUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/users/{userId}";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync(deleteUserUrl);
                return response.IsSuccessStatusCode;
            }
             catch (Exception ex)
            {
                 Console.WriteLine($"Keycloak Delete Error: {ex.Message}");
                 return false;
            }
        }

        public async Task<bool> CreateGroupAsync(string groupName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    return false;
                }

                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");
                var createGroupUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/groups";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync(createGroupUrl, new { name = groupName.Trim() });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Keycloak CreateGroup Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateGroupAsync(string currentGroupName, string newGroupName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentGroupName) || string.IsNullOrWhiteSpace(newGroupName))
                {
                    return false;
                }

                var trimmedCurrentName = currentGroupName.Trim();
                var trimmedNewName = newGroupName.Trim();
                if (string.Equals(trimmedCurrentName, trimmedNewName, StringComparison.Ordinal))
                {
                    return true;
                }

                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");

                var groupId = await GetGroupIdByNameAsync(trimmedCurrentName, token, keycloakSettings);
                if (string.IsNullOrWhiteSpace(groupId))
                {
                    return false;
                }

                var updateGroupUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/groups/{groupId}";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync(updateGroupUrl, new { name = trimmedNewName });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Keycloak UpdateGroup Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteGroupAsync(string groupName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(groupName))
                {
                    return false;
                }

                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");

                var groupId = await GetGroupIdByNameAsync(groupName.Trim(), token, keycloakSettings);
                if (string.IsNullOrWhiteSpace(groupId))
                {
                    return true;
                }

                var deleteGroupUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/groups/{groupId}";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync(deleteGroupUrl);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Keycloak DeleteGroup Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateRealmRoleAsync(string roleName, string? description = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return false;
                }

                var trimmedRoleName = roleName.Trim();
                var trimmedDescription = description?.Trim();
                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");

                var roleExists = await RealmRoleExistsAsync(trimmedRoleName, token, keycloakSettings);
                if (roleExists)
                {
                    return true;
                }

                var createRoleUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/roles";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync(createRoleUrl, new
                {
                    name = trimmedRoleName,
                    description = trimmedDescription
                });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Keycloak CreateRealmRole Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateRealmRoleAsync(string currentRoleName, string newRoleName, string? description = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentRoleName) || string.IsNullOrWhiteSpace(newRoleName))
                {
                    return false;
                }

                var trimmedCurrentRoleName = currentRoleName.Trim();
                var trimmedNewRoleName = newRoleName.Trim();
                var trimmedDescription = description?.Trim();
                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");

                var roleUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/roles/{Uri.EscapeDataString(trimmedCurrentRoleName)}";
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var getRoleResponse = await _httpClient.GetAsync(roleUrl);
                if (!getRoleResponse.IsSuccessStatusCode)
                {
                    return false;
                }

                string? currentDescription = null;
                var roleContent = await getRoleResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(roleContent))
                {
                    using var roleDoc = JsonDocument.Parse(roleContent);
                    if (roleDoc.RootElement.TryGetProperty("description", out var roleDescriptionElement))
                    {
                        currentDescription = roleDescriptionElement.GetString();
                    }
                }

                var updateRoleResponse = await _httpClient.PutAsJsonAsync(roleUrl, new
                {
                    name = trimmedNewRoleName,
                    description = string.IsNullOrWhiteSpace(trimmedDescription) ? currentDescription : trimmedDescription
                });

                return updateRoleResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Keycloak UpdateRealmRole Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteRealmRoleAsync(string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return false;
                }

                var trimmedRoleName = roleName.Trim();
                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");
                var deleteRoleUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/roles/{Uri.EscapeDataString(trimmedRoleName)}";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync(deleteRoleUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return true;
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Keycloak DeleteRealmRole Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<string>> GetUserGroupsAsync(string userId)
        {
            var groups = new List<string>();
            try
            {
                var token = await GetAccessTokenAsync();
                var keycloakSettings = _configuration.GetSection("KeycloakAdmin");
                var getGroupsUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/users/{userId}/groups";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync(getGroupsUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(content);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var element in doc.RootElement.EnumerateArray())
                        {
                            if (element.TryGetProperty("name", out var nameProperty))
                            {
                                var groupName = nameProperty.GetString();
                                if (!string.IsNullOrEmpty(groupName))
                                {
                                    groups.Add(groupName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Keycloak GetGroups Error: {ex.Message}");
            }
            return groups;
        }

        private async Task<string?> GetGroupIdByNameAsync(string groupName, string token, IConfigurationSection keycloakSettings)
        {
            var searchUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/groups?search={Uri.EscapeDataString(groupName)}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(searchUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);

            if (doc.RootElement.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            foreach (var group in doc.RootElement.EnumerateArray())
            {
                if (!group.TryGetProperty("name", out var nameProperty))
                {
                    continue;
                }

                var foundName = nameProperty.GetString();
                if (!string.Equals(foundName, groupName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (group.TryGetProperty("id", out var idProperty))
                {
                    return idProperty.GetString();
                }
            }

            return null;
        }

        private async Task<bool> RealmRoleExistsAsync(string roleName, string token, IConfigurationSection keycloakSettings)
        {
            var roleUrl = $"{keycloakSettings["AuthServerUrl"]}/admin/realms/{keycloakSettings["Realm"]}/roles/{Uri.EscapeDataString(roleName)}";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(roleUrl);
            return response.IsSuccessStatusCode;
        }

        private async Task<string> GetUserIdByUsername(string username, string token, string authServerUrl, string realm)
        {
            var searchUrl = $"{authServerUrl}/admin/realms/{realm}/users?username={username}&exact=true";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.GetAsync(searchUrl);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.GetArrayLength() > 0)
            {
                return doc.RootElement[0].GetProperty("id").GetString();
            }
            return null;
        }
    }
}
