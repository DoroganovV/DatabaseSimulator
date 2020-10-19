using System;

namespace Domain.Sql
{
    public class Answer : Base
    {
        public DateTime Date { get; set; }
        public string SqlAnswer { get; set; }
        public bool? IsCorrectAnswer { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
    }
}
