namespace SupportMicha.UnitTests.Domain;

using FluentAssertions;
using SupportMicha.Domain;

[TestClass]
public class LastNameTest
{
    [TestMethod]
    public void Validate_ShouldPassValidation()
    {
        var target = new LastName("Tester");

        var actual = target.Validate();

        actual.Should().BeEquivalentTo([]);
    }

    [TestMethod]
    public void Validate_ShouldFindNameTooShort()
    {
        var target = new LastName("T");
        List<string> expected = ["Last name must be at least 2 characters."];

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Validate_ShouldFindNameTooLong()
    {
        var target = new LastName("This name is too long for a last name.");
        List<string> expected = ["Last name must be at most 20 characters."];

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }
}