using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    const int VERSION = 69;
    const string DECOMPRESSION_METHOD = "GSA33";
    const byte VERSION_SIZE = 1;
    const byte DECOMPRESSION_METHOD_SIZE = 5;

    static void Main(string[] args)
    {
        var server = new TcpListener(IPAddress.Any, 9999);
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
        var random = new Random();
        var client = (TcpClient)c;
        var stream = client.GetStream();

        byte[] decompressionBytes = Encoding.ASCII.GetBytes(DECOMPRESSION_METHOD);

        for (int x = 0; x < 1; x++)
        {
            // var myData = new byte[16];
            var data = Encoding.ASCII.GetBytes("Oie");
            var dataSize = (byte)data.Length;


            // if(dataSize <= 16){
            // byte[] messageData = new byte[VERSION_SIZE + DECOMPRESSION_METHOD_SIZE + dataSize + 16];
            byte[] messageData = new byte[VERSION_SIZE + DECOMPRESSION_METHOD_SIZE + dataSize];

            messageData[0] = VERSION;

            Array.Copy(decompressionBytes, 0, messageData, 1, 5);

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