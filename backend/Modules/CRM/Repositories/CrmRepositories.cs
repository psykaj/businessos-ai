using backend.Entities;
using backend.Modules.CRM.Entities;
using backend.Modules.CRM.Interfaces;
using backend.Persistence;
using backend.Repositories;

namespace backend.Modules.CRM.Repositories;

public class LeadRepository : GenericRepository<Lead>, ILeadRepository
{
    public LeadRepository(ApplicationDbContext context) : base(context) { }
}

public class ContactRepository : GenericRepository<Contact>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context) { }
}

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context) { }
}

public class DealRepository : GenericRepository<Deal>, IDealRepository
{
    public DealRepository(ApplicationDbContext context) : base(context) { }
}

public class CrmActivityRepository : GenericRepository<CrmActivity>, ICrmActivityRepository
{
    public CrmActivityRepository(ApplicationDbContext context) : base(context) { }
}

public class CrmTaskRepository : GenericRepository<CrmTask>, ICrmTaskRepository
{
    public CrmTaskRepository(ApplicationDbContext context) : base(context) { }
}

public class NoteRepository : GenericRepository<Note>, INoteRepository
{
    public NoteRepository(ApplicationDbContext context) : base(context) { }
}

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext context) : base(context) { }
}

public class DealStageHistoryRepository : GenericRepository<DealStageHistory>, IDealStageHistoryRepository
{
    public DealStageHistoryRepository(ApplicationDbContext context) : base(context) { }
}
