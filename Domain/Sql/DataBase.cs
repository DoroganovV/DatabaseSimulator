using System.Collections.Generic;

namespace Domain.Sql
{
    public class DataBase : Base
    {
        public string Name { get; set; }
        public string ConnectingString { get; set; }
        public byte[] Scheme { get; set; }

        public ICollection<Exercise> Exercises { get; set; }
    }
}
