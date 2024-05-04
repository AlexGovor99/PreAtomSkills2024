namespace OrderStore.Contracts;

public record EditOrderRequest(string userId, Guid orderId, string name, uint fileId);