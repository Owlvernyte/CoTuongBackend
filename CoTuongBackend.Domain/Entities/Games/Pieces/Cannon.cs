using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public class Cannon : Piece
{
    public Cannon()
    {
        PieceType = PieceType.Cannon;
        Signature = "C";
    }
    public override bool IsValidMove(Coordinate destinationCoordinate, Board board)
    {
        bool isBaseValidMove = base.IsValidMove(destinationCoordinate, board);
        if (!isBaseValidMove)
            return false;

        return true;
    }
}
