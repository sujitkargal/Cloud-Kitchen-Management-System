namespace Spice_Cloud.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }

        public Dish Dish { get; set; }
    }

}
