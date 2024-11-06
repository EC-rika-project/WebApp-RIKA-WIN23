//using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    //private readonly AppDbContext _dbContext;

    //public OrderService(AppDbContext dbContext)
    //{
    //    _dbContext = dbContext;
    //}

    public async Task<bool> CreateOrderAsync(OrderDto orderDto)
    {
       
        var order = new OrderEntity
        {
            FirstName = orderDto.FirstName,
            LastName = orderDto.LastName,
            StreetName = orderDto.StreetName,
            PostalCode = orderDto.PostalCode,
            City = orderDto.City,
            Email = orderDto.Email,
            PhoneNumber = orderDto.PhoneNumber,
            TotalPrice = orderDto.TotalPrice,
            CreatedAt = DateTime.UtcNow
        };

       
        foreach (var item in orderDto.CartItems)
        {
            var orderItem = new OrderItem
            {
                ProductName = item.Name,
                Quantity = item.Quantity,
                Price = item.Price
            };

            order.Items.Add(orderItem);
        }

       
        //_dbContext.Orders.Add(order);
        //await _dbContext.SaveChangesAsync();

        return true;
    }
}

public interface IOrderService
{
    Task<bool> CreateOrderAsync(OrderDto orderDto);
}