namespace OrderStore.Core.Abstractions;

public interface IApprovalService
{
    public Task<DateTime?> SendToApproval();
}