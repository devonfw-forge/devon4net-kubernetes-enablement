namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities

{
    public partial class OrderDishExtraIngredient
    {
        public long Id { get; set; }
        public long IdOrderLine { get; set; }
        public long IdIngredient { get; set; }

        public Ingredient IdIngredientNavigation { get; set; }
        public OrderLine IdOrderLineNavigation { get; set; }
    }
}
