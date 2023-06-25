using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using static Core.Repository.Filter.FilterUtility;

namespace Core.Repository.Filter
{
    public static class QueryExtention
    {
        #region Filter
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, List<FilterModel> filters)
        {
            var predicate = PredicateBuilder.New<T>();
            foreach(var filter in filters)
            {
                if (filter.Filters.Any())
                {
                    if (predicate.IsStarted)
                    {
                        predicate = predicate.And(FilterExp<T>(filter.Filters, filter.Logic));
                    }
                    else
                    {
                        predicate = FilterExp<T>(filter.Filters, filter.Logic);
                    }
                }
            }
            return query.AsExpandable().Where(predicate);
        }

        private static Expression<Func<T, bool>> FilterExp<T>(List<FilterParams> filterParams, string logic)
        {
            var predicate = PredicateBuilder.New<T>();
            IEnumerable<string> distinctColumns = filterParams.Where(x => !String.IsNullOrEmpty(x.Field)).Select(x => x.Field).Distinct();
            Expression<Func<T, bool>> expression = null;
            if (distinctColumns.Any())
            {
                foreach(var colName in distinctColumns)
                {
                    var filterColumn = typeof(T).GetProperty(colName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
                    if(filterColumn != null)
                    {
                        IEnumerable<FilterParams> filterValues = filterParams.Where(x => x.Field.Equals(colName)).Distinct();
                        if(filterValues.Count() > 1)
                        {
                            foreach(var val in filterValues)
                            {
                                expression = FilterData<T>(filterColumn, val.Value, val.Operator);
                                if(expression != null)
                                {
                                    if(logic == "and")
                                    {
                                        predicate = predicate.And(expression);
                                    }
                                    if (logic == "or")
                                    {
                                        predicate = predicate.Or(expression);
                                    }
                                }
                            }
                        }
                        else
                        {
                            expression = FilterData<T>(filterColumn, filterValues.FirstOrDefault().Value, filterValues.FirstOrDefault().Operator);
                            if(expression != null)
                            {
                                if (logic == "and")
                                {
                                    predicate = predicate.And(expression);
                                }
                                else if (logic == "or")
                                {
                                    predicate = predicate.Or(expression);
                                }
                                else
                                {
                                    predicate = predicate.And(expression);
                                }
                            }
                        }
                    }
                }
            }
            return predicate;
        }
        private static Expression<Func<T, bool>> FilterData<T>(PropertyInfo prop, string value, FilterOptions filterOptions)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T));
                Expression memberAccess = Expression.Property(parameter, prop);
                int outIntValue = 0;
                decimal outDesValue = 0;
                DateTime outDateValue = new DateTime();

                bool propertyIsNullableValueType = memberAccess.Type.IsGenericType && memberAccess.Type.GetGenericTypeDefinition() == typeof(Nullable<>);
                Type propertyBasicType = propertyIsNullableValueType ? memberAccess.Type.GetGenericArguments().Single() : memberAccess.Type;

                string Operation = filterOptions.ToString();
                ConstantExpression constant;
                if (new[] {"equals", "equal", "notequals","notequal","greater", "greaterequal","less", "lessequal",
                "eq","neq","lt","lte", "gt","gte","isnull","isnotnull","isempty","isnotempty"}.Contains(Operation, StringComparer.OrdinalIgnoreCase))
                {
                    object convertedValue;
                    if (new[] {"isnull","isnotnull", "isempty", "isnotempty" }.Contains(Operation, StringComparer.OrdinalIgnoreCase))
                    {
                        value = null;
                    }
                    if(value != null || propertyBasicType.IsInstanceOfType(value))
                    {
                        convertedValue = value;
                    }
                    else if(propertyBasicType == typeof(Guid) && value is string)
                    {
                        convertedValue = Guid.Parse(value.ToString());
                    }
                    else if(propertyBasicType == typeof(DateTime) || propertyBasicType == typeof(Nullable<DateTime>)
                            && DateTime.TryParse(value, out outDateValue) && value is string)
                    {
                        //convertedValue = ParseJsonDateTime((string)value, prop.Name, propertyBasicType);
                        convertedValue = outDateValue;
                    }
                    else if (propertyBasicType == typeof(int) || propertyBasicType == typeof(Nullable<int>)
                            && int.TryParse(value, out outIntValue) && value is string)
                    {
                        //convertedValue = ParseJsonDateTime((string)value, prop.Name, propertyBasicType);
                        convertedValue = outIntValue;
                    }
                    else if (propertyBasicType == typeof(Decimal) || propertyBasicType == typeof(Nullable<Decimal>)
                            && Decimal.TryParse(value, out outDesValue) && value is string)
                    {
                        //convertedValue = ParseJsonDateTime((string)value, prop.Name, propertyBasicType);
                        convertedValue = outDesValue;
                    }
                    else
                    {
                        convertedValue = Convert.ChangeType(value, propertyBasicType);
                    }
                    if(convertedValue == null && memberAccess.Type.IsValueType && !propertyIsNullableValueType) 
                    {
                        Type nullableMemberType = typeof(Nullable<>).MakeGenericType(memberAccess.Type);
                        memberAccess = Expression.Convert(memberAccess, nullableMemberType);
                    }
                    constant = Expression.Constant(value.ToString(), typeof(string));
                }
                else if (new[] { "startwith","endwith","contains","notcontains", "doesnotcontains"}
                            .Contains(Operation, StringComparer.OrdinalIgnoreCase))
                {
                    constant = Expression.Constant(value.ToString(), typeof(string));
                }
                else
                {
                    throw new Exception($"unsupported generic filter option '{Operation}' on a property");
                }

                Expression expMethod = null;
                switch (filterOptions)
                {
                    case FilterOptions.startwith:
                        expMethod = Expression.Call(memberAccess
                                                , typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })
                                                , constant);
                        break;
                    case FilterOptions.endwith:
                        expMethod = Expression.Call(memberAccess
                                                , typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) })
                                                , constant);
                        break;
                    case FilterOptions.contains:
                    case FilterOptions.doesnotcontain:
                        var cconMethod = Expression.Call(memberAccess
                                                , typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                                                , constant);
                        if (Operation.Equals(FilterOptions.contains.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            expMethod = cconMethod;
                        }
                        if (Operation.Equals(FilterOptions.doesnotcontain.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            expMethod = Expression.Not(cconMethod);
                        }
                        break;

                    case FilterOptions.isnotnull:
                    case FilterOptions.isnotempty:
                    case FilterOptions.isempty:
                    case FilterOptions.isnull:
                        Expression empMethod = null;
                        if (propertyBasicType == typeof(string))
                        {
                            empMethod = Expression.Call(typeof(string), nameof(string.IsNullOrEmpty), null, memberAccess);
                        }
                        else
                        {
                            empMethod = Expression.Equal(memberAccess, constant);
                        }
                        if (Operation.Equals(FilterOptions.isempty.ToString(), StringComparison.OrdinalIgnoreCase)
                            || Operation.Equals(FilterOptions.isnull.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            expMethod = empMethod;
                        }
                        if (Operation.Equals(FilterOptions.isnotempty.ToString(), StringComparison.OrdinalIgnoreCase)
                            || Operation.Equals(FilterOptions.isnotnull.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            expMethod = Expression.Not(empMethod);
                        }
                            break;

                    case FilterOptions.gt:
                        expMethod = Expression.GreaterThan(memberAccess, constant);
                        break;
                    case FilterOptions.gte:
                        expMethod = Expression.GreaterThanOrEqual(memberAccess, constant);
                        break;
                    case FilterOptions.lt:
                        expMethod = Expression.LessThan(memberAccess, constant);
                        break;
                    case FilterOptions.lte:
                        expMethod = Expression.LessThanOrEqual(memberAccess, constant);
                        break;
                    case FilterOptions.eq:
                        if(value == string.Empty)
                        {
                            expMethod = Expression.Call(memberAccess
                                               , typeof(string).GetMethod(nameof(string.IsNullOrWhiteSpace), new[] { typeof(string) })
                                               , constant);
                        }
                        else
                        {
                            expMethod = Expression.Equal(memberAccess, constant);
                        }
                        break;
                        case FilterOptions.neq: 
                        expMethod = Expression.NotEqual(memberAccess, constant);
                        break;
                }
                return Expression.Lambda<Func<T, bool>>(expMethod,parameter);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static DateTime ParseJsonDateTime(string text, string infoPropertyName, Type infoPropertyType)
        {
            string dateString = "\"" + text.Replace("/", "\\/") + "\"";
            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(dateString))) 
            {
                var serializer = new DataContractJsonSerializer(typeof(DateTime));
                try
                {
                    return (DateTime)serializer.ReadObject(stream);
                }
                catch (Exception ex)
                {

                    throw new Exception($"Invalid json format of {infoPropertyType.Name} propert '{infoPropertyName}'", ex);
                }
            }
        }

        #endregion

        #region Sort
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, List<SortParam> sorts)
        {
            if(sorts == null)
                return query;
            var applicableSorts = sorts.Where(s => s != null);
            if(!applicableSorts.Any())
                return query;
            applicableSorts.Select((item, index) => new { Index = index, item.Field, item.Dir }).ToList().ForEach(sort =>
            {
                ParameterExpression parameterExpression = Expression.Parameter(query.ElementType, "entity");
                var propertyExpression = Expression.Property(parameterExpression, sort.Field);
                var sortPredicate = Expression.Lambda(propertyExpression, parameterExpression);
                string methodName = (sort.Index == 0 ? "Order" : "Then") + (sort.Dir == "asc" ? "By" : "ByDesecending");
                MethodCallExpression orderBy = Expression.Call(typeof(Queryable),
                                                                methodName,
                                                                new Type[] { query.ElementType, propertyExpression.Type },
                                                                query.Expression,
                                                                Expression.Quote(sortPredicate));
                query = query.Provider.CreateQuery<T>(orderBy);
            });
            return query;
        }
        #endregion
    }
}
