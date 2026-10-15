using System;
using System.Text.Json.Serialization;

public class Order
{

    [JsonPropertyName("idOrders")]
    public int IdOrders { get; set; }

    [JsonPropertyName("SenderName")]
    public string SenderName { get; set; } = string.Empty;

    [JsonPropertyName("RecipientName")]
    public string RecipientName { get; set; } = string.Empty;

    [JsonPropertyName("DeliveryAddress")]
    public string DeliveryAddress { get; set; } = string.Empty;

    [JsonPropertyName("CurrentStatus")]
    public string CurrentStatus { get; set; } = string.Empty;

    [JsonPropertyName("UpdatedAt")]
    public DateTime UpdatedAt { get; set; }
}