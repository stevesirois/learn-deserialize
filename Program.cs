using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeserializeFromFile
{
    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
        public string? SummaryField;
        public IList<DateTimeOffset>? DatesAvailable { get; set; }
        public Dictionary<string, HighLowTemps>? TemperatureRanges { get; set; }
        public string[]? SummaryWords { get; set; }
        public IList<object>? MixStructure { get; set; }
        [JsonExtensionData] // Catch overflow JSON from payload
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }

    public class HighLowTemps
    {
        public int High { get; set; }
        public int Low { get; set; }
    }

    public class MixStructure
    {
        public string Start { get; set; } = string.Empty;
        public Dictionary<string, string> Answers { get; set; } = new();
    }
    public class Program
    {
        public static void Main()
        {
            Dictionary<string, string> dict = new();
            string fileName = "WeatherForecast.json";
            string jsonString = File.ReadAllText(fileName);
            WeatherForecast weatherForecast =
                JsonSerializer.Deserialize<WeatherForecast>(jsonString)!;

            Console.WriteLine($"Date: {weatherForecast.Date}");
            Console.WriteLine($"TemperatureCelsius: {weatherForecast.TemperatureCelsius}");
            Console.WriteLine($"Summary: {weatherForecast.Summary}");

            MixStructure mixstuff = new();

            // Go thru the MixStructure section
            foreach (var protectionItem in weatherForecast.MixStructure!)
            {
                JsonElement innerArray = (JsonElement)protectionItem;
                // Since will dealing with plain object, make sure it's an array next
                if (innerArray.ValueKind == JsonValueKind.Array)
                {
                    // We are positionnal here since there's no attributes
                    mixstuff.Start = innerArray[0].ToString();
                    JsonElement listOfKV = innerArray[1];
                    // Tackel the key-values list next
                    if (listOfKV.ValueKind == JsonValueKind.Object)
                    {
                        foreach (JsonProperty property in listOfKV.EnumerateObject())
                        {
                            mixstuff.Answers.Add(
                                property.Name,
                                property.Value.GetString()!
                                );
                        }
                    }
                    // next be may null
                    JsonElement nullit = innerArray[2];
                    if (nullit.ValueKind != JsonValueKind.Null)
                    {
                        Console.WriteLine($"Wow, it NOT null: {nullit}");
                    }
                    // no class needed also
                    var lastOf = innerArray[3];

                }
            }
            Console.WriteLine($"Start of MixStructure: {mixstuff.Start}");
            Console.WriteLine(string.Join(", ", mixstuff.Answers.Select(pair => $"{pair.Key} => {pair.Value}")));
            Console.WriteLine("Leftover...");
            Console.WriteLine(string.Join(", ", weatherForecast?.ExtensionData?.Select(pair => $"{pair.Key} => {pair.Value}")!));
        }
    }
}