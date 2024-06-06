using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model
{
    public class Tricounts : EntityBase<PridContext>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        public DateTime Created_at { get; set; } = DateTime.Now;

        [Required, ForeignKey(nameof(CreatorTricount))]
        public int Creator { get; set; }

        public virtual User CreatorTricount { get; set; }

        public virtual ICollection<Subscriptions> Subscriptions { get; set; } = new HashSet<Subscriptions>();

        public virtual ICollection<Operations> Operations { get; set; } = new HashSet<Operations>();

        public Tricounts() { }

        public Tricounts(string title, string description, DateTime created_at, int  creator) {
            Title = title;
            Description = description;
            Created_at = created_at;
            Creator = creator;
        }

        public override bool Validate() {
            ClearErrors();

            if (string.IsNullOrEmpty(Title)) {
                AddError(nameof(Title), "Title is required");
            } else if (Context.Tricounts.Any(t => t.Title == Title)) {
                AddError(nameof(Title), "Title already exists");
            }

            return !HasErrors;
        }


        public void Delete() {

            var Subs = Context.Subscriptions.Where(s => s.TricountId == this.Id).ToList();

            foreach (var item in Subs) {
                Context.Subscriptions.Remove(item);
            }

            

            var Oper = Context.Operations.Where(o => o.TricountId == this.Id).ToList();

            foreach (var item in Oper) {
                var repar = Context.Repartitions.Where(r => r.OperationId == item.Id).ToList();

                foreach (var item1 in repar) {
                    Context.Repartitions.Remove(item1);
                }

                Context.Operations.Remove(item);
            }

            Context.Tricounts.Remove(this);
            Context.SaveChanges();
        }
    }
}
