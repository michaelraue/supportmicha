namespace SupportMicha.UnitTests.Ui.SupporterList;

using System.Reactive.Subjects;
using FluentAssertions;
using Moq;
using SupportMicha.ApiInterface;
using SupportMicha.WpfUi.SupporterList;

[TestClass]
public class SupporterListViewModelTest
{
    private Mock<ISupporterService> supporterServiceMock = null!;
    private SupporterListViewModel target = null!;

    [TestInitialize]
    public void Setup()
    {
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        this.supporterServiceMock = new Mock<ISupporterService>();
        this.target = new SupporterListViewModel(this.supporterServiceMock.Object);
    }

    [TestMethod]
    public async Task Initialize_ShouldPopulateSupporters()
    {
        var supporterSubject = new Subject<SupporterDto>();
        this.supporterServiceMock
            .Setup(s => s.GetSupporters(CancellationToken.None))
            .ReturnsAsync(supporterSubject);

        await this.target.Initialize(CancellationToken.None);

        this.target.Supporters.Should().BeEmpty();
        var newSupporter = new SupporterDto { FirstName = "Teddy", LastName = "Tester" };
        supporterSubject.OnNext(newSupporter);
        this.target.Supporters.Should().BeEquivalentTo([newSupporter]);
    }

    [TestMethod]
    public async Task UnsubscribeFromDomainSubscriber_ShouldNotAddNewSupportersAfterwards()
    {
        var supporterSubject = new Subject<SupporterDto>();
        this.supporterServiceMock
            .Setup(s => s.GetSupporters(CancellationToken.None))
            .ReturnsAsync(supporterSubject);
        await this.target.Initialize(CancellationToken.None);

        this.target.UnsubscribeFromDomainSubscriber();

        supporterSubject.OnNext(new SupporterDto { FirstName = "Teddy", LastName = "Tester" });
        this.target.Supporters.Should().BeEmpty();
    }
}