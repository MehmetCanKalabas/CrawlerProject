using System.ComponentModel.DataAnnotations;

namespace CrawlerProject.Models.Entity
{
    public class BASE_ENTITY
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDelete { get; set; }
        [MaxLength(100)]
        public DateTimeOffset? CreatedDate { get; set; }
        [MaxLength(100)]
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
