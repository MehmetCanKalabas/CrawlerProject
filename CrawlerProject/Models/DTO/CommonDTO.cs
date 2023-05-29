namespace CrawlerProject.Models.DTO
{
    public class ObjectItem
    {
        public Guid Id { get; set; }
    }

    public class PlayerViewModel
    {
        public List<Team> Teams { get; set; }
        public List<GetPlayer> Players { get; set; }
    }
    
}
