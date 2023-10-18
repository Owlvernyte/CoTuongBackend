using CoTuongBackend.Application.Matches.Dtos;
using System.Collections.Immutable;

namespace CoTuongBackend.Application.Matches;
public interface IMatchService
{
    Task<Guid> Create(CreateMatchWithRoomDto createMatchWithRoomDto);
    Task<ImmutableList<MatchDto>> Get();
    Task<MatchDto> Get(Guid id);
}