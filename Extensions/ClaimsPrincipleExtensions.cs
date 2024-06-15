using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace Api.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}