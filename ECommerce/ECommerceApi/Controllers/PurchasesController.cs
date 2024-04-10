using Domain;
using ECommerceApi.Filters;
using LogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebModels.Models.In;
using WebModels.Models.Out;

namespace ECommerceApi.Controllers
{
    [Route("api/purchases")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseLogic _purchaseLogic;

        public PurchasesController(IPurchaseLogic purchaseLogic)
        {
            _purchaseLogic = purchaseLogic;
        }

        [HttpGet]
        public OkObjectResult GetPurchases([FromQuery(Name = "userId")] Guid? userId)
        {
            IEnumerable<Purchase> purchases;
            if (userId.HasValue)
            {
                purchases = _purchaseLogic.GetUserPurchases(userId.Value);
            }
            else
            {
                purchases = _purchaseLogic.GetPurchases();
            }

            return Ok(purchases
                    .Select(purchase => new GetPurchaseResponse(purchase,""))
                    .ToList());
        }

        [HttpGet("{id}")]
        public OkObjectResult GetPurchase([FromRoute] Guid id)
        {
            GetPurchaseResponse response = new GetPurchaseResponse(_purchaseLogic.GetPurchase(id), "");
            return Ok(response);
        }

        [HttpPost]
        [AuthenticationFilter(RoleNeeded = "Buyer")]
        public IActionResult CreatePurchase([FromBody] CreatePurchaseRequest request)
        {
            (Purchase, string) purchase = _purchaseLogic.CreatePurchase(request.ToEntity());
            GetPurchaseResponse response = new GetPurchaseResponse(purchase.Item1, purchase.Item2);
            return CreatedAtAction(nameof(GetPurchase), new { id = response.Id }, response);
        }
    }
}