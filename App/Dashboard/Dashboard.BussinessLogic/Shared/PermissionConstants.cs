namespace Dashboard.BussinessLogic.Shared;

public static class Permissions
{
    // System Management (ADMIN only)
    public const string MANAGE_USERS = "MANAGE_USERS";
    public const string MANAGE_ROLES = "MANAGE_ROLES";
    public const string MANAGE_PERMISSIONS = "MANAGE_PERMISSIONS";
    public const string MANAGE_BRANCHES = "MANAGE_BRANCHES";
    public const string MANAGE_SUPPLIERS = "MANAGE_SUPPLIERS";
    public const string VIEW_SYSTEM_REPORTS = "VIEW_SYSTEM_REPORTS";
    public const string MANAGE_SYSTEM_SETTINGS = "MANAGE_SYSTEM_SETTINGS";

    // Employee Management (ADMIN + MANAGER)
    public const string MANAGE_EMPLOYEES = "MANAGE_EMPLOYEES";
    public const string MANAGE_PAYROLL = "MANAGE_PAYROLL";
    public const string MANAGE_EMPLOYEE_SHIFTS = "MANAGE_EMPLOYEE_SHIFTS";
    public const string VIEW_EMPLOYEE_REPORTS = "VIEW_EMPLOYEE_REPORTS";
    public const string MANAGE_EMPLOYEE_PERFORMANCE = "MANAGE_EMPLOYEE_PERFORMANCE";

    // Product & Inventory Management (ADMIN + MANAGER + EMPLOYEE)
    public const string MANAGE_PRODUCTS = "MANAGE_PRODUCTS";
    public const string MANAGE_INGREDIENTS = "MANAGE_INGREDIENTS";
    public const string MANAGE_INVENTORY = "MANAGE_INVENTORY";
    public const string VIEW_INVENTORY_REPORTS = "VIEW_INVENTORY_REPORTS";
    public const string PROCESS_ORDERS = "PROCESS_ORDERS";
    public const string MANAGE_PURCHASE_ORDERS = "MANAGE_PURCHASE_ORDERS";

    // Sales & Customer Management (ADMIN + MANAGER + EMPLOYEE)
    public const string MANAGE_ORDERS = "MANAGE_ORDERS";
    public const string MANAGE_CUSTOMERS = "MANAGE_CUSTOMERS";
    public const string VIEW_SALES_REPORTS = "VIEW_SALES_REPORTS";
    public const string PROCESS_PAYMENTS = "PROCESS_PAYMENTS";

    // Financial Management (ADMIN + MANAGER)
    public const string MANAGE_EXPENSES = "MANAGE_EXPENSES";
    public const string VIEW_FINANCIAL_REPORTS = "VIEW_FINANCIAL_REPORTS";
    public const string MANAGE_INVOICES = "MANAGE_INVOICES";
    public const string VIEW_PROFIT_REPORTS = "VIEW_PROFIT_REPORTS";

    // Basic Operations (ALL)
    public const string VIEW_DASHBOARD = "VIEW_DASHBOARD";
    public const string UPDATE_PROFILE = "UPDATE_PROFILE";
    public const string VIEW_OWN_SHIFTS = "VIEW_OWN_SHIFTS";
    public const string VIEW_BASIC_REPORTS = "VIEW_BASIC_REPORTS";
}

public static class Roles
{
    public const string ADMIN = "ADMIN";
    public const string MANAGER = "MANAGER";
    public const string EMPLOYEE = "EMPLOYEE";
}

public static class Resources
{
    public const string SYSTEM = "SYSTEM";
    public const string USERS = "USERS";
    public const string EMPLOYEES = "EMPLOYEES";
    public const string PRODUCTS = "PRODUCTS";
    public const string INVENTORY = "INVENTORY";
    public const string ORDERS = "ORDERS";
    public const string CUSTOMERS = "CUSTOMERS";
    public const string FINANCES = "FINANCES";
    public const string REPORTS = "REPORTS";
    public const string BRANCHES = "BRANCHES";
    public const string SUPPLIERS = "SUPPLIERS";
}

public static class Actions
{
    public const string CREATE = "CREATE";
    public const string READ = "READ";
    public const string UPDATE = "UPDATE";
    public const string DELETE = "DELETE";
    public const string MANAGE = "MANAGE";
    public const string VIEW = "VIEW";
    public const string PROCESS = "PROCESS";
}

