using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;
// Classe Operations
public class Operations : EntityBase<PridContext>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Title { get; set; }

    [Required]
    public double Amount { get; set; }

    [Required]
    public DateTime OperationDate { get; set; }

    [Required]
    [ForeignKey(nameof(Creator))]
    public int InitiatorId { get; set; } // Ajusté le nom pour correspondre à la convention
    public virtual User Creator { get; set; }

    [Required]
    [ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricounts Tricount { get; set; }

    public virtual ICollection<Repartitions> Repartitions { get; set; } = new HashSet<Repartitions>();


    public override bool Validate() {
        ClearErrors();

        if (string.IsNullOrEmpty(Title)) {
            AddError(nameof(Title), "Title is required");
        } else if (Context.Operations.Any(o => o.Title == Title)) {
            AddError(nameof(Title), "Title already exists");
        }
        return !HasErrors;
    }

    public void Delete() {
        var repar = Context.Repartitions.Where(r => r.OperationId == this.Id).ToList();

        foreach (var item in repar) {
            Context.Repartitions.Remove(item);
        }

        Context.Operations.Remove(this);
        Context.SaveChanges();
    }

}





