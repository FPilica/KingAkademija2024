using System.Text.Json.Serialization;

namespace KingAkademija2024.Models;

public class Product
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Title { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    [JsonPropertyName("thumbnail")]
    public string Image { get; set; }
    [JsonIgnore]
    public string Category { get; set; }
}