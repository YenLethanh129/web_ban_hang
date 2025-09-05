using Dashboard.BussinessLogic.Dtos.OrderDtos;
using Dashboard.Common.Enums;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.DataAccess.Specification;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dashboard.BussinessLogic.Specifications;

/// <summary>
/// Specification builder for Order queries based on business logic requirements
/// </summary>
public static class OrderSpecifications
{
    public static Specification<Order> WithBasicIncludes()
    {
        var spec = new Specification<Order>(_ => true);
        spec.IncludeStrings.Add("Customer");
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> WithFullDetails(long? orderId = null)
    {
        Expression<Func<Order, bool>> predicate = o => true;
        
        if (orderId.HasValue)
            predicate = o => o.Id == orderId.Value;

        var spec = new Specification<Order>(predicate);
        spec.IncludeStrings.Add("Customer");
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        spec.IncludeStrings.Add("OrderDetails.Product");
        spec.IncludeStrings.Add("OrderPayments.PaymentMethod");
        spec.IncludeStrings.Add("OrderShipments.DeliveryStatus");
        spec.IncludeStrings.Add("OrderDeliveryTrackings");
        return spec;
    }

    public static Specification<Order> BySearchCriteria(GetOrdersInput input)
    {
        Expression<Func<Order, bool>> predicate = o => true;

        if (!string.IsNullOrEmpty(input.OrderCode))
            predicate = CombinePredicates(predicate, o => o.OrderCode.Contains(input.OrderCode));

        if (input.CustomerId.HasValue)
            predicate = CombinePredicates(predicate, o => o.CustomerId == input.CustomerId.Value);

        if (input.BranchId.HasValue)
            predicate = CombinePredicates(predicate, o => o.BranchId == input.BranchId.Value);

        if (input.StatusId.HasValue)
            predicate = CombinePredicates(predicate, o => o.StatusId == input.StatusId.Value);

        if (input.FromDate.HasValue)
            predicate = CombinePredicates(predicate, o => o.CreatedAt >= input.FromDate.Value);

        if (input.ToDate.HasValue)
            predicate = CombinePredicates(predicate, o => o.CreatedAt.Date <= input.ToDate.Value.Date);

        if (input.MinAmount.HasValue)
            predicate = CombinePredicates(predicate, o => o.TotalMoney >= input.MinAmount.Value);

        if (input.MaxAmount.HasValue)
            predicate = CombinePredicates(predicate, o => o.TotalMoney <= input.MaxAmount.Value);

        var spec = new Specification<Order>(predicate);
        spec.IncludeStrings.Add("Customer");
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> ByDateAndBranch(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        Expression<Func<Order, bool>> predicate = o => 
            o.CreatedAt.Date >= fromDate.Date && 
            o.CreatedAt.Date <= toDate.Date;

        if (branchId.HasValue)
            predicate = CombinePredicates(predicate, o => o.BranchId == branchId.Value);

        var spec = new Specification<Order>(predicate);
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> ByCustomer(long customerId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        Expression<Func<Order, bool>> predicate = o => o.CustomerId == customerId;

        if (fromDate.HasValue)
            predicate = CombinePredicates(predicate, o => o.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            predicate = CombinePredicates(predicate, o => o.CreatedAt.Date <= toDate.Value.Date);

        var spec = new Specification<Order>(predicate);
        spec.IncludeStrings.Add("Customer");
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> ByStatus(long statusId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        Expression<Func<Order, bool>> predicate = o => o.StatusId == statusId;

        if (fromDate.HasValue)
            predicate = CombinePredicates(predicate, o => o.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            predicate = CombinePredicates(predicate, o => o.CreatedAt.Date <= toDate.Value.Date);

        var spec = new Specification<Order>(predicate);
        spec.IncludeStrings.Add("Customer");
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> ForDailySummary(DateTime date, long? branchId = null)
    {
        Expression<Func<Order, bool>> predicate = o => 
            o.CreatedAt.Date == date.Date &&
            o.Status!.Id == (long)OrderStatusEnum.Delivered;

        if (branchId.HasValue)
            predicate = CombinePredicates(predicate, o => o.BranchId == branchId.Value);

        var spec = new Specification<Order>(predicate);
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> ForBranchSummary(DateTime fromDate, DateTime toDate)
    {
        var spec = new Specification<Order>(o => 
            o.CreatedAt.Date >= fromDate.Date && 
            o.CreatedAt.Date <= toDate.Date &&
            o.Status!.Id == (long)OrderStatusEnum.Delivered);
        
        spec.IncludeStrings.Add("Branch");
        spec.IncludeStrings.Add("Status");
        return spec;
    }

    public static Specification<Order> ForRevenueSummary(DateTime fromDate, DateTime toDate, long? branchId = null)
    {
        Expression<Func<Order, bool>> predicate = o =>
            o.CreatedAt.Date >= fromDate.Date &&
            o.CreatedAt.Date <= toDate.Date;

        if (branchId.HasValue)
            predicate = CombinePredicates(predicate, o => o.BranchId == branchId.Value);

        var spec = new Specification<Order>(predicate);

        spec.Includes.Add(q => q.Include(o => o.OrderDetails)
                        .Include(o => o.Branch!)
                        .Include(o => o.Status!));
        return spec;
    }

    private static Expression<Func<Order, bool>> CombinePredicates(
        Expression<Func<Order, bool>> first,
        Expression<Func<Order, bool>> second)
    {
        return SpecificationHelper.CombinePredicates(first, second);
    }
}
