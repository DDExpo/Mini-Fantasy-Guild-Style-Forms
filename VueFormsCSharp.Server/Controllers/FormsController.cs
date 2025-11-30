using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace VueFormsCSharp.Server.Controllers
{
    [ApiController]
    [Route("api/")]
    public class FormsController() : ControllerBase
    {
        // GET: api/getAll
        [HttpGet("getAll")]
        public IActionResult GetAllApi()
        {
            FileLogger.LogInfo("Endpoint GETAll");
            var responseDb = Db.GetAll("forms");

            if (!responseDb.Success)
            {
                FileLogger.LogCritical($"error: {responseDb.Error}");
                return StatusCode(500);
            }
            FileLogger.LogInfo("OK");
            return Ok(responseDb.Result);
        }

        // POST: api/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateApi([FromBody] JsonElement jsonData)
        {
            FileLogger.LogInfo("Endpoint POST");
            var responseDb = Db.Create("forms", jsonData);

            if (!responseDb.Success)
            {
                FileLogger.LogCritical($"error: {responseDb.Error} data: {jsonData}");
                return StatusCode(500);
            }
            FileLogger.LogInfo("OK");
            return Ok(responseDb.Result);
        }

        // PUT: api/change/id
        [HttpPut("change/{id}")]
        public IActionResult UpdateApi(long id, [FromBody] JsonElement jsonData)
        {
            FileLogger.LogInfo("Endpoint PUT");
            var responseDb = Db.Update("forms", id, jsonData);

            if (!responseDb.Success)
            {
                if (responseDb.Error == "NOT FOUND")
                {
                    FileLogger.LogWarning($"error: {responseDb.Error} id: {id}");
                    return NotFound(404);
                }
                FileLogger.LogCritical($"error: {responseDb.Error} id: {id} data: {jsonData}");
                return StatusCode(500);
            }
            FileLogger.LogInfo("OK");
            return Ok(responseDb.Result);
        }

        // DELETE: api/delete/{id}
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteApi(long id)
        {
            FileLogger.LogInfo("Endpoint DELETE");
            var responseDb = Db.Delete("forms", id);

            if (!responseDb.Success)
            {
                FileLogger.LogCritical($"error: {responseDb.Error}");
                return StatusCode(500);
            }
            FileLogger.LogInfo("OK");
            return Ok(200);
        }
    }
}
