using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ADODataContext.Converters
{
    public class LambdaConverter<T>
    {

        public string DeconvertData(Expression<Func<T, bool>> condition, SqlParameterCollection parameters)
        {
            string resultString = String.Empty;

            if (condition.Body is BinaryExpression expression)
            {
                ParameterExpression parameter = condition.Parameters.First();

                resultString = Deconvert(expression.Left, parameter, parameters)
                    + GenerateOperator(expression.NodeType)
                    + Deconvert(expression.Right, parameter, parameters);
            }

            return resultString;
        }

        private string Deconvert(Expression expression, ParameterExpression head, SqlParameterCollection parameters)
        {
            switch (expression)
            {
                case BinaryExpression binary:
                    return "(" + Deconvert(binary.Left, head, parameters)
                        + GenerateOperator(binary.NodeType)
                        + Deconvert(binary.Right, head, parameters) + ")";
                case ConstantExpression constant:
                    string constResult = $"@constant{parameters.Count}";
                    parameters.Add(new SqlParameter(constResult, constant.Value));
                    return constResult;
                case MemberExpression member:
                    if (member.ToString().StartsWith($"{head.Name}."))
                    {
                        return member.Member.Name;
                    }
                    else
                    {
                        string memberResult = $"@member{parameters.Count}";
                        parameters.Add(new SqlParameter(memberResult,
                            Expression.Lambda(member).Compile().DynamicInvoke()));
                        return memberResult;
                    }
                default:
                    return String.Empty;
            }
        }

        public string DeconvertData(Expression<Func<T, bool>> condition)
        {
            string resultString = String.Empty;

            if (condition.Body is BinaryExpression expression)
            {
                ParameterExpression parameter = condition.Parameters.First();

                resultString = Deconvert(expression.Left, parameter)
                    + GenerateOperator(expression.NodeType)
                    + Deconvert(expression.Right, parameter);
            }

            return resultString;
        }

        private string GenerateOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.NotEqual:
                    return " != ";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.OrElse:
                    return " OR ";
                default:
                    return String.Empty;
            }
        }

        private string Deconvert(Expression expression, ParameterExpression head)
        {
            switch (expression)
            {
                case BinaryExpression binary:
                    return "(" + Deconvert(binary.Left, head)
                        + GenerateOperator(binary.NodeType)
                        + Deconvert(binary.Right, head) + ")";
                case ConstantExpression constant:
                    return constant.Value.ToString();
                case MemberExpression member:
                    if (member.ToString().StartsWith($"{head.Name}."))
                    {
                        return member.Member.Name;
                    }
                    else
                    {
                        return Expression.Lambda(member).Compile().DynamicInvoke().ToString();
                    }
                default:
                    return String.Empty;
            }
        }
    }
}
