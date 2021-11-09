using CategoryManagementTool.Client.Services;
using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CategoryManagementTool.Client.Pages
{
    public partial class JsonExportComponent
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject] 
        HttpClient HttpClient { get; set; }

        private string _json;

        private string Json
        {
            get => _json;
            set
            {
                _json = value;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await SerializeJson();
        }

        private void RedirectToCategoriesPage()
        {    
            NavigationManager.NavigateTo("/categories");
        }

        public async Task SerializeJson()
        {
            await HttpClient.PostAsJsonAsync<List<Category>>("/CategoryExport", ApplicationCacheService.Categories);
            var options = new JsonSerializerOptions { WriteIndented = true };
            Json = JsonSerializer.Serialize(ApplicationCacheService.Categories, options);
        }

    }
}
