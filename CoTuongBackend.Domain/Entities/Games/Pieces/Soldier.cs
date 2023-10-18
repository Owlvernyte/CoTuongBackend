using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Soldier : Piece
{
    public Soldier()
    {
        PieceType = PieceType.Soldier;
        Signature = "P";
    }
}
