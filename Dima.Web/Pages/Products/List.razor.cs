using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Products;

public partial class ListProductsPage : ComponentBase
{
    public bool IsBusy { get; set; } = false;
    public List<Product> Products {get; set;} = [];

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IProductHandler Handler { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllProductsRequest();
            var result = await  Handler.GetAllAsync(request);
            if(result.IsSuccess)
                Products = result.Data ?? [];
            else
                Snackbar.Add(result.Message, Severity.Error);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}