using System;
using System.ComponentModel.DataAnnotations;
namespace QuizManagement.Models
{
    public class QuizModel
    {
  
        public int? QuizID { get; set; }

        [Required(ErrorMessage = "QuizName is Required")]
   
        public string QuizName { get; set; }

        [Required(ErrorMessage = "TotalQuestions is Required")]
        public string TotalQuestions { get; set; }

        [Required(ErrorMessage = "QuizDate is Required")]
        public DateTime QuizDate { get; set; }

        [Required]
        public string UserID { get; set; }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }

    public class QuizDropdown
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
    }
}

