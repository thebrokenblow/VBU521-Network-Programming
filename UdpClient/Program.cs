using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Клиент запущен");

while (true)
{
    using var socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    Console.WriteLine("Введите сообщение");
    var message = Console.ReadLine();
    var data = Encoding.UTF8.GetBytes(message);
    var remoteAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
    var bytes = await socketClient.SendToAsync(data, remoteAddress);
}