﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest
{
    public static class ExtensionMethods
    {
        public static string EnsureTrailingSlash(this string path)
        {
            if (path.EndsWith("\\")) return path;

            return $"{path}\\";
        }

        public static string PrintFull(this Exception ex, int recursionLimit = 10)
        {
            string full = $"{ex.Message}\n{ex.StackTrace}";

            if (ex.InnerException != null && recursionLimit > 0)
                full += $"\n  Inner Exception: {PrintFull(ex.InnerException, recursionLimit - 1)}";

            return full;
        }
    }
}