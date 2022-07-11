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

    for (int i = 0; i < RoomList.Count; i++)
    {
        RoomInfo roominfo = new RoomInfo(RoomList[i].Id, RoomList[i].Name, RoomList[i].Count);
        RoomInfoList.Add(roominfo);
    }

    if (RoomInfoList.Count == 0)
        Results.NotFound(new { message = "�� ������� �� ����� �������." });

    return RoomInfoList;
});

app.MapGet("/api/room/{id}/clients", (string id) =>
{
    Room? room = RoomList.FirstOrDefault(u => u.Id == id);
    List<ClientInfo> ClientInfoList = new List<ClientInfo>();

    if (room == null)
        Results.NotFound(new { message = "������� �� �������." });
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

app.MapPost("/api/room/new/{nameroom}", (string nameroom) => 
{
    Room room = new Room(Guid.NewGuid().ToString(), nameroom);
    RoomList.Add(room);
});

/*
//������������� ����� �����  WebSocket ����������� �������� �� ����� ���������� �������� �� ������.
app.MapDelete("/api/room/{id}/delete", (string id) =>
{
    Room? room = RoomList.FirstOrDefault(u => u.Id == id);

    if (room == null) return Results.NotFound(new { message = "������� �� �������." });

    if (room.Count > 0) return Results.NotFound(new { message = "������� �� ������." });

    RoomList.Remove(room);
    room = null;
    return Results.Ok(new { message = "������� �������." });
});
*/

app.MapControllers();

app.Run();