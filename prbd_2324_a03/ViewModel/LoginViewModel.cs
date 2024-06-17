using prbd_2324_a03.Model;
using PRBD_Framework;
using System.Windows;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel;

public class LoginViewModel : ViewModelCommon
{
    public ICommand LoginCommand { get; set; }

    public ICommand LoginBenoit { get; set; }

    public ICommand SignUpCommand { get; set; }
    public ICommand LoginBoris { get; set; }
    public ICommand LoginXavier { get; set; }

    public ICommand LoginMarc { get; set; }
    public ICommand LoginAdmin { get; set; }

  

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

    public override bool Validate() {
        ClearErrors();

        var user = Context.Users.FirstOrDefault(u => u.Mail == Email);

        if (string.IsNullOrEmpty(Email)) {
            AddError(nameof(Email), "Email is required");
        } else {
            if (user == null) {
                AddError(nameof(Email), "Email does not exist, please sign up");
            } else {
                if (string.IsNullOrEmpty(Password)) {
                    AddError(nameof(Password), "Password is required");
                } else if (!SecretHasher.Verify(Password, user.Password)) {
                    AddError(nameof(Password), "Wrong password");
                }
            }
        }

        return !HasErrors;
    }

    public LoginViewModel() {
        LoginCommand = new RelayCommand(LoginAction,
            () => _email != null && _password != null && !HasErrors);

        LoginBoris = new RelayCommand(Log1);
        LoginBenoit = new RelayCommand(Log2);
        LoginXavier = new RelayCommand(Log3);
        LoginMarc = new RelayCommand(Log4);
   
   LoginAdmin = new RelayCommand(Log5);

        SignUpCommand = new RelayCommand(SignUp);

    }

    private void LoginAction() {
        if (Validate()) {
            var user = Context.Users.FirstOrDefault(u => u.Mail == Email);
            NotifyColleagues(App.Messages.MSG_LOGIN, user);
        }
    }



    protected override void OnRefreshData() {
        
    }

    private void SignUp() {
        NotifyColleagues(App.Messages.MSG_SIGNUP);
    }

    private void Log1() {
        var user1 = Context.Users.FirstOrDefault(x => x.UserId == 1);

        NotifyColleagues(App.Messages.MSG_LOGIN, user1);

    }

    private void Log2() {
        var user1 = Context.Users.FirstOrDefault(x => x.UserId == 2);

        NotifyColleagues(App.Messages.MSG_LOGIN, user1);

    }

    private void Log3() {
        var user1 = Context.Users.FirstOrDefault(x => x.UserId == 3);

        NotifyColleagues(App.Messages.MSG_LOGIN, user1);

    }
    private void Log4() {
        var user1 = Context.Users.FirstOrDefault(x => x.UserId == 4);

        NotifyColleagues(App.Messages.MSG_LOGIN, user1);

    }

    private void Log5() {
        var user1 = Context.Users.FirstOrDefault(x => x.UserId == 5);

        NotifyColleagues(App.Messages.MSG_LOGIN, user1);

    }

    

}