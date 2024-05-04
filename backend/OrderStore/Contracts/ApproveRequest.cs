namespace OrderStore.Contracts;

public record ApproveRequest(string userId, Guid orderId);