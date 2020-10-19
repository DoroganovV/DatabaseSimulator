using Domain.Sql;

namespace Domain.Model
{
    public class DataBaseModel : Base
    {
        public DataBaseModel() { }
        public DataBaseModel(DataBase dataBase)
        {
            Id = dataBase.Id;
            Name = dataBase.Name;
            Scheme = dataBase.Scheme;
        }
        public string Name { get; set; }
        public byte[] Scheme { get; set; }
    }
}