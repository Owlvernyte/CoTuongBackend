using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Advisor : Piece
{
    public Advisor()
    {
        PieceType = PieceType.Advisor;
        Signature = "A";
    }
}
