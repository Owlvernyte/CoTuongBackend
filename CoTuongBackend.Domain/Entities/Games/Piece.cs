using CoTuongBackend.Domain.Enums;

namespace CoTuongBackend.Domain.Entities.Games;

public abstract class Piece
{
    public PieceType PieceType { get; set; }
    public Coordinate? Coord { get; set; }
    public bool IsRed { get; set; } = true;
    public string Signature { get; set; } = string.Empty;
    public virtual bool IsValidMove(Coordinate destination, Board board)
    {
        if (Coord is null)
        {
            return false;
        }
        if (destination.X < 0 || destination.Y < 0 || destination.X >= board.Rows || destination.Y >= board.Columns)
        {
            return false;
        }

        var destinationPiece = board.Squares[destination.X][destination.Y];

        if (destinationPiece is { } && destinationPiece.IsRed == this.IsRed)
        {
            return false;
        }

        return true;
    }
}
