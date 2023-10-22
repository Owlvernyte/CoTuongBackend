using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Cannon : Piece
{
    public Cannon()
    {
        PieceType = PieceType.Cannon;
        Signature = "C";
    }
    public override bool IsValidMove(Coordinate destination, Board board)
    {
        var isBaseValidMove = base.IsValidMove(destination, board);
        if (!isBaseValidMove) return false;

        var directionX = destination.X - Coord!.X;
        var directionY = destination.Y - Coord!.Y;
        var moveX = Math.Abs(directionX);
        var moveY = Math.Abs(directionY);

        // Khong cho phep di cheo
        if (moveX > 0 && moveY > 0) return false;

        var isOverPiece = board.Squares[destination.X][destination.Y] != null;
        var count = 0;

        // Kiem tra cot
        if (moveY == 0)
        {
            var headIndex = directionX > 0 ? Coord.X : destination.X;
            var tailIndex = directionX < 0 ? Coord.X : destination.X;
            for (var i = headIndex + 1; i < tailIndex; i++)
            {
                if (board.Squares[i][Coord.Y] is { }) count++;
            }
        }

        // Kiem tra dong
        if (moveX == 0)
        {
            var headIndex = directionY > 0 ? Coord.Y : destination.Y;
            var tailIndex = directionY < 0 ? Coord.Y : destination.Y;
            for (var j = headIndex + 1; j < tailIndex; j++)
            {
                if (board.Squares[Coord.X][j] is { }) count++;
            }
        }

        if ((!isOverPiece && count != 0) || (isOverPiece && count != 1))
            return false;

        // TODO: Sẽ cập nhật lại khi có Xoay Bàn Cờ!
        return true;
    }
}
