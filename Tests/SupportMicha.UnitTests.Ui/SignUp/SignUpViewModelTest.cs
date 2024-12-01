namespace SupportMicha.UnitTests.Ui.SignUp;

using System.Windows;
using CommunityToolkit.Mvvm.Input;
using FluentAssertions;
using MaterialDesignThemes.Wpf;
using Moq;
using ResultLib;
using SupportMicha.ApiInterface;
using SupportMicha.WpfUi.SignUp;

[TestClass]
public class SignUpViewModelTest
{
    private Mock<ISupporterService> supporterServiceMock = null!;
    private Mock<ISnackbarMessageQueue> snackbarMessageQueueMock = null!;
    private SignUpViewModel viewModel = null!;

    [TestInitialize]
    public void Setup()
    {
        this.supporterServiceMock = new Mock<ISupporterService>();
        this.snackbarMessageQueueMock = new Mock<ISnackbarMessageQueue>();
        this.viewModel = new SignUpViewModel(this.supporterServiceMock.Object, this.snackbarMessageQueueMock.Object);
    }

    [TestMethod]
    public void InitialState_ShouldSetupCorrectly()
    {
        this.viewModel.FirstName.Should().BeEmpty();
        this.viewModel.LastName.Should().BeEmpty();
        this.viewModel.EmailAddress.Should().BeEmpty();
        this.viewModel.Salutations.Should().NotBeEmpty();
        this.viewModel.SelectedSalutation.Should().Be(SalutationDto.Mr);
        this.viewModel.NoButtonOpacity.Should().Be(1);
        this.viewModel.NoButtonVisibility.Should().Be(Visibility.Visible);
    }

    [TestMethod]
    [DataRow(0, 0, 0, 75, 1, 1, Visibility.Visible)]
    [DataRow(0, 0, 75, 0, 1, 1, Visibility.Visible)]
    [DataRow(0, 0, 7, 7, 0, 0, Visibility.Hidden)]
    [DataRow(0, 0, 32.5, 0, 0.3, 0.4, Visibility.Visible)]
    public void UpdateButtonOpacity_ShouldChangeOpacityAndVisibility(
        double givenCursorX,
        double givenCursorY,
        double givenButtonX,
        double givenButtonY,
        double expectedOpacityMin,
        double expectedOpacityMax,
        Visibility expectedVisibility)
    {
        var currentPosition = new Point(givenCursorX, givenCursorY);
        var buttonSize = new Size(100, 50);
        var buttonPosition = new Point(givenButtonX, givenButtonY);

        this.viewModel.UpdateButtonOpacity(currentPosition, buttonSize, buttonPosition);

        this.viewModel.NoButtonOpacity.Should()
            .BeGreaterThanOrEqualTo(expectedOpacityMin).And.BeLessThanOrEqualTo(expectedOpacityMax);
        this.viewModel.NoButtonVisibility.Should().Be(expectedVisibility);
    }

    [TestMethod]
    public async Task SignUpCommand_ShouldCallSignUp()
    {
        this.supporterServiceMock
            .Setup(service => service.SignUp(It.IsAny<SupporterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        await ((IAsyncRelayCommand)this.viewModel.SignUpCommand).ExecuteAsync(CancellationToken.None);

        this.supporterServiceMock.Verify(
            service => service.SignUp(It.IsAny<SupporterDto>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task SignUpCommand_ShouldResetForm_OnSuccessfulSignUp()
    {
        this.viewModel.FirstName = "Teddy";
        this.viewModel.LastName = "Tester";
        this.viewModel.EmailAddress = "teddy@test.er";
        this.viewModel.SelectedSalutation = SalutationDto.Admiral;

        this.supporterServiceMock
            .Setup(service => service.SignUp(It.IsAny<SupporterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        await ((IAsyncRelayCommand)this.viewModel.SignUpCommand).ExecuteAsync(CancellationToken.None);

        this.viewModel.FirstName.Should().BeEmpty();
        this.viewModel.LastName.Should().BeEmpty();
        this.viewModel.EmailAddress.Should().BeEmpty();
        this.viewModel.SelectedSalutation.Should().Be(SalutationDto.Mr);
        this.viewModel.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task SignUpCommand_ShouldAddErrors_OnFailedSignUp()
    {
        var result = Result.Create("FirstName", ["First name is required."])
            .Merge(Result.Create("EmailAddress", ["Valid email is required."]));

        this.supporterServiceMock
            .Setup(service => service.SignUp(It.IsAny<SupporterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        await ((IAsyncRelayCommand)this.viewModel.SignUpCommand).ExecuteAsync(CancellationToken.None);

        this.viewModel.HasErrors.Should().BeTrue();
        this.viewModel.GetErrors("FirstName").Should().BeEquivalentTo<List<string>>(["First name is required."]);
    }

    [TestMethod]
    public async Task SignUpCommand_ShouldShowBanner_OnSuccessfulSignUp()
    {
        this.supporterServiceMock
            .Setup(service => service.SignUp(It.IsAny<SupporterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        await ((IAsyncRelayCommand)this.viewModel.SignUpCommand).ExecuteAsync(CancellationToken.None);

        this.snackbarMessageQueueMock.Verify(queue => queue.Enqueue(It.IsAny<string>()), Times.Once);
    }
}