using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public class Horse : Piece
{
    public Horse()
    {
        PieceType = PieceType.Horse;
        Signature = "N";
    }
}
