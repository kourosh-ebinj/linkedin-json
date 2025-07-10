using System.Text;
using Json.More;
using JSONPath.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;
using System.Text.Json;
using System.IO.Pipelines;
using JSONPath.Services;

namespace JSONPath.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamingsController : ControllerBase
{
    private readonly IBlobDataFile _blobDataFile;

    public StreamingsController(IBlobDataFile blobDataFile)
    {
        _blobDataFile = blobDataFile;
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(CancellationToken cancellationToken = default)
    {
        var filePath = _blobDataFile.FilePath;

        if (TryFindPropertyById(filePath, "a4ea732cd3d5948a", "width", out var resultElement))
            Console.WriteLine("ResultElement = " + resultElement);

        return Ok();
    }

    private bool TryFindPropertyById(string filePath, string targetId, string propertyName, out JsonElement result)
    {
        using FileStream fs = System.IO.File.OpenRead(filePath);
        using StreamReader sr = new StreamReader(fs);
        ReadOnlySpan<byte> jsonBytes = Encoding.UTF8.GetBytes(sr.ReadToEnd());
        Utf8JsonReader reader = new Utf8JsonReader(jsonBytes);

        string? currentId = null;

        while (reader.Read())
        {
            //if (reader.TokenType == JsonTokenType.StartObject)
            //{
            //    currentId = null; // Reset for each object
            //}

            //if (reader.TokenType == JsonTokenType.PropertyName)
            //{

            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}Start Object");
                    break;
                case JsonTokenType.EndObject:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}End Object");
                    break;
                case JsonTokenType.StartArray:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}Start Array");
                    break;
                case JsonTokenType.EndArray:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}End Array");
                    break;
                case JsonTokenType.PropertyName:
                    string property = reader.GetString();
                    reader.Read(); // Move to value

                    if (property == "id")
                    {
                        currentId = reader.GetString();
                    }
                    else if (property == propertyName && currentId == targetId)
                    {
                        result = JsonDocument.ParseValue(ref reader).RootElement;
                        return true; // Found the target property
                    }

                    Console.WriteLine($"{Indent(reader.CurrentDepth)}Property: {property}");
                    break;
                case JsonTokenType.String:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}String: {reader.GetString()}");
                    break;
                case JsonTokenType.Number:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}Number: {reader.GetDouble()}");
                    break;
                case JsonTokenType.True:
                case JsonTokenType.False:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}Boolean: {reader.GetBoolean()}");
                    break;
                case JsonTokenType.Null:
                    Console.WriteLine($"{Indent(reader.CurrentDepth)}Null");
                    break;
            }
            //}
        }

        result = default;
        return false; // Not found
    }
    private string Indent(int depth) => new string(' ', depth * 2);

}
