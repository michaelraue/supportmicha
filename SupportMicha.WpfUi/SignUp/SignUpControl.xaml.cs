namespace SupportMicha.WpfUi.SignUp;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

public partial class SignUpControl
{
    public SignUpControl()
    {
        this.DataContext = MainWindow.Services.GetService<SignUpViewModel>();
        this.InitializeComponent();
    }

    private void OnCanvasMouseMove(object sender, MouseEventArgs e)
    {
        var viewModel = (SignUpViewModel)this.DataContext;
        viewModel.UpdateButtonOpacity(
            e.GetPosition(this.ButtonCanvas),
            new Size(this.NoButton.Width, this.NoButton.Height),
            new Point(Canvas.GetLeft(this.NoButton), Canvas.GetTop(this.NoButton)));
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        var viewModel = (SignUpViewModel)this.DataContext;
        viewModel.NoButtonOpacity = 1.0;
        viewModel.NoButtonVisibility = Visibility.Visible;
    }
}