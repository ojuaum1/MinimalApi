using MinimalAPI.Domains;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MinimalApi.Domains
{
    public class Order
    {
        [BsonId] // Define que esta prop é Id do objeto
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)] // Define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        public string Id { get; set; } // Identificador único do pedido

        [BsonElement("date")]
        public DateTime Date { get; set; } // Data do pedido

        [BsonElement("status")]
        public string Status { get; set; } // Status do pedido 

        [BsonElement("productId")]
        // [JsonIgnore] // Remova este atributo para incluir o ProductId na serialização JSON
        public List<string> ProductId { get; set; }

        // Referência aos produtos do pedido
        [BsonIgnore]
        public List<Product>? Products { get; set; }

        // Referência para referenciar pedido ao cliente
        [BsonElement("clientId")]
        [JsonIgnore] // Quando listarmos evitar que venha o id do cliente 
        public string ClientId { get; set; } // Identificador do cliente

        [BsonIgnore]
        public Client? Client { get; set; }

        // Adiciona um dicionário para atributos adicionais além dos já definidos
        public Dictionary<string, string>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um obj da classe Order, o atributo AdditionalAttributes já virá com um novo dicionário e, portanto, habilitado para adicionar mais atributos
        /// </summary>
        public Order()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
