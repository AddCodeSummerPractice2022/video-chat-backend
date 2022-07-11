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
    if(RoomList.Count == 0)
        Results.NotFound(new { message = "�� ������� �� ����� �������." });
    return RoomList;
});

app.MapGet("/api/room/{id}/clients", (string id) =>
{
    Room? room = RoomList.FirstOrDefault(u => u.Id == id);

    if (room == null)
        Results.NotFound(new { message = "������� �� �������." });

    return room;
});

app.MapPost("/api/room/new/{nameroom}", (string nameroom) => 
{
    Room room = new Room();
    room.Name = nameroom;
    room.Id = Guid.NewGuid().ToString();
    RoomList.Add(room);
});

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

app.MapControllers();

app.Run();