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
}