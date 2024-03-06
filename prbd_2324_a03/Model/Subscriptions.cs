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
    [Required, ForeignKey(nameof(TricountId))]
    public int Tricounts { get; set; }
    [Required, ForeignKey(nameof(UsersId))]
    public int Users { get; set;}

    public virtual Tricounts TricountId { get; set; }

    public virtual Users UsersId { get; set;}


    
}
