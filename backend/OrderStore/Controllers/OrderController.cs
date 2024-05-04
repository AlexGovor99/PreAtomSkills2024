using Microsoft.AspNetCore.Mvc;
using OrderStore.Contracts;
using OrderStore.Core.Abstractions;
using OrderStore.Core.Models;

namespace OrderStore.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrderController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpPost("Test")]
    public async Task<ActionResult<string>> Test()
    {
        return Ok("test");
    }

    [HttpGet("GetAll/{userId}")]
    public async Task<ActionResult<List<Order>>> GetAll(string userId)
    {
        if (CheckUser(userId, Users.User.InspectorRole))
        {
            return await _ordersService.GetAll();
        }

        if (CheckUser(userId, Users.User.ScientistRole))
        {
            return await _ordersService.GetAllByUser(userId);
        }

        return BadRequest("Bad user");
    }
    
    [HttpGet("Get/{id:guid}")]
    public async Task<ActionResult<Order>> Get(Guid id)
    {
        var result = await _ordersService.Get(id);
        
        if (result == null) 
            return BadRequest();

        return result;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateRequest request)
    {
        if (!CheckUser(request.userId, Users.User.ScientistRole))
        {
            return BadRequest("Bad role");
        }

        var order = Order.Create(
            Guid.NewGuid(),
            request.name,
            request.userId,
            (int) Status.Created,
            DateTime.UtcNow,
            string.Empty,
            request.fileId
            );

        var result = await _ordersService.CreateOrder(order.Order!);
        return Ok(result);
    }

    [HttpPost("EditOrder")]
    public async Task<ActionResult<EditOrderResponse>> EditOrder([FromBody] EditOrderRequest request)
    {
        if (!CheckUser(request.userId, Users.User.ScientistRole))
        {
            return BadRequest("Bad role");
        }

        var order = await _ordersService.Get(request.orderId);

        if (order == null)
        {
            return BadRequest("No order");
        }

        if (order.UserId != request.userId)
        {
            return BadRequest("Another user");
        }

        if (order.Status != (int)Status.Created && order.Status != (int)Status.Rejected)
        {
            return BadRequest("Bad status");
        }

        await _ordersService.UpdateOrder(order with 
        {
            Name = request.name,
            FileId = request.fileId,
            Status = (int)Status.Created,
            Comment = "",
        });

        return Ok();
    }

    [HttpPost("Approve")]
    public async Task<ActionResult<ApproveResponse>> Approve([FromBody] ApproveRequest request)
    {
        if (!CheckUser(request.userId, Users.User.ScientistRole))
        {
            return BadRequest("Bad role");
        }

        var order = await _ordersService.Get(request.orderId);

        if (order == null)
        {
            return BadRequest("No order");
        }

        await _ordersService.UpdateOrder(order with 
        {
            Status = (int)Status.Approved,
            EditDate = DateTime.UtcNow,
            Comment = "",
        });

        return Ok();
    }

    [HttpPost("Reject")]
    public async Task<ActionResult<RejectResponse>> Reject([FromBody] RejectRequest request)
    {
        if (!CheckUser(request.userId, Users.User.InspectorRole))
        {
            return BadRequest("Bad role");
        }

        var order = await _ordersService.Get(request.orderId);

        if (order == null)
        {
            return BadRequest("No order");
        }

        await _ordersService.UpdateOrder(order with 
        {
            Status = (int)Status.Rejected,
            EditDate = DateTime.UtcNow,
            Comment = request.comment,
        });

        return Ok();
    }

    private bool CheckUser(string userId, string userRole)
    {
        var user = Users.Users.GetUser(userId);

        return user != null || user!.Role.Equals(userRole);
    }
}