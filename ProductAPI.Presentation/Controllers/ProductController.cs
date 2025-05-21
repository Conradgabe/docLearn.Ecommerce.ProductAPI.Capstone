using Azure;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.DTOs.Conversions;
using ProductAPI.Application.Interfaces;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            // Get all products from the repository
            var products = await productInterface.GetAllAsync();

            if (!products.Any())
                return NotFound("No products found");

            // convert data from entity to DTO
            var (_, productList) = ProductConversions.FromEntity(null!, products);
            return productList!.Any() ? Ok(productList) : NotFound("No products found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct([FromRoute] int id)
        {
            // Get product by id from the repository
            var product = await productInterface.FindByIdAsync(id);

            if (product is null)
                return NotFound($"Product with id {id} not found");

            // convert data from entity to DTO
            var (singleProduct, _) = ProductConversions.FromEntity(product, null!);
            return singleProduct is not null ? Ok(singleProduct) : NotFound($"Product with id {id} not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct([FromBody] ProductDTO product)
        {
            // Check the model state for validation errors
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // convert data from DTO to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);

            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct([FromBody] ProductDTO product)
        {
            // Check the model state for validation errors
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // convert data from DTO to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);

            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            // convert data from DTO to entity
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);

            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
