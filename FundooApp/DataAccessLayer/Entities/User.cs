using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
   public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [StringLength(50,ErrorMessage ="name cant be longer than 50 char")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8,ErrorMessage ="min 8 characters required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "city is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "phone is required")]
        [Phone(ErrorMessage ="invalid phone number")]
        public string phone { get; set; }
        
        public ICollection<Note> notes { get; set; }


       
    }
}
