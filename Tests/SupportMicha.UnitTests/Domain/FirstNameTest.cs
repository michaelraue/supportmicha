namespace SupportMicha.UnitTests.Domain;

using FluentAssertions;
using SupportMicha.Domain;

[TestClass]
public class FirstNameTest
{
    [TestMethod]
    public void Validate_ShouldPassValidation()
    {
        var target = new FirstName("Teddy");

        var actual = target.Validate();

        actual.Should().BeEquivalentTo([]);
    }

    [TestMethod]
    public void Validate_ShouldFindNameTooShort()
    {
        var target = new FirstName("T");
        List<string> expected = ["First name must be at least 2 characters."];

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Validate_ShouldFindNameTooLong()
    {
        var target = new FirstName("This name is too long for a first name.");
        List<string> expected = ["First name must be at most 20 characters."];

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }
}