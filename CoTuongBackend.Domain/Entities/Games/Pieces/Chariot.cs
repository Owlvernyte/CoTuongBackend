using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Chariot : Piece
{
    public Chariot()
    {
        PieceType = PieceType.Chariot;
        Signature = "R";
    }
    public override bool IsValidMove(Coordinate destinationCoordinate, Board board)
    {
        bool isBaseValidMove = base.IsValidMove(destinationCoordinate, board);
        if (!isBaseValidMove)
            return false;
        if (destinationCoordinate.X != Coord!.X)
            return CheckRow(destinationCoordinate, board);
        if (destinationCoordinate.Y != Coord.Y)
            return CheckColums(destinationCoordinate, board);
        return true;
    }

    public bool CheckRow(Coordinate destinationCoordinate, Board board)
    {
        if (Coord!.X < destinationCoordinate.X)
        {
            for (int i = Coord.X; i < destinationCoordinate.X; i++)
            {
                if (board.Squares[i][destinationCoordinate.Y] != null)
                    return false;
            }
            return true;
        }
        for (int i = Coord.X; i > destinationCoordinate.X; i--)
        {
            if (board.Squares[i][destinationCoordinate.Y] != null)
                return false;
        }
        return true;
    }

    public bool CheckColums(Coordinate destinationCoordinate, Board board)
    {
        if (Coord!.Y < destinationCoordinate.Y)
        {
            for (int i = Coord.Y; i < destinationCoordinate.Y; i++)
            {
                if (board.Squares[destinationCoordinate.X][i] != null)
                    return false;
            }
            return true;
        }
        for (int i = Coord.Y; i > destinationCoordinate.Y; i--)
        {
            if (board.Squares[destinationCoordinate.X][i] != null)
                return false;
        }
        return true;
    }
}
