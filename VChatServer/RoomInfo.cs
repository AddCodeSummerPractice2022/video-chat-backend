namespace VChatServer
{
    public class RoomInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int NumberOfClients { get; set; }

        public RoomInfo(string Id, string Name, int NumberOfClients)
        {
            this.Id = Id;
            this.Name = Name;
            this.NumberOfClients = NumberOfClients;
        }
    }
}
