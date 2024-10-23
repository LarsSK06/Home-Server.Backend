using System.Text.Json.Serialization;

namespace HomeServer.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PublicityStatus{
    Public,
    Private
}