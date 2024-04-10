using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebModels.Models.Out;
using WebModels.Models.In;
using Domain;
using ECommerceApi.Filters;

namespace ECommerceApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductLogic _productLogic;
        public ProductsController(IProductLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet]
        public IActionResult GetAllProducts([FromQuery(Name = "category")] string? category, 
            [FromQuery(Name = "brand")] string? brand, [FromQuery(Name = "text")] string? text, 
            [FromQuery(Name = "min")] int? minPrice, [FromQuery(Name = "max")] int? maxPrice, 
            [FromQuery(Name = "promo")] bool? promo)
        {
            return Ok(_productLogic.GetAllProducts(category, brand, text, minPrice, maxPrice, promo)
                .Select(products => new GetProductResponse(products)).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct([FromRoute] Guid id)
        {
            GetProductResponse product = new GetProductResponse(_productLogic.GetProductById(id));
            return Ok(product);
        }

        [HttpPost]
        [AuthenticationFilter(RoleNeeded = "Admin")]
        public IActionResult CreateProduct([FromBody] CreateProductRequest received)
        {
            Product product = received.toEntity();
            Product logic = _productLogic.AddNewProduct(product);
            GetProductResponse result = new GetProductResponse(logic);
            return CreatedAtAction(nameof(CreateProduct), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [AuthenticationFilter(RoleNeeded = "Admin")]
        public OkObjectResult UpdateProduct(Guid id, [FromBody] UpdateProductRequest received)
        {
            Product product = received.toEntity();
            Product logic = _productLogic.UpdateProduct(id, product);
            GetProductResponse result = new GetProductResponse(logic);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [AuthenticationFilter(RoleNeeded = "Admin")]
        public IActionResult DeleteProduct(Guid id)
        {
            _productLogic.DeleteProduct(id);
            return Ok("Product deleted");
        }
    }
}