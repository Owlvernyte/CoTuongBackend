using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public class Advisor : Piece
{
    public Advisor()
    {
        PieceType = PieceType.Advisor;
        Signature = "A";
    }
}
