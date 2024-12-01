namespace SupportMicha.WpfUi.SignUp;

using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SupportMicha.ApiInterface;
using SupportMicha.WpfUi.Shared;

public class SignUpViewModel : ValidationViewModel
{
    private readonly ISupporterService supporterService;
    private readonly SupporterDto supporter;
    private double noButtonOpacity = 1;
    private Visibility noButtonVisibility = Visibility.Visible;

    public SignUpViewModel(ISupporterService supporterService, ISnackbarMessageQueue snackbarMessageQueue)
    {
        this.supporterService = supporterService;
        this.supporter = new SupporterDto();
        this.SignUpCommand = new AsyncRelayCommand(this.OnSignUp);
        this.SnackbarMessageQueue = snackbarMessageQueue;
    }

    public IEnumerable<SalutationDto> Salutations => Enum.GetValues<SalutationDto>();

    public ISnackbarMessageQueue SnackbarMessageQueue { get; }

    public double NoButtonOpacity
    {
        get => this.noButtonOpacity;
        set => this.SetProperty(ref this.noButtonOpacity, value);
    }

    public Visibility NoButtonVisibility
    {
        get => this.noButtonVisibility;
        set => this.SetProperty(ref this.noButtonVisibility, value);
    }

    public string FirstName
    {
        get => this.supporter.FirstName;
        set => this.SetProperty(this.supporter.FirstName, value.Trim(), this.supporter, (o, v) => o.FirstName = v);
    }

    public string LastName
    {
        get => this.supporter.LastName;
        set => this.SetProperty(this.supporter.LastName, value.Trim(), this.supporter, (o, v) => o.LastName = v);
    }

    public string EmailAddress
    {
        get => this.supporter.EmailAddress;
        set => this.SetProperty(this.supporter.EmailAddress, value.Trim(), this.supporter, (o, v) => o.EmailAddress = v);
    }

    public SalutationDto SelectedSalutation
    {
        get => this.supporter.Salutation;
        set => this.SetProperty(this.supporter.Salutation, value, this.supporter, (o, v) => o.Salutation = v);
    }

    public ICommand SignUpCommand { get; }

    public void UpdateButtonOpacity(Point currentPosition, Size buttonSize, Point buttonPosition)
    {
        var (opacity, visibility) = NoButtonOpacityCalculator.CalculateOpacityAndVisibility(
            currentPosition,
            buttonSize,
            buttonPosition);
        this.NoButtonOpacity = opacity;
        this.NoButtonVisibility = visibility;
    }

    private async Task OnSignUp(CancellationToken cancellationToken)
    {
        var result = await this.supporterService.SignUp(this.supporter, cancellationToken);
        if (result.IsSuccessful)
        {
            this.ResetForm();
            this.SnackbarMessageQueue.Enqueue("Thank you ❤");
            return;
        }

        this.ClearErrors();
        foreach (var message in result.ValidationMessages)
        {
            foreach (var error in message.Value)
            {
                this.AddError(message.Key, error);
            }
        }
    }

    private void ResetForm()
    {
        this.FirstName = string.Empty;
        this.LastName = string.Empty;
        this.EmailAddress = string.Empty;
        this.SelectedSalutation = this.Salutations.First();
        this.ClearErrors();
    }
}