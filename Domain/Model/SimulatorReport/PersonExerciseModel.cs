using System;

namespace Domain.Model.SimulatorReport
{
    public class PersonExerciseModel
    {
        public int Id { get; set; }
        public byte TaskLevel { get; set; }
        public string TaskLevelStar => (new string('★', TaskLevel) + new string('☆', 5 - TaskLevel));
        public int Attempts { get; set; }
        public DateTime LastAttempt { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
    }
}
