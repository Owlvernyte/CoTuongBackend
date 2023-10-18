using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games.Pieces;

public sealed class Cannon : Piece
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
        Piece? desPiece = board.GetPiece(destinationCoordinate);
        if (desPiece == null)
        {
            if (destinationCoordinate.X != Coord!.X)
                return CheckRow(destinationCoordinate, board);
            if (destinationCoordinate.Y != Coord.Y)
                return CheckColums(destinationCoordinate, board);
        }
        else
        {
            if(destinationCoordinate.X != Coord!.X)
                return CheckRemoveDesRow(desPiece, board);
            if (destinationCoordinate.Y != Coord.Y)
                return CheckRemoveDesColums(desPiece,board);
        }
        return true;
    }

    public bool CheckRow(Coordinate destinationCoordinate, Board board)
    {
        if (Coord!.X < destinationCoordinate.X)
        {
            for (int i = Coord.X + 1; i < destinationCoordinate.X; i++)
            {
                if (board.Squares[i][destinationCoordinate.Y] != null)
                    return false;
            }
            return true;
        }
        for (int i = Coord.X - 1; i > destinationCoordinate.X; i--)
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
            for (int i = Coord.Y + 1; i < destinationCoordinate.Y; i++)
            {
                if (board.Squares[destinationCoordinate.X][i] != null)
                    return false;
            }
            return true;
        }
        for (int i = Coord.Y - 1; i > destinationCoordinate.Y; i--)
        {
            if (board.Squares[destinationCoordinate.X][i] != null)
                return false;
        }
        return true;
    }
    public bool CheckRemoveDesRow(Piece desPiece, Board board)
    {
        int count = 0;
        if (Coord!.X < desPiece.Coord!.X)
        {
            for (int i = Coord.X + 1; i < desPiece.Coord.X; i++)
            {
                if (board.Squares[i][desPiece.Coord.Y] != null)
                    count++;
            }
        }
        else
        {
            for (int i = Coord.X - 1; i > desPiece.Coord.X; i--)
            {
                if (board.Squares[i][desPiece.Coord.Y] != null)
                    count++;
            }
        }
        if(count == 1)
            return true;
        return false;
    }

    public bool CheckRemoveDesColums(Piece desPiece, Board board)
    {
        int count = 0;
        if (Coord!.Y < desPiece.Coord!.Y)
        {
            for (int i = Coord.Y + 1; i < desPiece.Coord.Y; i++)
            {
                if (board.Squares[Coord.X][i] != null)
                    count++;
            }
        }
        else
        {
            for (int i = Coord.Y - 1; i > desPiece.Coord.Y; i--)
            {
                if (board.Squares[desPiece.Coord.X][i] != null)
                    count++;
            }
        }
        if (count == 1)
            return true;
        return false;
    }
}
