using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller1.Data;
using Taller1.Models;
using Microsoft.AspNetCore.Authorization;
using Taller1.Services;
using CloudinaryDotNet.Actions;

namespace Taller1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinary;

        public ProductController(ApplicationDbContext context, CloudinaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product product, IFormFile? image)
        {
            if (image != null)
            {
                var imageUrl = await _cloudinary.UploadImageAsync(image);
                product.ImageUrl = imageUrl;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Product updated, IFormFile? image)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = updated.Name;
            product.Description = updated.Description;
            product.Price = updated.Price;
            product.Stock = updated.Stock;

            if (image != null)
            {
                var imageUrl = await _cloudinary.UploadImageAsync(image);
                product.ImageUrl = imageUrl;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // Eliminar imagen de Cloudinary si existe
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var publicId = GetPublicIdFromUrl(product.ImageUrl);
                if (!string.IsNullOrEmpty(publicId))
                {
                    await _cloudinary.DeleteImageAsync(publicId);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private string GetPublicIdFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var parts = uri.AbsolutePath.Split('/');
                var fileName = parts.Last();
                var publicId = fileName.Substring(0, fileName.LastIndexOf('.'));
                return string.Join("/", parts.SkipWhile(p => p != "upload").Skip(1).Take(parts.Length - 1)).Replace($"/{fileName}", "");
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}