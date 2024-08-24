using FoodDelivery.Models.DominModels.Address;
using FoodDelivery.Models.DominModels.Orders;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.DominModels;

public class Customer : User
{
    public ICollection<MealReview> MealReviews { get; set; }

    public virtual ChiefReview ChiefReview { get; set; }

    public ICollection<Order> orders { get; set; }

    public ICollection<CustomerPromoCode> CustomerPromoCodes { get; set; }

}
