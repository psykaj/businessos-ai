using System.Text;
using backend.Modules.Documents.Approvals.DTOs;
using backend.Modules.Documents.Approvals.Repositories;
using backend.Modules.Documents.Approvals.Services;
using backend.Modules.Documents.AuditLogs.Repositories;
using backend.Modules.Documents.AuditLogs.Services;
using backend.Modules.Documents.Documents.DTOs;
using backend.Modules.Documents.Documents.Repositories;
using backend.Modules.Documents.Documents.Services;
using backend.Modules.Documents.ESignatures.DTOs;
using backend.Modules.Documents.ESignatures.Repositories;
using backend.Modules.Documents.ESignatures.Services;
using backend.Modules.Documents.Folders.DTOs;
using backend.Modules.Documents.Folders.Repositories;
using backend.Modules.Documents.Folders.Services;
using backend.Modules.Documents.Storage.Services;
using backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Modules.Documents.Tests;

public class DocumentManagementSystemTests
{
    private static ApplicationDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static LocalStorageService CreateStorageService()
    {
        var inMemoryConfig = new Dictionary<string, string?>
        {
            { "Storage:LocalPath", Path.Combine(Path.GetTempPath(), "BusinessOS_DMSTests_" + Guid.NewGuid()) },
            { "Storage:SecretKey", "TestSecretKey123!" }
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryConfig)
            .Build();

        return new LocalStorageService(config);
    }

    public static async Task RunAllTestsAsync()
    {
        Console.WriteLine("🚀 Running Document Management System Tests...");

        await TestDocumentUploadAndVersioningAsync();
        await TestFolderHierarchyAndMoveAsync();
        await TestApprovalWorkflowEngineAsync();
        await TestESignatureWorkflowAndHashAsync();
        await TestMultiTenantIsolationAsync();

        Console.WriteLine("✅ All Document Management System Tests Passed Successfully!");
    }

    private static async Task TestDocumentUploadAndVersioningAsync()
    {
        var db = CreateInMemoryDbContext("TestDb_UploadAndVersion");
        var storage = CreateStorageService();
        var auditLogRepo = new DocumentAuditLogRepository(db);
        var auditLogService = new DocumentAuditLogService(auditLogRepo);

        var docRepo = new DocumentRepository(db);
        var docService = new DocumentService(docRepo, storage, auditLogService);

        var orgId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        // 1. Upload initial document
        var content = Encoding.UTF8.GetBytes("Contract version 1 content");
        using var stream1 = new MemoryStream(content);

        var uploadDto = new UploadDocumentDto(null, "Test Contract", new List<string> { "Legal", "Contract" }, null);
        var doc = await docService.UploadAsync(orgId, ownerId, "John Doe", stream1, "contract.pdf", "application/pdf", uploadDto);

        if (doc.VersionCount != 1) throw new Exception($"Expected VersionCount 1, got {doc.VersionCount}");
        if (doc.Status != "Active") throw new Exception($"Expected status Active, got {doc.Status}");

        // 2. Upload version 2
        var contentV2 = Encoding.UTF8.GetBytes("Contract version 2 updated content");
        using var stream2 = new MemoryStream(contentV2);

        var docV2 = await docService.UploadNewVersionAsync(orgId, doc.Id, ownerId, "John Doe", stream2, "contract.pdf", "application/pdf", "Updated clause 4");
        if (docV2.VersionCount != 2) throw new Exception($"Expected VersionCount 2, got {docV2.VersionCount}");

        // 3. Check version history
        var versions = await docService.GetVersionsAsync(orgId, doc.Id);
        if (versions.Count != 2) throw new Exception($"Expected 2 versions in history, got {versions.Count}");

        // 4. Revert to version 1
        var revertedDoc = await docService.RevertToVersionAsync(orgId, doc.Id, versions[1].Id, ownerId, "John Doe");
        if (revertedDoc.CurrentVersionId != versions[1].Id) throw new Exception("Failed to revert to Version 1.");

        Console.WriteLine("  ✓ Document Upload, Versioning, and Reversion Test Passed");
    }

    private static async Task TestFolderHierarchyAndMoveAsync()
    {
        var db = CreateInMemoryDbContext("TestDb_FolderHierarchy");
        var folderRepo = new FolderRepository(db);
        var folderService = new FolderService(folderRepo);

        var orgId = Guid.NewGuid();

        // 1. Create parent folder
        var parent = await folderService.CreateAsync(orgId, new CreateFolderDto(null, "Contracts 2026", "Parent contracts folder", "#FF0000", "folder"));
        
        // 2. Create subfolder
        var child = await folderService.CreateAsync(orgId, new CreateFolderDto(parent.Id, "Vendor Contracts", "Vendor folder", "#00FF00", "folder-sub"));
        if (!child.Path.Contains("Contracts 2026")) throw new Exception($"Child folder path incorrect: {child.Path}");

        // 3. Create tree
        var tree = await folderService.GetTreeAsync(orgId);
        if (tree.Count != 1 || tree[0].Children.Count != 1) throw new Exception("Folder tree structure invalid.");

        Console.WriteLine("  ✓ Folder Hierarchy and Tree Test Passed");
    }

