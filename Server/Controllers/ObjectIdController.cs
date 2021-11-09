using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;


namespace CategoryManagementTool.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ObjectIdController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
