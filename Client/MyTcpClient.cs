using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client;

public class MyTcpClient : IDisposable
{
    private readonly Socket _socket;
    private readonly string _address;
    private readonly int _port;

    private const int BufferSize = 1024;

    public MyTcpClient(string address, int port)
    {
        _port = port;
        _address = address;

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public async Task OpenConnectionAsync()
    {
        await _socket.ConnectAsync(_address, _port);
    }

    public async Task SendAsync(string request)
    {
        var requestBytes = Encoding.UTF8.GetBytes(request);
        await _socket.SendAsync(requestBytes);

        _socket.Shutdown(SocketShutdown.Send);
    }

    public async Task<T> ReadAsync<T>()
    {
        var responceStr = await ReciveAsync();
        var responce = JsonSerializer.Deserialize<T>(responceStr);

        return responce;
    }

    private async Task<string> ReciveAsync()
    {
        int readBytes;
        var buffer = new byte[BufferSize];
        var responceBuilder = new StringBuilder();
        do
        {
            readBytes = await _socket.ReceiveAsync(buffer);
            var responce = Encoding.UTF8.GetString(buffer, 0, readBytes);
            responceBuilder.Append(responce);
        }
        while (readBytes > 0);

        return responceBuilder.ToString();
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}
