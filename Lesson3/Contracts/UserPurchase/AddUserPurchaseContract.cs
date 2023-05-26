namespace Lesson3.Contracts.UserPurchase
{
    public class AddUserPurchaseContract
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }
}