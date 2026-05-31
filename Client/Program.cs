using Client;
using Core;
using System.Text.Json;

Console.WriteLine("Клиент");
while (true)
{
    Console.WriteLine("Введите команду:");
    var command = int.Parse(Console.ReadLine());

    using var client = new MyTcpClient("127.0.0.1", 8888);
    await client.OpenConnectionAsync();

    if (command == 1)
    {
        Console.WriteLine("Введите название:");
        var name = Console.ReadLine();

        Console.WriteLine("Введите автора:");
        var author = Console.ReadLine();

        Console.WriteLine("Введите описание:");
        var description = Console.ReadLine();

        var book = new Book
        {
            Name = name,
            Author = author,
            Description = description
        };

        var serializeBook = JsonSerializer.Serialize(book);
        var requestDto = new RequestDto
        {
            Operation = DictionaryOperation.Create,
            Body = serializeBook,
        };
        var serializeRequestDto = JsonSerializer.Serialize(requestDto);

        await client.SendAsync(serializeRequestDto);
    }
    else if (command == 2)
    {
        var requestDto = new RequestDto
        {
            Operation = DictionaryOperation.Read,
            Body = string.Empty,
        };

        var serializeRequestDto = JsonSerializer.Serialize(requestDto);

        await client.SendAsync(serializeRequestDto);
        var books = await client.ReadAsync<List<Book>>();

        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    else if (command == 3)
    {
        Console.WriteLine("Введите id:");
        var id = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите название:");
        var name = Console.ReadLine();

        Console.WriteLine("Введите автора:");
        var author = Console.ReadLine();

        Console.WriteLine("Введите описание:");
        var description = Console.ReadLine();



        var myBook = new RequestDto()
        {
            Operation = DictionaryOperation.Create,
            Body = string.Empty,
        };

        var serializeBook = JsonSerializer.Serialize(myBook);
        var requestDto = new RequestDto
        {
            Operation = DictionaryOperation.Update,
            Body = serializeBook,
        };
        var serializeRequestDto = JsonSerializer.Serialize(requestDto);

        await client.SendAsync(serializeRequestDto);
        var ResponceDto = await client.ReadAsync<ResponceDto>();
    }
    else if (command == 4)
    {
        var id = int.Parse(Console.ReadLine());

        var serializeIdBook = JsonSerializer.Serialize(id);
        var requestDto = new RequestDto
        {
            Operation = DictionaryOperation.Delete,
            Body = serializeIdBook,
        };
        var serializeRequestDto = JsonSerializer.Serialize(requestDto);

        await client.SendAsync(serializeRequestDto);
    }

}