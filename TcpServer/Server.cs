using Core;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using TcpServer.Repositories;

namespace TcpServer;

public class Server : IDisposable
{
    private readonly Socket _serverSocket;
    private Socket? _clientSocket;

    private const int BufferSize = 1024;

    public Server(EndPoint endPoint, int backlog)
    {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        _serverSocket.Bind(endPoint);
        _serverSocket.Listen(backlog);
    }

    public async Task RunAsync()
    {
        try
        {
            while (true)
            {
                await ReceiveAsync();
            }
        }
        catch (Exception)
        { 

        }
    }

    public async Task ReceiveAsync()
    {
        _clientSocket = await _serverSocket.AcceptAsync();
        var requestDto = await ReadResponceAsync(_clientSocket);

        switch (requestDto.Operation)
        {
            case DictionaryOperation.Create:
                await HandleCreateAsync(requestDto);
                break;
            case DictionaryOperation.Read:
                await HandleReadAsync(_clientSocket);
                break;
            case DictionaryOperation.Update:
                await HandleUpdateAsync(_clientSocket, requestDto);
                break;
            case DictionaryOperation.Delete:
                await HandleDeleteAsync(requestDto);
                break;
        }
    }

    private static async Task HandleCreateAsync(RequestDto requestDto)
    {
        var book = JsonSerializer.Deserialize<Book>(requestDto.Body);

        var bookRepository = new BookRepository();
        await bookRepository.AddAsync(book);
    }

    private static async Task HandleReadAsync(Socket clientSocket)
    {
        var bookRepository = new BookRepository();
        var books = await bookRepository.GetAllAsync();

        var serializeBooks = JsonSerializer.Serialize(books);

        await SendAsync(clientSocket, serializeBooks);
    }

    private static async Task HandleUpdateAsync(Socket clientSocket, RequestDto requestDto)
    {
        Book? book = null;
        try
        {
            book = JsonSerializer.Deserialize<Book>(requestDto.Body);

            var bookRepository = new BookRepository();
            await bookRepository.UpdateAsync(book);
        }
        catch (Exception )
        {
            var responceDto = new ResponceDto
            {
                Responce = DictionaryResponce.BadRequest,
                ResponceMessage = "Некорректный запрос"
            };
            var responce = JsonSerializer.Serialize(responceDto);

            await SendAsync(clientSocket, responce);
        }

       
    }

    private static async Task HandleDeleteAsync(RequestDto requestDto)
    {
        var id = JsonSerializer.Deserialize<int>(requestDto.Body);

        var bookRepository = new BookRepository();
        await bookRepository.RemoveAsync(id);
    }

    private static async Task<RequestDto> ReadResponceAsync(Socket clientSocket)
    {
        int readBytes;
        var buffer = new byte[BufferSize];
        var requestBuilder = new StringBuilder();
        do
        {
            readBytes = await clientSocket.ReceiveAsync(buffer);
            var request = Encoding.UTF8.GetString(buffer, 0, readBytes);
            requestBuilder.Append(request);
        }
        while (readBytes > 0);

        var requestString = requestBuilder.ToString();
        var requestDto = JsonSerializer.Deserialize<RequestDto>(requestString);

        return requestDto;
    }

    public static async Task SendAsync(Socket clientSocket, string responce)
    {
        var responceBytes = Encoding.UTF8.GetBytes(responce);
        await clientSocket.SendAsync(responceBytes);
        
        clientSocket.Shutdown(SocketShutdown.Send);

        clientSocket.Dispose();
    }

    public void Dispose()
    {
        _serverSocket.Dispose();
    }
}