using System.ComponentModel.DataAnnotations;

namespace RScript.Models
{
    public class GenerateReportModel
    {
        [Required]
        public int ModelId { get; set; }

        [Required]
        public string Server { get; set; }

        [Required]
        public string Database { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}