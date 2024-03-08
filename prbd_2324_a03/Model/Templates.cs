using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;
public class Templates : EntityBase<PridContext>
{

    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricounts Tricount { get; set; }

    public virtual ICollection<Template_items> Template_Items { get; set; } = new HashSet<Template_items>();
}