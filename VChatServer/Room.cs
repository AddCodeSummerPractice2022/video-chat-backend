﻿namespace VChatServer
{
    public class Room : List<Client>
    {
        public string Id { get; set; } = "";
    }
}
