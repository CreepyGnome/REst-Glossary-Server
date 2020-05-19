using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Regs.Server.Models;
using Regs.Server.Services;

namespace Regs.Server.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RegsController : ControllerBase
    {
        private readonly IRegsService _regsService;
        private readonly ILogger<RegsController> _logger;

        public RegsController(IRegsService regsService, ILogger<RegsController> logger)
        {
            _regsService = regsService ?? throw new ArgumentNullException(nameof(regsService));
            _logger = logger ?? NullLogger<RegsController>.Instance;
        }

        /// <summary>
        /// Gets the value for the specified key in the specified database.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <response code="200">If requested value was retrieved successfully.</response>
        /// <response code="204">If requested value was not found.</response>
        /// <response code="500">If something unknown occurs with the service.</response>
        /// <response code="502">If upstream dependent service is unreachable.</response>
        /// <response code="504">If upstream dependent service doesn't respond in time.</response>
        /// <returns>The value for the key.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [HttpGet("{db}/{key}")]
        public ActionResult<string> Get(ushort db, string key)
        {
            var entry = _regsService.Get(db, new RegsKey(key));

            if (entry != null)
                return new OkObjectResult(entry.GetString());
            
            return new NoContentResult();
        }

        /// <summary>
        /// Sets the specified key/value pair in the specified database.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        /// <response code="201">If requested key/value was set successfully.</response>
        /// <response code="204">If request was not successful.</response>
        /// <response code="500">If something unknown occurs with the service.</response>
        /// <response code="502">If upstream dependent service is unreachable.</response>
        /// <response code="504">If upstream dependent service doesn't respond in time.</response>
        /// <returns>The value that was set.</returns>
        [HttpPut("{db}/{key}/{value}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public ActionResult<string> Set(ushort db, string key, string value, ApiVersion version)
        {
            var entry = _regsService.Set(db, new RegsKey(key, RegsDataType.String), new RegsStringEntry(value));

            if (entry != null)
                return new CreatedResult(string.Empty, entry.GetString());
            
            return new NoContentResult();
        }

        /// <summary>
        /// Sets the specified key/value pair in the specified database.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <param name="body">The value of the entry.</param>
        /// <response code="201">If requested key/value was set successfully.</response>
        /// <response code="204">If request was not successful.</response>
        /// <response code="500">If something unknown occurs with the service.</response>
        /// <response code="502">If upstream dependent service is unreachable.</response>
        /// <response code="504">If upstream dependent service doesn't respond in time.</response>
        /// <returns>The value that was set.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [HttpPost("{db}/{key}/")]
        [Consumes("text/plain","application/json")]
        [Produces("text/plain", "application/json")]
        public ActionResult<string> SetViaBody(ushort db, string key, [FromBody] dynamic body)
        {
            string value = string.Empty;

            //using (var stream = new StreamReader(Request.Body))
            //    value = stream.ReadToEnd();

            switch (body)
            {
                case string stringValue:
                    value = stringValue ?? string.Empty;
                    break;
                case JsonElement jsonElement:
                    value = JsonSerializer.Serialize(body);
                    break;
                default:
                    value = body?.ToString() ?? string.Empty;
                    break;
            }

            var entry = _regsService.Set(db, new RegsKey(key, RegsDataType.String), new RegsStringEntry(value));

            if (entry != null)
                return new CreatedResult(string.Empty, entry.GetString());

            return new NoContentResult();
        }

        /// <summary>
        /// Deletes the key/value pair for the specified key in the specified database.
        /// </summary>
        /// <param name="db">The database to operate on.</param>
        /// <param name="key">The key of the entry.</param>
        /// <response code="200">If requested key/value pair was deleted successfully.</response>
        /// <response code="204">If request was not successful.</response>
        /// <response code="500">If something unknown occurs with the service.</response>
        /// <response code="502">If upstream dependent service is unreachable.</response>
        /// <response code="504">If upstream dependent service doesn't respond in time.</response>
        /// <returns>The value for the key.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        [HttpDelete("{db}/{key}")]
        public IActionResult Delete(ushort db, string key)
        {
            var successful = _regsService.Delete(db, new RegsKey(key));

            if (successful)
                return new OkResult();

            return new NoContentResult();
        }
    }
}
