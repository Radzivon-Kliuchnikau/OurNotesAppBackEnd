namespace OurNotesAppBackEnd.Utils;

public class ProductQueryObject
{
    public string Brand { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public bool IsDecsending { get; set; } = false;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}