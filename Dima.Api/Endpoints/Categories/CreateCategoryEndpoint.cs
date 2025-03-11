using Azure;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;

namespace Dima.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPost("/", HandleAsync )
            .WithName("Create Category")
            .WithSummary("Cria uma nova categoria.")
            .WithOrder(1)
            .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ICategoryHandler handler, 
        CreateCategoryRequest request)
    {
        var result = await handler.CreateAsync(request);
        
        return result.IsSuccess 
            ? Results.Created($"/{result.Data?.Id}", result.Data) 
            : Results.BadRequest(result.Data);
    }

}