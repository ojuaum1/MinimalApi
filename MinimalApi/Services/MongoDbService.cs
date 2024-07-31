using MongoDB.Bson;
using MongoDB.Driver;

namespace MinimalAPI.Services
{
    public class MongoDbService
    {
        //armazena a configuração da aplicação
        private readonly IConfiguration _configuration;

        //Armazena uma referência ao MongoDB
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Recebe a config da aplicação como configuration
        /// </summary>
        /// <param name="configuration"></param>
        public MongoDbService(IConfiguration configuration)
        {
            //atribui a configuração recebida em _configuration
            _configuration = configuration;

            //obtem a string de conexão através do _configuration
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //cria um objeto mongoUrl
            var mongoUrl = MongoUrl.Create(connectionString);

            //cria um client mongoClient para de conectar ao MongoDB
            var mongoClient = new MongoClient(mongoUrl);

            //Obtem a referencia ao banco ocm o nome específico da string de conexão
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        //Propriedade para acessar o banco de dados
        public IMongoDatabase GetDatabase => _database;

        public bool ClientExist(string clientId)
        {
            var collection = GetDatabase.GetCollection<BsonDocument>("client");
            var filter = Builders<BsonDocument>.Filter.Eq("clientId", clientId);
            return collection.Find(filter).Any();
        }

        public bool ProductExist(string productId)
        {
            var collection = GetDatabase.GetCollection<BsonDocument>("product");
            var filter = Builders<BsonDocument>.Filter.Eq("productId", productId);
            return collection.Find(filter).Any();
        }
    }

}