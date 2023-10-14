using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.Application.Games.Dtos;

public sealed record MovePieceDto(string RoomCode, Coordinate Source, Coordinate Destination);
