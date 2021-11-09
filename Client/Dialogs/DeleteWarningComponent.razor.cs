using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class DeleteWarningComponent
    {
        [Parameter]
        public EventCallback OnModalClose { get; set; }
        [Parameter]
        public List<Category> SubCategories { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        [Parameter]
        public bool Erase { get; set; }

        private string _btnText;
        private string _text;

        protected override void OnParametersSet()
        {
            if (Erase)
            {
                _btnText = "No";
                _text = "Do you really want to delete this category permanently?";
            }
            else if(SubCategories.Count - 1 == 0)
            {
                _btnText = "No";
                _text = "Do you really want to delete this category?";
            }
            else
            {
                _btnText = "Close";
                if(SubCategories.Count - 1 == 1)
                {
                    _text = "You can't delete this category, it has " + (SubCategories.Count - 1) + " subcategory!";
                }
                else
                {
                    _text = "You can't delete this category, it has " + (SubCategories.Count - 1) + " subcategories!";
                }
                
            }
            
        }

        public async Task Confirm()
        {
            await OnConfirm.InvokeAsync();
        }

        public async Task Close()
        {
            await OnModalClose.InvokeAsync();
        }

    }
}
