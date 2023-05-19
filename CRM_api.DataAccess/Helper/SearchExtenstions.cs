using CRM_api.DataAccess.Context;
using System.Linq.Expressions;

namespace CRM_api.DataAccess.Helper
{
    public static class SearchExtenstions
    {
        public static IQueryable<T> SearchByField<T>(this CRMDbContext dbContext, Dictionary<string, object> fieldValues) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression expression = null;

            foreach (var fieldValue in fieldValues)
            {
                var fieldName = fieldValue.Key;
                var value = fieldValue.Value;

                var property = Expression.Property(parameter, fieldName);
                var propertyType = property.Type;

                Expression comparisonExpression;

                if (propertyType == typeof(string) && value is string stringValue)
                {
                    var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var searchValue = Expression.Constant(stringValue, typeof(string));
                    comparisonExpression = Expression.Call(property, methodInfo, searchValue);
                }
                else
                {
                    var convertedValue = Convert.ChangeType(value, propertyType);

                    var constant = Expression.Constant(convertedValue, propertyType);
                    comparisonExpression = Expression.Equal(property, constant);
                }

                if (expression == null)
                    expression = comparisonExpression;
                else
                    expression = Expression.Or(expression, comparisonExpression);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);

            return dbContext.Set<T>().Where(lambda);
        }
    }
}
