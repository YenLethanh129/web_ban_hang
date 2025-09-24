namespace Dashboard.Winform.Events
{
    using Dashboard.Winform.ViewModels.EmployeeModels;
    using System;
    using System.Collections.Generic;

    public class EmployeesLoadedEventArgs : EventArgs
    {
        public List<EmployeeViewModel> Employees { get; }

        public EmployeesLoadedEventArgs(List<EmployeeViewModel> employees)
        {
            Employees = employees;
        }
    }
}

