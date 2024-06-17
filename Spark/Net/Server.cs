using Spark.Serialization;
using Spark.Util;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Net;
public class Server
{
    private bool _shutdown = false;
    private readonly int _port;
    private readonly IPAddress _ipAddress;
    private readonly UdpClient _udpServer;
    private readonly TcpListener _tcpListener;
    private readonly ConcurrentDictionary<IPEndPoint, IPEndPoint> _udpClients = new();
    private readonly ConcurrentDictionary<TcpClient, NetworkStream> _tcpClients = new();

    public Server(string ip = "127.0.0.1", int port = 8080)
    {
        _ipAddress = IPAddress.Parse(ip);
        _port = port;
        _udpServer = new UdpClient(new IPEndPoint(_ipAddress, _port));
        _tcpListener = new TcpListener(_ipAddress, _port);
    }

    public void Start()
    {
        Task.Run(() => StartUdpReceive());
        Task.Run(() => StartTcpAccept());
        Logger.Trace("[SERVER STARTED]: [IP]: " + _ipAddress + " [PORT]: " + _port);

        while (!_shutdown)
        {
            Thread.Sleep(100); // Prevents the while loop from consuming too much CPU
        }
    }

    private async Task StartUdpReceive()
    {
        while (!_shutdown)
        {
            try
            {
                var udpResult = await _udpServer.ReceiveAsync();
                var message = Encoding.UTF8.GetString(udpResult.Buffer);
                Logger.Trace("[UDP SERVER RECEIVED]: " + message);

                var envelope = Serializer.Deserialize<Envelope>(message);

                Logger.Trace($"[ENVELOPE RECEIVED]: Source: {envelope.Source}, Destination: {envelope.Destination}, Packet Type: {envelope.Packet.Type}, Packet Version: {envelope.Packet.Version}, Packet Data: {envelope.Packet.Data}");

                _udpClients[udpResult.RemoteEndPoint] = udpResult.RemoteEndPoint;

                // Broadcast to all UDP clients
                foreach (var clientEndpoint in _udpClients.Values)
                {
                    if (!clientEndpoint.Equals(udpResult.RemoteEndPoint)) // Avoid echoing back to the sender
                    {
                        await _udpServer.SendAsync(udpResult.Buffer, udpResult.Buffer.Length, clientEndpoint);
                    }
                }
            }
            catch (Exception e)
            {
                if (!_shutdown) // Avoid logging errors after shutdown
                {
                    Logger.Error("[UDP SERVER ERROR]: " + e.Message);
                }
            }
        }
    }

    private async Task StartTcpAccept()
    {
        _tcpListener.Start();
        while (!_shutdown)
        {
            try
            {
                var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                var stream = tcpClient.GetStream();
                _tcpClients[tcpClient] = stream;
                Task.Run(() => HandleTcpClient(tcpClient));
            }
            catch (Exception e)
            {
                if (!_shutdown) // Avoid logging errors after shutdown
                {
                    Logger.Error("[TCP SERVER ERROR]: " + e.Message);
                }
            }
        }
    }

    private async Task HandleTcpClient(TcpClient tcpClient)
    {
        var buffer = new byte[4096];
        var stream = tcpClient.GetStream();

        try
        {
            while (tcpClient.Connected && !_shutdown)
            {
                var byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (byteCount > 0)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Logger.Trace("[TCP SERVER RECEIVED]: " + message);

                    var envelope = Serializer.Deserialize<Envelope>(message);
                    // Process the packet within the envelope
                    Logger.Trace($"[ENVELOPE RECEIVED]: Source: {envelope.Source}, Destination: {envelope.Destination}, Packet Type: {envelope.Packet.Type}, Packet Version: {envelope.Packet.Version}, Packet Data: {envelope.Packet.Data}");

                    foreach (var clientStream in _tcpClients.Values)
                    {
                        if (clientStream != stream) // Avoid echoing back to the sender
                        {
                            await clientStream.WriteAsync(buffer, 0, byteCount);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            if (!_shutdown) // Avoid logging errors after shutdown
            {
                Logger.Error("[TCP SERVER CLIENT ERROR]: " + e.Message);
            }
        }
        finally
        {
            _tcpClients.TryRemove(tcpClient, out _);
            tcpClient.Close();
        }
    }

    public async Task SendUdpAsync(string message, string ip, int port)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
        await _udpServer.SendAsync(bytes, bytes.Length, endpoint);
    }

    public async Task SendTcpAsync(string message, string ip, int port)
    {
        using (var tcpClient = new TcpClient())
        {
            await tcpClient.ConnectAsync(IPAddress.Parse(ip), port);
            var stream = tcpClient.GetStream();
            var bytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }

    public async Task SendPacketAsync(Packet packet, string ip, int port, bool isTcp)
    {
        var envelope = new Envelope("Server", "Client", packet);
        var message = Serializer.Serialize(envelope);

        if (isTcp)
        {
            await SendTcpAsync(message, ip, port);
        }
        else
        {
            await SendUdpAsync(message, ip, port);
        }
    }

    public void Shutdown()
    {
        _shutdown = true;
        _udpServer.Close();
        _tcpListener.Stop();
        
        foreach (var client in _tcpClients.Keys)
            client.Close();

        Logger.Trace("[SERVER SHUTDOWN]");
    }
}