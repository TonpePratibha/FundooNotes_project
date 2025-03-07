using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Entities
{
    public class Note

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(70, ErrorMessage = "Title can't be longer than 70 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string Color { get; set; } = "#FFFFFF"; 

        
        public int UserId { get; set; }

       
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }

        public bool Archive { get; set; }=false;
        public bool Trash { get; set; } = false;

        [JsonIgnore]
        public IEnumerable<NoteLabel> ?NoteLabels { get; set; }

    }
}
