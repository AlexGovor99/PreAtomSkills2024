namespace OrderStore.Users;

public class User
{
    public string Login { get; set; }
    
    public string Password { get; set; }

    public string FullName { get; set; }
    
    public string Id { get; set; }
    
    public string Role { get; set; }

    public static string InspectorRole = "inspector";
    public static string ScientistRole = "scientist";
}

public static class Users
{
    public static List<User> SystemUsers = new List<User>()
    {
        new User()
        {
            Login = "Inspector0",
            Password = "Inspector00",
            FullName = "Marguerite Brown Sr.",
            Id = "1000",
            Role = "inspector"
        },
        new User()
        {
            Login = "Inspector1",
            Password = "Inspector01",
            FullName = "Salvatore Baumbach",
            Id = "1001",
            Role = "inspector"
        },
        new User()
        {
            Login = "Scientist1",
            Password = "Scientist01",
            FullName = "Bryan Sanford",
            Id = "2001",
            Role = "scientist"




            
        },
        new User()
        {
            Login = "Scientist2",
            Password = "Scientist02",
            FullName = "Silvia Jakubowski",
            Id = "2002",
            Role = "scientist"
        }
    };

    public static User? GetUser(string userId) => SystemUsers.FirstOrDefault(x => x.Id == userId);
}