using System.Net.WebSockets;

namespace VChatServer
{
    public class Client
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public WebSocket? WebSocket { get; set; } = null;

    }
}
