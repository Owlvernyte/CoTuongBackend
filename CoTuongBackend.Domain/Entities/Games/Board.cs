using CoTuongBackend.Domain.Entities.Games.Pieces;

namespace CoTuongBackend.Domain.Entities.Games;

public class Board
{
    public const int DefaultColumns = 9;
    public const int DefaultRows = 10;
    public int Columns { get; set; } = DefaultColumns;
    public int Rows { get; set; } = DefaultRows;
    public List<List<Piece?>> Squares { get; set; } = new List<List<Piece?>>();
    public Board()
        => Squares = GetDefaultSquares();

    public static List<List<Piece?>> GetDefaultSquares()
    {
        var initSquares = new List<List<Piece?>>()
        {
                new List<Piece?> { new Chariot { IsRed = false }, new Horse { IsRed = false }, new Elephant { IsRed = false }, new Advisor { IsRed = false }, new General { IsRed = false }, new Advisor { IsRed = false }, new Elephant { IsRed = false }, new Horse { IsRed = false }, new Chariot { IsRed = false }},
                new List<Piece?> { null, null, null, null, null, null, null, null, null},
                new List<Piece?> { null, new Cannon { IsRed = false }, null, null, null, null, null, new Cannon { IsRed = false }, null},
                new List<Piece?> { new Soldier { IsRed = false }, null, new Soldier { IsRed = false }, null, new Soldier { IsRed = false }, null, new Soldier { IsRed = false }, null, new Soldier { IsRed = false }},
                new List<Piece?> { null, null, null, null, null, null, null, null, null},
                new List<Piece?> { null, null, null, null, null, null, null, null, null},
                new List<Piece?> { new Soldier(), null, new Soldier(), null, new Soldier(), null, new Soldier(), null, new Soldier()},
                new List<Piece?> { null, new Cannon(), null, null, null, null, null, new Cannon(), null},
                new List<Piece?> { null, null, null, null, null, null, null, null, null},
                new List<Piece?> { new Chariot(), new Horse(), new Elephant(), new Advisor(), new General(), new Advisor(), new Elephant(), new Horse(), new Chariot()},
        };

        for (int i = 0; i < initSquares.Count; i++)
        {
            for (int j = 0; j < initSquares[i].Count; j++)
            {
                var piece = initSquares[i][j];
                if (piece is { })
                {
                    piece.Coord = new Coordinate(i, j);
                }
            }
        }

        return initSquares;
    }

    public Board Reset()
    {
        Squares = GetDefaultSquares();
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
                rowList.Add(Squares[i][j]);
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

        Squares[sourcePiece.Coord!.X][sourcePiece.Coord.Y] = null;

        sourcePiece.Coord = destination;

        Squares[destination.X][destination.Y] = sourcePiece;

        return true;
    }

    public Piece? GetPiece(Coordinate coordinate)
        => Squares[coordinate.X][coordinate.Y];
}
