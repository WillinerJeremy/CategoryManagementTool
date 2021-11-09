using CategoryManagementTool.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CategoryManagementTool.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryExportController : Controller
    {
        private static byte[] _lastFileBytes = null;

        [HttpGet]
        public IActionResult Get()
        {
            if (_lastFileBytes == null) throw new Exception("No Data available");
            return File(_lastFileBytes, "application/octed-stream", "categories.json");
        }

        [HttpPost]
        public void Export(IList<Category> categories)
        {
            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            _lastFileBytes = StringToByteArray(json);
        }

        private byte[] StringToByteArray(string json)
        {
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                TextWriter tw = new StreamWriter(ms);
                tw.Write(json);
                tw.Flush();
                ms.Position = 0;
                bytes = ms.ToArray();
            }
            return bytes;
        }
    }
}
