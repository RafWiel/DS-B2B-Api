using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace WebApiService.Extensions
{
    public static partial class LinqExtensions
    {
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);

            return expr;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name, bool descending)
        {
            name = name.Length > 1 ? $"{name[0].ToString().ToUpper()}{name.Substring(1)}" : name;

            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m =>
                descending ?
                    m.Name == "OrderByDescending" :
                    m.Name == "OrderBy"
                && m.GetParameters().Length == 2);

            var genericMethod = method?.MakeGenericMethod(typeof(T), propInfo.PropertyType);

            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name, bool descending)
        {
            name = name.Length > 1 ? $"{name[0].ToString().ToUpper()}{name.Substring(1)}" : name;
            
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => 
                descending ? 
                    m.Name == "OrderByDescending" : 
                    m.Name == "OrderBy" 
                && m.GetParameters().Length == 2);

            var genericMethod = method?.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        public static IOrderedEnumerable<TSource> ThenByWithDirection<TSource, TKey>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            bool descending)
        {
            return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
        }
    }
}
