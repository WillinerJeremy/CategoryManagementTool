using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CategoryManagementTool.Client.Services;
using CategoryManagementTool.Shared.Enums;
using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class EditCategoryComponent
    {
        [Parameter]
        public Category Category { get; set; }
        [Parameter]
        public Category ParentCategory { get; set; }
        [Parameter]
        public EventCallback OnModalClose { get; set; }
        [Parameter]
        public EventCallback RerenderWholePage { get; set; }
        [Inject] 
        HttpClient HttpClient { get; set; }

        private bool _shouldRerender;
        private List<Category> parentCategories = new();
        private Category uneditedCategory;
        private CategoryAttribute _selectedAttribute;
        private bool _new = false;
        private string _title;
        private bool _showEditAttributeModal;
        private bool _showValidationWarningModal;
        private Category _selectedCategory;
        private string _validationWarningText = "";

        protected override void OnParametersSet()
        {
            if (Category == null)
            {
                Category = new Category(null, new List<LanguageEntry>(), ParentCategory?.Id, new List<CategoryAttribute>());
                _new = true;
                _title = "Add";
            }
            else
            {
                uneditedCategory = (Category)Category.Clone();
                uneditedCategory.LanguageEntries = new List<LanguageEntry>();
                foreach (var languageEntry in Category.LanguageEntries)
                {
                    uneditedCategory.LanguageEntries.Add(languageEntry);
                }
                _new = false;
                _title = "Edit";
            }
        }

        private void EditAttribute(CategoryAttribute attribute)
        {
            _selectedCategory = Category;
            _selectedAttribute = attribute;
            _showEditAttributeModal = true;
        }

        private void AddAttribute()
        {
            _selectedCategory = Category;
            _selectedAttribute = null;
            _showEditAttributeModal = true;
        }

        private void DeleteAttribute(CategoryAttribute categoryAttribute)
        {
            Category.CategoryAttributes.Remove(categoryAttribute);
        }

        private void AddLanguageEntry()
        {
            Category.LanguageEntries.Add(new LanguageEntry(Language.German, null, null));
        }

        private void DeleteLanguageEntry(LanguageEntry languageEntry)
        {
            Category.LanguageEntries.Remove(languageEntry);
        }
    
        private void GoToEditParentCategory()
        {
            Category = ApplicationCacheService.Categories.Single(x => x.Id == Category.ParentCategoryId);
            uneditedCategory = (Category)Category.Clone();
            uneditedCategory.LanguageEntries = new List<LanguageEntry>();
            foreach (var languageEntry in Category.LanguageEntries)
            {
                uneditedCategory.LanguageEntries.Add(languageEntry);
            }
        }

        private async Task OnChangeCategoryId()
        {
            Category.Id = await GetObjectId();
        }

        private string GetExitBtnString()
        {
            if (_title == "Add")
            {
                return "Add";
            }
            else
            {
                return "Save";
            }
        }

        public async Task Close()
        {
            if(_title == "Edit")
            {
                Category.Id = uneditedCategory.Id;
                Category.LanguageEntries = uneditedCategory.LanguageEntries;
            }
            await OnModalClose.InvokeAsync();
        }

        public async Task CloseAndSave()
        {
            int sameLanguageText = 0;
            if(_title == "Add")
            {
                foreach (Category category in ApplicationCacheService.Categories)
                {
                    foreach (LanguageEntry comparisonLanguageEntry in category.LanguageEntries)
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

            if(sameLanguageText > 0)
            {
                _validationWarningText = "A used language entry already exists!";
                _showValidationWarningModal = true;
            }else if (String.IsNullOrEmpty(Category.Id))
            {
                _validationWarningText = "You need to create a valid id!";
                _showValidationWarningModal = true;
            }
            else if (Category.LanguageEntries.Where(l => l.Language == Language.German).ToList().Count() == 0)
            {
                _validationWarningText = "You need a valid language entry in german!";
                _showValidationWarningModal = true;
            }
            else if (Category.LanguageEntries.Where(l => l.Text == null).ToList().Count() > 0)
            {
                _validationWarningText = "Every language entry needs a text!";
                _showValidationWarningModal = true;
            }else
            {
                if (_new)
                {
                    ApplicationCacheService.Categories.Add(Category);
                    ApplicationCacheService.AddedCategories.Add(Category);
                }
                else
                {
                    if (!ApplicationCacheService.EditedCategories.Contains(Category))
                    {
                        ApplicationCacheService.EditedCategories.Add(Category);
                    }
                }
                await OnModalClose.InvokeAsync();
            }
            await RerenderWholePage.InvokeAsync();
        }
        private List<Category> GetParentCategories(Category category, List<Category> allCategories)
        {
            Category parentCategory = allCategories.Single(c => c.Id == category.ParentCategoryId);
            parentCategories.Add(parentCategory);
            if (parentCategory.ParentCategoryId != null)
            {
                GetParentCategories(parentCategory, allCategories);
            }
            return parentCategories;
        }

        private async Task<string> GetObjectId()
        {
           return await HttpClient.GetStringAsync("/objectid");
        }
    }
}
