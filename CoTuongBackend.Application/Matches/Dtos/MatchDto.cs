using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Application.Matches.Dtos;

public sealed record MatchDto(Guid Id, MatchStatus Status, DateTime MatchDate, IEnumerable<UserMatchDto> UserMatches);
