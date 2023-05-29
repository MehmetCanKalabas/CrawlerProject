namespace CrawlerProject.Models.Entity
{
    public class TEAM : BASE_ENTITY
    {
        public string TeamName { get; set; }
        public string Logo { get; set; }
        public virtual List<PLAYER> Players { get; set; }
    }
}
