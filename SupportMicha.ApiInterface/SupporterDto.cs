namespace SupportMicha.ApiInterface;

/// <summary>
/// Represents a data transfer object for a supporter.
/// </summary>
public class SupporterDto
{
    public SupporterDto()
    {
        this.FirstName = string.Empty;
        this.LastName = string.Empty;
        this.EmailAddress = string.Empty;
        this.Salutation = SalutationDto.Mr;
    }

    public SupporterDto(string firstName, string lastName, string emailAddress, SalutationDto salutation)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.EmailAddress = emailAddress;
        this.Salutation = salutation;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public SalutationDto Salutation { get; set; }
}