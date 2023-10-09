using CoTuongBackend.Domain.Entities.Games.Pieces;

namespace CoTuongBackend.Domain.Entities.Games;

public class Board
{
    public const int DefaultColumns = 9;
    public const int DefaultRows = 10;
    public int Columns { get; set; } = DefaultColumns;
    public int Rows { get; set; } = DefaultRows;
    public Piece?[,] Squares { get; set; } = new Piece?[DefaultRows, DefaultColumns];
    public Board()
        => Squares = (Piece?[,])DefaultPieces.Clone();
    public static Board GetDefault()
    {
        return new()
        {
            Squares = (Piece?[,])DefaultPieces.Clone()
        };
    }
    public static Piece?[,] DefaultPieces
    {
        get
        {
            var initBoard = new Piece?[,]
            {
                { new Chariot { IsRed = true }, new Horse { IsRed = true }, new Elephant { IsRed = true }, new Advisor { IsRed = true }, new General { IsRed = true }, new Advisor { IsRed = true }, new Elephant { IsRed = true }, new Horse { IsRed = true }, new Chariot { IsRed = true }},
                { null, null, null, null, null, null, null, null, null},
                { null, new Cannon { IsRed = true }, null, null, null, null, null, new Cannon { IsRed = true }, null},
                { null, null, null, null, null, null, null, null, null},
                { new Soldier { IsRed = true }, null, new Soldier { IsRed = true }, null, new Soldier { IsRed = true }, null, new Soldier { IsRed = true }, null, new Soldier { IsRed = true }},
                { null, null, null, null, null, null, null, null, null},
                { new Soldier(), null, new Soldier(), null, new Soldier(), null, new Soldier(), null, new Soldier()},
                { null, new Cannon(), null, null, null, null, null, new Cannon(), null},
                { null, null, null, null, null, null, null, null, null},
                { new Chariot(), new Horse(), new Elephant(), new Advisor(), new General(), new Advisor(), new Elephant(), new Horse(), new Chariot()},
            };

            for (int row = 0; row < DefaultRows; row++)
            {
                for (int col = 0; col < DefaultColumns; col++)
                {
                    var piece = initBoard[row, col];
                    if (piece != null)
                    {
                        piece.Coord = new Coordinate(row, col);
                        initBoard[row, col] = piece;
                    }
                }
            }

            return initBoard;
        }
    }

    public Board Reset()
    {
        Squares = (Piece?[,])DefaultPieces.Clone();

        return this;
    }
    public List<List<Piece?>> GetPieceMatrix()
    {
        var pieces = new List<List<Piece>>();
        for (int i = 0; i < Rows; i++)
        {
            var rowList = new List<Piece?>();
            for (int j = 0; j < Columns; j++)
            {
                rowList.Add(Squares[i, j]);
            }
            pieces.Add(rowList!);
        }
        return pieces!;
    }
    public bool Move(Piece sourcePiece, Coordinate destination)
    {
        if (sourcePiece is null) return false;
        var isValid = sourcePiece.IsValidMove(destination, this);
        if (!isValid) return false;

        Squares[sourcePiece.Coord!.X, sourcePiece.Coord.Y] = null;

        sourcePiece.Coord = destination;

        Squares[destination.X, destination.Y] = sourcePiece;

        return true;
    }

    public Piece? GetPiece(Coordinate coordinate)
        => Squares[coordinate.X, coordinate.Y];
}
