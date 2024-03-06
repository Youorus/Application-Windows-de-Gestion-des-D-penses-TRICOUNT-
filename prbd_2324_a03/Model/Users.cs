using PRBD_Framework;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a03.Model;

public enum Role
{
    Administrator = 1,
    User = 0
}

public class Users : EntityBase<PridContext> {
    [Key]
    public int UserId { get; set; }
    [Required]
    [MinLength(3)]
    public string Full_name { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Mail { get; set; }
    [Required]
    public Role Role { get; protected set; } = Role.User;
    public Users() { }

    public virtual ICollection<Tricounts> UserTricounts { get; set; } = new HashSet<Tricounts>();

    public Users (string mail,string full_name, string password) {
        Mail = mail;
        Full_name = full_name;
        Password = password;
        Role = Role.User;
    }
}