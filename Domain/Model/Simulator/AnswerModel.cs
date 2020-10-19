namespace Domain.Model.Simulator
{
    public class AnswerModel
    {
        public string SqlAnswer { get; set; }
        public SqlResultModel SqlResult { get; set; }
        public string Result { get; set; }
        public bool IsDone { get; set; }
    }
}
