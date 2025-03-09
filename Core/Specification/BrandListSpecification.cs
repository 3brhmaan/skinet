using Core.Entities;

namespace Core.Specification;
public class BrandListSpecification : BaseSpecifications<Product , string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}
