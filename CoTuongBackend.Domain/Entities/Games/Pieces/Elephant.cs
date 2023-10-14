using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public class Elephant : Piece
{
    public Elephant()
    {
        PieceType = PieceType.Elephant;
        Signature = "B";
    }
}
