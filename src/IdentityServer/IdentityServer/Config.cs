// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_basket"){Scopes = {"basket_fullpermission"}},
            new ApiResource("resource_payment"){Scopes = {"payment_fullpermission"}},
            new ApiResource("resource_catalog"){Scopes = {"catalog_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes = {"gateway_fullpermission"}},
            new ApiResource("resource_discount"){Scopes = {"discount_fullpermission"}},
            new ApiResource("resource_ordering"){Scopes = {"ordering_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes = {"photo_stock_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){ Name = "roles",
                                               DisplayName = "Roles",
                                               Description="User roles",
                                               UserClaims = new[]{"role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("basket_fullpermission","Full permission for Basket.API"),
                new ApiScope("payment_fullpermission","Full permission for Payment.API"),
                new ApiScope("catalog_fullpermission", "Full permission for Catalog.API"),
                new ApiScope("gateway_fullpermission", "Full permission for Gateway.API"),
                new ApiScope("discount_fullpermission", "Full permission for Discount.API"),
                new ApiScope("ordering_fullpermission", "Full permission for Ordering.API"),
                new ApiScope("photo_stock_fullpermission", "Full permission for PhotoStock.API"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "client",
                    ClientId = "WebClient",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catalog_fullpermission", "photo_stock_fullpermission", "gateway_fullpermission",
                                       IdentityServerConstants.LocalApi.ScopeName }
                },
                 new Client
                {
                    ClientName = "client",
                    ClientId = "WebClientForUser",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "payment_fullpermission", "basket_fullpermission",
                                      "discount_fullpermission", "ordering_fullpermission", "gateway_fullpermission",
                                      IdentityServerConstants.LocalApi.ScopeName,
                                      IdentityServerConstants.StandardScopes.Email,
                                      IdentityServerConstants.StandardScopes.OpenId,
                                      IdentityServerConstants.StandardScopes.Profile,
                                      IdentityServerConstants.StandardScopes.OfflineAccess, "roles" },
                    AccessTokenLifetime = 1*60*60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse
                }
            };
    }
}