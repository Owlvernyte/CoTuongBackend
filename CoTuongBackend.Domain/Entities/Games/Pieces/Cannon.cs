using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Cannon : Piece
{
    public Cannon()
    {
        PieceType = PieceType.Cannon;
        Signature = "C";
    }
}
