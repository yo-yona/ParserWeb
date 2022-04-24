using System.ComponentModel.DataAnnotations;

namespace ParserWeb.Models
{
    public class SiteContent
    {
        public int Id { get; set; }
        [Required]
        public int CashId { get; set; }
        public virtual Cash Site { get; set; }
        [Required]
        public string Word { get; set; }
        [Required]
        public uint Count { get; set; }
    }
}
