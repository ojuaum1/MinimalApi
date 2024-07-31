using MinimalAPI.Domains;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MinimalApi.ViewModels
{
    public class OrderViewModel
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Identificador único do pedido

        public DateTime Date { get; set; } // Data do pedido

        public string Status { get; set; } // Status do pedido

        [BsonElement("productId")]
        public List<string> ProductId { get; set; } // Identificadores dos produtos

        [BsonElement("clientId")]
        public string ClientId { get; set; } // Identificador do cliente

        [BsonIgnore]
        [JsonIgnore]
        public List<Product>? Products { get; set; } // Produtos do pedido (opcional)

        [BsonIgnore]
        [JsonIgnore]
        public Client? Client { get; set; } // Cliente do pedido (opcional)
    }
}
