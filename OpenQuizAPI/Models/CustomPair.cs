using System;
using System.ComponentModel.DataAnnotations;


namespace OpenQuizAPI.Models
{
    [Serializable]
    public class CustomPair
    {

        [Key]

        public String type { get; set; }
        public String var { get; set; }



    }
}
