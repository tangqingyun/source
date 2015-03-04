using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Basement.Framework.Unit
{
    public static class MyLambdaExpression
    {
        //public static string ResloveName(Expression<Func<T, object>> expression)
        //{
        //    return ResloveName(expression as LambdaExpression);
        //}

        //public static string ResloveName(LambdaExpression expression)
        //{
        //    var exp = expression.Body as Expression;           
        //    string expStr = exp.ToString();
        //    return expStr.Substring(expStr.IndexOf(".") + 1);
        //}

        public static string ResloveName<T>(Expression<Func<T, object>> expression)
        {
            var expr = expression.Body as MemberExpression;
            if (expr == null)
            {
                var u = expression.Body as UnaryExpression;
                expr = u.Operand as MemberExpression;
            }
            return expr.ToString().Substring(expr.ToString().IndexOf(".") + 1); ;
        }

    }
}
