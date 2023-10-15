using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.Application.Games.Dtos;

public sealed record MovePieceDto(Coordinate Source, Coordinate Destination);
