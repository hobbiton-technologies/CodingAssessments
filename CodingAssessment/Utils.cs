using System.Linq.Expressions;

namespace CodingAssessment;

public static class Utils
{

    public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
}