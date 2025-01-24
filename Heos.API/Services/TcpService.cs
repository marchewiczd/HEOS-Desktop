using System.Net.Sockets;
using System.Text;
using Heos.API.Models.HEOS;
using Newtonsoft.Json;

namespace Heos.API.Services;

public class TcpService : IDisposable
{
    private const string Protocol = "heos";
    private const int BufferSize = 1024;
    private readonly TcpClient _tcpClient;

    public TcpService(string hostname, int port = 1255)
    {
        _tcpClient = new TcpClient();
        _tcpClient.Connect(hostname, port);
    }
    
    public string Send(string rawCommand)
    {
        SendMessage(rawCommand);
        return ReceiveResponse();
    }
    
    public TResponse Send<TRequest, TResponse>(TRequest request)
        where TRequest : HeosRequest
        where TResponse : HeosResponse
    {
        SendMessage(request);
        return ReceiveResponse<TResponse>();
    }
    
    private void SendMessage(string rawCommand)
    {
        var payload = $"{Protocol}://{rawCommand}\r\n";
        var data = Encoding.UTF8.GetBytes(payload);
        
        _tcpClient.GetStream().Write(data, 0, data.Length);
    }

    private string ReceiveResponse()
    {
        var data = new byte[BufferSize];
        var responseLength = _tcpClient.GetStream().Read(data, 0, data.Length);
        
        return Encoding.UTF8.GetString(data, 0, responseLength);
    }

    private void SendMessage<TRequest>(TRequest request)
        where TRequest : HeosRequest =>
        SendMessage(request.Build());

    private TResponse ReceiveResponse<TResponse>()
        where TResponse : HeosResponse
    {
        var response = JsonConvert.DeserializeObject<TResponse>(ReceiveResponse());
        ArgumentNullException.ThrowIfNull(response);

        return response;
    }

    public void Dispose()
    {
        _tcpClient.Dispose();
    }
}