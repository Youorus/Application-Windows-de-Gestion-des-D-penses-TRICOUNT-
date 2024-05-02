using PRBD_Framework;
using System.Windows;

namespace prbd_2324_a03.View;

public partial class LoginView : WindowBase
{
    public LoginView() {
        InitializeComponent();
    }

    public void btnCancel_Click(object sender, RoutedEventArgs e) {
        Close();
    }

}