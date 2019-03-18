using System;
using System.ComponentModel.DataAnnotations;


namespace OpenQuizAPI.Models
{
    [Serializable]
    public class Quiz4Post : QuizInfo
    {
        [Key]
        public string Question { get; set; }
        public string A0 { get; set; }

        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public int Answer { get; set; }

    }
}
