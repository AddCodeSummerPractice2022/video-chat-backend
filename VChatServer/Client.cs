using System.Net.WebSockets;

namespace VChatServer

{
    public class Client
    {
        public WebSocket WebSocket { get; set; }
        public WebSocketReceiveResult MessageInformation { get; set; }
        public byte[] Message { get; set; }
    }
}
