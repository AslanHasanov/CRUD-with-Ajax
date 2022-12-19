using System.ComponentModel.DataAnnotations;

namespace DemoApplication.Areas.Admin.ViewModels.Author
{
    public class AddViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }

        public AddViewModel()
        {

        }

        public AddViewModel( string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
    }
}
