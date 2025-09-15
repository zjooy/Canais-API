using Amazon.DynamoDBv2.Model;

namespace Canais.Infrastructure.Helpers;

public class FiltroDynamo
{
    public Dictionary<string, string> AttributeNames { get; set; }
    public Dictionary<string, AttributeValue> AttributeValues { get; set; }
    public string FilterExpression { get; set; }
}
