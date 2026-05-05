using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class OrderHandler(AppDbContext context) : IOrderHandler
{
    public async Task<Response<Order?>> CanceOrderlAsync(CancelOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context
                .Orders
                .Include(x=> x.Product)
                .Include(x=> x.Voucher)
                .FirstOrDefaultAsync(x=> x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
            {
                return new Response<Order?>(null, 404, "Pedido não encontrado");
            }
        }
        catch
        {
            return new Response<Order?>(null, 500, "Falha ao obter o pedido");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Este pedido já foi cancelado");
            
            case EOrderStatus.WaitingPayment:
                break;
            
            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "Este pedido já foi pago");
            
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Este pedido já foi reembolsado");
            
            default:
                return new Response<Order?>(order, 400, "Esse pedido não pode ser cancelado.");
        }
        
        order.Status = EOrderStatus.Canceled;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(order, 500, "Não foi possível cancelar seu pedido");
        }
        
        return new Response<Order?>(order, 200, $"Pedido {order.Number} cancelado com sucesso.");
    }

    public async Task<Response<Order?>> CreateOrderAsync(CreateOrderRequest request)
    {
        Product? product;
        try
        {
            product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ProductId && x.IsActive == true);
            
            if(product is null)
                return new Response<Order?>(null, 400, "Produto não encontrado");
            
            context.Attach(product);
            
        }
        catch
        {
            return new Response<Order?>(null, 500, "Não foi possível obter o produto");
        }
        
        Voucher? voucher = null;
        try
        {
            if (request.VoucherId is not null)
            {
                voucher  = await context.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.VoucherId && x.IsActive == true);

                if (voucher is null)
                    return new Response<Order?>(null, 400, "voucher inválido");

                if (voucher.IsActive == false)
                    return new Response<Order?>(null, 400, "Este voucher já foi utilizado");
                
                voucher.IsActive = false;
                context.Vouchers.Update(voucher);
            }
        }
        catch
        {
            return new Response<Order?>(null, 500, "falha ao obter o voucher informado");
        }

        var order = new Order()
        {
            UserId = request.UserId,
            Product =  product,
            ProductId =  request.ProductId,
            Voucher = voucher,
            VoucherId = request.VoucherId,
        };

        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Não foi possível realizar seu pedido");
        }
        
        return new Response<Order?>(order, 201, $"Pedido {order.Number} criado com sucesso.");
    }

    public async Task<Response<Order?>> PayOrderAsync(PayOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pagamento não encontrado!");
        }
        catch
        {
            return new Response<Order?>(null, 500, "Falha ao consultar o pedido.");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Este pedido já foi cancelado");
            
            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "Este pedido já foi pago");
            
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Este pedido já foi reembolsado e não pode ser pago.");
            
            case EOrderStatus.WaitingPayment:
                break;
            
            default:
                return new Response<Order?>(order, 400, "Não foi possível concluir a transação.");
        }
        
        order.Status = EOrderStatus.Paid;
        order.ExternalReference = request.ExternalReference;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            return new Response<Order?>(order, 500, "Falha ao tentar pagar o pedido.");
        }
        
        return new Response<Order?>(order, 200, $"Pedido {order.Number} pago com sucesso.");
    }

    public async Task<Response<Order?>> RefundOrderAsync(RefundOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (order is null)
                return new Response<Order?>(null, 400, "Pedido não encontrado");
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Falha ao obter o pedido.");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "Este pedido já foi cancelado");
            
            case EOrderStatus.Paid:
                break;
            
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "Este pedido já foi reembolsado e não pode ser pago.");
            
            case EOrderStatus.WaitingPayment:
                return new Response<Order?>(order, 400, "Este pedido não foi pago e não pode ser reembolsado.");
            
            default:
                return new Response<Order?>(order, 400, "Não foi possível reembolsar o pedido.");
        }
        
        order.Status = EOrderStatus.Refunded;
        order.UpdatedAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch 
        {
            return new Response<Order?>(order, 500, "Falha ao reembolsar o pagamento.");
        }
        
        return new Response<Order?>(order, 200, $"Pedido {order.Number} estornado com sucesso.");
    }

    public async Task<PagedResponse<List<Order>?>> GetAllOrderAsync(GetAllOrdersRequest request)
    {
        try
        {
            var query = context
                .Orders
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x=> x.Voucher)
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.CreatedAt);
            
            var orders = await query
                .Skip((request.PageNumber - 1)  * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var count = await query.CountAsync();
            
            return new PagedResponse<List<Order>?>(orders, count, request.PageNumber, request.PageSize);
        }
        catch 
        {
            return new PagedResponse<List<Order>?>(null, 500, "Não foi possível obter os pedidos.");
        }
    }

    public async Task<Response<Order?>> GetOrderByNumberAsync(GetOrderByNumberRequest request)
    {
        try
        {
            var order = await context
                .Orders
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x=> x.Number == request.Number && x.UserId == request.UserId);

            return order is null
                ? new Response<Order?>(null, 404, "Pedido não encontrado")
                : new Response<Order?>(order);
        }
        catch 
        {
            return new Response<Order?>(null, 500, "Não foi possível recuperar o pedido.");
        }
    }
}