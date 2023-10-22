using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Elephant : Piece
{
    public Elephant()
    {
        PieceType = PieceType.Elephant;
        Signature = "B";
    }

    public override bool IsValidMove(Coordinate destination, Board board)
    {
        var isBaseValidMove = base.IsValidMove(destination, board);
        if (!isBaseValidMove) return false;

        var directionX = destination.X - Coord!.X;
        var directionY = destination.Y - Coord.Y;
        var moveX = Math.Abs(directionX);
        var moveY = Math.Abs(directionY);

        // Cho phép đi chéo với đường chéo 2 ô
        if (moveX > 2 || moveY > 2 || moveX + moveY != 4)
        {
            return false;
        }

        // Tượng không được phép qua sông
        if (IsRed)
        {
            if (destination.X < 5) return false;
        }
        else
        {
            if (destination.X > 4) return false;
        }

        // Kiểm tra xem ở giữa  quân cờ và điểm đến có bị chặn không
        if (
            board.Squares[Coord.X + (directionX > 0 ? 1 : -1)][
                Coord.Y + (directionY > 0 ? 1 : -1)
            ] is { }
        )
        {
            return false;
        }

        // TODO: Sẽ cập nhật lại khi có Xoay Bàn Cờ!
        return true;
    }
}
