using System;
namespace Mastodot.Enums
{
    public enum Scope
    {
        Read = 1,
        Write = 2,
        Follow = 4,
    }

    static internal class ScopeExtentions
    {
        public static string ToString(this Scope scope)
        {
            return scope.ToString(false);
        }

        public static string ToString(this Scope scope, bool encoding = false)
        {
            var scopeStr = "";
            if (scope.HasFlag(Scope.Read)) scopeStr += " read";
            if (scope.HasFlag(Scope.Write)) scopeStr += " write";
            if (scope.HasFlag(Scope.Follow)) scopeStr += " follow";
            scopeStr = scopeStr.Trim();

            return encoding ? scopeStr.Replace(" ", "%20"): scopeStr;
        }
    }
}
