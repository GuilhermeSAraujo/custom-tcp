using System.Net.Sockets;
using System.Text;

TcpClient client = new TcpClient("localhost", 9999);

// Get the stream used for reading and writing.
NetworkStream stream = client.GetStream();

// Send the message to the connected server. 
string message = "Hello Server!\n";
byte[] data = Encoding.ASCII.GetBytes(message);
Console.WriteLine(data.Length);
stream.Write(data, 0, data.Length);

Console.WriteLine("Sent: {0}", message);

// Buffer to store the response bytes.
data = new byte[256];

// String to store the response ASCII representation.
string responseData = string.Empty;

// Read the first batch of the server response bytes.
int bytes = stream.Read(data, 0, data.Length);
responseData = Encoding.ASCII.GetString(data, 0, bytes);
Console.WriteLine("Received: {0}", responseData);

// Close everything.
stream.Close();
client.Close();