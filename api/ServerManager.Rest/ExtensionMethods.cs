using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest
{
    public static class ExtensionMethods
    {
        public static string EnsureTrailingBackslash(this string path)
        {
            if (string.IsNullOrEmpty(path) || path.EndsWith("\\")) return path;

            return $"{path}\\";
        }

        public static string EnsureTrailingForwardSlash(this string path)
        {
            if (string.IsNullOrEmpty(path) || path.EndsWith("/")) return path;

            return $"{path}/";
        }

        public static string PrintFull(this Exception ex, int recursionLimit = 10)
        {
            string full = $"{ex.Message}\n{ex.StackTrace}";

            if (ex.InnerException != null && recursionLimit > 0)
                full += $"\n  Inner Exception: {PrintFull(ex.InnerException, recursionLimit - 1)}";

            return full;
        }

        public static T ThrowIfNull<T>(this T instance, string argName)
        {
            if (instance == null) throw new ArgumentNullException(argName);

            return instance;
        }
    }
}
