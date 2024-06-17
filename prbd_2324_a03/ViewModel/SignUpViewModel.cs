using prbd_2324_a03.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel
{
    public class SignUpViewModel : ViewModelCommon
    {
        public ICommand SignUpCommand { get; set; }


        private string _pseudo;

        public string Pseudo {
            get => _pseudo;
            set => SetProperty(ref _pseudo, value, () => Validate());
        }

     
        private string _email;

        public string Email {
            get => _email;
            set => SetProperty(ref _email, value, () => Validate());
        }

        private string _password;

        public string Password {
            get => _password;
            set => SetProperty(ref _password, value, () => Validate());
        }

        private string _passwordConfirm;

        public string PasswordConfirm {
            get => _passwordConfirm;
            set => SetProperty(ref _passwordConfirm, value, () => Validate());
        }


        public override bool Validate() {
            ClearErrors();

            var emailExiste = Context.Users.Any(u => u.Mail == Email);

            // Validate Email
            if (string.IsNullOrEmpty(Email)) {
                AddError(nameof(Email), "Email is required");
            } else if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) {
                AddError(nameof(Email), "Invalid email format");
            } else if (emailExiste) {
                AddError(nameof(Email), "Email already exists, please sign in");
            }

            // Validate Pseudo
            if (string.IsNullOrEmpty(Pseudo)) {
                AddError(nameof(Pseudo), "Pseudo is required");
            } else if (Pseudo.Length < 3) {
                AddError(nameof(Pseudo), "Pseudo must be >= 3 characters");
            }


            // Validate Password
            if (string.IsNullOrEmpty(Password)) {
                AddError(nameof(Password), "Password is required");
            } else if (Password.Length < 8) {
                AddError(nameof(Password), "Password must be >= 8 characters");
            } else if (!Regex.IsMatch(Password, @"[A-Z]")) {
                AddError(nameof(Password), "Password must contain at least one uppercase letter");
            } else if (!Regex.IsMatch(Password, @"[0-9]")) {
                AddError(nameof(Password), "Password must contain at least one digit");
            } else if (!Regex.IsMatch(Password, @"[\W_]")) {
                AddError(nameof(Password), "Password must contain at least one special character");
            }


            // Validate PasswordConfirm
            if (string.IsNullOrEmpty(PasswordConfirm)) {
                AddError(nameof(PasswordConfirm), "Password confirmation is required");
            } else if (Password != PasswordConfirm) {
                AddError(nameof(PasswordConfirm), "Passwords do not match");
            }

            return !HasErrors;
        }

        public SignUpViewModel() {
            SignUpCommand = new RelayCommand(SignUpAction, () => _email != null && _password != null && _pseudo != null && _passwordConfirm != null && !HasErrors);
        }


        public void SignUpAction() {
            if (Validate()) {
                var newUser = new User {
                    Full_name = Pseudo,
                    Mail = Email,
                    Password = SecretHasher.Hash(Password) // Assurez-vous que `SecretHasher.Hash` est une méthode valide
                };

                Context.Users.Add(newUser);
                Context.SaveChanges();

                // Notify the application or user about the successful sign-up
                NotifyColleagues(App.Messages.MSG_LOGIN, newUser);
            }
        }


    }
}
