﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlSugar 
{
    public class DynamicCoreHelper
    {
        public static LambdaExpression GetWhere(Type entityType, string shortName, FormattableString whereSql)
        {
            var parameter = Expression.Parameter(entityType, "it");

            // 提取 FormattableString 中的参数值
            var arguments = whereSql.GetArguments();


            var sql = ReplaceFormatParameters(whereSql.Format);

            // 构建动态表达式，使用常量表达式和 whereSql 中的参数值
            var lambda = SqlSugarDynamicExpressionParser.ParseLambda(
                new[] { parameter },
                typeof(bool),
               sql,
               whereSql.GetArguments()
            );

            return lambda;
        }
        private static string ReplaceFormatParameters(string format)
        {
            int parameterIndex = 0; // 起始参数索引
            return Regex.Replace(format, @"\{\d+\}", match =>
            {
                string replacement = $"@{parameterIndex}";
                parameterIndex++;
                return replacement;
            });
        }
    }
}