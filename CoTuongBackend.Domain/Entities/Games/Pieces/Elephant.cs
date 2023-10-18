using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Elephant : Piece
{
    public Elephant()
    {
        PieceType = PieceType.Elephant;
        Signature = "B";
    }
}
