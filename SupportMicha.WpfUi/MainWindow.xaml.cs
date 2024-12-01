namespace SupportMicha.WpfUi;

public partial class MainWindow
{
    public MainWindow(IServiceProvider services)
    {
        Services = services;
        this.InitializeComponent();
    }

    public static IServiceProvider Services { get; private set; } = null!;
}