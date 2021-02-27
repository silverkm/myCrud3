using System.ComponentModel.DataAnnotations;

namespace myCrud3
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string Name { get; set; }
        public string EmailAddress { get; set; }

    }
}