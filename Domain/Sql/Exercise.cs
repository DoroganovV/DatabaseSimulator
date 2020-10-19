using System.Collections.Generic;

namespace Domain.Sql
{
    public class Exercise : Base
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string CorrectSql { get; set; }
        public byte TaskLevel { get; set; }

        public int DataBaseId { get; set; }
        public DataBase DataBase { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
