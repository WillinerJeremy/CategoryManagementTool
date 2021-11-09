using CategoryManagementTool.Shared.Enums;
using CategoryManagementTool.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManagementTool.Client.Shared
{
    public static class LanguageEntryHelper
    {
        public static string GetGermanText(this IEnumerable<LanguageEntry> languageEntries)
        {
            return languageEntries.FirstOrDefault(x => x.Language == Language.German)?.Text ?? string.Empty;
        }
    }
}
