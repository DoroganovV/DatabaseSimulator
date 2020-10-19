using Domain.Sql;

namespace Domain.Model
{
    public class ExerciseModel : Base
    {
        public ExerciseModel(Exercise exercise)
        {
            Id = exercise.Id;
            Name = exercise.Name;
            Text = exercise.Text;
            TaskLevel = exercise.TaskLevel;
            DataBaseId = exercise.DataBaseId;
        }
        public string Name { get; set; }
        public string Text { get; set; }
        public byte TaskLevel { get; set; }
        public string TaskLevelStar => (new string('★', TaskLevel) + new string('☆', 5 - TaskLevel));
        public int DataBaseId { get; set; }
    }
}