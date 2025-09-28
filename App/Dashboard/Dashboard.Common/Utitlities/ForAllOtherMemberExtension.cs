using System.Runtime.CompilerServices;
using AutoMapper.Configuration;
using AutoMapper;
#if NET8_0_OR_GREATER
#else
using System.Reflection;
#endif

public static class AutoMapperExtensions
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_TypeMapActions")]
    private static extern List<Action<TypeMap>> GetTypeMapActions(TypeMapConfiguration typeMapConfiguration);
#else
    private static readonly PropertyInfo TypeMapActionsProperty = typeof(TypeMapConfiguration).GetProperty("TypeMapActions", BindingFlags.NonPublic | BindingFlags.Instance)!;
#endif

    public static void ForAllOtherMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Action<IMemberConfigurationExpression<TSource, TDestination, object>> memberOptions)
    {
        var typeMapConfiguration = (TypeMapConfiguration)expression;

#if NET8_0_OR_GREATER
        var typeMapActions = GetTypeMapActions(typeMapConfiguration);
#else
        var typeMapActions = (List<Action<TypeMap>>)TypeMapActionsProperty.GetValue(typeMapConfiguration)!;
#endif

        typeMapActions.Add(typeMap =>
        {
            var destinationTypeDetails = typeMap.DestinationTypeDetails;

            foreach (var accessor in destinationTypeDetails.WriteAccessors.Where(m => typeMapConfiguration.GetDestinationMemberConfiguration(m) == null))
            {
                expression.ForMember(accessor.Name, memberOptions);
            }
        });
    }
}