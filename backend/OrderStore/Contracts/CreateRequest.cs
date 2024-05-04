namespace OrderStore.Contracts;

public record CreateRequest(string userId, string name, uint fileId);