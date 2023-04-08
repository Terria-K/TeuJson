using System;
using System.Text;

namespace TeuJson.Generator;

public static class QuoteWriter 
{
    public static string AddStatement(Func<StringBuilder, string> stmt) 
    {
        var builder = new StringBuilder();
        return stmt(builder);
    }

    public static string AddExpression(Func<string> expr) 
    {
        return expr();
    }
}
