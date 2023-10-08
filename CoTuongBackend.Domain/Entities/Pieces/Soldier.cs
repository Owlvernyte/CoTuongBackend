using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public class Soldier : Piece
{
    public Soldier()
    {
        PieceType = PieceType.Soldier;
        Signature = "P";
    }
}
