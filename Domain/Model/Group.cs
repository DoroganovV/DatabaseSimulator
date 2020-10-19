using Domain.Sql;

namespace Domain.Model
{
    public class GroupModel
    {
        public GroupModel() { }
        public GroupModel(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
