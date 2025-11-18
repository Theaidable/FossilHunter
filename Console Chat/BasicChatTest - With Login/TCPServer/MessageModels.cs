using System.Text.Json.Serialization;

namespace Chat_Test
{
    /// <summary>
    /// En fælles struktur for datatyper for JSON-beskeder
    /// Er med til at gøre koden renere
    /// </summary>

    // Beskeder FRA klienten TIL serveren.
    // type: "register" | "login" | "chat" | "quit" | (evt. flere senere)
    // username/password bruges ved REGISTER/LOGIN; text bruges ved CHAT.
    public record ClientEnvelope(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("username")] string? Username,
        [property: JsonPropertyName("password")] string? Password,
        [property: JsonPropertyName("text")] string? Text
    );

    // Generelle korte svar FRA serveren TIL klienten.
    // type: "ok" | "error" | "system" | "bye"
    // message: en lille tekst, fx "logged_in", "username_taken" osv.
    public record ServerMsg(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("message")] string Message
    );

    // Chatlinje FRA serveren TIL alle klienter (undtagen afsenderen).
    // type = "msg", from = brugernavn, text = besked, ts = timestamp (UTC ISO8601).
    public record ChatMsg(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("from")] string From,
        [property: JsonPropertyName("text")] string Text,
        [property: JsonPropertyName("ts")] string TimestampUtcIso
    );
}
