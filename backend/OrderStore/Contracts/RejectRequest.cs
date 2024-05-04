namespace OrderStore.Contracts;

public record RejectRequest(string userId, Guid orderId, string comment);