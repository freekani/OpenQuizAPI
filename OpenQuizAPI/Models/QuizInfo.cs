using System;
using System.ComponentModel.DataAnnotations;

namespace OpenQuizAPI.Models
{
    [Serializable]
    public class QuizInfo

    {
        [Key]

        public string Genre { get; set; }
        public string Presenter { get; set; }
    }
}
