namespace CoTuongBackend.Domain.Entities.Games;

public class Board
{
    public int Columns { get; set; } = 9;
    public int Rows { get; set; } = 10;
    public Piece?[,] Squares { get; set; } = new Piece?[9, 10];
}
