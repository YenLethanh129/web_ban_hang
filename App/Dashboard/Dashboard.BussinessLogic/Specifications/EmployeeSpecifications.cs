using Dashboard.BussinessLogic.Dtos.EmployeeDtos;
using Dashboard.DataAccess.Models.Entities.Employees;
using Dashboard.DataAccess.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Specifications
{
    public static class EmployeeSpecifications
    {
        public static Specification<Employee> BySearchCriteria(GetEmployeesInput input)
        {
            var spec = new Specification<Employee>(e => true);
            if (input.BranchId.HasValue)
            {
                spec = new Specification<Employee>(e => e.BranchId == input.BranchId.Value);
            }
            if (!string.IsNullOrEmpty(input.SearchTerm))
            {
                spec = new Specification<Employee>(e => e.FullName.Contains(input.SearchTerm) || 
                                                        (e.Status != null && e.Status.Contains(input.SearchTerm)) ||
                                                        (e.Email != null && e.Email.Contains(input.SearchTerm)) || 
                                                        (e.PhoneNumber != null && e.PhoneNumber.Contains(input.SearchTerm)) ||
                                                        (e.Position != null && e.Position.Name.Contains(input.SearchTerm)));
            }
            if (input.HiredAfter.HasValue)
            {
                spec = new Specification<Employee>(e => e.HireDate >= input.HiredAfter.Value);
            }
            if (input.HiredBefore.HasValue)
            {
                spec = new Specification<Employee>(e => e.HireDate <= input.HiredBefore.Value);
            }
            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = new Specification<Employee>(e => e.ResignDate == null);
                }
                else
                {
                    spec = new Specification<Employee>(e => e.ResignDate != null);
                }
            }
            return spec;

        }

        public static Specification<Employee> WithEmployeeBranchInclude()
        {
            var spec = new Specification<Employee>(e => true);
            spec.IncludeStrings.Add("Branch");
            return spec;
        }

        public static Specification<Employee> WithEmployeeSalaryInclude(long employeeId)
        {
            var spec = new Specification<Employee>(e => e.Id == employeeId);
            spec.IncludeStrings.Add("EmployeeSalaries");
            return spec;
        }
        public static Specification<Employee> WithEmployeeShiftInclude(long employeeId)
        {
            var spec = new Specification<Employee>(e => e.Id == employeeId);
            spec.IncludeStrings.Add("EmployeeShifts");
            return spec;    
        }
        
        public static Specification<Employee> ByBranchId(long branchId)
        {
            var spec = new Specification<Employee>(e => e.BranchId == branchId);
            return spec;
        }
    }
}
