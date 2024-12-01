namespace SupportMicha.BehaviorTests.Steps;

using FluentAssertions;
using Reqnroll;
using SupportMicha.ApiInterface;
using SupportMicha.BehaviorTests.Contexts;
using SupportMicha.BehaviorTests.Drivers;

[Binding]
public class SupporterSteps(SupporterContext supporterContext, SupporterDriver supporterDriver)
{
    [Given("^the supporter's salutation is (.+)$")]
    public void GivenTheSupportersSalutationIs(SalutationDto salutation)
    {
        supporterContext.SetSalutation(salutation);
    }

    [Given("^the supporter's first name is (.+)$")]
    public void GivenTheSupportersFirstNameIs(string firstName)
    {
        supporterContext.SetFirstName(firstName);
    }

    [Given("^the supporter's last name is (.+)$")]
    public void GivenTheSupportersLastNameIs(string lastName)
    {
        supporterContext.SetLastName(lastName);
    }

    [Given("^the supporter's email address is (.+)$")]
    public void GivenTheSupportersEmailAddressIs(string emailAddress)
    {
        supporterContext.SetEmailAddress(emailAddress);
    }

    [Given("the supporter signs up")]
    public async Task GivenTheSupporterSignsUp()
    {
        var result = await supporterDriver.SignUp(supporterContext.Supporter);
        result.IsSuccessful.Should().BeTrue();
    }

    [When("the supporter signs up")]
    public async Task WhenTheSupporterSignsUp()
    {
        supporterContext.ReceiveResult(await supporterDriver.SignUp(supporterContext.Supporter));
    }

    [Then("there are no errors")]
    public void ThenThereAreNoErrors()
    {
        supporterContext.Result.Should().NotBeNull();
        supporterContext.Result!.IsSuccessful.Should().BeTrue();
    }

    [Then("^(.+) is in the list of supporting people$")]
    public async Task ThenIsInTheListOfSupportingPeople(string firstName)
    {
        var supporters = (await supporterDriver.GetSupporters()).Select(x => x.FirstName);
        supporters.Should().ContainEquivalentOf(firstName);
    }

    [Then("nobody has signed up yet")]
    public async Task ThenNobodyHasSignedUpYet()
    {
        var supporters = await supporterDriver.GetSupporters();
        supporters.Should().BeEmpty();
    }

    [Then("there are the errors:")]
    public void ThenThereAreTheErrors(DataTable table)
    {
        var expected = table
            .CreateSet<ValidationMessage>()
            .GroupBy(x => x.Field)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Error).ToList());

        supporterContext.Result.Should().NotBeNull();
        supporterContext.Result!.IsFailure.Should().BeTrue();
        supporterContext.Result!.ValidationMessages.Should().BeEquivalentTo(expected);
    }

    [Then("^field (.+) has the error: (.+)$")]
    public void ThenFieldHasTheError(string field, string error)
    {
        var expected = new KeyValuePair<string, IEnumerable<string>>(field, [error]);

        supporterContext.Result.Should().NotBeNull();
        supporterContext.Result!.IsFailure.Should().BeTrue();
        supporterContext.Result!.ValidationMessages.Should().ContainEquivalentOf(expected);
    }

    private record ValidationMessage(string Field, string Error);
}