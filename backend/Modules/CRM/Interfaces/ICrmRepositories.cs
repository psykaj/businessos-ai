using backend.Interfaces;
using backend.Modules.CRM.Entities;
using backend.Entities;

namespace backend.Modules.CRM.Interfaces;

public interface ILeadRepository : IGenericRepository<Lead>
{
}

public interface IContactRepository : IGenericRepository<Contact>
{
}

public interface ICompanyRepository : IGenericRepository<Company>
{
}

public interface IDealRepository : IGenericRepository<Deal>
{
}

public interface ICrmActivityRepository : IGenericRepository<CrmActivity>
{
}

public interface ICrmTaskRepository : IGenericRepository<CrmTask>
{
}

public interface INoteRepository : IGenericRepository<Note>
{
}

public interface ITagRepository : IGenericRepository<Tag>
{
}

public interface IDealStageHistoryRepository : IGenericRepository<DealStageHistory>
{
}