public static class RolePermissions
{
    public static readonly Dictionary<string, List<string>> DefaultPermissions = new()
    {
        [Roles.ADMIN] = new List<string>
        {
            // System Management
            Permissions.MANAGE_USERS,
            Permissions.MANAGE_ROLES,
            Permissions.MANAGE_PERMISSIONS,
            Permissions.MANAGE_BRANCHES,
            Permissions.MANAGE_SUPPLIERS,
            Permissions.VIEW_SYSTEM_REPORTS,
            Permissions.MANAGE_SYSTEM_SETTINGS,
            
            // Employee Management
            Permissions.MANAGE_EMPLOYEES,
            Permissions.MANAGE_PAYROLL,
            Permissions.MANAGE_EMPLOYEE_SHIFTS,
            Permissions.VIEW_EMPLOYEE_REPORTS,
            Permissions.MANAGE_EMPLOYEE_PERFORMANCE,
            
            // Product & Inventory
            Permissions.MANAGE_PRODUCTS,
            Permissions.MANAGE_INGREDIENTS,
            Permissions.MANAGE_INVENTORY,
            Permissions.VIEW_INVENTORY_REPORTS,
            Permissions.PROCESS_ORDERS,
            Permissions.MANAGE_PURCHASE_ORDERS,
            
            // Sales & Customer
            Permissions.MANAGE_ORDERS,
            Permissions.MANAGE_CUSTOMERS,
            Permissions.VIEW_SALES_REPORTS,
            Permissions.PROCESS_PAYMENTS,
            
            // Financial
            Permissions.MANAGE_EXPENSES,
            Permissions.VIEW_FINANCIAL_REPORTS,
            Permissions.MANAGE_INVOICES,
            Permissions.VIEW_PROFIT_REPORTS,
            
            // Basic
            Permissions.VIEW_DASHBOARD,
            Permissions.UPDATE_PROFILE,
            Permissions.VIEW_OWN_SHIFTS,
            Permissions.VIEW_BASIC_REPORTS
        },
        
        [Roles.MANAGER] = new List<string>
        {
            // Employee Management
            Permissions.MANAGE_EMPLOYEES,
            Permissions.MANAGE_PAYROLL,
            Permissions.MANAGE_EMPLOYEE_SHIFTS,
            Permissions.VIEW_EMPLOYEE_REPORTS,
            Permissions.MANAGE_EMPLOYEE_PERFORMANCE,
            
            // Product & Inventory
            Permissions.MANAGE_PRODUCTS,
            Permissions.MANAGE_INGREDIENTS,
            Permissions.MANAGE_INVENTORY,
            Permissions.VIEW_INVENTORY_REPORTS,
            Permissions.PROCESS_ORDERS,
            Permissions.MANAGE_PURCHASE_ORDERS,
            
            // Sales & Customer
            Permissions.MANAGE_ORDERS,
            Permissions.MANAGE_CUSTOMERS,
            Permissions.VIEW_SALES_REPORTS,
            Permissions.PROCESS_PAYMENTS,
            
            // Financial
            Permissions.MANAGE_EXPENSES,
            Permissions.VIEW_FINANCIAL_REPORTS,
            Permissions.MANAGE_INVOICES,
            Permissions.VIEW_PROFIT_REPORTS,
            
            // Basic
            Permissions.VIEW_DASHBOARD,
            Permissions.UPDATE_PROFILE,
            Permissions.VIEW_OWN_SHIFTS,
            Permissions.VIEW_BASIC_REPORTS
        },
        
        [Roles.EMPLOYEE] = new List<string>
        {
            // Product & Inventory (Limited)
            Permissions.MANAGE_INVENTORY,
            Permissions.VIEW_INVENTORY_REPORTS,
            Permissions.PROCESS_ORDERS,
            
            // Sales & Customer
            Permissions.MANAGE_ORDERS,
            Permissions.MANAGE_CUSTOMERS,
            Permissions.VIEW_SALES_REPORTS,
            Permissions.PROCESS_PAYMENTS,
            
            // Basic
            Permissions.VIEW_DASHBOARD,
            Permissions.UPDATE_PROFILE,
            Permissions.VIEW_OWN_SHIFTS,
            Permissions.VIEW_BASIC_REPORTS
        }
    };
}
