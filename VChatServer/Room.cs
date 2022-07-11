namespace VChatServer
{
    public class Room : List<Client>
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";

        public Room(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }

    public class RoomInfo
    {
        public string Id = "";
        public string Name = "";
        public int NumberOfClients = 0;

        public RoomInfo(string Id, string Name, int NumberOfClients)
        {
            this.Id = Id;
            this.Name = Name;
            this.NumberOfClients = NumberOfClients;
        }
    }
}