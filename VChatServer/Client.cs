using System.Net.WebSockets;

namespace VChatServer
{
    public class Client
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public WebSocket? WebSocket { get; set; } = null;

    }

    public class ClientInfo
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public ClientInfo(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}
