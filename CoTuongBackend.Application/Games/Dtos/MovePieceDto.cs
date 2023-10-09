using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.Application.Games.Dtos;

public sealed record MovePieceDto(string RoomId, Coordinate Source, Coordinate Destination);
