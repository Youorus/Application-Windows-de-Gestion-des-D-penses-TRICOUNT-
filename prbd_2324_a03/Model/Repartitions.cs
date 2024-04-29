using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;
public class Repartitions : EntityBase<PridContext>
{
   
    [Required]
    [ForeignKey(nameof(Operations))]
    public int OperationId { get; set; }
    public virtual Operations Operations { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [Required]
    public int Weight { get; set; }


    

}
