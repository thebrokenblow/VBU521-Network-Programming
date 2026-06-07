using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Сервер запущен");
using var socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
socketServer.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555));

while (true)
{
    var clientIp = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
    var buffer = new byte[65535];
    var result = await socketServer.ReceiveFromAsync(buffer, clientIp);
    var message = Encoding.UTF8.GetString(buffer, 0, result.ReceivedBytes);
    Console.WriteLine($"Сервер получил: {message}");
}