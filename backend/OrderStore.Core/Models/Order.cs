namespace OrderStore.Core.Models;

public record Order
{
    private Order(Guid id, string name, string userId, int status, DateTime editDate, string comment, uint fileId)
    {
        Id = id;
        Name = name;
        UserId = userId;
        Status = status;
        EditDate = editDate;
        Comment = comment;
        FileId = fileId;
    }
    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string UserId { get; set; }
    
    public int Status { get; set; }
    
    public DateTime EditDate { get; set; }
    
    public string Comment { get; set; }
    
    public uint FileId { get; set; }

    public static (Order? Order, string Error) Create(Guid id, string name, string userId, int status, DateTime editDate, string comment, uint fileId)
    {
        return (new Order(id, name, userId, status, editDate, comment, fileId), string.Empty);
    }
}