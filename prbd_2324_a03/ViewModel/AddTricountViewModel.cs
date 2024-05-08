using prbd_2324_a03.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.ViewModel
{
    public class AddTricountViewModel : ViewModelCommon {
        private readonly int _userId = 1;

        private string _fullName;
        public string FullName {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        private bool _isNew;
        public bool IsNew {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        private string _title;
        public string Title {
            get => _title;
            set => SetProperty(ref _title, value, () => Validate());
        }

        private string _description;
        public string Description {
            get => _description;
            set => SetProperty(ref _description, value, () => Validate());
        }

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate {
            get => _creationDate;
            set => SetProperty(ref _creationDate, value, () => Validate());
        }

        public override bool Validate() {
            ClearErrors();

            if (string.IsNullOrEmpty(Title)) {
                AddError(nameof(Title), "Title is required");
            } else if (Context.Tricounts.Any(t => t.Creator == _userId && t.Title == Title)) {
                AddError(nameof(Title), "Title already exists");
            }

            if (CreationDate > DateTime.Now) {
                AddError(nameof(CreationDate), "The Date cannot be in the future");
            }

            return !HasErrors;
        }

        public void UserName() {
            var user = Context.Users.FirstOrDefault(u => u.UserId == _userId);

            if (user != null) {
                FullName = user.Full_name;
            }
        }

        public AddTricountViewModel(Tricounts tricount, bool isNew) {
            Tricount = tricount;
            IsNew = isNew;
            UserName();
        }

        protected override void OnRefreshData() {

            // Implementer la logique de rafraîchissement des données si nécessaire
        }

    }

}
