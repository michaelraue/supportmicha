namespace SupportMicha.WpfUi.SupporterList;

using Microsoft.Extensions.DependencyInjection;

public partial class SupporterListControl
{
    public SupporterListControl()
    {
        this.InitializeComponent();
        var viewModel = MainWindow.Services.GetRequiredService<SupporterListViewModel>();
        this.DataContext = viewModel;
        this.Loaded += async (_, _) => await viewModel.Initialize();
        this.Unloaded += (_, _) => viewModel.UnsubscribeFromDomainSubscriber();
    }
}