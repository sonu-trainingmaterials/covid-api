using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    [Table("Feedbacks")]
    public class FeedbackData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required, StringLength(50)]
        public string Email { get; set; }

        [Required, StringLength(1000)]
        public string Feedback { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SubmitedDate { get; set; }
    }
}
