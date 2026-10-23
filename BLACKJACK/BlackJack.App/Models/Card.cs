namespace BlackJack.App.Models;

public class Card
{
    public string? ImageUrl { get; set; }
    public int[]? Value { get; set; }

    public override string ToString()
    {
        return ImageUrl ?? string.Empty;
    }
}
