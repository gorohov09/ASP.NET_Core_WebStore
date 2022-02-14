using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
