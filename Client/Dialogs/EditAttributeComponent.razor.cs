using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using CategoryManagementTool.Shared.Enums;
using CategoryManagementTool.Client.Services;
using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class EditAttributeComponent
    {
        [Parameter]
        public Category Category { get; set; }
        [Parameter]
        public EventCallback OnModalClose { get; set; }
        [Parameter]
        public CategoryAttribute CategoryAttribute { get; set; }
        [Inject] 
        HttpClient HttpClient { get; set; }

        private CategoryAttribute uneditedCategoryAttribute;
        private string _title;
        private bool _showValidationWarningModal;
        private string _validationWarningText = "";

        protected override void OnInitialized()
        {
            if (CategoryAttribute == null)
            {
                CategoryAttribute = new CategoryAttribute(GetAttributeId().ToString(), new List<LanguageEntry>(), DataType.Undefined, null, new List<LanguageEntry>(), false, null, false, PresentationType.Undefined, false, new List<LanguageEntry>());
                _title = "Add"; 
            }
            else
            {
                uneditedCategoryAttribute = (CategoryAttribute)CategoryAttribute.Clone();
                uneditedCategoryAttribute.LanguageEntries = new List<LanguageEntry>();
                if(CategoryAttribute.LanguageEntries != null)
                {
                    foreach (var languageEntry in CategoryAttribute.LanguageEntries)
                    {
                        uneditedCategoryAttribute.LanguageEntries.Add(languageEntry);
                    }
                }
                if (CategoryAttribute.PossibleValues != null)
                {
                    uneditedCategoryAttribute.PossibleValues = new List<LanguageEntry>();
                    foreach (var possibleValues in CategoryAttribute.PossibleValues)
                    {
                        uneditedCategoryAttribute.PossibleValues.Add(possibleValues);
                    }
                }
                if (CategoryAttribute.RegexDescriptions != null)
                {
                    uneditedCategoryAttribute.RegexDescriptions = new List<LanguageEntry>();
                    foreach (var regexDescriptions in CategoryAttribute.RegexDescriptions)
                    {
                        uneditedCategoryAttribute.RegexDescriptions.Add(regexDescriptions);
                    }
                }
                _title = "Edit";
            }
        }

        private void AddLanguageEntry()
        {
            if(CategoryAttribute.LanguageEntries == null)
            {
                CategoryAttribute.LanguageEntries = new List<LanguageEntry>();
                CategoryAttribute.LanguageEntries.Add(new LanguageEntry(Language.German, null, null));
            }
            else
            {
                CategoryAttribute.LanguageEntries.Add(new LanguageEntry(Language.German, null, null));
            }
        }

        private void AddRegexDescription()
        {
            if (CategoryAttribute.RegexDescriptions == null)
            {
                CategoryAttribute.RegexDescriptions = new List<LanguageEntry>();
                CategoryAttribute.RegexDescriptions.Add(new LanguageEntry(Language.German, null, null));
            }
            else
            {
                CategoryAttribute.RegexDescriptions.Add(new LanguageEntry(Language.German, null, null));
            }
        }

        private void AddPossibleValue()
        {
            if (CategoryAttribute.PossibleValues == null)
            {
                CategoryAttribute.PossibleValues = new List<LanguageEntry>();
                CategoryAttribute.PossibleValues.Add(new LanguageEntry(Language.German, null, null));
            }
            else
            {
                CategoryAttribute.PossibleValues.Add(new LanguageEntry(Language.German, null, null));
            }
        }

        private void DeleteLanguageEntry(LanguageEntry languageEntry)
        {
            CategoryAttribute.LanguageEntries.Remove(languageEntry);
        }

        private void DeleteRegexDescription(LanguageEntry regexDescription)
        {
            CategoryAttribute.RegexDescriptions.Remove(regexDescription);
        }

        private void DeletePossibleValue(LanguageEntry possibleValue)
        {
            CategoryAttribute.PossibleValues.Remove(possibleValue);
        }

        private string GetExitBtnString()
        {
            if(_title == "Add")
            {
                return "Add";
            }else
            {
                return "Save";
            }
        }

        public async Task Close()
        {
            if(_title == "Edit")
            {
                CategoryAttribute.ValidationRegex = uneditedCategoryAttribute.ValidationRegex;
                CategoryAttribute.DataType = uneditedCategoryAttribute.DataType;
                CategoryAttribute.PresentationType = uneditedCategoryAttribute.PresentationType;
                CategoryAttribute.UnitType = uneditedCategoryAttribute.UnitType;
                CategoryAttribute.PossibleValues = uneditedCategoryAttribute.PossibleValues;
                CategoryAttribute.LanguageEntries = uneditedCategoryAttribute.LanguageEntries;
                CategoryAttribute.RegexDescriptions = uneditedCategoryAttribute.RegexDescriptions;
                CategoryAttribute.IsFilter = uneditedCategoryAttribute.IsFilter;
                CategoryAttribute.IsIncludedInPreview = uneditedCategoryAttribute.IsIncludedInPreview;
                CategoryAttribute.IsRequired = uneditedCategoryAttribute.IsRequired;
            }
            await OnModalClose.InvokeAsync();
        }

        public async Task CloseAndSave()
        {
            int sameLanguageText = 0;
            if(_title == "Add")
            {
                foreach (CategoryAttribute categoryAttribute in Category.CategoryAttributes)
                {
                    foreach (LanguageEntry comparisonLanguageEntry in categoryAttribute.LanguageEntries)
                    {
                        foreach (LanguageEntry languageEntry in Category.LanguageEntries)
                        {
                            if (languageEntry.Text == comparisonLanguageEntry.Text)
                            {
                                sameLanguageText++;
                            }
                        }
                    }
                }
            }
            
            if (sameLanguageText > 0)
            {
                _validationWarningText = "A used language entry already exists!";
                _showValidationWarningModal = true;
            }else if (CategoryAttribute.LanguageEntries.Where(l => l.Language == Language.German).ToList().Count() == 0)
            {
                _validationWarningText = "You need a valid language entry in german!";
                _showValidationWarningModal = true;
            }
            else if (CategoryAttribute.LanguageEntries.Where(l => l.Text == null).ToList().Count() > 0)
            {
                _validationWarningText = "Every language entry needs a text!";
                _showValidationWarningModal = true;
            }
            else if (CategoryAttribute.DataType == DataType.Undefined)
            {
                _validationWarningText = "You need a valid Data-Type entry!";
                _showValidationWarningModal = true;
            }
            else if (CategoryAttribute.PresentationType == PresentationType.Undefined)
            {
                _validationWarningText = "You need a valid Presentation-Type entry!";
                _showValidationWarningModal = true;
            }
            else
            {
                if(!Category.CategoryAttributes.Where(c => c.Id == CategoryAttribute.Id).Any())
                {
                    Category.CategoryAttributes?.Add(CategoryAttribute);
                }
                await OnModalClose.InvokeAsync();
            }
        }
        private async Task<string> GetAttributeId()
        {
            string id = await HttpClient.GetStringAsync("/objectid");
            foreach (Category category in ApplicationCacheService.Categories)
            {
                if (category.CategoryAttributes.Where(c => c.Id == id).Any())
                {
                    GetAttributeId();
                }
            }

            return id;
        }
    }
}
