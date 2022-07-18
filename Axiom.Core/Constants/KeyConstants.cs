using System;
using System.Collections.Generic;
using System.Text;

namespace Axiom.Application.Constants
{
    public static class KeyConstants
    {
        public static class Strings
        {
            public static readonly string IdentityToken = "Token";

            /// <summary>
            /// JwtClaimIdentifiers
            /// </summary>
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            /// <summary>
            /// JwtClaims
            /// </summary>
            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }

            public static class JwtClaimsType
            {
                public const string UserName = "username";
                public const string CustomerKey = "customerKey";
                public const string Role = "role";
            }
        }

        public static readonly string PolicyAuthoApiUser = "ApiUser";
    }
}
