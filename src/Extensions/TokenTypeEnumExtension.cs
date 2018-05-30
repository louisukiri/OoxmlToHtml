using System;
using System.Reflection;

namespace OoxmlToHtml.Extensions
{
    public static class TokenTypeEnumExtension
    {
        public static string GetText<T>(this T value)
        {
            var tokenType = (TokenAttribute)Attribute.GetCustomAttribute(ForValue(value), typeof(TokenAttribute));
            return tokenType.Text;
        }
        public static bool IsCharTokenType<T>(this T value)
        {
            var tokenType = (TokenAttribute)Attribute.GetCustomAttribute(ForValue(value), typeof(TokenAttribute));
            return tokenType.IsChar;
        }

        //public static string GetText<T>(this SymbolsToken value)
        //{
        //    var tokenType = (TokenAttribute)Attribute.GetCustomAttribute(ForValue(value), typeof(TokenAttribute));
        //    return tokenType.Text;
        //}
        private static MemberInfo ForValue<T>(T p) => typeof(T).GetField(Enum.GetName(typeof(T), p));
    }
}