using Microsoft.EntityFrameworkCore;
using prbd_2324_a03.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel
{
    public class AddTricountViewModel : ViewModelCommon
    {
        private readonly int _userId = 1;

        private User _selectedParticipant;
        public User SelectedParticipant {
            get => _selectedParticipant;
            set => SetProperty(ref _selectedParticipant, value);
        }


        private bool _isVisibleOperationTricount;
        public bool IsVisibleOperationTricount {
            get => _isVisibleOperationTricount;
            set => SetProperty(ref _isVisibleOperationTricount, value);
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand AddAllUserCommand { get; set; }
        public ICommand SaveTricountCommand { get; set; }
        public ICommand AddMySelfCommand { get; set; }

        public bool IsVisibleEditTricount { get; set; } = false;
     



        private string _fullName;
        public string FullName {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private ObservableCollectionFast<User> _allUsers;
        public ObservableCollectionFast<User> AllUsers {
            get => _allUsers;
            set => SetProperty(ref _allUsers, value);
        }

        private ObservableCollectionFast<User> _otherUsers;
        public ObservableCollectionFast<User> Users {
            get => _otherUsers;
            set => SetProperty(ref _otherUsers, value);
        }

        private ObservableCollectionFast<User> _usersParticipants;
        public ObservableCollectionFast<User> Participants {
            get => _usersParticipants;
            set => SetProperty(ref _usersParticipants, value);
        }

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set {
                if (SetProperty(ref _tricount, value)) {
                    if (_tricount != null) {
                        LoadParticipants();
                    }
                }
            }
        }

        private bool _isNew;
        public bool IsNew {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        private string _title;
        public string Title {
            get => Tricount?.Title;
            set => SetProperty(Tricount.Title, value, Tricount, (t, v) => {
                t.Title = v;
                Validate();
                NotifyColleagues(App.Messages.MSG_TITLE_CHANGED, Tricount);
            });
        }

        private string _description;
        public string Description {
            get => Tricount?.Description;
            set => SetProperty(Tricount.Description, value, Tricount, (t, v) => {
                t.Description = v;
                Validate();
            });
        }

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate {
            get => (DateTime)(Tricount?.Created_at);
            set => SetProperty(Tricount.Created_at, value, Tricount, (t, v) => {
                t.Created_at = v;
                Validate();
            });
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

        private void LoadParticipants() {
            var participants = Context.Subscriptions
                .Where(s => s.TricountId == Tricount.Id)
                .Select(s => s.User)
                .ToList();

            Participants = new ObservableCollectionFast<User>(participants);
        }

        public void UserName() {
            var user = Context.Users.FirstOrDefault(u => u.UserId == _userId);

            if (user != null) {
                FullName = user.Full_name;
            }
        }

        public void UsersList() {
            var usersList = Context.Users.Where(user => user.UserId != _userId).ToList();

            if (usersList != null) {
                Users = new ObservableCollectionFast<User>(usersList);
            } else {
                Users = new ObservableCollectionFast<User>();
            }
        }

        public void AllUsersList() {
            var allUsersList = Context.Users.ToList();

            if (allUsersList != null) {
                AllUsers = new ObservableCollectionFast<User>(allUsersList);
            } else {
                AllUsers = new ObservableCollectionFast<User>();
            }
        }

        public void UsersParticipantDefault() {
            if (Participants == null) {
                Participants = new ObservableCollectionFast<User>();
            }

            var userDefault = Context.Users.FirstOrDefault(user => user.UserId == _userId);
            if (userDefault != null) {
                userDefault.IsDefault = true; // Marquer l'utilisateur par défaut
                Participants.Add(userDefault);
            }
        }

        private void AddAction() {
            if (SelectedParticipant != null && !Participants.Contains(SelectedParticipant)) {
                Participants.Add(SelectedParticipant);
                Users.Remove(SelectedParticipant);
            }
        }

        public override void SaveAction() {
            if (IsNew) {
                // Créer un nouveau Tricount avec les données fournies
                var tricount = new Tricounts {
                    Title = Title,
                    Description = string.IsNullOrEmpty(Description) ? "Description Vide" : Description,
                    Created_at = CreationDate,
                    Creator = _userId
                };

                // Ajouter le Tricount au contexte
                Context.Tricounts.Add(tricount);
                Context.SaveChanges();

                // Ajouter toutes les souscriptions associées au nouveau Tricount
                foreach (var user in Participants) {
                    var subscription = new Subscriptions {
                        TricountId = tricount.Id,
                        UserId = user.UserId
                    };
                    Context.Subscriptions.Add(subscription);
                }

                // Sauvegarder tous les changements dans la base de données
                Context.SaveChanges();

                // Mettre à jour le statut IsNew et notifier les collègues
                IsNew = false;
                NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
            }
        }

        private void AddAllUsersAction() {
            // Créer une copie de la liste des autres utilisateurs
            var otherUsersCopy = new List<User>(Users);

            // Ajouter chaque utilisateur de la copie à la liste des participants
            foreach (var user in otherUsersCopy) {
                if (!Participants.Contains(user)) {
                    Participants.Add(user);
                }
            }

            // Effacer la liste des autres utilisateurs
            Users.Clear();
        }

        private void AddMySelfAction() {
            var user = Context.Users.FirstOrDefault(u => u.UserId == _userId);
            if (user != null && !Participants.Contains(user)) {
                Participants.Add(user);
                Users.Remove(user);
            }
        }

        public AddTricountViewModel(Tricounts tricount, bool isNew) {
            Tricount = tricount;
            IsNew = isNew;
            OnRefreshData();
        }

        protected override void OnRefreshData() {
            UserName();
            UsersList();
            UsersParticipantDefault();

            AddUserCommand = new RelayCommand(AddAction, () => SelectedParticipant != null);
            AddAllUserCommand = new RelayCommand(AddAllUsersAction, () => Users.Count() != 0);
            SaveTricountCommand = new RelayCommand(SaveAction);
            AddMySelfCommand = new RelayCommand(AddMySelfAction, () => !Participants.Contains(Context.Users.FirstOrDefault(u => u.UserId == _userId)));
        }
    }
}
