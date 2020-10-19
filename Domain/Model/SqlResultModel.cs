using System.Collections.Generic;

namespace Domain.Model
{
    public class SqlResultModel
    {
        public List<string> Columns { get; set; }
        public List<string[]> DataTable { get; set; }
        public bool HasException { get; set; }
        public string Exception { get; set; }
        public int CountRows => DataTable?.Count ?? 0;
        public int CountColumns => DataTable?[0].Length ?? 0;
    }
}