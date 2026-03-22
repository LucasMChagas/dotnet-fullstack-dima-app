using System.Security.Claims;
using Dima.Api.Common.Api;

namespace Dima.Api.Endpoints.Identity;

public class GetRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/roles", Handle)
            .RequireAuthorization();

    private static Task<IResult> Handle(ClaimsPrincipal user)
    {
        if(user.Identity is null || user.Identity.IsAuthenticated)
            return Task.FromResult(Results.Unauthorized());
        
        var identity = user.Identity as ClaimsIdentity;
        
        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value,
                claim.ValueType
            });
        
        return  Task.FromResult<IResult>(TypedResults.Json(roles));
    }
}