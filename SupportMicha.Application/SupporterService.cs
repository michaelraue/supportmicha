namespace SupportMicha.Application;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using ResultLib;
using SupportMicha.ApiInterface;
using SupportMicha.Domain;

public class SupporterService : ISupporterService
{
    private readonly ILogger<SupporterService> logger;
    private readonly ISupporterRepository supporterRepository;
    private readonly Subject<SupporterDto> subjectForUpdates;
    private readonly IObservable<SupporterDto> supportersObservable;

    public SupporterService(ILogger<SupporterService> logger, ISupporterRepository supporterRepository)
    {
        this.logger = logger;
        this.supporterRepository = supporterRepository;
        this.subjectForUpdates = new Subject<SupporterDto>();
        this.supportersObservable = this.subjectForUpdates.Publish().RefCount();
    }

    public async Task<Result> SignUp(SupporterDto supporterDto, CancellationToken cancellationToken)
    {
        var supporter = ToDomain(supporterDto);
        var result = supporter.Validate();
        if (result.IsFailure)
        {
            this.logger.LogWarning("Supporter {@Supporter} failed to sign up, problems: {@Errors}", supporterDto, result.ValidationMessages);
            return result;
        }

        var existingSupporters = await this.supporterRepository.GetSupporters(cancellationToken);
        if (existingSupporters.Any(x => x.EmailAddress.Value == supporter.EmailAddress.Value))
        {
            return Result.Create("EmailAddress", ["Each email address can only signup once."]);
        }

        await this.supporterRepository.AddSupporter(supporter, cancellationToken);
        this.logger.LogInformation("Supporter {@Supporter} signed up.", supporterDto);
        this.subjectForUpdates.OnNext(ToDto(supporter));
        return Result.Success();
    }

    public async Task<IObservable<SupporterDto>> GetSupporters(CancellationToken cancellationToken)
    {
        var supporters = await this.supporterRepository.GetSupporters(cancellationToken);
        return supporters.Select(ToDto).ToObservable().Concat(this.supportersObservable);
    }

    private static Supporter ToDomain(SupporterDto dto) =>
        new(
            new FirstName(dto.FirstName),
            new LastName(dto.LastName),
            new EmailAddress(dto.EmailAddress),
            ToDomain(dto.Salutation));

    private static Salutation ToDomain(SalutationDto dto) =>
        dto switch
        {
            SalutationDto.Mr => Salutation.Mr,
            SalutationDto.Mrs => Salutation.Mrs,
            SalutationDto.Ensign => Salutation.Ensign,
            SalutationDto.Lieutenant => Salutation.Lieutenant,
            SalutationDto.LieutenantCommander => Salutation.LieutenantCommander,
            SalutationDto.Commander => Salutation.Commander,
            SalutationDto.Captain => Salutation.Captain,
            SalutationDto.Admiral => Salutation.Admiral,
            _ => throw new ArgumentOutOfRangeException(nameof(dto), dto, null),
        };

    private static SupporterDto ToDto(Supporter supporter) =>
        new(
            supporter.FirstName.Value,
            supporter.LastName.Value,
            supporter.EmailAddress.Value,
            ToDto(supporter.Salutation));

    private static SalutationDto ToDto(Salutation salutation)
    {
        if (salutation == Salutation.Mr)
        {
            return SalutationDto.Mr;
        }

        if (salutation == Salutation.Mrs)
        {
            return SalutationDto.Mrs;
        }

        if (salutation == Salutation.Ensign)
        {
            return SalutationDto.Ensign;
        }

        if (salutation == Salutation.Lieutenant)
        {
            return SalutationDto.Lieutenant;
        }

        if (salutation == Salutation.LieutenantCommander)
        {
            return SalutationDto.LieutenantCommander;
        }

        if (salutation == Salutation.Commander)
        {
            return SalutationDto.Commander;
        }

        if (salutation == Salutation.Captain)
        {
            return SalutationDto.Captain;
        }

        if (salutation == Salutation.Admiral)
        {
            return SalutationDto.Admiral;
        }

        throw new ArgumentOutOfRangeException(nameof(salutation), salutation, null);
    }
}