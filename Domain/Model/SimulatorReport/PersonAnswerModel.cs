namespace Domain.Model.SimulatorReport
{
    public class PersonAnswerModel
    {
        public int Id { get; set; }
        public bool Result { get; set; }
        public string SqlAnswer { get; set; }
        public string SqlCorrectAnswer { get; set; }
        public SqlResultModel SqlResult { get; set; }
        public SqlResultModel SqlCorrectResult { get; set; }
    }
}
