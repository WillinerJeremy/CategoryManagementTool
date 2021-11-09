using System.Threading.Tasks;
using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class ViewAttribute
    {
        [Parameter]
        public EventCallback OnModalClose { get; set; }

        [Parameter]
        public CategoryAttribute CategoryAttribute { get; set; }

        public async Task Close()
        {
            await OnModalClose.InvokeAsync();
        }
    }
}
