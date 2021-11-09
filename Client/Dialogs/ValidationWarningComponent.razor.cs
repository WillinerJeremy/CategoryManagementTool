using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CategoryManagementTool.Client.Dialogs
{
    public partial class ValidationWarningComponent
    {
        [Parameter]
        public EventCallback OnModalClose { get; set; }
        [Parameter]
        public string Text { get; set; }

        public async Task Close()
        {
            await OnModalClose.InvokeAsync();
        }
    }
}
