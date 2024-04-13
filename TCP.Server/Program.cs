using System.Net.Sockets;
using System.Net;
using System.Text;



var server = new TcpListener(IPAddress.Any, 9999);
server.Start();


while (true)
{
    // wait for client connection
    using TcpClient newClient = server.AcceptTcpClient();
    Console.WriteLine("New Client = {0}", newClient.Connected);

    // sets two streams
    var sReader = new StreamReader(newClient.GetStream(), Encoding.ASCII);
    var sWriter = new StreamWriter(newClient.GetStream(), Encoding.ASCII);

    string sData = null;
    Console.WriteLine("Client connected: {0}", newClient.Connected);
    char[] buffer = new char[1024];

    if (newClient.Connected)
    {
        Console.WriteLine($"newClient = {newClient.Connected}");
        // reads from stream
        sData = sReader.ReadLine();
        Console.WriteLine($"Data = {sData}");

        // shows content on the console.
        Console.WriteLine("Client says: " + sData);

        // to write something back.
        sWriter.WriteLine("OK!");
        sWriter.Flush();

        newClient.Close();
    }
}
