using Lesson1;

Console.WriteLine("Введите запрос");
var message = Console.ReadLine();

using var client = new Client("127.0.0.1", 8888);
await client.OpenConnectionAsync();
await client.SendAsync(message);

var responce = await client.ReciveAsync();

Console.WriteLine(responce);

Console.ReadLine();