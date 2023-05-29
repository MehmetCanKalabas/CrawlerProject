namespace CrawlerProject.Models.Entity
{
    public class PLAYER : BASE_ENTITY
    {
        public string PlayerName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public Guid? TeamId { get; set; }
        public virtual TEAM Team { get; set; }
    }
}
