using CoTuongBackend.Domain.Entities.Games.Pieces;

namespace CoTuongBackend.Domain.Entities.Games;

public class Board
{
    public int Columns { get; set; } = 9;
    public int Rows { get; set; } = 10;
    public Piece?[,] Squares { get; set; } = new Piece?[10, 9];
    public Board()
        => Squares = (Piece?[,])DefaultPieces.Clone();
    public static Board GetDefault()
    {
        return new()
        {
            Squares = (Piece?[,])DefaultPieces.Clone()
        };
    }
    public static Piece?[,] DefaultPieces => new Piece?[,]
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
}
