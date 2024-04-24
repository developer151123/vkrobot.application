using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RelationalPropertyExtensions = Microsoft.EntityFrameworkCore.RelationalPropertyExtensions;

namespace vkrobot.application.services.Utilities;

public static class QueryableExtensions
{
    public enum QueryableFilterCompareEnum
    {
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Equal
    };

    public static IQueryable<T> BuildExpression<T>(this IQueryable<T> source, DbContext context, string columnName, string value, QueryableFilterCompareEnum? compare = QueryableFilterCompareEnum.Equal)
    {
        var param = Expression.Parameter(typeof(T));

        // Get the field/column from the Entity that matches the supplied columnName value
        // If the field/column does not exists on the Entity, throw an exception; There is nothing more that can be done
        MemberExpression? dataField;
	
        var model = context.Model.FindEntityType(typeof(T)); // start with our own entity
        var props = model!.GetPropertyAccessors(param); // get all available field names including navigations
        var reference = props.First(p => RelationalPropertyExtensions.GetColumnName(p.Item1) == columnName); // find the filtered column - you might need to handle cases where column does not exist

        dataField = reference.Item2 as MemberExpression; // we happen to already have correct property accessors in our Tuples	

        ConstantExpression constant = !string.IsNullOrWhiteSpace(value)
            ? Expression.Constant(value.Trim(), typeof(string))
            : Expression.Constant(value, typeof(string));

        Expression binary = GetBinaryExpression(dataField!, constant, compare);
        Expression<Func<T, bool>> lambda = (Expression<Func<T, bool>>)Expression.Lambda(binary, param);
        return source.Where(lambda);
    }

    private static Dictionary<QueryableFilterCompareEnum, ExpressionType> _comparisonMapping =
        new Dictionary<QueryableFilterCompareEnum, ExpressionType>
        {
            [QueryableFilterCompareEnum.NotEqual] = ExpressionType.NotEqual,
            [QueryableFilterCompareEnum.GreaterThan] = ExpressionType.GreaterThan,
            [QueryableFilterCompareEnum.GreaterThanOrEqual] = ExpressionType.GreaterThanOrEqual,
            [QueryableFilterCompareEnum.LessThan] = ExpressionType.LessThan,
            [QueryableFilterCompareEnum.LessThanOrEqual] = ExpressionType.LessThanOrEqual,
            [QueryableFilterCompareEnum.Equal] = ExpressionType.Equal
        };

    private static Expression GetBinaryExpression(MemberExpression member, ConstantExpression constant, QueryableFilterCompareEnum? comparisonOperation) {
        QueryableFilterCompareEnum operation = comparisonOperation ?? QueryableFilterCompareEnum.Equal;
        var expressionType = _comparisonMapping[operation];
        return Expression.MakeBinary(
            expressionType,
            member,
            constant
        );
    }
    
    private static IEnumerable<Tuple<IProperty, Expression>> GetPropertyAccessors(this IEntityType model, Expression param)
    {
        var result = new List<Tuple<IProperty, Expression>>();

        result.AddRange(model.GetProperties()
            .Where(p => !p.IsShadowProperty()) // this is your chance to ensure property is actually declared on the type before you attempt building Expression
            .Select(p => new Tuple<IProperty, Expression>(p, Expression.Property(param, p.Name)))); // Tuple is a bit clunky but hopefully conveys the idea

        foreach (var nav in model.GetNavigations().Where(p => p is Navigation))
        {
            var parentAccessor = Expression.Property(param, nav.Name); // define a starting point so following properties would hang off there
            result.AddRange(GetPropertyAccessors(nav.ForeignKey.PrincipalEntityType, parentAccessor)); //recursively call ourselves to travel up the navigation hierarchy
        }

        return result;
    }
}