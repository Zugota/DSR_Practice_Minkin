using System.ComponentModel.DataAnnotations;

namespace DSR_Practice_Debts.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string? Role { get; set; }
    }
}
