using System.Net.WebSockets;

namespace VChatServer
{
    public class Package
    {
        public WebSocketReceiveResult? MessageInformation { get; set; } = null;
        public byte[] Message { get; set; }
    }
}
