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

app.MapGet("/api/roomlist", () =>
{
    if(RoomList.Count == 0)
        Results.NotFound(new { message = "Не найдено ни одной комнаты." });
    return RoomList;
});

app.MapPost("/api/newroom", () => 
{
    Room room = new Room();
    room.Id = Guid.NewGuid().ToString();
    RoomList.Add(room);
});

//Дополнительно нужно через  WebSocket реализовать проверку на выход последнего человека из списка.
app.MapDelete("/api/del_empty_room/{id}", (string id) =>
{
    Room? room = RoomList.FirstOrDefault(u => u.Id == id);

    if (room == null) return Results.NotFound(new { message = "Комната не найдена." });

    if (room.Count > 0) return Results.NotFound(new { message = "Комната не пустая." });

    RoomList.Remove(room);
    room = null;
    return Results.Ok(new { message = "Комната удалена." });
});

app.MapControllers();

app.Run();