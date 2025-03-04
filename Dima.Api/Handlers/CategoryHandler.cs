using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            var categoty = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
            };
            await context.Categories.AddAsync(categoty);
            await context.SaveChangesAsync();

            return new Response<Category?>(categoty, 201, "Categoria criada com sucesso");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Erro ao criar uma categoria.");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category =
                await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(null, 404, "Categoria não encontrada!");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria atualizada com sucesso!");
        }
        catch 
        {
            return new Response<Category?>(null, 500, "Erro ao atualizar a categoria.");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category =
                await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(null, 404, "Categoria não encontrada!");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, message: "Categoria excluida com sucesso!");
        }
        catch 
        {
            return new Response<Category?>(null, 500, "Erro ao excluir a categoria.");
        }
    }

    public Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        throw new NotImplementedException();
    }
}