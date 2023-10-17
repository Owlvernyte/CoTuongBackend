using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Application.Matches.Dtos;

public sealed record UserMatchDto(Guid Id, string? UserName, string? Email, MatchResult Result);
