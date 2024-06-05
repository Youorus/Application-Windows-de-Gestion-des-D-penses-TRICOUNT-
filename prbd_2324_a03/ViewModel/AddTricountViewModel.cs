using prbd_2324_a03.Model;
using PRBD_Framework;
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

        public ObservableCollectionFast<ParticipantsListViewModel> ParticipantsUsers { get; set; } = new ObservableCollectionFast<ParticipantsListViewModel>();

        private bool _isVisibleOperationTricount;
        public bool IsVisibleOperationTricount {
            get => _isVisibleOperationTricount;
            set => SetProperty(ref _isVisibleOperationTricount, value);
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand AddAllUserCommand { get; set; }
        public ICommand SaveTricountCommand { get; set; }
        public ICommand CancelTricountCommand { get; set; }
        public ICommand RemoveParticipant { get; set; }
        public ICommand AddMySelfCommand { get; set; }

        public bool IsVisibleEditTricount { get; set; } = false;

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

        private bool _isCreator;
        public bool IsCreator {
            get => _isCreator;
            set => SetProperty(ref _isCreator, value);
        }

        private ObservableCollectionFast<User> _usersParticipants;
        public ObservableCollectionFast<User> Participants {
            get => _usersParticipants;
            set => SetProperty(ref _usersParticipants, value);
        }

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        public bool IsSavedAndValid => !IsNew && !HasChanges;
        public override bool MayLeave => IsSavedAndValid;

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
            Tricount.Validate();
            AddErrors(Tricount.Errors);
            return !HasErrors;
        }

        public void UsersList() {
            var usersList = Context.Users.Where(user => user.UserId != _userId).ToList();
            Users = new ObservableCollectionFast<User>(usersList ?? new List<User>());
        }

        public void AllUsersList() {
            var allUsersList = Context.Users.ToList();
            AllUsers = new ObservableCollectionFast<User>(allUsersList ?? new List<User>());
        }

        private void AddAction() {
            var selectItem = new ParticipantsListViewModel(SelectedParticipant, Tricount.Id);
            if (SelectedParticipant != null && !ParticipantsUsers.Contains(selectItem)) {
                ParticipantsUsers.Add(selectItem);
                Users.Remove(SelectedParticipant);
            }
        }

        public override void SaveAction() {
            if (IsNew) {
                var tricount = new Tricounts {
                    Title = Title,
                    Description = string.IsNullOrEmpty(Description) ? "Description Vide" : Description,
                    Created_at = CreationDate,
                    Creator = _userId
                };

                Context.Tricounts.Add(tricount);
                Context.SaveChanges();

                foreach (var participantVm in ParticipantsUsers) {
                    var subscription = new Subscriptions {
                        TricountId = tricount.Id,
                        UserId = participantVm.User.UserId
                    };
                    Context.Subscriptions.Add(subscription);
                }

                Context.SaveChanges();
                IsNew = false;
                NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
                NotifyColleagues(App.Messages.MSG_CANCEL_TRICOUNT, Tricount);
            }
        }

        private void CancelTricount() {
            if (IsNew) {
                IsNew = false;
                NotifyColleagues(App.Messages.MSG_CANCEL_TRICOUNT, Tricount);
            } else {
                Tricount.Reload();
                RaisePropertyChanged();
            }
        }

        private void AddAllUsersAction() {
            var otherUsersCopy = new List<User>(Users);
            foreach (var user in otherUsersCopy) {
                if (!ParticipantsUsers.Contains(new ParticipantsListViewModel(user, Tricount.Id))) {
                    var vm = new ParticipantsListViewModel(user, Tricount.Id);
                    ParticipantsUsers.Add(vm);
                }
            }
            Users.Clear();
        }

        private void AddMySelfAction() {
            var user = Context.Users.FirstOrDefault(u => u.UserId == _userId);
            if (user != null && !ParticipantsUsers.Any(p => p.User.UserId == user.UserId)) {
                var participantVm = new ParticipantsListViewModel(user, Tricount.Id);
                ParticipantsUsers.Add(participantVm);
                Users.Remove(user);
            }
        }

        private bool CanCancelAction() {
            return Tricount != null && (IsNew || Tricount.IsModified);
        }

        private void RemoveParticipantAction(User participantToRemove) {
            if (participantToRemove != null) {
                Participants.Remove(participantToRemove);
            }
        }

        public AddTricountViewModel(Tricounts tricount, bool isNew) {
            Tricount = tricount;
            IsNew = isNew;
            RaisePropertyChanged();
            OnRefreshData();
        }

        private bool CanSaveAction() {
            if (IsNew)
                return !string.IsNullOrEmpty(Title);
            return Tricount != null && Tricount.IsModified;
        }

        protected override void OnRefreshData() {
            UsersList();

            if (IsNew) {
                var user = Context.Users.FirstOrDefault(u => u.UserId == _userId);
                var vm = new ParticipantsListViewModel(user, Tricount.Id);
                ParticipantsUsers.Add(vm);
            } else {
                var participants = Context.Subscriptions
                    .Where(s => s.TricountId == Tricount.Id)
                    .Select(s => s.User)
                    .ToList();

                foreach (var item in participants) {
                    var vm = new ParticipantsListViewModel(item, Tricount.Id);
                    ParticipantsUsers.Add(vm);
                }
            }

            AddUserCommand = new RelayCommand(AddAction, () => SelectedParticipant != null);
            AddAllUserCommand = new RelayCommand(AddAllUsersAction, () => Users.Any());
            SaveTricountCommand = new RelayCommand(SaveAction, CanSaveAction);
            AddMySelfCommand = new RelayCommand(AddMySelfAction, () => !ParticipantsUsers.Any(p => p.User.UserId == _userId));
            RemoveParticipant = new RelayCommand<User>(RemoveParticipantAction);
            CancelTricountCommand = new RelayCommand(CancelTricount, CanCancelAction);
        }
    }
}
