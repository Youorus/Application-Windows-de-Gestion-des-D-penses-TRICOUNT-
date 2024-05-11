using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;

public class Subscriptions : EntityBase<PridContext>
{
   
    [Required]
    [ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricounts Tricount { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; }


 
}
