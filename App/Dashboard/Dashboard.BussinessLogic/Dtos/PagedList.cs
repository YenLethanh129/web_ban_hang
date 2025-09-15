using AutoMapper;

namespace Dashboard.BussinessLogic.Dtos;

public class PagedList<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }

    public int TotalPages => (PageSize + TotalRecords - 1) / PageSize;
    public bool HasNextPage => PageNumber * PageSize < TotalRecords;
    public bool HasPreviousPage => PageNumber > 1;

    public IEnumerable<T> Items { get; set; } = new List<T>();
}

public class PagedListToListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, List<TDestination>>
{
    public List<TDestination> Convert(PagedList<TSource> source, List<TDestination> destination, ResolutionContext context)
    {
        var list = new List<TDestination>();
        if (source == null)
            return list;

        IEnumerable<TSource>? items = null;

        if (source is IEnumerable<TSource> seq)
        {
            items = seq;
        }
        else
        {
            var type = source.GetType();
            var prop = type.GetProperty("Items") ?? type.GetProperty("Data") ?? type.GetProperty("Results") ?? type.GetProperty("List");
            if (prop != null)
            {
                var val = prop.GetValue(source);
                items = val as IEnumerable<TSource>;
            }
        }

        if (items == null)
            return list;

        foreach (var item in items)
        {
            list.Add(context.Mapper.Map<TDestination>(item));
        }

        return list;
    }
}
