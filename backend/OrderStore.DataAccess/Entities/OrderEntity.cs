namespace OrderStore.DataAccess.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string UserId { get; set; }
    
    public int Status { get; set; }
    
    public DateTime EditDate { get; set; }
    
    public string Comment { get; set; }
    
    public uint FileId { get; set; }
}