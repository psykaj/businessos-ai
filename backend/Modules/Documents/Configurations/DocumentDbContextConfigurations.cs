using backend.Modules.Documents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Modules.Documents.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasIndex(d => d.OrganizationId);
        builder.HasIndex(d => d.FolderId);
        builder.HasIndex(d => d.OwnerId);
        builder.HasIndex(d => d.Status);
        builder.HasIndex(d => d.IsFavorite);
        builder.HasIndex(d => d.Name);

        builder.HasOne(d => d.Folder)
            .WithMany(f => f.Documents)
            .HasForeignKey(d => d.FolderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.HasIndex(f => f.OrganizationId);
        builder.HasIndex(f => f.ParentFolderId);
        builder.HasIndex(f => f.Name);

        builder.HasOne(f => f.ParentFolder)
            .WithMany(f => f.SubFolders)
            .HasForeignKey(f => f.ParentFolderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersion>
{
    public void Configure(EntityTypeBuilder<DocumentVersion> builder)
    {
        builder.HasIndex(v => v.OrganizationId);
        builder.HasIndex(v => v.DocumentId);

        builder.HasOne(v => v.Document)
            .WithMany(d => d.Versions)
            .HasForeignKey(v => v.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.HasIndex(t => t.OrganizationId);
        builder.HasIndex(t => t.Category);
        builder.HasIndex(t => t.IsActive);
    }
}

public class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
{
    public void Configure(EntityTypeBuilder<ApprovalRequest> builder)
    {
        builder.HasIndex(a => a.OrganizationId);
        builder.HasIndex(a => a.DocumentId);
        builder.HasIndex(a => a.RequestedById);
        builder.HasIndex(a => a.Status);

        builder.HasOne(a => a.Document)
            .WithMany(d => d.ApprovalRequests)
            .HasForeignKey(a => a.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
{
    public void Configure(EntityTypeBuilder<ApprovalStep> builder)
    {
        builder.HasIndex(s => s.OrganizationId);
        builder.HasIndex(s => s.ApprovalRequestId);
        builder.HasIndex(s => s.ApproverId);
        builder.HasIndex(s => s.ApproverEmail);
        builder.HasIndex(s => s.Status);

        builder.HasOne(s => s.ApprovalRequest)
            .WithMany(a => a.Steps)
            .HasForeignKey(s => s.ApprovalRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SignatureRequestConfiguration : IEntityTypeConfiguration<SignatureRequest>
{
    public void Configure(EntityTypeBuilder<SignatureRequest> builder)
    {
        builder.HasIndex(r => r.OrganizationId);
        builder.HasIndex(r => r.DocumentId);
        builder.HasIndex(r => r.CreatedById);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.SecurityHash);

        builder.HasOne(r => r.Document)
            .WithMany(d => d.SignatureRequests)
            .HasForeignKey(r => r.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SignatureRecipientConfiguration : IEntityTypeConfiguration<SignatureRecipient>
{
    public void Configure(EntityTypeBuilder<SignatureRecipient> builder)
    {
        builder.HasIndex(r => r.OrganizationId);
        builder.HasIndex(r => r.SignatureRequestId);
        builder.HasIndex(r => r.SignerEmail);
        builder.HasIndex(r => r.SecurityToken).IsUnique();
        builder.HasIndex(r => r.Status);

        builder.HasOne(r => r.SignatureRequest)
            .WithMany(s => s.Recipients)
            .HasForeignKey(r => r.SignatureRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SharedDocumentConfiguration : IEntityTypeConfiguration<SharedDocument>
{
    public void Configure(EntityTypeBuilder<SharedDocument> builder)
    {
        builder.HasIndex(s => s.OrganizationId);
        builder.HasIndex(s => s.DocumentId);
        builder.HasIndex(s => s.SharedWithUserId);
        builder.HasIndex(s => s.PublicShareToken);

        builder.HasOne(s => s.Document)
            .WithMany(d => d.Shares)
            .HasForeignKey(s => s.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DocumentAuditEntryConfiguration : IEntityTypeConfiguration<DocumentAuditEntry>
{
    public void Configure(EntityTypeBuilder<DocumentAuditEntry> builder)
    {
        builder.HasIndex(a => a.OrganizationId);
        builder.HasIndex(a => a.DocumentId);
        builder.HasIndex(a => a.PerformedById);
        builder.HasIndex(a => a.Action);
        builder.HasIndex(a => a.Timestamp);

        builder.HasOne(a => a.Document)
            .WithMany()
            .HasForeignKey(a => a.DocumentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
