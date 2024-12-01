namespace SupportMicha.BehaviorTests.Contexts;

using ResultLib;
using SupportMicha.ApiInterface;

public class SupporterContext
{
    public SupporterDto Supporter { get; } = new();

    public Result? Result { get; private set; }

    public void SetSalutation(SalutationDto salutation) =>
        this.Supporter.Salutation = salutation;

    public void SetFirstName(string firstName) =>
        this.Supporter.FirstName = firstName;

    public void SetLastName(string lastName) =>
        this.Supporter.LastName = lastName;

    public void SetEmailAddress(string emailAddress) =>
        this.Supporter.EmailAddress = emailAddress;

    public void ReceiveResult(Result result) =>
        this.Result = result;
}