using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using VChatServer;

namespace WebSocketsSample.Controllers;


// <snippet>
public class WebSocketController : ControllerBase
{
    public static List<Client> clients = new List<Client>();
    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            Client client = new Client();
            client.WebSocket = webSocket;
            clients.Add(client);
            Console.WriteLine(clients.Count());

            await Receive(client);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    // </snippet>


    public static async Task Receive(Client client)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await client.WebSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        client.MessageInformation = receiveResult;
        client.Message = buffer;

        while (!receiveResult.CloseStatus.HasValue)
        {
            await Send(client);
            receiveResult = await client.WebSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        clients.Remove(client);

        await client.WebSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
    private static async Task Send(Client client)
    {
        foreach (var c in clients)
        {
            if (c != client)
            {
                await c.WebSocket.SendAsync(
                    new ArraySegment<byte>(client.Message, 0, client.MessageInformation.Count),
                    client.MessageInformation.MessageType,
                    client.MessageInformation.EndOfMessage,
                    CancellationToken.None);

                await client.WebSocket.SendAsync(
                    new ArraySegment<byte>(c.Message, 0, c.MessageInformation.Count),
                    c.MessageInformation.MessageType,
                    c.MessageInformation.EndOfMessage,
                    CancellationToken.None);
            }
        }
    }

}
