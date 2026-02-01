using System.ComponentModel.DataAnnotations;

namespace QuizManagement.Models
{
    public class QuestionLevelModel
    {
        public int? QuestionLevelID {  get; set; }

        [Required(ErrorMessage ="Question Level is Required")]
        [StringLength(100)]
        public string QuestionLevel {  get; set; }

        [Required]
        public string UserID {  get; set; }
        public DateTime Created {  get; set; }
        public DateTime Modified {  get; set; }
    }
    public class QuestionLevelDropDown
    {
        public int QuestionLevelID { get; set; }
        public string QuestionLevel { get; set; }
    }
}
