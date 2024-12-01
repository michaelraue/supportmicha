namespace SupportMicha.UnitTests.Persistence;

using FluentAssertions;
using SupportMicha.Domain;
using SupportMicha.Persistence;

[TestClass]
public class SupporterRepositoryTest
{
    [TestMethod]
    public async Task AddSupporter_ShouldAddSupporter()
    {
        var target = new SupporterRepository();
        var supporter = new Supporter(
            new FirstName("Teddy"),
            new LastName("Tester"),
            new EmailAddress("teddy@test.er"),
            Salutation.Admiral);
        List<Supporter> expected = [supporter];

        await target.AddSupporter(supporter, CancellationToken.None);

        var supporters = await target.GetSupporters(CancellationToken.None);
        supporters.Should().BeEquivalentTo(expected);
    }
}