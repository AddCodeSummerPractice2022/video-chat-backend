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
        Results.NotFound(new { message = "Не найдено ни одной комнаты." });
    return RoomList;
});

//Сделать возврат списка подключённых к комнате пользователей, (Id пользователя, имя пользователя)
//Запрос должен приходить со страницы с комнатой.
app.MapGet("/api/room/{id}/clients", (string id) =>
{
    Room? room = RoomList.FirstOrDefault(u => u.Id == id);

    if (room == null)
        Results.NotFound(new { message = "Комната не найдена." });

    return room;
});

//Как в поле CurrentRoomId пользователя установить Id созданной комнаты?
app.MapPost("/api/room/new/{nameroom}", (string nameroom) => 
{
    Room room = new Room();
    room.Name = nameroom;
    room.Id = Guid.NewGuid().ToString();
    RoomList.Add(room);
});

//Дополнительно нужно через  WebSocket реализовать проверку на выход последнего человека из списка.
app.MapDelete("/api/room/{id}/delete", (string id) =>
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