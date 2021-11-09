using System;
using System.Collections.Generic;
using CategoryManagementTool.Shared.Enums;

namespace CategoryManagementTool.Shared.Models
{

    public class CategoryAttribute : ICloneable
    {
        public string Id { get; set; }
        public List<LanguageEntry> LanguageEntries { get; set; }
        public DataType DataType { get; set; }
        public string? ValidationRegex { get; set; }
        public List<LanguageEntry>? RegexDescriptions { get; set; }
        public bool IsIncludedInPreview { get; set; }
        public UnitType? UnitType { get; set; }
        public bool IsRequired { get; set; }
        public PresentationType PresentationType { get; set; }
        public bool IsFilter { get; set; }
        public List<LanguageEntry>? PossibleValues { get; set; }

        public CategoryAttribute(string id, List<LanguageEntry> languageEntries, DataType dataType, string validationRegex, List<LanguageEntry> regexDescriptions, bool isIncludedInPreview, UnitType? unitType, bool isRequired, PresentationType presentationType, bool isFilter, List<LanguageEntry> possibleValues)
        {
            Id = id;
            LanguageEntries = languageEntries;
            DataType = dataType;
            ValidationRegex = validationRegex;
            RegexDescriptions = regexDescriptions;
            IsIncludedInPreview = isIncludedInPreview;
            UnitType = unitType;
            IsRequired = isRequired;
            PresentationType = presentationType;
            IsFilter = isFilter;
            PossibleValues = possibleValues;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
