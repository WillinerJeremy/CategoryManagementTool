using CategoryManagementTool.Shared.Models;
using CategoryManagementTool.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class CategoryTreeViewerComponent
    {
        [Parameter]
        public Category ParentCategory { get; set; }
        [Parameter]
        public List<Category> AllCategories { get; set; } // CategoriesToShow
        [Parameter]
        public List<Category> LevelCategories { get; set; }
        [Parameter]
        public int Level { get; set; }
        [Parameter]
        public EventCallback RerenderWholePage { get; set; }
        [Parameter]
        public bool IsDeleteView { get; set; }
        [Parameter]
        public bool IsCompareView { get; set; }
        [Parameter]
        public bool IsAllView { get; set; }

        private bool _shouldRerender = false;
        private Category _selectedCategory;
        private Category _selectedParentCategory;
        private List<Category> _allCategories;
        private Category _rootDeleteCategory;
        private string _restoreWarningText;
        private bool _showEditCategoryModal;
        private bool _showViewCategoryModal;
        private bool _showDeleteWarningModal;
        private bool _showRestoreWarningModal;
        private bool _erase;
        private bool _undeletable;

        private void ViewCategory(Category category)
        {
            _selectedCategory = category;
            _showViewCategoryModal = true;
        }

        private void EditCategory(Category category)
        {
            _selectedCategory = category;
            _showEditCategoryModal = true;
        }

        private void AddCategory(Category parentCategory)
        {
            _selectedCategory = null;
            _selectedParentCategory = parentCategory;
            _showEditCategoryModal = true;
        }

        private void RestoreCategory(Category category)
        {
            if (category.ParentCategoryId != null && ApplicationCacheService.DeletedCategories.Where(c => c.Id == category.ParentCategoryId).Any())
            {
                _restoreWarningText = "You can't restore this subcategory, you need to restore its parentcategory first!";
                _showRestoreWarningModal = true;
            }
            else
            {
                ApplicationCacheService.Categories.Add(category);
                ApplicationCacheService.DeletedCategories.Remove(category);
            }
        }

        private async Task DeleteCategoryWarning(Category category)
        {
            _rootDeleteCategory = category;
            _allCategories = GetCategories(_rootDeleteCategory, ApplicationCacheService.Categories);
            if(_allCategories == null)
            {
                _undeletable = true;

            }else if (IsDeleteView)
            {
                _erase = true;
            }
            _showDeleteWarningModal = true;
        }

        private async Task DeleteCategory(Category rootCategory)
        {
            if (!_undeletable)
            {
                if (!_erase)
                {
                    _allCategories = GetCategories(rootCategory, ApplicationCacheService.Categories);
                    foreach (Category category in _allCategories)
                    {
                        if (ApplicationCacheService.AddedCategories.Contains(category))
                        {
                            ApplicationCacheService.AddedCategories.Remove(category);
                        }
                        else if (ApplicationCacheService.EditedCategories.Contains(category))
                        {
                            ApplicationCacheService.EditedCategories.Remove(category);
                        }
                        ApplicationCacheService.DeletedCategories.Add(category);
                        ApplicationCacheService.Categories.Remove(category);
                    }
                    await RerenderWholePage.InvokeAsync();
                }
                else
                {
                    ApplicationCacheService.PermanentlyDeletedCategories.Add(rootCategory);
                    ApplicationCacheService.DeletedCategories.Remove(rootCategory);
                    RerenderThisWholePage();
                }
                _showDeleteWarningModal = false;
            }
        }

        private List<Category> GetCategories(Category category, List<Category> allCategories)
        {
            List<Category> categories = allCategories.Where(c => c.ParentCategoryId == category.Id)
                .SelectMany(subCategory => GetCategories(subCategory, allCategories))
                .ToList();
            categories.Insert(0, category);
            return categories;
        }

        private int GetLevel(Category category)
        {
            return Level + 1;
        }

        private List<Category> GetSubCategories(Category category)
        {
            return AllCategories.Where(x => x.ParentCategoryId == category.Id).ToList();
        }

        private string GetCardColor(Category category)
        {
            if (IsCompareView)
            {
                if (ApplicationCacheService.AddedCategories.Contains(category))
                {
                    return "#96ff61;";
                }
                else if (ApplicationCacheService.EditedCategories.Contains(category))
                {
                    return "#ffea61;";
                }
                else if (ApplicationCacheService.DeletedCategories.Contains(category))
                {
                    return "#ff6161;";
                }
                else if (ApplicationCacheService.PermanentlyDeletedCategories.Contains(category))
                {
                    return "#ab2120";
                }
                else
                {
                    return "white;";
                }
            }
            else
            {
                return "white;";
            }

        }

        protected override bool ShouldRender()
        {
            return _shouldRerender || base.ShouldRender();
        }

        public void RerenderThisWholePage()
        {
            _erase = false;
            _shouldRerender = true;
            StateHasChanged();
        }

        private async Task OnEditModalClose()
        {
            RerenderThisWholePage();
            _showEditCategoryModal = false;
        }
    }
}
