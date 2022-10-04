namespace Devon4Net.Application.WebAPI.Implementation.Domain.Entities

{
    public partial class UserFavourite
    {
        public long Id { get; set; }
        public long IdUser { get; set; }
        public long IdDish { get; set; }

        public Dish IdDishNavigation { get; set; }
        public User IdUserNavigation { get; set; }
    }
}
