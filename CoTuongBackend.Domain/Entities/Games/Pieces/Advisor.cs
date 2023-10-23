using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Advisor : Piece
{
    public Advisor()
    {
        PieceType = PieceType.Advisor;
        Signature = "A";
    }

    public override bool IsValidMove(Coordinate destination, Board board)
    {
        var isBaseValidMove = base.IsValidMove(destination, board);
        if (!isBaseValidMove) return false;

        var moveX = Math.Abs(Coord!.X - destination.X);
        var moveY = Math.Abs(Coord.Y - destination.Y);

        //Kiểm tra điều kiện di chuyển quân Sĩ
        if (moveX + moveY != 2 || moveX != moveY)
            return false;

        // Kiểm tra Sĩ phải di chuyển ở phạm vi cung 3x3
        if (!new int[] { 0, 1, 2, 7, 8, 9 }.Contains(destination.X)
        || !new int[] { 3, 4, 5 }.Contains(destination.Y))
            return false;

        return true;
    }
}
