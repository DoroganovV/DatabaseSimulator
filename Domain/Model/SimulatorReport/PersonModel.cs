using Domain.Sql;
using System;

namespace Domain.Model.SimulatorReport
{
    public class PersonModel
    {
        public PersonModel(){}

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int Completed { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
