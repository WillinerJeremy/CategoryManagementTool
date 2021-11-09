using System;
using System.Collections.Generic;
using System.Linq;
using CategoryManagementTool.Shared.Models;
using CategoryManagementTool.Shared.Enums;
using CategoryManagementTool.Client.Services;
using Microsoft.AspNetCore.Components;


namespace CategoryManagementTool.Client.Pages
{
    public partial class MainPage
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }
        public string SearchText
        {
            get => SearchString;
            set
            {
                SearchString = value;
                OnSearchStringChanged();
            }
        }
        public string SearchString = "";
        public ViewSelection ViewSelectionSelected;
        private List<Category> _categoriesToShow = new List<Category>();
        private List<Category> _rootCategories = new List<Category>();
        private List<Category> _compareCategories;
        private bool _shouldRerender = false;
        private bool _isDeleteView = false;
        private bool _isCompareView = false;
        private bool _isAllView = true;
        private bool _isAddBtnDisabled = false;
        private bool _showEditCategoryModal;
        
        public ViewSelection ViewSelection
        {
            get => ViewSelectionSelected;
            set
            {
                ViewSelectionSelected = value;
                switch (ViewSelectionSelected)
                {
                    default:
                    case ViewSelection.All:
                        _isDeleteView = false;
                        _isCompareView = false;
                        _isAllView = true;
                        _isAddBtnDisabled = false;
                        SetCategoryView(ApplicationCacheService.Categories);
                        break;
                    case ViewSelection.Added:
                        _isDeleteView = false;
                        _isCompareView = false;
                        _isAllView = false;
                        _isAddBtnDisabled = true;
                        SetCategoryView(ApplicationCacheService.AddedCategories);
                        break;
                    case ViewSelection.Edited:
                        _isDeleteView = false;
                        _isCompareView = false;
                        _isAllView = false;
                        _isAddBtnDisabled = true;
                        SetCategoryView(ApplicationCacheService.EditedCategories);
                        break;
                    case ViewSelection.Deleted:
                        _isDeleteView = true;
                        _isCompareView = false;
                        _isAllView = false;
                        _isAddBtnDisabled = true;
                        SetCategoryView(ApplicationCacheService.DeletedCategories);
                        break;
                    case ViewSelection.Compare:
                        _isDeleteView = false;
                        _isCompareView = true;
                        _isAllView = false;
                        _isAddBtnDisabled = true;
                        _compareCategories = new List<Category>();
                        foreach (var category in ApplicationCacheService.Categories)
                        {
                            _compareCategories.Add(category);
                        }
                        foreach (var category in ApplicationCacheService.DeletedCategories)
                        {
                            _compareCategories.Add(category);
                        }
                        foreach (var category in ApplicationCacheService.PermanentlyDeletedCategories)
                        {
                            _compareCategories.Add(category);
                        }
                        SetCategoryView(_compareCategories);
                        break;
                }
            }
        }
        protected override void OnParametersSet()
        {
            ViewSelection = ViewSelection.All;
            SetCategoryView(ApplicationCacheService.Categories);
        }

        protected override bool ShouldRender()
        {
            return _shouldRerender || base.ShouldRender();
        }

        public void RerenderWholePage()
        {
            //use this method to not skip search after a deletion
            OnSearchStringChanged();
            _shouldRerender = true;
            StateHasChanged();
        }

        private void OnSearchStringChanged()
        {
            if (string.IsNullOrEmpty(SearchString))
            {
                switch (ViewSelectionSelected)
                {
                    default:
                    case ViewSelection.All:
                        SetCategoryView(ApplicationCacheService.Categories);
                        break;
                    case ViewSelection.Added:
                        SetCategoryView(ApplicationCacheService.AddedCategories);
                        break;
                    case ViewSelection.Edited:
                        SetCategoryView(ApplicationCacheService.EditedCategories);
                        break;
                    case ViewSelection.Deleted:
                        SetCategoryView(ApplicationCacheService.DeletedCategories);
                        break;
                    case ViewSelection.Compare:
                        SetCategoryView(_compareCategories);
                        break;
                }
            }
            else
            {
                switch (ViewSelectionSelected)
                {
                    default:
                    case ViewSelection.All:
                        SearchForCategories(ApplicationCacheService.Categories);
                        break;
                    case ViewSelection.Added:
                        SearchForCategories(ApplicationCacheService.AddedCategories);
                        break;
                    case ViewSelection.Edited:
                        SearchForCategories(ApplicationCacheService.EditedCategories);
                        break;
                    case ViewSelection.Deleted:
                        SearchForCategories(ApplicationCacheService.DeletedCategories);
                        break;
                    case ViewSelection.Compare:
                        SearchForCategories(_compareCategories);
                        break;
                }
            }
        }

        private void SetCategoryView(List<Category> categories)
        {
            _rootCategories = new();
            foreach (Category category in categories)
            {
                if(!HasParentCategory(category, categories) || category.ParentCategoryId == null)
                {
                    _rootCategories.Add(category);
                }
            }
            
            if (!_rootCategories.Any())
            {
                _rootCategories = categories;
            }
            else
            {
                _categoriesToShow = categories;
            }
        }

        private bool HasParentCategory(Category category, List<Category> categories)
        {
            if ((bool)categories?.Where(x => x.Id == category.ParentCategoryId).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SearchForCategories(List<Category> categories)
        {
            _categoriesToShow = new List<Category>();
            _rootCategories = categories?.Where(x =>
            x.Id.ToString().Contains(SearchString) ||
            (x.ParentCategoryId?.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
            x.LanguageEntries.Any(le => le.Text.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
            (x.CategoryAttributes?.Any(ca => ca.LanguageEntries.Any(le => le.Text.Contains(SearchString, StringComparison.OrdinalIgnoreCase))) ?? false))
                .ToList();
        }

        private void AddCategory()
        {
            _showEditCategoryModal = true;
        }

        private void RedirectToExportPage()
        {
            NavigationManager.NavigateTo("/export");
        }

        private void OnEditModalClose()
        {
            _showEditCategoryModal = false;
        }
    }
}
