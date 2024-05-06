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

        [NotMapped]
        public DateTime LastOperationDate { get; set; }

        [NotMapped]
        public int CountUser { get; set; }

        [NotMapped]
        public int CountOperation { get; set; }

        [NotMapped]
        public double ExpenseTricount { get; set; }

        [NotMapped]
        public double MyExpenseTricount { get; set; }

        [NotMapped]
        public double MyBalanceTricount { get; set; }

        [NotMapped]
        public Dictionary<int, double> Balances { get; set; } = new Dictionary<int, double>();

        public Tricounts() { }

        public Tricounts(string title, string description, DateTime created_at) {
            Title = title;
            Description = description;
            Created_at = created_at;
        }
    }
}
