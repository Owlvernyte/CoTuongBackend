using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Horse : Piece
{
    public Horse()
    {
        PieceType = PieceType.Horse;
        Signature = "N";
    }
}
