using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ClaimFlow.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        //Idential to Linq's Include, but only performs inclusion if the condition is true
        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> navigationPropertyPath)
            where T : class
        {
            return condition ? source.Include(navigationPropertyPath) : source;
        }
    }
}
