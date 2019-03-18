using System;

namespace OpenQuizAPI.Models.Core
{
    public class AppConfiguration
    {
        public static AppConfiguration Current;

        public AppConfiguration()
        {
            Current = this;
        }

        public string ConnectionString { get; set; }
    }
}
