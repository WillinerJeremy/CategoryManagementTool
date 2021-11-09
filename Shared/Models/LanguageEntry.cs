using CategoryManagementTool.Shared.Enums;

namespace CategoryManagementTool.Shared.Models
{
    public class LanguageEntry
    {

        public Language Language { get; set; }
        public string Text { get; set; }
        public string? Value { get; set; }

        public LanguageEntry(Language language, string text, string value)
        {
            Language = language;
            Text = text;
            Value = value;
        }
    }
}
