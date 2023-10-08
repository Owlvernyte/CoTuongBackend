using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public class Chariot : Piece
{
    public Chariot()
    {
        PieceType = PieceType.Chariot;
        Signature = "R";
    }
}
