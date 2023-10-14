using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public class Advisor : Piece
{
    public Advisor()
    {
        PieceType = PieceType.Advisor;
        Signature = "A";
    }
}
