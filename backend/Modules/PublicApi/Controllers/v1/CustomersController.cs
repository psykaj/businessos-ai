using backend.Entities;
using backend.Modules.PublicApi.Common;
using backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Modules.PublicApi.Controllers.v1;

public class CustomersController : BasePublicApiController
{
    private readonly ApplicationDbContext _dbContext;

    public CustomersController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Customer>>> GetCustomers([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var query = _dbContext.Customers.Where(c => c.OrganizationId == orgId && !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(paginationParams.Filter))
        {
            var f = paginationParams.Filter.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(f));
        }

        if (!string.IsNullOrWhiteSpace(paginationParams.SortBy))
        {
            query = paginationParams.SortBy.ToLower() switch
            {
                "name" => paginationParams.SortDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "createdat" => paginationParams.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(c => c.CreatedAt);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return Ok(new PagedResult<Customer>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paginationParams.PageNumber,
            PageSize = paginationParams.PageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        var orgId = GetOrganizationId();
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id && c.OrganizationId == orgId && !c.IsDeleted, cancellationToken);
        
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }
}
