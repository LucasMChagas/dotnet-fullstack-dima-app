using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class EditCategoryPage : ComponentBase
{
    public bool IsBusy { get; set; } =  false;

    public UpdateCategoryRequest Request { get; set; } =  new UpdateCategoryRequest();

    [Parameter]
    public string Id { get; set; } = string.Empty;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] 
    public ICategoryHandler Handler { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        GetCategoryByIdRequest request = new();
        try
        {
            request.Id = long.Parse(this.Id);
        }
        catch 
        {
            Snackbar.Add("Parâmetro inválido");
        }

        try
        {
            var reponse = await Handler.GetByIdAsync(request);

            if (reponse.IsSuccess && reponse.Data != null)
            {
                Request = new UpdateCategoryRequest()
                {
                    Id = reponse.Data.Id,
                    Title = reponse.Data.Title,
                    Description = reponse.Data.Description
                };
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await Handler.UpdateAsync(Request);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/categorias");
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}