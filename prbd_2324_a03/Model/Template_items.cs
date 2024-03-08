using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;
public class Template_items: EntityBase<PridContext>
{

    [Required]
    [ForeignKey(nameof(Templates))]
    public int TemplateId { get; set; }
    public virtual Templates Templates { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [Required]
    public int Weight { get; set; }


    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();


}
