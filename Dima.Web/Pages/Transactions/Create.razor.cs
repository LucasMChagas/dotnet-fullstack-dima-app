using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class CreateTransactionPage : ComponentBase
{
    private bool IsBusy { get; set; }  = false;
    public CreateTransactionRequest Request { get; set; } = new();
    public List<Category> Categories { get; set; } = [];
    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;
    [Inject] 
    public ICategoryHandler CategoryHandler { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await CategoryHandler.GetAllAsync(request);

            if (result.IsSuccess)
            {
                Categories = result.Data ?? [];
                Request.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
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
            var result = await TransactionHandler.CreateAsync(Request);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/lancamentos/historico");
            }
            else
                Snackbar.Add(result.Message, Severity.Error);
        }
        catch (Exception exception)
        {
            Snackbar.Add(exception.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}