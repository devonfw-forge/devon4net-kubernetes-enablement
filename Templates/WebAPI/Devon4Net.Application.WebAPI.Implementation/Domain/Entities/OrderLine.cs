using System.Collections.Generic;

namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities

{
    public partial class OrderLine
    {
        public OrderLine()
        {
            OrderDishExtraIngredient = new HashSet<OrderDishExtraIngredient>();
        }

        public long Id { get; set; }
        public long IdDish { get; set; }
        public int? Amount { get; set; }
        public string Comment { get; set; }
        public long IdOrder { get; set; }

        public Dish IdDishNavigation { get; set; }
        public Order IdOrderNavigation { get; set; }
        public ICollection<OrderDishExtraIngredient> OrderDishExtraIngredient { get; set; }
    }
}
