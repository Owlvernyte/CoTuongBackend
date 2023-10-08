using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public class Cannon : Piece
{
    public Cannon()
    {
        PieceType = PieceType.Cannon;
        Signature = "C";
    }
}
