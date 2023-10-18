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
        if (Coord is null)
        {
            return false;
        }
        if (destinationCoordinate.X < 0 || destinationCoordinate.Y < 0 || destinationCoordinate.X >= board.Rows || destinationCoordinate.Y >= board.Columns)
        {
            return false;
        }

        var destinationPiece = board.Squares[destinationCoordinate.X][destinationCoordinate.Y];

        if (destinationPiece is { } && destinationPiece.IsRed == this.IsRed)
        {
            return false;
        }

        return true;
    }
}
