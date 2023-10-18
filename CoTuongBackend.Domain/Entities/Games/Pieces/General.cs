using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class General : Piece
{
    public General()
    {
        PieceType = PieceType.General;
        Signature = "K";
    }

    public override bool IsValidMove(Coordinate destinationCoordinate, Board board)
    {
        var isBaseValidMove = base.IsValidMove(destinationCoordinate, board);
        if (!isBaseValidMove) return false;

        if (Coord is null) return false;

        var moveX = Math.Abs(Coord.X - destinationCoordinate.X);
        var moveY = Math.Abs(Coord.Y - destinationCoordinate.Y);

        // Cho phép đi từng ô một ngang hoặc dọc
        if (moveX + moveY != 1)
        {
            return false;
        }

        // Kiểm tra Tướng phải di chuyển ở phạm vi cung 3x3
        if (!new int[] { 0, 1, 2, 7, 8, 9 }.Contains(destinationCoordinate.X)
        || !new int[] { 3, 4, 5 }.Contains(destinationCoordinate.Y))
        {
            return false;
        }

        // Kiểm tra trường hợp chống tướng đối mặt
        if (!IsRed)
            for (var i = Coord.X + 1; i < board.Rows; i++)
            {
                var piece = board.Squares[i][destinationCoordinate.Y];
                if (piece is { } && piece.PieceType != PieceType.General)
                {
                    return true;
                }
                if (piece is { } && piece.PieceType == PieceType.General)
                {
                    return false;
                }
            }
        else
            for (var i = Coord.X - 1; i >= 0; i--)
            {
                var piece = board.Squares[i][destinationCoordinate.Y];
                if (piece is { } && piece.PieceType != PieceType.General)
                {
                    return true;
                }
                if (piece is { } && piece.PieceType == PieceType.General)
                {
                    return false;
                }
            }

        return true;
    }
}
