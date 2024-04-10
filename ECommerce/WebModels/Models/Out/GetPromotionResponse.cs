using Domain;

namespace WebModels.Models.Out
{
    public class GetPromotionResponse
    {
        public double Amount { get; set; }
        public Promotion.PromotionType Type { get; set; } 

        public string Message { get; set; }

        public GetPromotionResponse(Promotion promotion, string message)
        {
            Amount = promotion.Amount;
            Type = promotion.Type;
            Message = message;
        }
    }
}
