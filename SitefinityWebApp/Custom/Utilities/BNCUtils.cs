using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SitefinityWebApp.Custom.Utilities
{
    public static class BNCUtils
    {
        public static string SanitizeURLKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return "menu";
            // keep letters/numbers/underscore only
            return Regex.Replace(key.ToLowerInvariant(), @"[^\w\-]", "");
        }

    }
}