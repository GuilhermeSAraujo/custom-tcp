using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    public static TcpConfigs TcpProtocol = new();
    static void Main(string[] args)
    {
        var server = new TcpListener(IPAddress.Any, TcpProtocol.Port);

        server.Start();
        Console.WriteLine("Server Running!");

        while (true)
        {
            TcpClient newClient = server.AcceptTcpClient();

            var thread = new Thread(new ParameterizedThreadStart(SendMessage));

            thread.Start(newClient);
        }
    }

    static void SendMessage(object c)
    {
        var client = (TcpClient)c;
        var stream = client.GetStream();

        for (int x = 0; x < 1; x++)
        {
            var data = Encoding.ASCII.GetBytes("12345678901234567");
            var dataSize = (byte)data.Length;

            if (dataSize <= 16)
            {
                // divide packages
            }
            byte[] messageData = new byte[TcpProtocol.VersionSize + TcpProtocol.DecompressionMethodSize + dataSize];

            messageData[0] = (byte)TcpProtocol.Version;

            Array.Copy(TcpProtocol.DecompressionMethodAsBytes, 0, messageData, 1, 5);

            messageData[6] = dataSize;

            // Array.Copy(data, 0, messageData, 7, dataSize);

            // message array - contains my custom package
            // VERSION | DECOMPRESSION_METHOD | DATA_SIZE | DATA
            // 0 write from the start of the message array
            // messageData.Length write the entire message array
            stream.Write(messageData, 0, messageData.Length);

            Thread.Sleep(1250);
        }
    }
}

class TcpConfigs
{
    public int Port = 9999;
    public int Version = 1;
    public byte VersionSize => (byte)Version;
    public string DecompressionMethod = "GSA33";
    public byte DecompressionMethodSize => (byte)DecompressionMethod.Length;
    public byte[] DecompressionMethodAsBytes => Encoding.ASCII.GetBytes(DecompressionMethod);
}