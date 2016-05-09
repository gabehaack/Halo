using System.ComponentModel.DataAnnotations;

namespace GHaack.Halo.Web.Models
{
    public class HomeViewModel
    {
        [Required, Display(Name = "Gamertag")]
        public string Player { get; set; }
    }
}