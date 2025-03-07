using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class NoteLabel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("noteid")]
        public int NoteId { get; set; }
        public Note note { get; set; }
        [ForeignKey("labelid")]
        public int LabelId { get; set; }
        public Label label {get;set;}
    }
}
