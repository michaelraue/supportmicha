namespace SupportMicha.UnitTests.Domain;

using FluentAssertions;
using ResultLib;
using SupportMicha.Domain;

[TestClass]
public class SupporterTest
{
    [TestMethod]
    public void Validate_ShouldPassValidation()
    {
        var target = new Supporter(
            new FirstName("Teddy"),
            new LastName("Tester"),
            new EmailAddress("teddy@test.er"),
            Salutation.Admiral);
        var expected = Result.Success();

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Validate_ShouldFindProblemWithFirstName()
    {
        var target = new Supporter(
            new FirstName("T"),
            new LastName("Tester"),
            new EmailAddress("teddy@test.er"),
            Salutation.Admiral);
        var expected = Result.Create("FirstName", ["First name must be at least 2 characters."]);

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Validate_ShouldFindProblemWithLastName()
    {
        var target = new Supporter(
            new FirstName("Teddy"),
            new LastName("T"),
            new EmailAddress("teddy@test.er"),
            Salutation.Admiral);
        var expected = Result.Create("LastName", ["Last name must be at least 2 characters."]);

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Validate_ShouldFindProblemWithEmail()
    {
        var target = new Supporter(
            new FirstName("Teddy"),
            new LastName("Tester"),
            new EmailAddress("teddy@tester"),
            Salutation.Admiral);
        var expected = Result.Create("EmailAddress", ["Email address is not valid."]);

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Validate_ShouldFindAllProblems()
    {
        var target = new Supporter(
            new FirstName("T"),
            new LastName("T"),
            new EmailAddress("teddy@tester"),
            Salutation.Admiral);
        var expected =
            Result.Create("FirstName", ["First name must be at least 2 characters."])
                .Merge(Result.Create("LastName", ["Last name must be at least 2 characters."]))
                .Merge(Result.Create("EmailAddress", ["Email address is not valid."]));

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }
}