namespace SupportMicha.UnitTests.Application;

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SupportMicha.ApiInterface;
using SupportMicha.Application;
using SupportMicha.Domain;

[TestClass]
public class SupporterServiceTest
{
    private Mock<ISupporterRepository> supporterRepositoryMock = null!;
    private SupporterService target = null!;

    [TestInitialize]
    public void Setup()
    {
        this.supporterRepositoryMock = new Mock<ISupporterRepository>();
        this.target = new SupporterService(new Mock<ILogger<SupporterService>>().Object, this.supporterRepositoryMock.Object);
    }

    [TestMethod]
    public async Task SignUp_ShouldSaveAndPublishSupporter()
    {
        this.supporterRepositoryMock.Setup(
                x => x.AddSupporter(
                    It.IsAny<Supporter>(),
                    It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var terminateObservable = new Subject<Unit>();
        var actualSupporters = new List<SupporterDto>();
        using var observable = this.CreateSubscriptionToSupporters(terminateObservable, actualSupporters);
        var supporter = new SupporterDto("Teddy", "Tester", "teddy@test.er", SalutationDto.Admiral);
        List<SupporterDto> expectedSupporters = [supporter];

        var result = await this.target.SignUp(supporter, CancellationToken.None);
        terminateObservable.OnNext(Unit.Default);

        result.IsSuccessful.Should().BeTrue();
        this.supporterRepositoryMock.Verify(
            x => x.AddSupporter(
            It.Is<Supporter>(
                s =>
                s.FirstName.Value == supporter.FirstName &&
                s.LastName.Value == supporter.LastName &&
                s.EmailAddress.Value == supporter.EmailAddress &&
                s.Salutation.Value == Enum.GetName(supporter.Salutation)),
            It.IsAny<CancellationToken>()),
            Times.Once);
        actualSupporters.Should().BeEquivalentTo(expectedSupporters);
    }

    [TestMethod]
    public async Task SignUp_ShouldReturnValidationErrors()
    {
        var supporter = new SupporterDto("T", "Tester", "teddy@test.er", SalutationDto.Admiral);
        var errors = new Dictionary<string, List<string>> { { "FirstName", ["First name must be at least 2 characters."] } };

        var result = await this.target.SignUp(supporter, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.ValidationMessages.Should().BeEquivalentTo(errors);
    }

    [TestMethod]
    public async Task SignUp_ShouldNotPublishInvalidSupporters()
    {
        var terminateObservable = new Subject<Unit>();
        var actualSupporters = new List<SupporterDto>();
        using var observable = this.CreateSubscriptionToSupporters(terminateObservable, actualSupporters);
        var supporter = new SupporterDto("T", "Tester", "teddy@test.er", SalutationDto.Admiral);

        await this.target.SignUp(supporter, CancellationToken.None);
        terminateObservable.OnNext(Unit.Default);

        actualSupporters.Should().BeEmpty();
    }

    [TestMethod]
    public async Task SignUp_ShouldNotSaveInvalidSupporters()
    {
        var supporter = new SupporterDto("T", "Tester", "teddy@test.er", SalutationDto.Admiral);

        await this.target.SignUp(supporter, CancellationToken.None);

        this.supporterRepositoryMock.Verify(
            x => x.AddSupporter(It.IsAny<Supporter>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task SignUp_ShouldNotAllowTheSameEmailSigningUpTwice()
    {
        this.supporterRepositoryMock.Setup(
                x => x.GetSupporters(It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Supporter(new FirstName("Teddy"), new LastName("Tester"), new EmailAddress("teddy@test.er"), Salutation.Admiral)]);
        var supporter = new SupporterDto("Arno", "Amoebe", "teddy@test.er", SalutationDto.Mr);
        var errors = new Dictionary<string, List<string>> { { "EmailAddress", ["Each email address can only signup once."] } };

        var result = await this.target.SignUp(supporter, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.ValidationMessages.Should().BeEquivalentTo(errors);
        this.supporterRepositoryMock.Verify(
            x => x.AddSupporter(It.IsAny<Supporter>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private async Task<IDisposable> CreateSubscriptionToSupporters(Subject<Unit> terminateSubject, List<SupporterDto> supporters) =>
        (await this.target.GetSupporters(CancellationToken.None))
        .TakeUntil(terminateSubject)
        .Do(supporters.Add)
        .Subscribe();
}