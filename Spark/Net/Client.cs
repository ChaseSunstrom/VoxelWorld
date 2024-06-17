using Spark.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Spark.Util;

namespace Spark.Net;

public class Client
{
    private TcpClient _tcpClient;
    private NetworkStream _tcpNetworkStream;
    private UdpClient _udpClient;
    private IPEndPoint _udpRemoteEndPoint;
    private readonly bool _useTcp;
    private readonly bool _useUdp;
    private bool _running;

    public event EventHandler<Envelope> PacketReceived;

    public Client(string tcpHost = null, int tcpPort = 0, string udpHost = null, int udpPort = 0)
    {
        if (tcpHost != null)
        {
            _tcpClient = new TcpClient(tcpHost, tcpPort);
            _tcpNetworkStream = _tcpClient.GetStream();
            _useTcp = true;
        }
        if (udpHost != null)
        {
            _udpClient = new UdpClient();
            _udpRemoteEndPoint = new IPEndPoint(IPAddress.Parse(udpHost), udpPort);
            _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0)); // Binds to a local endpoint
            _useUdp = true;
        }
    }

    public async Task SendPacketAsync(Packet packet)
    {
        var envelope = new Envelope("Client", "Server", packet);
        var message = Serializer.Serialize(envelope);
        var bytes = Encoding.UTF8.GetBytes(message);

        if (_useTcp)
        {
            await _tcpNetworkStream.WriteAsync(bytes, 0, bytes.Length);
        }
        if (_useUdp)
        {
            await _udpClient.SendAsync(bytes, bytes.Length, _udpRemoteEndPoint);
        }
    }

    public void StartReceiving()
    {
        _running = true;
        if (_useTcp)
        {
            Task.Run(() => ReceiveTcpPackets());
        }
        if (_useUdp)
        {
            Task.Run(() => ReceiveUdpPackets());
        }
    }

    private async Task ReceiveTcpPackets()
    {
        var buffer = new byte[4096];
        while (_running)
        {
            try
            {
                var byteCount = await _tcpNetworkStream.ReadAsync(buffer, 0, buffer.Length);
                if (byteCount > 0)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    var envelope = Serializer.Deserialize<Envelope>(message);
                    OnPacketReceived(envelope);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("[TCP CLIENT ERROR]: " + ex.Message);
            }
        }
    }

    private async Task ReceiveUdpPackets()
    {
        while (_running)
        {
            try
            {
                var result = await _udpClient.ReceiveAsync();
                var message = Encoding.UTF8.GetString(result.Buffer);
                var envelope = Serializer.Deserialize<Envelope>(message);
                OnPacketReceived(envelope);
            }
            catch (Exception ex)
            {
                Logger.Error("[UDP CLIENT ERROR]: " + ex.Message);
            }
        }
    }

    protected virtual void OnPacketReceived(Envelope envelope)
    {
        PacketReceived?.Invoke(this, envelope);
    }

    public void StopReceiving()
    {
        _running = false;
    }

    public void Close()
    {
        StopReceiving();
        if (_useTcp)
        {
            _tcpNetworkStream.Close();
            _tcpClient.Close();
        }
        if (_useUdp)
        {
            _udpClient.Close();
        }
    }
}