using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartPantry.Api.Models;

public class PantryItem
{
    [BsonId] // Indica que este campo é a chave primária no MongoDB
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}