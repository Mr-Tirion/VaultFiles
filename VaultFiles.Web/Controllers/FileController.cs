using Microsoft.AspNetCore.Mvc;
using VaultFiles.Services.Fluent;

namespace VaultFiles.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly string _uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public FileController()
        {
            // Ensure the Uploads folder exists
            if (!Directory.Exists(_uploadRoot))
                Directory.CreateDirectory(_uploadRoot);
        }

        /// <summary>
        /// Uploads a file and optionally encrypts it with a password.
        /// </summary>
        /// <param name="file">The file sent in the form.</param>
        /// <param name="password">Optional password for encryption.</param>
        /// <returns>Result of the save operation.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string password = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            // Save file temporarily to disk
            var tempPath = Path.GetTempFileName();
            await using (var stream = System.IO.File.Create(tempPath))
            {
                await file.CopyToAsync(stream);
            }

            // Use VaultFiles Fluent API to save the file
            var result = await FileBuilder
                .Save(tempPath)
                .ToFolder(_uploadRoot)
                .WithPassword(password)
                .AllowExtensions(".jpg", ".zip", ".txt")
                .Overwrite()
                .Async();

            return Ok(new
            {
                result.Success,
                result.FileName,
                result.FullPath
            });
        }

        /// <summary>
        /// Deletes a file from the Uploads folder.
        /// </summary>
        /// <param name="fileName">Name of the file to delete.</param>
        /// <returns>Status message.</returns>
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("File name is required.");

            var filePath = Path.Combine(_uploadRoot, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            System.IO.File.Delete(filePath);

            return Ok(new { Message = "File deleted successfully." });
        }
    }
}