using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFiltering<T>(
            this IQueryable<T> query,
            List<FilterCriteria> filters)
        {
            if (filters == null || !filters.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression filterExpression = null;

            foreach (var filter in filters)
            {
                var property = Expression.Property(parameter, filter.PropertyName);
                var propertyType = ((PropertyInfo)property.Member).PropertyType;
                
                object convertedValue;
                if (propertyType == typeof(Guid))
                {
                    if (!Guid.TryParse(filter.Value, out Guid guidValue))
                        continue; // Skip this filter if Guid parsing fails
                    convertedValue = guidValue;
                }
                else
                {
                    try
                    {
                        convertedValue = Convert.ChangeType(filter.Value, propertyType);
                    }
                    catch
                    {
                        continue; // Skip this filter if conversion fails
                    }
                }

                var value = Expression.Constant(convertedValue);

                Expression comparison = filter.Operation.ToLower() switch
                {
                    "eq" => Expression.Equal(property, value),
                    "neq" => Expression.NotEqual(property, value),
                    "gt" => Expression.GreaterThan(property, value),
                    "lt" => Expression.LessThan(property, value),
                    "contains" => Expression.Call(property,
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        value),
                    _ => null
                };

                filterExpression = filterExpression == null
                    ? comparison
                    : Expression.AndAlso(filterExpression, comparison);
            }

            if (filterExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query,
            string sortBy,
            bool isDescending)
        {
            if (string.IsNullOrEmpty(sortBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = isDescending ? "OrderByDescending" : "OrderBy";
            var genericMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, lambda });
        }

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            QueryParameters parameters)
        {
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasNextPage = parameters.PageNumber < (int)Math.Ceiling(totalItems / (double)parameters.PageSize),
                HasPreviousPage = parameters.PageNumber > 1
            };
        }

        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return query;

            var stringProperties = typeof(T)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression searchExpression = null;

            foreach (var property in stringProperties)
            {
                var propertyAccess = Expression.Property(parameter, property);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchConstant = Expression.Constant(searchTerm);
                var containsExpression = Expression.Call(propertyAccess, containsMethod, searchConstant);

                searchExpression = searchExpression == null
                    ? containsExpression
                    : Expression.OrElse(searchExpression, containsExpression);
            }

            if (searchExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }
    }
}
