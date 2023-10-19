using CoTuongBackend.Application.Matches.Dtos;
using CoTuongBackend.Domain.Entities;
using CoTuongBackend.Domain.Enums;
using CoTuongBackend.Domain.Exceptions;
using CoTuongBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CoTuongBackend.Application.Matches;

public sealed class MatchService : IMatchService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public MatchService(ApplicationDbContext applicationDbContext)
        => _applicationDbContext = applicationDbContext;
    public async Task<Guid> Create(CreateMatchWithRoomCodeDto createMatchWithRoomCodeDto)
    {
        var (roomCode, winnerId) = createMatchWithRoomCodeDto;

        var room = await _applicationDbContext.Rooms
            .SingleOrDefaultAsync(x => x.Code == roomCode)
            ?? throw new NotFoundException(typeof(Room).Name, roomCode);

        var match = new Match();

        if (winnerId is null)
        {
            match.Status = MatchStatus.Play;
        }
        else if (room.OpponentUserId is null)
        {
            throw new InvalidOperationException("Cannot create a match history with no opponent");
        }
        else if (winnerId == room.HostUserId && winnerId == room.OpponentUserId)
        {
            throw new InvalidOperationException("Cannot create a match history with 2 user same id");
        }
        else if (winnerId != room.HostUserId && winnerId != room.OpponentUserId)
        {
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.HostUserId,
                Result = MatchResult.Draw
            });
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.OpponentUserId.Value,
                Result = MatchResult.Draw
            });
        }
        else
        {
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.HostUserId,
                Result = room.HostUserId == winnerId ? MatchResult.Win : MatchResult.Lose
            });
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.OpponentUserId.Value,
                Result = room.OpponentUserId == winnerId ? MatchResult.Win : MatchResult.Lose
            });
        }

        await _applicationDbContext
            .AddAsync(match);

        await _applicationDbContext
            .SaveChangesAsync();

        return match.Id;
    }
    public async Task<Guid> Create(CreateMatchWithRoomIdDto createMatchWithRoomIdDto)
    {
        var (roomId, winnerId) = createMatchWithRoomIdDto;

        var room = await _applicationDbContext.Rooms
            .SingleOrDefaultAsync(x => x.Id == roomId)
            ?? throw new NotFoundException(typeof(Room).Name, roomId);

        var match = new Match();

        if (winnerId is null)
        {
            match.Status = MatchStatus.Play;
        }
        else if (room.OpponentUserId is null)
        {
            throw new InvalidOperationException("Cannot create a match history with no opponent");
        }
        else if (winnerId == room.HostUserId && winnerId == room.OpponentUserId)
        {
            throw new InvalidOperationException("Cannot create a match history with 2 user same id");
        }
        else if (winnerId != room.HostUserId && winnerId != room.OpponentUserId)
        {
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.HostUserId,
                Result = MatchResult.Draw
            });
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.OpponentUserId.Value,
                Result = MatchResult.Draw
            });
        }
        else
        {
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.HostUserId,
                Result = room.HostUserId == winnerId ? MatchResult.Win : MatchResult.Lose
            });
            match.UserMatches.Add(new UserMatch
            {
                UserId = room.OpponentUserId.Value,
                Result = room.OpponentUserId == winnerId ? MatchResult.Win : MatchResult.Lose
            });
        }

        await _applicationDbContext
            .AddAsync(match);

        await _applicationDbContext
            .SaveChangesAsync();

        return match.Id;
    }
    public async Task<ImmutableList<MatchDto>> Get()
    {
        var matchDtos = await _applicationDbContext.Matches
            .Include(x => x.UserMatches)
            .Select(match => new MatchDto(
                match.Id,
                match.Status,
                match.MatchDate,
                match.UserMatches
                    .Where(x => x != null)
                    .Select(x => new UserMatchDto(x.UserId, x.User!.UserName, x.User!.Email, x.Result)))
            ).ToListAsync();

        return matchDtos.ToImmutableList();
    }

    public async Task<MatchDto> Get(Guid id)
    {
        var match = await _applicationDbContext.Matches
            .Include(x => x.UserMatches)
            .SingleOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException(typeof(Match).Name, id);

        var matchDto = new MatchDto(
            match.Id,
            match.Status,
            match.MatchDate,
            match.UserMatches.Where(x => x != null).Select(x => new UserMatchDto(x.UserId, x.User?.UserName, x.User?.Email, x.Result)));

        return matchDto;
    }
}
