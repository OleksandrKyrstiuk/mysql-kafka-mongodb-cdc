using System;
using MongoDB.Bson.Serialization.Attributes;

public class MongoOrder
{
    [BsonId]
    public int IdOrders { get; set; }

    public string SenderName { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string CurrentStatus { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }

    public static MongoOrder FromOrder(Order order) => new MongoOrder
    {
        IdOrders = order.IdOrders,
        SenderName = order.SenderName,
        RecipientName = order.RecipientName,
        DeliveryAddress = order.DeliveryAddress,
        CurrentStatus = order.CurrentStatus,
        UpdatedAt = order.UpdatedAt
    };
}