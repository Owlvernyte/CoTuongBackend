﻿using CoTuongBackend.Domain.Entities.Games;
using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public abstract class Piece
{
    public PieceType PieceType { get; set; }
    public Coordinate? Coord { get; set; }
    public bool IsRed { get; set; } = true;
    public string Signature { get; set; } = string.Empty;
}
