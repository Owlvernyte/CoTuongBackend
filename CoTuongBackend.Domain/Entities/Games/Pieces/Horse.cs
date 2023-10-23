using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Horse : Piece
{
    public Horse()
    {
        PieceType = PieceType.Horse;
        Signature = "N";
    }

    public override bool IsValidMove(Coordinate destination, Board board)
    {
        var isBaseValidMove = base.IsValidMove(destination, board);
        if (!isBaseValidMove) return false;

        var moveX = Math.Abs(Coord!.X - destination.X);
        var moveY = Math.Abs(Coord.Y - destination.Y);

        // Kiểm tra điều kiện di chuyển của Mã
        if (!((moveX == 1 && moveY == 2) || (moveX == 2 && moveY == 1)))
        {
            return false;
        }

        // Kiểm tra trường hợp bị cản trở
        if (moveX == 2)
        {
            var piece = board.Squares[Coord.X + (destination.X - Coord.X) / 2][Coord.Y];
            if (piece is { })
            {
                return false;
            }
        }
        else
        {
            var piece = board.Squares[Coord.X][Coord.Y + (destination.Y - Coord.Y) / 2];
            if (piece is { })
            {
                return false;
            }
        }

        // TODO: Sẽ cập nhật lại khi có Xoay Bàn Cờ!
        return true;
    }
}
