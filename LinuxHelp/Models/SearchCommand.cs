
using System.ComponentModel.DataAnnotations;

namespace LinuxHelp.Models
{
    public class SearchCommand
    {
        [Required(ErrorMessage = "Пожалуйста, введите команду")]
        public string Command { get; set;}
    }
}
