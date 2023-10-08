using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Pieces;

public sealed class General : Piece
{
    public General()
    {
        PieceType = PieceType.General;
        Signature = "K";
    }
}
