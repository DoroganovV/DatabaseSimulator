using Domain.Sql;

namespace Domain.Model
{
    public class PersonModel
    {
        public PersonModel() { }
        public PersonModel(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            MiddleName = person.MiddleName;
            GroupName = person.Group.Name;
            IsAdmin = person.Group.IsAdmin;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string GroupName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
