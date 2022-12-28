using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatesWebApiPro.Data;
using SatesWebApiPro.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SatesWebApiPro.Controllers
{
    [EnableCors]
    [ApiController] //sen bi api controllersin
    //buna erişmek için route kullan
    [Route("api/[controller]/[action]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result= await _productRepository.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data= await _productRepository.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound(id);
            }
            //DATA BOSSA bulunamadı dön değilse datayı dön
            return Ok(data);
        }
        //create için HttpPost
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var addedProduct =await  _productRepository.CreateAsync(product);
            return Created(string.Empty, addedProduct);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Product product)
        {
            //bana gelen ıd de product varmı onu kontrol edelim
            var checkProduct = await _productRepository.GetByIdAsync(product.Id);
            if (checkProduct==null)
            {
                return NotFound(product.Id);
            }
            await _productRepository.UpdateAsync(product);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var checkProduct = await _productRepository.GetByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _productRepository.RemoveAsync(id);
            return NoContent();
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            var newName =Guid.NewGuid()+"."+ Path.GetExtension(formFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newName);
            var stream = new FileStream(path, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return Created(string.Empty, formFile);
        }
    }
}
