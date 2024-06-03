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


            if (Created_at > DateTime.Now) {
                AddError(nameof(Created_at), "The Date cannot be in the future");
            }

            return !HasErrors;
        }


        public void Delete() {
            // Supprime les relations 
            Subscriptions.Clear();
            Operations.Clear();
            
            // Supprime le Tricount lui-même
            Context.Tricounts.Remove(this);
            Context.SaveChanges();
        }
    }
}
