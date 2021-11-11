using System;
using System.Linq.Expressions;
using LinqToDB.Linq;
using LinqToDB.SqlQuery;
using PN = LinqToDB.ProviderName;

namespace LinqToDB
{
	partial class Sql
	{
		// Can't be close to Overlaps method definition because SqlRow is generic
		private class OverlapsAttribute : ExpressionAttribute
		{
			public OverlapsAttribute(string configuration)
				: base(configuration, "{0} OVERLAPS {1}")
			{
				ServerSideOnly = true;
				IsPredicate = true;
			}

			public override ISqlExpression? GetExpression<TContext>(TContext context, IDataContext dataContext, SelectQuery query,
				Expression expression, Func<TContext, Expression, Mapping.ColumnDescriptor?, ISqlExpression> converter)
			{				
				string? exprStr = Expression;
				PrepareParameterValues(expression, ref exprStr, true, out var knownExpressions, out var genericTypes);
				
				// The main purpose of this derived ExpressionAttribute is to validate that types are valid.
				// SQL standard only defines OVERLAPS on couples of (nullable) dates, or date + interval types.
				if (!IsValidRow(knownExpressions![0]!) || !IsValidRow(knownExpressions![1]!))
					throw new LinqException("OVERLAPS only works with dates and interval types");

				var parameters = PrepareArguments(context, exprStr!, ArgIndices, addDefault: false, knownExpressions, genericTypes, converter);
				return new SqlExpression(typeof(bool), exprStr!, SqlQuery.Precedence.Comparison, SqlFlags.IsPredicate | SqlFlags.IsPure, parameters);

				bool IsValidRow(Expression e) {
					var rowType = e.Type;
					return IsValidType(rowType.GenericTypeArguments[0])
					    && IsValidType(rowType.GenericTypeArguments[1], allowTimeSpan: true);
				}

				bool IsValidType(Type type, bool allowTimeSpan = false) {
					// Nullable types are ok, unwrap them
					type = Nullable.GetUnderlyingType(type) ?? type;
					// .NET 6: add DateOnly (possibly TimeOnly?) to this check
					return type == typeof(DateTime)
					    || type == typeof(DateTimeOffset)						
						|| (allowTimeSpan && type == typeof(TimeSpan));
				}
			}
		}

		public abstract class SqlRow<T1, T2>
		{ 
			// Prevent someone from inheriting this class and creating instances.
			// This class is never instantiated and its operators are never actually called.
			// It's all just for typing in LINQ expressions that will translate to SQL.
			private SqlRow() {}

			public static bool operator > (SqlRow<T1, T2> x, SqlRow<T1, T2> y)
				=> throw new NotImplementedException();

			public static bool operator < (SqlRow<T1, T2> x, SqlRow<T1, T2> y)
				=> throw new NotImplementedException();

			public static bool operator >= (SqlRow<T1, T2> x, SqlRow<T1, T2> y)
				=> throw new NotImplementedException();

			public static bool operator <= (SqlRow<T1, T2> x, SqlRow<T1, T2> y)
				=> throw new NotImplementedException();

			[Overlaps(PN.DB2)]
			[Overlaps(PN.Oracle)]
			[Overlaps(PN.PostgreSQL)]
			public bool Overlaps<T3, T4>(SqlRow<T3, T4> other)
				=> throw new NotImplementedException();
		}

		[Sql.Expression(PN.Informix, "ROW ({0}, {1})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1})", ServerSideOnly = true)]
		public static SqlRow<T1, T2> Row<T1, T2>(T1 value1, T2 value2)
			=> throw new LinqException("Row is only server-side method.");

		// Nesting SqlRow looks inefficient, but it will never actually be instantiated.
		// It's only for static typing and it's good enough for that purpose 
		// without creating lots of types and operators.
		[Sql.Expression(PN.Informix, "ROW ({0}, {1}, {2})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1}, {2})", ServerSideOnly = true)]
		public static SqlRow<T1, SqlRow<T2, T3>> Row<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
			=> throw new LinqException("Row is only server-side method.");

		[Sql.Expression(PN.Informix, "ROW ({0}, {1}, {2}, {3})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1}, {2}, {3})", ServerSideOnly = true)]
		public static SqlRow<T1, SqlRow<T2, SqlRow<T3, T4>>> Row<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
			=> throw new LinqException("Row is only server-side method.");

		[Sql.Expression(PN.Informix, "ROW ({0}, {1}, {2}, {3}, {4})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1}, {2}, {3}, {4})", ServerSideOnly = true)]
		public static SqlRow<T1, SqlRow<T2, SqlRow<T3, SqlRow<T4, T5>>>> Row<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
			=> throw new LinqException("Row is only server-side method.");

		[Sql.Expression(PN.Informix, "ROW ({0}, {1}, {2}, {3}, {4}, {5})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1}, {2}, {3}, {4}, {5})", ServerSideOnly = true)]
		public static SqlRow<T1, SqlRow<T2, SqlRow<T3, SqlRow<T4, SqlRow<T5, T6>>>>> Row<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
			=> throw new LinqException("Row is only server-side method.");

		[Sql.Expression(PN.Informix, "ROW ({0}, {1}, {2}, {3}, {4}, {5}, {6})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1}, {2}, {3}, {4}, {5}, {6})", ServerSideOnly = true)]
		public static SqlRow<T1, SqlRow<T2, SqlRow<T3, SqlRow<T4, SqlRow<T5, SqlRow<T6, T7>>>>>> Row<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
			=> throw new LinqException("Row is only server-side method.");

		[Sql.Expression(PN.Informix, "ROW ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", ServerSideOnly = true)]
		[Sql.Expression("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", ServerSideOnly = true)]
		public static SqlRow<T1, SqlRow<T2, SqlRow<T3, SqlRow<T4, SqlRow<T5, SqlRow<T6, SqlRow<T7, T8>>>>>>> Row<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
			=> throw new LinqException("Row is only server-side method.");
	}
}
