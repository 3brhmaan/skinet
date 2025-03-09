using Core.Entities;

namespace Core.Specification;
public class TypeListSpecification: BaseSpecifications<Product, string>
{
    public TypeListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
