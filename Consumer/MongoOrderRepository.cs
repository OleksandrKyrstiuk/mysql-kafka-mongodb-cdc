using System.Threading.Tasks;
using MongoDB.Driver;

public class MongoOrderRepository
{
    private readonly IMongoCollection<MongoOrder> _ordersCollection;

    public MongoOrderRepository(string connectionString, string databaseName, string collectionName)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);
        _ordersCollection = mongoDatabase.GetCollection<MongoOrder>(collectionName);
    }

    /// <summary>
    /// Виконує ідемпотентне збереження (Upsert). 
    /// Повертає true, якщо було створено новий запис (INSERT), або false, якщо оновлено наявний (UPDATE).
    /// </summary>
    public async Task<bool> UpsertOrderAsync(MongoOrder mongoDoc)
    {
        var filter = Builders<MongoOrder>.Filter.Eq(doc => doc.IdOrders, mongoDoc.IdOrders);
        var options = new ReplaceOptions { IsUpsert = true };

        var result = await _ordersCollection.ReplaceOneAsync(filter, mongoDoc, options);

        return result.UpsertedId != null;
    }
}