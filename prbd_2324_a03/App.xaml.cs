using prbd_2324_a03.Model;
using prbd_2324_a03.ViewModel;
using System.Windows;
using System.Globalization;
using PRBD_Framework;

namespace prbd_2324_a03;

public partial class App : ApplicationBase<User, PridContext>
{

    public enum Messages
    {
        MSG_NEW_TRICOUNT,
        MSG_NEW_OPERATION,
        MSG_EDIT_TRICOUNT,
        MSG_VIEWTRICOUNT_CHANGED,
        MSG_CANCEL_TRICOUNT,
        MSG_TITLE_CHANGED,
        MSG_TRICOUNT_CHANGED,
        MSG_DISPLAY_TRICOUNT,
        MSG_DISPLAY_MEMBER,
        MSG_SIGNUP,
        MSG_LOGOUT,
        MSG_CLOSE_TAB,
        MSG_RESET,
        MSG_LOGIN
    } 


    public App() {
        var ci = new CultureInfo("fr-BE") {
            DateTimeFormat = {
                ShortDatePattern = "dd/MM/yyyy",
                DateSeparator = "/"
            }
        };
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;
    }

    protected override void OnStartup(StartupEventArgs e) {
        PrepareDatabase();
        TestQueries();


        Register<User>(this, Messages.MSG_LOGIN, User => {
            Login(User);
            NavigateTo<TricountViewModel, User, PridContext>();
        });

        Register(this, Messages.MSG_LOGOUT, () => {
            Logout();
            NavigateTo<LoginViewModel, User, PridContext>();
        });

        Register(this, Messages.MSG_SIGNUP, () => {
            NavigateTo<SignUpViewModel, User, PridContext>();
        });


        Register(this, Messages.MSG_RESET, () => {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        });

    }

    private static void PrepareDatabase() {
        // Clear database and seed data
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        // Cold start
        Console.Write("Cold starting database... ");
      
        Console.WriteLine("done");
    }

    protected override void OnRefreshData() {
        // TODO
    }

    private static void TestQueries() {
        // Un endroit pour tester vos requêtes LINQ
    }
}