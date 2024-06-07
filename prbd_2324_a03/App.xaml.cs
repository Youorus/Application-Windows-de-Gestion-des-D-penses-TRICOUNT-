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
        MSG_CLOSE_TAB,
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
        
    }

    private static void PrepareDatabase() {
        // Clear database and seed data
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        // Cold start
        Console.Write("Cold starting database... ");
        Context.Users.Find(1);
        Console.WriteLine("done");
    }

    protected override void OnRefreshData() {
        // TODO
    }

    private static void TestQueries() {
        // Un endroit pour tester vos requêtes LINQ
    }
}