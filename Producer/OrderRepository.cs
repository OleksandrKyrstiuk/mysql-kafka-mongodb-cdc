using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;

public class OrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Order>> GetUpdatedOrdersAsync(DateTime lastProcessedTime)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = @"
            SELECT idOrders, SenderName, RecipientName, DeliveryAddress, CurrentStatus, UpdatedAt 
            FROM orders 
            WHERE UpdatedAt > @LastProcessedTime 
            ORDER BY UpdatedAt ASC";

        return await connection.QueryAsync<Order>(sql, new { LastProcessedTime = lastProcessedTime });
    }
}