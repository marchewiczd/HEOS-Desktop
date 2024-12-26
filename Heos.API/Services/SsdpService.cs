using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Heos.API.Extensions;
using Heos.API.Mapping;
using Heos.API.Models.SSDP;

namespace Heos.API.Services;

/// <summary>
/// SSDP(Simple Service Discovery Protocol) Service used to discover HEOS Denon devices in network. 
/// </summary>
public class SsdpService
{
    private const int HostPort = 1900;
    private const string SearchTarget = "urn:schemas-denon-com:device:ACT-Denon:1";
    private const string MaxWait = "3";

    /// <summary>
    /// Represents 239.255.255.250 in HEX in big endian notation
    /// </summary>
    private readonly IPAddress _hostAddress = new(0xFAFFFFEF);
    private readonly IPEndPoint _ssdpEndpoint;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

    public SsdpService()
    {
        _ssdpEndpoint = new IPEndPoint(_hostAddress, HostPort);
    }
    
    public SsdpService(TimeSpan timeout) : this()
    {
        _timeout = timeout;
    }
    
    public IEnumerable<SsdpDiscoveryResponse> Discover()
    {
        using var udpSocket = GetUdpSocket();
        BroadcastDiscover(udpSocket);
        
        return ReceiveResponse(udpSocket, _timeout);
    }

    private Socket GetUdpSocket()
    {
        var localEndpoint = new IPEndPoint(IPAddress.Any, 0);
        var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.Bind(localEndpoint);

        return udpSocket;
    }

    private void BroadcastDiscover(Socket udpSocket) => 
        udpSocket.SendTo(GetRequestBytes(), SocketFlags.None, _ssdpEndpoint);

    private IEnumerable<SsdpDiscoveryResponse> ReceiveResponse(Socket udpSocket, TimeSpan timeout)
    {
        var receiveBuffer = new byte[1024];
        var response = new List<SsdpDiscoveryResponse>();
        
        var timeoutTimer = new Stopwatch();
        timeoutTimer.Start();
        
        while (timeoutTimer.ElapsedMilliseconds < timeout.TotalMilliseconds)
        {
            if (udpSocket.Available <= 0) continue;
            
            var receivedBytes = udpSocket.Receive(receiveBuffer, SocketFlags.None);

            if (receivedBytes > 0)
            {
                var responseString = Encoding.UTF8.GetString(receiveBuffer, 0, receivedBytes);
                response.Add(ResponseMapper.MapSsdpDiscovery(responseString));
            }
            
            timeoutTimer.Restart();
        }

        return response;
    }
    
    private byte[] GetRequestBytes()
    {
        var message = new SsdpDiscoveryRequest
        {
            Host = _ssdpEndpoint.ToString(),
            MaxWaitResponseTime = MaxWait,
            SearchTarget = SearchTarget
        };

        return message.ToUtf8Bytes();
    }
}