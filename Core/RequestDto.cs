namespace Core;

public class RequestDto
{
    public required DictionaryOperation Operation { get; init; }
    public required string Body { get; init; }
}