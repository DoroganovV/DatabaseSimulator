using System.Collections.Generic;

namespace Domain.Sql
{
    public class Group : Base
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
