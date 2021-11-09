using System;
using CategoryManagementTool.Client.Services;
using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CategoryManagementTool.Client.Pages
{
    public partial class JsonImportComponent
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }

        private bool _isContinueBtnDisabled = true;
        private string _json;
       
        private string Json { get => _json;
            set
            {
                _isContinueBtnDisabled = false;
                _json = value;
                ApplicationCacheService.OriginalCategoriesJson = value;
                ApplicationCacheService.OriginalCategories = JsonConvert.DeserializeObject<List<Category>>(value);
                ApplicationCacheService.Categories = JsonConvert.DeserializeObject<List<Category>>(value);
            }
        }

        private async Task SingleUpload(InputFileChangeEventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            byte[] bytes = ms.ToArray();
            Json = Encoding.Default.GetString(bytes);
        }
        
        private void RedirectToCategoriesPage()
        {
            NavigationManager.NavigateTo("/categories");
        }
    }
}
