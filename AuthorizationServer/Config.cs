﻿using Duende.IdentityServer.Models;
using IdentityModel;
using System.Collections.Generic;

namespace Pluralsight.AuthorizationServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "wiredbrain_api",
                    DisplayName = "Wired Brain Coffee API",
                    ApiSecrets = {new Secret("apisecret".Sha256())},
                    Scopes = { "wiredbrain_api.rewards" },
                },
            };
        }

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                 new ApiScope("wiredbrain_api.rewards")
            };

        public static IEnumerable<IdentityResource> GetIdentityScopes()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "simple_client",
                    ClientName = "Unground (auth code)",
                    AllowedScopes = {"wiredbrain_api.rewards"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:5001/home/callback"},
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowOfflineAccess = true,
                    RequirePkce = false
                },
                new Client
                {
                    ClientId = "implicit_client",
                    ClientName = "Unground (implicit)",
                    AllowedScopes = {"wiredbrain_api.rewards"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {"https://localhost:5004/callback.html"},
                    AllowedCorsOrigins = {"https://localhost:5004"},
                    AllowAccessTokensViaBrowser = true,
                    RequirePkce = false
                },
                new Client
                {
                    ClientId = "clientcreds_client",
                    ClientName = "Client Credentials OAuth Client",
                    AllowedScopes = {"wiredbrain_api.rewards"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RequirePkce = false
                },
                new Client
                {
                    ClientId = "ropc_client",
                    ClientName = "ROPC OAuth Client",
                    AllowedScopes = {"wiredbrain_api.rewards"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RequirePkce = false
                },
                new Client
                {
                    ClientId = "native_client",
                    ClientName = "Windows Native Client",
                    AllowedScopes = { "wiredbrain_api.rewards" },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"com.pluralsight.windows:/callback"},
                    RequireClientSecret = false,
                    RequirePkce = true
                },
                new Client
                {
                    ClientId = "oidc_client",
                    ClientName = "OpenID Connect Client",
                    AllowedScopes = {"openid", "profile","email", "wiredbrain_api.rewards"},
                    PostLogoutRedirectUris = {"https://localhost:5001/signin-oidc"},
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = {"https://localhost:5005/signin-oidc"},
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RequirePkce = false
                }
            };
        }
    }
}