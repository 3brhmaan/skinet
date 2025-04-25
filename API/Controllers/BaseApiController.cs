using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<IActionResult> CreatePageResult<T>(
        IGenericRepository<T> repository ,
        ISpecification<T> specification ,
        int pageIndex ,
        int pageSize
    ) where T : BaseEntity
    {
        var items = await repository.ListAsync(specification);
        var count = await repository.CountAsync(specification);

        var pagination = new Pagination<T>(
            pageIndex , pageSize , count , items
        );

        return Ok(pagination);
    }

    protected async Task<IActionResult> CreatePageResult<T, TDto>(
        IGenericRepository<T> repository ,
        ISpecification<T> specification ,
        Func<T, TDto> toDto ,
        int pageIndex ,
        int pageSize
    ) where T : BaseEntity, IDtoConvertible
    {
        var items = await repository.ListAsync(specification);
        var count = await repository.CountAsync(specification);

        var dtoItems = items.Select(toDto).ToList();

        var pagination = new Pagination<TDto>(
            pageIndex , pageSize , count , dtoItems
        );

        return Ok(pagination);
    }
}
