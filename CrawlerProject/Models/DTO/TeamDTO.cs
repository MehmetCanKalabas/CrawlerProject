namespace CrawlerProject.Models.DTO
{
    public class AddTeam
    {
        public string TeamName { get; set; }
        public string Logo { get; set; }
    }

    public class AddOrUpdateTeamModel
    {
        public Guid? Id { get; set; }
        public string TeamName { get; set; }
        public IFormFile Logo { get; set; }
    }

    public class DeleteTeamModel
    {
        public Guid Id { get; set; }
    }

    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
