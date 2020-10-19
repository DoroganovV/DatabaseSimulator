using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Base
    {
        [Key]
        public int Id { get; set; }
    }
}
