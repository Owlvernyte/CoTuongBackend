using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Soldier : Piece
{
    public Soldier()
    {
        PieceType = PieceType.Soldier;
        Signature = "P";
    }

    public override bool IsValidMove(Coordinate destination, Board board)
    {
        var isBaseValidMove = base.IsValidMove(destination, board);
        if (!isBaseValidMove) return false;

        var moveX = Math.Abs(Coord!.X - destination.X);
        var moveY = Math.Abs(Coord.Y - destination.Y);

        // Cho phép đi từng ô một ngang hoặc dọc
        if (moveX + moveY != 1)
        {
            return false;
        }

        // Tot do
        if (
            IsRed &&
            // Ngăn không cho con tốt đi lùi
            (Coord.X < destination.X ||
                // Ngăn không di ngang khi chua qua song
                (Coord.X >= 5 && moveY != 0))
        )
        {
            return false;
        }

        // Tot xanh/den
        if (
            !IsRed &&
            // Ngăn không cho con tốt đi lùi
            (Coord.X > destination.X ||
                // Ngăn không di ngang khi chua qua song
                (Coord.X <= 4 && moveY != 0))
        )
        {
            return false;
        }

        // TODO: Sẽ cập nhật lại khi có Xoay Bàn Cờ!

        return true;
    }
}
