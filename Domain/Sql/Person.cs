namespace Domain.Sql
{
    public class Person : Base
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Password { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
