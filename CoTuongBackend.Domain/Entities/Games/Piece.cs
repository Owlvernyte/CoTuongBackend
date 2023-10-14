using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games;

public abstract class Piece
{
    public PieceType PieceType { get; set; }
    public Coordinate? Coord { get; set; }
    public bool IsRed { get; set; } = true;
    public string Signature { get; set; } = string.Empty;
    public virtual bool IsValidMove(Coordinate destinationCoordinate, Board board)
    {
        if (destinationCoordinate.X >= board.Rows
            || destinationCoordinate.X < 0)
            return false;
        if (destinationCoordinate.Y >= board.Columns
            || destinationCoordinate.Y < 0)
            return false;

        return true;
    }
}
