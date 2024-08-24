using FoodDelivery.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models.Utility;

public class FilteringData
{
    public IEnumerable<MealTag>? TagFilter { get; set; }
    public IEnumerable<MealSizeOption>? SizeFilter { get; set; }
    public SortBy? SortBy { get; set; }
    public IEnumerable<MealSpiceLevel>? MealSpiceLevel { get; set; }
    public IEnumerable<MealCategory>? MealCategory { get; set; }
    public IEnumerable<MealStyle>? MealStyle { get; set; }
    public List<Guid>? ChiefFilter { get; set; }
    public double? StartPrice { get; set; } = 0;
    public double? EndPrice { get; set; } = 500000;
    [Range(1, 25, ErrorMessage = "Maximum page size is 25")]
    public int PageSize { get; set; } = 10;
    [Range(1, int.MaxValue, ErrorMessage = "Wrong page number")]
    public int PageNumber { get; set; } = 1;

}
