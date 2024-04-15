using System.Net.Sockets;
using System.Text;

var client = new TcpClient("localhost", 9999);

// Get the stream used for reading and writing.
var stream = client.GetStream();

// Buffer to store the response bytes.
var data = new byte[256];

string responseData = string.Empty;

var screen = new string[5, 5];
for (int i = 0; i < 5; i++)
{
    for (int j = 0; j < 5; j++)
    {
        screen[i, j] = "X ";
    }
}
Console.WriteLine("-----------------");
DisplayScreen();

while (stream.CanRead) // Keep reading until no more data
{
    int bytes = await stream.ReadAsync(data);
    responseData += Encoding.ASCII.GetString(data, 0, bytes);

    // Process all complete messages
    while (responseData.Contains(';'))
    {
        var messageEnd = responseData.IndexOf(';');
        var message = responseData.Substring(0, messageEnd);
        // remove previous "0"

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (screen[i, j] == "O ")
                {
                    screen[i, j] = "X ";
                }
            }
        }

        var x = int.Parse(message.Split('/')[0]);
        var y = int.Parse(message.Split('/')[1]);
        screen[x, y] = "O ";
        Console.WriteLine("-----------------");

        DisplayScreen();

        responseData = responseData.Substring(messageEnd + 1);
    }
}

stream.Close();
client.Close();


void DisplayScreen()
{
    Console.Clear();
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            Console.Write(screen[i, j] + " ");
        }
        Console.WriteLine();
    }
}