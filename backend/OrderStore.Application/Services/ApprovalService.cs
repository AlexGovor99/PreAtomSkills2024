using OrderStore.Core.Abstractions;
using OrderStore.Core.Models;

namespace OrderStore.Application.Services;

public class ApprovalService  : IApprovalService
{
    private readonly IOrdersRepository _repository;

    public ApprovalService(IOrdersRepository repository)
    {
        _repository = repository;
    }

    public async Task<DateTime?> SendToApproval()
    {
        var orders = await _repository.Get();
        var order = orders.OrderBy(x => x.EditDate).FirstOrDefault(x => x.Status == (int)Status.Approved);

        if (order == null)
        {
            return null;
        }

        var result = await _repository.Update(order with 
        {
            Status = (int)Status.Sent,
            EditDate = DateTime.UtcNow,
        });

        return DateTime.Now;
    }
}