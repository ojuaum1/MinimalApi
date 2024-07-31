using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalAPI.Domains
{
    public class Product
    {   //define que esta prop é um id do objeto
        [BsonId]
        //define o nome do campo no mongoDb como _id e o tipo como objectId
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        //adiciona um dicionario para atributos adicionais
        public Dictionary<string, string> AdditionalAtribucts { get; set; }

        /// <summary>
        /// Ao ser instaciado um objeto da classe Prodcut, o atributo AdditionalAtribucts já irá com um novo dicionário, portanto habilitado para adicionar mais atributos
        /// </summary>
        public Product()
        {
            AdditionalAtribucts = new Dictionary<string, string>();
        }
    }
}