    private static async Task TestApprovalWorkflowEngineAsync()
    {
        var db = CreateInMemoryDbContext("TestDb_ApprovalWorkflow");
        var storage = CreateStorageService();
        var auditLogRepo = new DocumentAuditLogRepository(db);
        var auditLogService = new DocumentAuditLogService(auditLogRepo);
        var docRepo = new DocumentRepository(db);
        var docService = new DocumentService(docRepo, storage, auditLogService);
        var approvalRepo = new ApprovalRepository(db);
        var approvalService = new ApprovalService(approvalRepo, docRepo, auditLogService);

        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var approver1 = Guid.NewGuid();
        var approver2 = Guid.NewGuid();

        // Upload doc
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Doc content"));
        var doc = await docService.UploadAsync(orgId, userId, "Owner", stream, "doc.pdf", "application/pdf", new UploadDocumentDto(null, null, null, null));

        // Create approval request with 2 sequential steps
        var steps = new List<CreateApprovalStepDto>
        {
            new CreateApprovalStepDto(1, approver1, "manager@corp.com", "Manager"),
            new CreateApprovalStepDto(2, approver2, "director@corp.com", "Director")
        };

        var request = await approvalService.CreateAsync(orgId, userId, "Owner", new CreateApprovalRequestDto(doc.Id, "Legal Approval", true, DateTime.UtcNow.AddDays(3), false, null, steps));
        if (request.Status != "Pending") throw new Exception($"Expected status Pending, got {request.Status}");

        // Approve step 1
        var approvedStep1 = await approvalService.ApproveStepAsync(orgId, request.Id, approver1, "manager@corp.com", new ApproveRejectStepDto("Looks good to me"));
        if (approvedStep1.CurrentStepSequence != 2) throw new Exception($"Expected step sequence 2, got {approvedStep1.CurrentStepSequence}");

        // Approve step 2
        var approvedStep2 = await approvalService.ApproveStepAsync(orgId, request.Id, approver2, "director@corp.com", new ApproveRejectStepDto("Approved by Director"));
        if (approvedStep2.Status != "Approved") throw new Exception($"Expected overall status Approved, got {approvedStep2.Status}");

        Console.WriteLine("  ✓ Multi-level Sequential Approval Workflow Test Passed");
    }

    private static async Task TestESignatureWorkflowAndHashAsync()
    {
        var db = CreateInMemoryDbContext("TestDb_ESignature");
        var storage = CreateStorageService();
        var auditLogRepo = new DocumentAuditLogRepository(db);
        var auditLogService = new DocumentAuditLogService(auditLogRepo);
        var docRepo = new DocumentRepository(db);
        var docService = new DocumentService(docRepo, storage, auditLogService);
        var esigRepo = new ESignatureRepository(db);
        var providerService = new NativeESignatureProviderService();
        var esigService = new ESignatureService(esigRepo, docRepo, providerService, auditLogService);

        var orgId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Agreement text"));
        var doc = await docService.UploadAsync(orgId, userId, "Owner", stream, "nda.pdf", "application/pdf", new UploadDocumentDto(null, null, null, null));

        var recipients = new List<CreateSignatureRecipientDto>
        {
            new CreateSignatureRecipientDto("Jane Signer", "jane@client.com", null, "Signer", 1, "1234")
        };

        var req = await esigService.CreateAsync(orgId, userId, "Sender", new CreateSignatureRequestDto(doc.Id, "Sign NDA", "Please sign", DateTime.UtcNow.AddDays(7), recipients));
        if (string.IsNullOrWhiteSpace(req.SecurityHash)) throw new Exception("Security hash was not generated.");

        var token = req.Recipients[0].SecurityToken;
        var recipientView = await esigService.GetRecipientByTokenAsync(token);
        if (recipientView.Status != "Viewed") throw new Exception($"Expected Viewed status, got {recipientView.Status}");

        // Submit signature
        var signed = await esigService.SubmitSignatureAsync(token, new SubmitSignatureDto("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=", "1234", "192.168.1.1", "Mozilla/5.0"));
        if (signed.Status != "Signed") throw new Exception($"Expected Signed status, got {signed.Status}");

        var completedReq = await esigService.GetByIdAsync(orgId, req.Id);
        if (completedReq.Status != "Completed") throw new Exception($"Expected request status Completed, got {completedReq.Status}");

        Console.WriteLine("  ✓ E-Signature Request, Token Validation, SHA256 Hash & Signing Test Passed");
    }

    private static async Task TestMultiTenantIsolationAsync()
    {
        var db = CreateInMemoryDbContext("TestDb_MultiTenant");
        var storage = CreateStorageService();
        var auditLogRepo = new DocumentAuditLogRepository(db);
        var auditLogService = new DocumentAuditLogService(auditLogRepo);
        var docRepo = new DocumentRepository(db);
        var docService = new DocumentService(docRepo, storage, auditLogService);

        var orgA = Guid.NewGuid();
        var orgB = Guid.NewGuid();
        var user = Guid.NewGuid();

        using var stream1 = new MemoryStream(Encoding.UTF8.GetBytes("Org A File"));
        await docService.UploadAsync(orgA, user, "UserA", stream1, "orga.pdf", "application/pdf", new UploadDocumentDto(null, null, null, null));

        using var stream2 = new MemoryStream(Encoding.UTF8.GetBytes("Org B File"));
        await docService.UploadAsync(orgB, user, "UserB", stream2, "orgb.pdf", "application/pdf", new UploadDocumentDto(null, null, null, null));

        var searchOrgA = await docService.SearchAsync(orgA, new DocumentSearchQueryDto(null, null, null, null, null, null, false, 1, 10, "UpdatedAt", true));
        if (searchOrgA.TotalCount != 1 || searchOrgA.Items[0].Name != "orga.pdf")
        {
            throw new Exception("Multi-tenant isolation breach detected for Org A!");
        }

        Console.WriteLine("  ✓ Multi-tenant Organization Isolation Security Test Passed");
    }
}
