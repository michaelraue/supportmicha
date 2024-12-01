namespace SupportMicha.UnitTests.Domain;

using FluentAssertions;
using SupportMicha.Domain;

[TestClass]
public class EmailAddressTest
{
    [TestMethod]
    public void Validate_ShouldPassValidation()
    {
        var target = new EmailAddress("teddy@test.er");

        var actual = target.Validate();

        actual.Should().BeEquivalentTo([]);
    }

    [TestMethod]
    public void Validate_ShouldFindEmailTooLong()
    {
        var target = new EmailAddress("Thisisaverylongemailaddress@whoeverstufflikethish.as");
        List<string> expected = ["Email address must be at most 40 characters."];

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    [DataRow("i")]
    [DataRow("i@i")]
    [DataRow("i.i")]
    [DataRow("i@i.i")]
    [DataRow("i@i!?/.is")]
    public void Validate_ShouldFindInvalidEmail(string email)
    {
        var target = new EmailAddress(email);
        List<string> expected = ["Email address is not valid."];

        var actual = target.Validate();

        actual.Should().BeEquivalentTo(expected);
    }
}