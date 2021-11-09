using System;
using System.Collections.Generic;

namespace CategoryManagementTool.Shared.Models
{
    public class Category : ICloneable
    {
        public string Id { get; set; }
        public List<LanguageEntry> LanguageEntries { get; set; }
        public string? ParentCategoryId { get; set; }
        public List<CategoryAttribute>? CategoryAttributes { get; set; }

        public Category(string id, List<LanguageEntry> languageEntries, string? parentCategoryId, List<CategoryAttribute>? categoryAttributes)
        {
            Id = id;
            LanguageEntries = languageEntries ?? new List<LanguageEntry>();
            ParentCategoryId = parentCategoryId;
            CategoryAttributes = categoryAttributes;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
