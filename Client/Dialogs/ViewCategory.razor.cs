using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CategoryManagementTool.Shared.Models;
using CategoryManagementTool.Client.Services;


using Microsoft.AspNetCore.Components;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class ViewCategory
    {
        [Parameter]
        public Category Category { get; set; }
        [Parameter]
        public Category ParentCategory { get; set; }
        [Parameter]
        public EventCallback OnModalClose { get; set; }

        private List<Category> parentCategories = new();
        private CategoryAttribute _selectedAttribute;
        private bool _showViewAttributeModal;

        private void ViewAttribute(CategoryAttribute attribute)
        {
            _selectedAttribute = attribute;
            _showViewAttributeModal = true;
        }

        private void GoToViewParentCategory()
        {
            Category = ApplicationCacheService.Categories.Single(x => x.Id == Category.ParentCategoryId);
        }

        private List<Category> GetParentCategories(Category category, List<Category> allCategories)
        {
            if(category.ParentCategoryId != null)
            {
                List<Category> parentCategory = allCategories.Where(c => c.Id == category.ParentCategoryId).ToList();
                parentCategories.Add(parentCategory[0]);
                if (parentCategory[0].ParentCategoryId != null)
                {
                    GetParentCategories(parentCategory[0], allCategories);
                }
            }
            return parentCategories;
        }

        public async Task Close()
        {
            await OnModalClose.InvokeAsync();
        }
    }
}
