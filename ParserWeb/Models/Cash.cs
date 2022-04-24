using System.ComponentModel.DataAnnotations;

namespace ParserWeb.Models
{
    public class Cash
    {
        public int Id { get; set; }
        [Required]
        public string Site { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public virtual ICollection<Cash> SiteContent { get; set; }

    }
}
