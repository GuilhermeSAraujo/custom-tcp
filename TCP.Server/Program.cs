using System.Net.Sockets;
using System.Net;
using System.Text;



var server = new TcpListener(IPAddress.Any, 9999);
server.Start();
Console.WriteLine("Server Running!");

while (true)
{
    TcpClient newClient = await server.AcceptTcpClientAsync();

    var thread = new Thread(new ParameterizedThreadStart(SendMessage));

    thread.Start(newClient);
}

async void SendMessage(object c)
{
    var client = (TcpClient)c;

    var stream = client.GetStream();

    // Console.WriteLine("Data length = {0}", messageData.Length);

    var random = new Random();
    for (int x = 0; x < 150; x++)
    {
        var messageData = Encoding.ASCII.GetBytes($"{random.Next(0, 5)}/{random.Next(0, 5)};");
        await stream.WriteAsync(messageData);
        await Task.Delay(1250);
    }
}
