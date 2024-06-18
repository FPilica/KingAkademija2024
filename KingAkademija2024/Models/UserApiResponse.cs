namespace KingAkademija2024.Models;

public class UserApiResponse
{
    public List<User> Users { get; set; }
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}