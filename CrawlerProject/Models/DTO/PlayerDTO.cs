namespace CrawlerProject.Models.DTO
{
    public class AddPlayer
    {
        public string PlayerName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public Guid? TeamId { get; set; }
    }
    public class GetPlayer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public GetTeamName TeamInfo { get; set; }

    }
    public class GetTeamName
    {
        public string TeamName { get; set; }
        public Guid TeamId { get; set; }
    }
    public class GetPlayerName
    {
        public string PlayerName { get; set; }
    }
    public class UpdatePlayer
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public Guid TeamId { get; set; }

    }
    public class UpdatePlayerTeams
    {
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
    }
}
