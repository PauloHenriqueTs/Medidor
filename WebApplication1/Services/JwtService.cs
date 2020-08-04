using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class JwtService
    {
        public static string GetJwt(HttpContext httpContext)
        {
            return httpContext.Session.GetString("token");
        }

        public static void SetJwt(HttpContext httpContext, string token)
        {
            httpContext.Session.SetString("token", token);
        }
    }
}