using Domain;
using ECommerceApi.Filters;
using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebModels.Models.Out;

namespace ECommerceApi.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionLogic _promotionLogic;

        public PromotionController(IPromotionLogic promotionLogic)
        {
            _promotionLogic = promotionLogic;
        }

        [HttpPost]
        [AuthenticationFilter(RoleNeeded = "Buyer")]
        public IActionResult CreatePromotion([FromBody] List<Product> request)
        {
            string errorMessage = _promotionLogic.AdjustCartToStock(request);
            Promotion promotionForCart = _promotionLogic.CalculateBestPromotion(request);
            GetPromotionResponse response = new GetPromotionResponse(promotionForCart, errorMessage);
            return Ok(response);
        }
    }
}