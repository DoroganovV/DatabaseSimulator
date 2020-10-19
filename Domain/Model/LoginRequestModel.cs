namespace Domain.Model
{
    public class LoginRequestModel
    {
        public int GroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
    }
}
