namespace Core.Specification;
public class ProductSpecParams : PaginParams
{
    private List<string> _brands = [];
    private List<string> _types = [];

    public string? Sort { get; set; }
    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value;
    }
    public List<string> Brands
    {
        get => _brands;
        set
        {
            _brands = value.SelectMany(x => x.Split(',' ,
                StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }
    public List<string> Types
    {
        get => _types;
        set
        {
            _types = value.SelectMany(x => x.Split(',' ,
                StringSplitOptions.RemoveEmptyEntries))
                .ToList();
        }
    }
}
