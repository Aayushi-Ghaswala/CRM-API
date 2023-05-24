using CRM_api.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                else if(propertyType.GenericTypeArguments[0].Name.ToLower() == "datetime")
                {
                    DateTime? nullableDateTime;

                    if (DateTime.TryParse(value.ToString(), out DateTime parsedDateTime))
                    {
                        nullableDateTime = parsedDateTime;
                    }
                    else
                    {
                        nullableDateTime = null;
                    }

                    var constant = Expression.Constant(nullableDateTime, propertyType);
                    comparisonExpression = Expression.Equal(property, constant);
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
                    expression = Expression.And(expression, comparisonExpression);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);

            return dbContext.Set<T>().Where(lambda);
        }

        public static IQueryable<T> Search<T>(this CRMDbContext dbContext, string value) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression expression = null;
            var fieldValues = typeof(T).GetProperties().Where(x => x.PropertyType != typeof(bool));

            foreach (var fieldValue in fieldValues)
            {
                var fieldName = fieldValue.Name;

                var property = Expression.Property(parameter, fieldName);
                var propertyType = property.Type;
                Expression comparisonExpression = null;

                if (((propertyType == typeof(int?) || propertyType == typeof(long?) || propertyType == typeof(int) || propertyType == typeof(long)) && int.TryParse(value, out var intVal)) || ((propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) && DateTime.TryParse(value, out var dateVal)))
                {
                    if (propertyType == typeof(int?))
                        propertyType = typeof(int);
                    if (propertyType == typeof(long?))
                        propertyType = typeof(long);
                    if (propertyType == typeof(DateTime?))
                        propertyType = typeof(DateTime);
                    var convertedValue = Convert.ChangeType(value, propertyType);

                    var constant = Expression.Constant(convertedValue, property.Type);

                    if(property.Type.Name == typeof(Nullable<>).Name)
                    {
                        comparisonExpression = Expression.NotEqual(property, Expression.Constant(null));
                        comparisonExpression = Expression.And(comparisonExpression, Expression.Equal(property, constant));
                    }
                    else
                    {
                        comparisonExpression = Expression.Equal(property, constant);
                    }
                }
                else if(propertyType == typeof(string))
                {
                    var containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var searchValue = Expression.Constant(value, typeof(string));
                    comparisonExpression = Expression.Call(property, containsMethodInfo, searchValue);
                }
                
                if(comparisonExpression != null)    
                {
                    if (expression == null)
                            expression = comparisonExpression;
                    else
                        expression = Expression.Or(expression, comparisonExpression);
                }
            }

            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);

            return dbContext.Set<T>().Where(lambda);
        }
    }
}
