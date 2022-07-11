using VChatServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

List<Room> RoomList = new List<Room>();

app.UseWebSockets(webSocketOptions);

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/room/list", () =>
{
    List<RoomInfo> RoomInfoList = new List<RoomInfo>();
    if(RoomList.Count == 0)
        Results.NotFound(new { message = "Не найдено ни одной комнаты." });
    foreach (var room in RoomList)
    {
        RoomInfo roominfo = new RoomInfo(room.Id, room.Name, room.Count);
        RoomInfoList.Add(roominfo);
    }
    return RoomInfoList;
});

app.MapGet("/api/room/{id}", (string id) =>
{
    Room? room = RoomList.FirstOrDefault(u => u.Id == id);
    List<ClientInfo> ClientInfoList = new List<ClientInfo>();

    if (room == null)
        Results.NotFound(new { message = "Êîìíàòà íå íàéäåíà." });
    else
    {
        for (int i = 0; i < room.Count; i++)
        {
            ClientInfo clientinfo = new ClientInfo(room[i].Id, room[i].Name);
            ClientInfoList.Add(clientinfo);
        }
    }

    return ClientInfoList;
});

app.MapPost("/api/room/new", async (HttpContext context) => 
{
    string roomName = "";

    using (var reader = new StreamReader(context.Request.Body))
    {
        roomName = await reader.ReadToEndAsync();
    }
    Room room = new Room(Guid.NewGuid().ToString(), roomName);
    RoomList.Add(room);
});

app.MapGet("/ws/{roomId}", async (string roomId, HttpContext context) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var currentRoom = RoomList.FirstOrDefault(r => r.Id == roomId);
        if (currentRoom == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        Client client = new Client();
        client.WebSocket = webSocket;
        client.Id = Guid.NewGuid().ToString();
        client.Name = Guid.NewGuid().ToString();
        currentRoom.Add(client);
        Console.WriteLine(currentRoom.Count());

        await Receive(client, currentRoom, RoomList);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

static async Task Receive(Client client, Room currentRoom, List<Room> roomList)
{
    var buffer = new byte[1024 * 4];
    var receiveResult = await client.WebSocket.ReceiveAsync(
        new ArraySegment<byte>(buffer), CancellationToken.None);
    var package = new Package();
    package.MessageInformation = receiveResult;
    package.Message = buffer;

    while (!receiveResult.CloseStatus.HasValue)
    {
        await Send(client, currentRoom, package);
        receiveResult = await client.WebSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    currentRoom.Remove(client);

    await client.WebSocket.CloseAsync(
        receiveResult.CloseStatus.Value,
        receiveResult.CloseStatusDescription,
        CancellationToken.None);

    if (currentRoom.Count == 0)
    {
        roomList.Remove(currentRoom);
    }
}


static async Task Send(Client client, Room currentRoom, Package package)
{
    foreach (var c in currentRoom)
    {
        if (c != client)
        {
            await c.WebSocket.SendAsync(
                new ArraySegment<byte>(package.Message, 0, package.MessageInformation.Count),
                package.MessageInformation.MessageType,
                package.MessageInformation.EndOfMessage,
                CancellationToken.None);

            await client.WebSocket.SendAsync(
                new ArraySegment<byte>(package.Message, 0, package.MessageInformation.Count),
                package.MessageInformation.MessageType,
                package.MessageInformation.EndOfMessage,
                CancellationToken.None);
        }
    }
}

app.Run();