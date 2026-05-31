using Lesson1.Server;
using System.Net;


var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8888);
var server = new Server(ipEndPoint, 1000);

Console.WriteLine("Сервер запущен");

while (true)
{
    var request = await server.ReceiveAsync();
    Console.WriteLine($"Запрос '{request}' получен");
    Console.WriteLine($"Отправляю ответ 'Привет, {request}'");
    await server.SendAsync($"Отправляю ответ 'Привет, {request}'");
}