using System.Net.Sockets;
using System.Text;

var client = new TcpClient("localhost", 9999);

var stream = client.GetStream();

var version = new byte[1];
var decompressionMethod = new byte[5];
var dataSize = new byte[1];

string responseData = string.Empty;

while (stream.CanRead)
{
    await stream.ReadExactlyAsync(version);
    await stream.ReadExactlyAsync(decompressionMethod);
    await stream.ReadExactlyAsync(dataSize);

    Console.WriteLine($"Version = {version[0]}");
    Console.WriteLine($"Decompression method = {Encoding.ASCII.GetString(decompressionMethod)}");
    Console.WriteLine($"Data size = {dataSize[0]}");
}