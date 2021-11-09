using CategoryManagementTool.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using CategoryManagementTool.Shared.Enums;

namespace CategoryManagementTool.Client.Services
{
    public static class ApplicationCacheService
    {
        public static string OriginalCategoriesJson { get; set; }
        public static List<Category> OriginalCategories { get; set; }
        public static List<Category> Categories { get; set; }
        public static List<Category> EditedCategories { get; set; } = new();
        public static List<Category> AddedCategories { get; set; } = new();
        public static List<Category> DeletedCategories { get; set; } = new();
        public static List<Category> PermanentlyDeletedCategories { get; set; } = new();
    }
}
