using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Build.Security.AspNetCore.Middleware.Dto;
using Build.Security.AspNetCore.Middleware.Request;
using Microsoft.AspNetCore.Http;

namespace API;

public class SampleRequestEnricher : IRequestEnricher
{
    public Task EnrichRequestAsync(OpaQueryRequest request, HttpContext httpContext)
    {
        //PUll the access-token from httpContext.Request
        var authHeader = httpContext.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            //Trim "Bearer" from the access token
            var token = authHeader.Substring("Bearer ".Length).Trim();

            //Add access-token in the enriched request
            request.Input.Enriched["access_token"] = token;
        }
        else
        {
            // Set access_token to null if not available
            request.Input.Enriched["access_token"] = null;
        }
        return Task.CompletedTask;
    }
}
