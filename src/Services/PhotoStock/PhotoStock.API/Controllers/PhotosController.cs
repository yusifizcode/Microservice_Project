using Microsoft.AspNetCore.Mvc;
using PhotoStock.API.DTOs;

namespace PhotoStock.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotosController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo != null && photo.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

            using var stream = new FileStream(path, FileMode.Create);
            await photo.CopyToAsync(stream, cancellationToken);

            var returnPath = "photos/" + photo.FileName;

            PhotoDto photoDto = new() { Url = returnPath };

            return Ok(photoDto);
        }

        return BadRequest("Photo is empty!");
    }

    public IActionResult PhotoDelete(string photoUrl)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
        if (!System.IO.File.Exists(path)) return NotFound("Path not found!");

        System.IO.File.Delete(path);
        return NoContent();
    }
}
