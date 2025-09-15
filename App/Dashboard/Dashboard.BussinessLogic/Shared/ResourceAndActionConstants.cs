using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Shared
{
    public static class ResourceAndActionConstants
    {
        public static class Resources
        {
            public const string Products = "Products";
            public const string Categories = "Categories";
            public const string Employees = "Employees";
            public const string FinancialReports = "FinancialReports";
            public const string Ingredients = "Ingredients";
            public const string Suppliers = "Suppliers";
            public const string Customers = "Customers";
            public const string Inventory = "Inventory";
            public const string Shipments = "Shipments";
            public const string Invoices = "Invoices";
            public const string Reviews = "Reviews";
            public const string Analytics = "Analytics";
            public const string Branchs = "Brannchs";
            public const string Orders = "Orders";
            public const string Users = "Users";
            public const string Roles = "Roles";
            public const string Permissions = "Permissions";
        }
        public static class Actions
        {
            public const string Create = "Create";
            public const string Read = "Read";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string Manage = "Manage"; 
        }
    }
}
