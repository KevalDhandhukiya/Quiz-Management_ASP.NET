using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Models
{
    public class QuestionModel
    {
        public int? QuestionID { get; set; }

        [Required(ErrorMessage = "Question Name Required")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Select Question Level")]
        public int Level { get; set; }


        [Required(ErrorMessage = "Enter Option A")]

        public string OptA { get; set; }


        [Required(ErrorMessage = "Enter Option B")]

        public string OptB { get; set; }


        [Required(ErrorMessage = "Enter Option C")]

        public string OptC { get; set; }


        [Required(ErrorMessage = "Enter Option D")]

        public string OptD { get; set; }


        [Required(ErrorMessage = "Select Correct Option")]

        public string Correct { get; set; }

        [Required(ErrorMessage = "Marks Required")]

        public int Marks { get; set; }

        [Required(ErrorMessage = "Select User ID")]
        public int UserID { get; set; }

        public DateTime? Modified { get; set; }

        //public bool isActive { get; set; } = true;

    }
    public class QuestionDropdown
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
    }
}
