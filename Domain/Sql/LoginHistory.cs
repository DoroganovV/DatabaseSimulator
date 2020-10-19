using System;

namespace Domain.Sql
{
    public class LoginHistory : Base
    {
        public DateTime DateLogin { get; set; }
        public string Host { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
