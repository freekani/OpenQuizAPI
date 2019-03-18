using System;
using System.ComponentModel.DataAnnotations;


namespace OpenQuizAPI.Models
{
    [Serializable]
    public class Quiz4 : Quiz4Post
    {

        [Key]

        public int QuizId { get; set; }


    }
}
