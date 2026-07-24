using backend.Common;
using backend.Entities;
using backend.Modules.QRCode.Models;
using backend.Modules.CRM.Entities;
using backend.Modules.Forms.Entities;
using backend.Modules.LeadCapture.Entities;
using backend.Modules.CustomerJourney.Entities;
using backend.Modules.Webhooks.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<QRCode> QRCodes => Set<QRCode>();
    public DbSet<backend.Modules.Analytics.Models.QRScan> QRScans => Set<backend.Modules.Analytics.Models.QRScan>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<LandingPage> LandingPages => Set<LandingPage>();
    public DbSet<AIConversation> AIConversations => Set<AIConversation>();
    public DbSet<AIMessage> AIMessages => Set<AIMessage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    public DbSet<AutomationRule> AutomationRules => Set<AutomationRule>();
    public DbSet<AutomationLog> AutomationLogs => Set<AutomationLog>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<BusinessCard> BusinessCards => Set<BusinessCard>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<CustomDomain> CustomDomains => Set<CustomDomain>();
    public DbSet<Theme> Themes => Set<Theme>();
    public DbSet<LandingPageSection> LandingPageSections => Set<LandingPageSection>();
    public DbSet<SEOSettings> SEOSettings => Set<SEOSettings>();

    // CRM Entities
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<CrmActivity> CrmActivities => Set<CrmActivity>();
    public DbSet<CrmTask> CrmTasks => Set<CrmTask>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<DealStageHistory> DealStageHistories => Set<DealStageHistory>();

    public DbSet<WhatsAppSettings> WhatsAppSettings => Set<WhatsAppSettings>();
    public DbSet<WhatsAppTemplate> WhatsAppTemplates => Set<WhatsAppTemplate>();
    public DbSet<CampaignContact> CampaignContacts => Set<CampaignContact>();

    // Day 12 - Lead Capture & Marketing Automation
    public DbSet<Form> Forms => Set<Form>();
    public DbSet<FormField> FormFields => Set<FormField>();
    public DbSet<FormSubmission> FormSubmissions => Set<FormSubmission>();
    public DbSet<LeadSource> LeadSources => Set<LeadSource>();
    public DbSet<backend.Modules.CustomerJourney.Entities.CustomerJourney> CustomerJourneys => Set<backend.Modules.CustomerJourney.Entities.CustomerJourney>();
    public DbSet<WebhookSubscription> WebhookSubscriptions => Set<WebhookSubscription>();
    public DbSet<WebhookDelivery> WebhookDeliveries => Set<WebhookDelivery>();

    // Day 13 - Workflow Automation, Integrations & Webhook Platform
    public DbSet<backend.Modules.Workflow.Entities.Workflow> Workflows => Set<backend.Modules.Workflow.Entities.Workflow>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowTrigger> WorkflowTriggers => Set<backend.Modules.Workflow.Entities.WorkflowTrigger>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowAction> WorkflowActions => Set<backend.Modules.Workflow.Entities.WorkflowAction>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowCondition> WorkflowConditions => Set<backend.Modules.Workflow.Entities.WorkflowCondition>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowExecution> WorkflowExecutions => Set<backend.Modules.Workflow.Entities.WorkflowExecution>();
    public DbSet<backend.Modules.Workflow.Entities.WorkflowExecutionLog> WorkflowExecutionLogs => Set<backend.Modules.Workflow.Entities.WorkflowExecutionLog>();
    public DbSet<backend.Modules.Workflow.Entities.Integration> Integrations => Set<backend.Modules.Workflow.Entities.Integration>();

    // Day 14 - Business Intelligence, Executive Dashboard & AI Decision Engine
    public DbSet<backend.Modules.BusinessIntelligence.Entities.KPI> KPIs => Set<backend.Modules.BusinessIntelligence.Entities.KPI>();
    public DbSet<backend.Modules.BusinessIntelligence.Entities.Goal> Goals => Set<backend.Modules.BusinessIntelligence.Entities.Goal>();
    public DbSet<backend.Modules.BusinessIntelligence.Entities.Report> Reports => Set<backend.Modules.BusinessIntelligence.Entities.Report>();
    public DbSet<backend.Modules.BusinessIntelligence.Entities.Insight> Insights => Set<backend.Modules.BusinessIntelligence.Entities.Insight>();
    public DbSet<backend.Modules.BusinessIntelligence.Entities.Forecast> Forecasts => Set<backend.Modules.BusinessIntelligence.Entities.Forecast>();

    // Day 15 - AI Business Agent, Task Execution Engine & Enterprise AI Copilot
    public DbSet<backend.Modules.AiAgent.Entities.Conversation> Conversations => Set<backend.Modules.AiAgent.Entities.Conversation>();
    public DbSet<backend.Modules.AiAgent.Entities.Message> Messages => Set<backend.Modules.AiAgent.Entities.Message>();
    public DbSet<backend.Modules.AiAgent.Entities.CommandExecution> CommandExecutions => Set<backend.Modules.AiAgent.Entities.CommandExecution>();
    public DbSet<backend.Modules.AiAgent.Entities.ToolDefinition> ToolDefinitions => Set<backend.Modules.AiAgent.Entities.ToolDefinition>();
    public DbSet<backend.Modules.AiAgent.Entities.Recommendation> Recommendations => Set<backend.Modules.AiAgent.Entities.Recommendation>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure composite key for UserRole
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        // Day 10 - AI & Automation constraints and filters
        modelBuilder.Entity<AIConversation>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<AIMessage>().HasIndex(m => m.ConversationId);
        modelBuilder.Entity<Notification>().HasIndex(n => n.OrganizationId);
        modelBuilder.Entity<EmailTemplate>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<AutomationRule>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<AutomationLog>().HasIndex(l => l.RuleId);
        modelBuilder.Entity<WhatsAppTemplate>().HasIndex(w => w.OrganizationId);
        
        // CRM Constraints
        modelBuilder.Entity<Lead>().HasIndex(l => l.OrganizationId);
        modelBuilder.Entity<Company>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<Deal>().HasIndex(d => d.OrganizationId);
        modelBuilder.Entity<CrmActivity>().HasIndex(a => a.OrganizationId);
        modelBuilder.Entity<CrmTask>().HasIndex(t => t.OrganizationId);
        modelBuilder.Entity<Note>().HasIndex(n => n.OrganizationId);
        modelBuilder.Entity<Tag>().HasIndex(t => t.OrganizationId);
        
        // Day 12 Constraints
        modelBuilder.Entity<Form>().HasIndex(f => f.OrganizationId);
        modelBuilder.Entity<FormSubmission>().HasIndex(fs => fs.OrganizationId);
        modelBuilder.Entity<LeadSource>().HasIndex(ls => ls.OrganizationId);
        modelBuilder.Entity<backend.Modules.CustomerJourney.Entities.CustomerJourney>().HasIndex(cj => cj.OrganizationId);
        modelBuilder.Entity<WebhookSubscription>().HasIndex(ws => ws.OrganizationId);
        
        // Day 13 Workflow Constraints
        modelBuilder.Entity<backend.Modules.Workflow.Entities.Workflow>().HasIndex(w => w.OrganizationId);
        modelBuilder.Entity<backend.Modules.Workflow.Entities.WorkflowExecution>().HasIndex(e => e.OrganizationId);
        modelBuilder.Entity<backend.Modules.Workflow.Entities.Integration>().HasIndex(i => i.OrganizationId);
        
        // Day 14 BI Constraints
        modelBuilder.Entity<backend.Modules.BusinessIntelligence.Entities.KPI>().HasIndex(k => k.OrganizationId);
        modelBuilder.Entity<backend.Modules.BusinessIntelligence.Entities.Goal>().HasIndex(g => g.OrganizationId);
        modelBuilder.Entity<backend.Modules.BusinessIntelligence.Entities.Report>().HasIndex(r => r.OrganizationId);
        modelBuilder.Entity<backend.Modules.BusinessIntelligence.Entities.Insight>().HasIndex(i => i.OrganizationId);
        modelBuilder.Entity<backend.Modules.BusinessIntelligence.Entities.Forecast>().HasIndex(f => f.OrganizationId);

        // Day 15 AI Agent Constraints
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.Conversation>().HasIndex(c => c.OrganizationId);
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.Message>().HasIndex(m => m.ConversationId);
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.CommandExecution>().HasIndex(ce => ce.OrganizationId);
        modelBuilder.Entity<backend.Modules.AiAgent.Entities.Recommendation>().HasIndex(r => r.OrganizationId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    }

    public override int SaveChanges()
    {
        HandleSaveChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleSaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // CreatedBy could be populated from a scoped service holding the CurrentUser context
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    // Soft delete logic
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
