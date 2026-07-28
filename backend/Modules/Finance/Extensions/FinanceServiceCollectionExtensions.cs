using backend.Modules.Accounting.Interfaces;
using backend.Modules.Accounting.Repositories;
using backend.Modules.Accounting.Services;
using backend.Modules.AccountsPayable.Interfaces;
using backend.Modules.AccountsPayable.Repositories;
using backend.Modules.AccountsPayable.Services;
using backend.Modules.AccountsReceivable.Interfaces;
using backend.Modules.AccountsReceivable.Repositories;
using backend.Modules.AccountsReceivable.Services;
using backend.Modules.CashFlow.Interfaces;
using backend.Modules.CashFlow.Services;
using backend.Modules.Expenses.Interfaces;
using backend.Modules.Expenses.Repositories;
using backend.Modules.Expenses.Services;
using backend.Modules.Finance.Interfaces;
using backend.Modules.Finance.Services;
using backend.Modules.FinancialReports.Interfaces;
using backend.Modules.FinancialReports.Services;
using backend.Modules.Invoices.Interfaces;
using backend.Modules.Invoices.Repositories;
using backend.Modules.Invoices.Services;
using backend.Modules.Payments.Interfaces;
using backend.Modules.Payments.Repositories;
using backend.Modules.Payments.Services;
using backend.Modules.Taxes.Interfaces;
using backend.Modules.Taxes.Repositories;
using backend.Modules.Taxes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Modules.Finance.Extensions;

public static class FinanceServiceCollectionExtensions
{
    public static IServiceCollection AddFinanceModule(this IServiceCollection services)
    {
        // 1. Accounting
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountingService, AccountingService>();

        // 2. Expenses
        services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IReceiptStorageService, LocalReceiptStorageService>();
        services.AddScoped<IExpenseService, ExpenseService>();

        // 3. Invoices
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IInvoiceService, InvoiceService>();

        // 4. Payments
        services.AddScoped<IFinancePaymentRepository, FinancePaymentRepository>();
        services.AddScoped<IFinancePaymentService, FinancePaymentService>();

        // 5. Accounts Receivable
        services.AddScoped<IAccountsReceivableRepository, AccountsReceivableRepository>();
        services.AddScoped<IAccountsReceivableService, AccountsReceivableService>();

        // 6. Accounts Payable
        services.AddScoped<IAccountsPayableRepository, AccountsPayableRepository>();
        services.AddScoped<IAccountsPayableService, AccountsPayableService>();

        // 7. Cash Flow Engine
        services.AddScoped<ICashFlowEngine, CashFlowEngine>();

        // 8. Tax Engine
        services.AddScoped<ITaxRecordRepository, TaxRecordRepository>();
        services.AddScoped<ITaxEngine, TaxEngine>();

        // 9. Financial Reports
        services.AddScoped<IFinancialReportGenerator, FinancialReportGenerator>();

        // 10. Finance Overview
        services.AddScoped<IFinanceOverviewService, FinanceOverviewService>();

        return services;
    }
}
