using CoTuongBackend.Application.Matches.Dtos;
using System.Collections.Immutable;

namespace CoTuongBackend.Application.Matches;
public interface IMatchService
{
    Task<Guid> Create(CreateMatchWithRoomCodeDto createMatchWithRoomCodeDto);
    Task<Guid> Create(CreateMatchWithRoomIdDto createMatchWithRoomIdDto);
    Task<ImmutableList<MatchDto>> Get();
    Task<MatchDto> Get(Guid id);
}