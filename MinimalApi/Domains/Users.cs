using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MinimalAPI.Domains
{
    public class User
    {
        /// <summary>
        /// Identificador único do usuário, mapeado para o campo _id no MongoDB.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        [BsonElement("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [BsonElement("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Senha do usuário (deve ser armazenada de forma segura, como um hash).
        /// </summary>
        [BsonElement("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Atributos adicionais do usuário.
        /// </summary>
        [BsonElement("additionalAttributes")]
        public Dictionary<string, string> AdditionalAttributes { get; set; }

        /// <summary>
        /// Construtor da classe User, inicializa o dicionário AdditionalAttributes.
        /// </summary>
        public User()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
