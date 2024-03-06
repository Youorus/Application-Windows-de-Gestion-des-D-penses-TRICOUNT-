using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;

   public class Tricounts : EntityBase<PridContext>{

    [Key]
    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public DateTime Created_at { get; set; } = DateTime.Now;
    [Required, ForeignKey(nameof(CreatorTricount))]
    public int Creator { get; set; }
    public virtual Users CreatorTricount { get; set; }

    public Tricounts() { }

    public Tricounts (string title, string description, DateTime created_at, int creator) {
        Title = title;
        Description = description;
        Created_at = created_at;
        Creator = creator;
    }

}