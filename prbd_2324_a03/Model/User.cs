using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_a03.Model;

public enum Role
{
    Administrator = 1,
    User = 0
}

public class User : EntityBase<PridContext> {
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
    public Role Role { get; set; } = Role.User;
    public User() { }

    public virtual ICollection<Tricounts> CreatedTricounts { get; set; } = new HashSet<Tricounts>();
    public virtual ICollection<Subscriptions> Subscriptions { get; set; } = new HashSet<Subscriptions>();

    public virtual ICollection<Operations> CreatedOperations { get; set; } = new HashSet<Operations>();

    public virtual ICollection<Repartitions> Repartitions { get; set; } = new HashSet<Repartitions>();
    public User (string mail,string full_name, string password) {
        Mail = mail;
        Full_name = full_name;
        Password = password;
        Role = Role.User;
    }
}