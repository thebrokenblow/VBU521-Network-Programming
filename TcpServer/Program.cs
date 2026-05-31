using System.Net;
using TcpServer;

Console.WriteLine("Сервер");

var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8888);
var server = new Server(ipEndPoint, 1000);
await server.RunAsync();