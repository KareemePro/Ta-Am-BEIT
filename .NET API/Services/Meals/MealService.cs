using Azure.Core;
using FoodDelivery.Data;
using FoodDelivery.Enums;
using FoodDelivery.Migrations;
using FoodDelivery.Models.DominModels;
using FoodDelivery.Models.DominModels.Meals;
using FoodDelivery.Models.DTO.ImageDTO;
using FoodDelivery.Models.DTO.MealDTO;
using FoodDelivery.Models.DTO.MealReviewDTO;
using FoodDelivery.Models.Utility;
using FoodDelivery.Services.Common;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace FoodDelivery.Services.Meals;

public class MealService(DBContext context, IImageService image) : IMealService
{
    private readonly DBContext _context = context;

    private readonly IImageService _image = image;

    public async Task<IEnumerable<GetMealRequest>> GetMeals(FilteringData filteringData)
    {
        var mealsQuery = FilterData(filteringData);

        var response = await mealsQuery.Select(x => new GetMealRequest()
        {
            ChiefID = x.ChiefID,
            ChiefName = " ",
            ChiefImage = "",
            ChiefDescription = x.Chief.Description ?? "",
            ChiefOrderCount = x.Chief.Meals.SelectMany(m => m.MealOptions).SelectMany(o => o.OrderItems).Count(),
            MealID = x.ID,
            CreatedDate = x.CreatedDate,
            Title = x.Name,
            Description = x.Description,
            Rating = x.MealReviews.Any() ? x.MealReviews.Average(x => x.Rating) : 0,
            ReviewCount = x.MealReviews.Count,
            MealCategory = x.MealCategory,
            MealSpiceLevel = x.MealSpiceLevel,
            MealStyle = x.MealStyle,
            MealTags = x.MealTags.Select(x => x.Tag).ToArray(),
            GetMealOptionsRequest = x.MealOptions.Select(opt => new GetMealOptionRequest()
            {
                MealOptionID = opt.ID,
                MealSizeOption = opt.MealSizeOption,
                IsAvailable = opt.IsAvailable,
                Quantity = (int)(opt.AvailableQuantity == null ? 0 : opt.AvailableQuantity),
                SaveQuantity = (opt.DailyQuantity != null),
                Price = opt.Price,
                FullScreenImage = opt.FullScreenImage,
                ThumbnailImage = opt.ThumbnailImage,
                GetMealSideDishesRequest = opt.MealSideDishes.Select(x => new GetMealSideDishRequest()
                {
                    MealSideDishID = x.ID,
                    IsFree = x.IsFree,
                    IsTopping = x.IsTopping,
                    GetMealSideDishOptionsRequest = x.MealSideDishOptions.Select(y => new GetMealSideDishOptionRequest()
                    {
                        SideDishID = y.SideDishID,
                        SideDishSizeOption = y.SideDishSizeOption,
                        Name = y.SideDishOption.SideDish.Name,
                        Price = y.SideDishOption.Price,
                        Quantity = y.SideDishOption.Quantity,
                    }).ToArray()
                }).ToArray()
            }).ToArray(),
        }).ToArrayAsync();


        return response;
    }


    public async Task<ListResult<Guid>> AddMealOption(CreateMealOptionRequest request, byte[] image, Guid ChiefID)
    {
        if (!await _context.Meals.AnyAsync(x => x.ID == request.MealID))
            return ListResult<Guid>.Failure([("This meal doesn't exist")], HttpStatusCode.NotFound);

        //if (!await _context.MealOptions.AnyAsync(x => request.mealOptionRequests.Select(y => y.SizeOptionID).Contains(x.SizeOptionID) && x.MealID == request.MealID))
        //return ListResult<Guid>.Failure([("The selected size option are wrong or has been added to this meal already")], HttpStatusCode.Conflict);

        if (await _context.MealOptions.AnyAsync(x => x.MealID == request.MealID && x.MealSizeOption == request.MealSizeOption))
            return ListResult<Guid>.Failure([("The selected size option is wrong or has been added to this meal already")], HttpStatusCode.Conflict);

        //var sideDishIds = request.MealSideDishes.SelectMany(msd => msd.SideDishOptions.Select(sdo => new { sdo.SideDishID, sdo.SideDishSizeOption })).ToList();

        //var ValidSideDishOptionCounts = await _context.SideDishOptions.CountAsync(sdo => sideDishIds.Select(x => x.SideDishID).Contains(sdo.SideDishID));


        //var ValidSideDishOptionCount = await _context.SideDishOptions.Where(x =>
        //{
        //    request.MealSideDishes.Select(x => x.SideDishOptions.Select(x => x.SideDishID)).Contains(x.SideDishID);
        //})
        //var sideDishOptions = request.MealSideDishes
        //    .SelectMany(msd => msd.SideDishOptions
        //        .Select(sdo => new { sdo.SideDishID, sdo.SideDishSizeOption }))
        //    .ToList();

        //var ValidSideDishOptionCount = await _context.SideDishOptions
        //    .CountAsync(sdo => sideDishOptions
        //        .Any(so => so.SideDishID == sdo.SideDishID && so.SideDishSizeOption == sdo.SideDishSizeOption));

        //var sideDishOptions = request.MealSideDishes
        //    .SelectMany(msd => msd.SideDishOptions
        //        .Select(sdo => new { sdo.SideDishID, sdo.SideDishSizeOption }))
        //    .ToList();

        var sideDishIds = request.MealSideDishes.SelectMany(x => x.SideDishOptions.Select(x => x.SideDishID)).ToList();

        var sideDishSizeOption = request.MealSideDishes.SelectMany(x => x.SideDishOptions.Select(x => x.SideDishSizeOption)).ToList();

        var matchingSideDishOptionCount = await _context.SideDishOptions
            .CountAsync(x => sideDishIds.Contains(x.SideDishID) && sideDishSizeOption.Contains(x.SideDishSizeOption));

        //if (matchingSideDishOptionCount != sideDishIds.Count)
        //{
        //    return ListResult<Guid>.Failure([("One or more side dishes or side dish options are not valid")], HttpStatusCode.NotFound);
        //}



        //if (ValidSideDishOptionCount != request.MealSideDishes.SelectMany(msd => msd.SideDishOptions).Count())
        //{
        //    return ListResult<Guid>.Failure([("One or more side dishes or side dish options are not valid")], HttpStatusCode.NotFound);
        //}


        //var ValidSideDishOptionCount = await _context.SideDishOptions.CountAsync(x => request.MealSideDish.Select(x => { x.SideDishOptions}).Contains(x.SideDishSizeOption));

        //if (ValidTagsCount == request.TagsID.Count)
        //{

        //    foreach (var TagID in request.TagsID)
        //    {
        //        newMeal.MealTags.Add(TagID);//(new Enums.MealTag() { MealID = MealID, Tag = Enums.MealTag.Vegan });
        //    }
        //}
        //else
        //{
        //    return SingleResult<Guid>.Failure([("One or more tags are not correct")], HttpStatusCode.NotFound);
        //}

        var MealOptionID = Guid.NewGuid();

        var MealOption = new MealOption()
        {
            ID = MealOptionID,
            IsAvailable = request.IsAvailable,
            MealID = request.MealID,
            AvailableQuantity = request.AvailableQuantity,
            Price = request.Price,
            DailyQuantity = request.SaveQuantitySetting ? request.AvailableQuantity : 0,
            MealSizeOption = request.MealSizeOption,
        };
        var Images = await _image.Process(new ImageInput() { Content = new MemoryStream(image), FileName = $"{MealOptionID}", Path = "Images/Meal" });
        MealOption.ThumbnailImage = Images[0] ?? "";
        MealOption.FullScreenImage = Images[1] ?? "";
        if (request.AddIngredients != null)
        {
            foreach (var foodIngredient in request.AddIngredients)
            {
                MealOption.MealOptionIngredients.Add(new MealOptionIngredient
                {
                    MealOptionID = MealOptionID,
                    ChiefID = ChiefID.ToString(),
                    FoodIngredient = foodIngredient.FoodIngredient,
                    AmountInGrams = foodIngredient.AmountInGrams

                });
            }
        }

        MealSideDish MealSideDish = new();
        MealSideDishOption MealSideDishOption = new();


        foreach (var requestMealSideDish in request.MealSideDishes)
        {
            var MealSideDishID = Guid.NewGuid();
            MealSideDish.ID = Guid.NewGuid();
            MealSideDish.IsFree = requestMealSideDish.IsFree;
            MealSideDish.IsTopping = requestMealSideDish.IsTopping;
            MealOption.MealSideDishes.Add(new()
            {
                ID = MealSideDishID,
                IsFree = requestMealSideDish.IsFree,
                IsTopping = MealSideDish.IsTopping,
                MealSideDishOptions = MapDTOToDomin(requestMealSideDish.SideDishOptions, MealSideDishID)
            });
            foreach (var requestSideDishOption in MealSideDish.MealSideDishOptions)
            {
                MealSideDishOption.SideDishID = requestSideDishOption.SideDishID;
                MealSideDishOption.SideDishSizeOption = requestSideDishOption.SideDishSizeOption;
                MealSideDishOption.MealSideDishID = MealSideDish.ID;
            }
        }


        /*var MealOptions = new List<MealOption>();
        foreach (var MealOption in request.mealOptionRequests)
        {
            var MealOptionID = Guid.NewGuid();
            MealOptions.Add(new MealOption()
            {
                ID = MealOptionID,
                IsAvailable = MealOption.IsAvailable,
                MealID = request.MealID,
                SizeOptionID = MealOption.SizeOptionID,
                Image = await _image.Process(new ImageInput() { Content = MealOption.Image.OpenReadStream(), FileName = $"{MealOptionID}", Path = "Images/Meal" })
            });
        }
        await _context.AddRangeAsync(MealOptions);*/
        await _context.AddAsync(MealOption);
        await _context.SaveChangesAsync();
        return ListResult<Guid>.Success([MealOption.ID]);
    }
    public async Task<SingleResult<GetMealRequest>> GetMeal(Guid ID)
    {

        var meal = await _context.Meals.Where(x => x.ID == ID)
        .Select(x => new GetMealRequest()
        {
            ChiefID = x.ChiefID,
            ChiefName = x.Chief.FirstName + " " + x.Chief.LastName,
            ChiefImage = x.Chief.UserImage ?? "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png",
            ChiefDescription = x.Chief.Description ?? "",
            ChiefOrderCount = x.Chief.Meals.SelectMany(m => m.MealOptions).SelectMany(o => o.OrderItems).Count(),
            MealID = x.ID,
            CreatedDate = x.CreatedDate,
            MealCategory = x.MealCategory,
            MealSpiceLevel = x.MealSpiceLevel,
            MealStyle = x.MealStyle,
            Title = x.Name,
            Description = x.Description,
            Rating = x.MealReviews.Any() ? x.MealReviews.Average(x => x.Rating) : 0,
            ReviewCount = x.MealReviews.Count,
            GetMealReviewsRequest = x.MealReviews.Take(4).Select(x => new GetMealReview()
            {
                ReviewText = x.Title,
                CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                CustomerImage = x.Customer.UserImage ?? "",
                Rating = (int)x.Rating,
                ReviewDate = x.CreatedAt,
            }).ToArray(),
            MealTags = x.MealTags.Select(x => x.Tag).ToArray(),
            GetMealOptionsRequest = x.MealOptions.Select(x => new GetMealOptionRequest()
            {
                MealOptionID = x.ID,
                MealSizeOption = x.MealSizeOption,
                IsAvailable = x.IsAvailable,
                Quantity = (int)(x.AvailableQuantity == null ? 0 : x.AvailableQuantity),
                SaveQuantity = (x.DailyQuantity != null),
                Price = x.Price,
                ThumbnailImage = x.ThumbnailImage,
                FullScreenImage = x.FullScreenImage,
                UsedIngredients = x.MealOptionIngredients.Select(ingredient => new UsedIngredient()
                {
                    Ingredient = ingredient.FoodIngredient,
                    AmountInGrams = ingredient.AmountInGrams,
                }).ToArray(),
                GetMealSideDishesRequest = x.MealSideDishes.Select(y => new GetMealSideDishRequest()
                {
                    MealSideDishID = y.ID,
                    IsFree = y.IsFree,
                    IsTopping = y.IsTopping,
                    GetMealSideDishOptionsRequest = y.MealSideDishOptions.Select(z => new GetMealSideDishOptionRequest()
                    {
                        SideDishID = z.SideDishID,
                        SideDishSizeOption = z.SideDishSizeOption,
                        Name = z.SideDishOption.SideDish.Name,
                        Price = z.SideDishOption.Price,
                        Quantity = z.SideDishOption.Quantity
                    }).ToArray()
                }).ToArray()
            }).ToArray(),
        })
        .FirstOrDefaultAsync();


        if (meal != null)
        {
            return SingleResult<GetMealRequest>.Success(meal);
        }
        return SingleResult<GetMealRequest>.Failure([("This meal doesn't exist")], HttpStatusCode.NotFound);
    }

    public async Task<SingleResult<object>> UpdateMeal(UpdateMealRequest request)
    {

        var Meal = await _context.Meals.AsNoTracking().FirstOrDefaultAsync(x => x.ID == request.MealID);

        if (Meal != null)
        {
            Meal.Name = request.Name ?? Meal.Name;
            Meal.Description = request.Description ?? Meal.Description;
            Meal.MealCategory = request.MealCategory ?? Meal.MealCategory;
            Meal.MealSpiceLevel = request.MealSpiceLevel ?? Meal.MealSpiceLevel;
            Meal.MealStyle = request.MealStyle ?? Meal.MealStyle;
            Meal.UpdatedDate = DateTime.Now;



            if (request.TagsID != null && request.TagsID.Count > 0)
            {
                //error
                //foreach (var TagID in request.TagsID)
                //{
                //    if (Meal.MealTags.Any(x => x.Tag == TagID))
                //        continue;
                //    Meal.MealTags.Add(new Models.DominModels.Meals.MealTag() { MealID = Meal.ID, Tag = TagID });
                //}
            }

            _context.Meals.Update(Meal);
            await _context.SaveChangesAsync();
            return SingleResult<object>.Success(null, HttpStatusCode.NoContent);
        }
        return SingleResult<object>.Failure([("This meal doesn't exist")], HttpStatusCode.NotFound);
    }

    public async Task<bool> UpdateMealOption(UpdateMealOptionRequest request, byte[]? image, Guid ChiefID)
    {
        var MealOption = await _context.MealOptions.AsTracking().Include(x => x.MealOptionIngredients).FirstOrDefaultAsync(x => x.ID == request.MealOptionID);

        if (MealOption != null)
        {
            MealOption.Price = request.Price;
            MealOption.DailyQuantity = request.SaveQuantitySetting ? request.AvailableQuantity : 0;
            MealOption.IsAvailable = request.IsAvailable;
            var Images = new List<string>();
            //Images = image != null ? Images = request.Image != null ? await _image.Process(new ImageInput() { Content = new MemoryStream(image), FileName = $"{MealOption.ID}", Path = "Images/Meal" }) : [] : [];
            //MealOption.ThumbnailImage = Images != null && Images.Count > 0 ? Images[0] : MealOption.ThumbnailImage;//Images[0] ?? MealOption.ThumbnailImage;
            //MealOption.FullScreenImage = Images != null && Images.Count > 0 ? Images[1] : MealOption.FullScreenImage;//Images[1] ?? MealOption.FullScreenImage;
            MealOption.MealSideDishes.Clear();

            foreach (var foodIngredient in request.AddIngredients)
            {
                // Check if the food ingredient already exists
                var existingIngredient = MealOption.MealOptionIngredients.FirstOrDefault(x => x.FoodIngredient == foodIngredient.FoodIngredient);

                if (existingIngredient != null)
                {
                    existingIngredient.AmountInGrams = foodIngredient.AmountInGrams;
                }
                else
                {
                    // Add a new ingredient
                    MealOption.MealOptionIngredients.Add(new MealOptionIngredient
                    {
                        MealOptionID = MealOption.ID,
                        ChiefID = ChiefID.ToString(),
                        FoodIngredient = foodIngredient.FoodIngredient,
                        AmountInGrams = foodIngredient.AmountInGrams
                    });
                }
            }

            var sideDishIds = request.MealSideDishes.SelectMany(x => x.SideDishOptions.Select(x => x.SideDishID)).ToList();

            var sideDishSizeOption = request.MealSideDishes.SelectMany(x => x.SideDishOptions.Select(x => x.SideDishSizeOption)).ToList();

            var matchingSideDishOptionCount = await _context.SideDishOptions
                .CountAsync(x => sideDishIds.Contains(x.SideDishID) && sideDishSizeOption.Contains(x.SideDishSizeOption));

            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<GetMealTableRequest>> GetMealTable(FilteringData filteringData)
    {
        var mealQuery = FilterData(filteringData);

        var response = await mealQuery.Select(x => new GetMealTableRequest()
        {
            MealID = x.ID,
            Title = x.Name,
            Category = x.MealCategory,
            SpiceLevel = x.MealSpiceLevel,
            MealStyle = x.MealStyle,
            Rating = (float?)x.MealReviews.Average(r => r.Rating) ?? 0,
            IsAvailable = (x.MealOptions.Any(x => x.IsAvailable == true)),
            getMealOptionsTable = x.MealOptions.Select(y => new GetMealOptionTable()
            {
                MealOptionID = y.ID,
                MealSizeOption = y.MealSizeOption,
                Price = y.Price,
                Sold = y.OrderItems.Select(x => x.Quantity).Sum(),
                IsAvailable = y.IsAvailable,
                ThumbnailImage = y.ThumbnailImage,
            }).ToList()
        }).ToArrayAsync();

        return response;
    }

    private IQueryable<Meal> FilterData(FilteringData filteringData)
    {
        var mealsQuery = _context.Meals
            .Include(x => x.MealTags)
            .Include(x => x.Chief)
            .Include(x => x.MealReviews)
            .Where(x => x.IsAvailable == true && x.MealOptions.Any(opt => opt.Price >= filteringData.StartPrice && opt.Price <= filteringData.EndPrice));
        //error

        if (filteringData.TagFilter != null)
        {
            mealsQuery = mealsQuery.Where(x => filteringData.TagFilter.All(tagId => x.MealTags.Any(tag => tag.Tag == tagId)));
        }

        if (filteringData.MealCategory != null)
        {
            //mealsQuery = mealsQuery.Where(x => x.MealCategory == filteringData.MealCategory);
            mealsQuery = mealsQuery.Where(x => filteringData.MealCategory.Any(mealCategoty => x.MealCategory == mealCategoty));

        }

        if (filteringData.MealSpiceLevel != null)
        {
            //mealsQuery = mealsQuery.Where(x => x.MealSpiceLevel == filteringData.MealSpiceLevel);
            mealsQuery = mealsQuery.Where(x => filteringData.MealSpiceLevel.Any(mealSpiceLevel => x.MealSpiceLevel == mealSpiceLevel));

        }

        if (filteringData.MealStyle != null)
        {
            //mealsQuery = mealsQuery.Where(x => x.MealStyle == filteringData.MealStyle);
            mealsQuery = mealsQuery.Where(x => filteringData.MealStyle.Any(mealStyle => x.MealStyle == mealStyle));

        }

        if (filteringData.ChiefFilter != null)
        {
            var CheifIDs = filteringData.ChiefFilter.Select(x => x.ToString()).ToArray();
            mealsQuery = mealsQuery.Where(x => x.Chief.Meals.Any(chiefMeal => CheifIDs.Contains(chiefMeal.ChiefID)));
        }

        if (filteringData.SizeFilter != null)
        {
            mealsQuery = mealsQuery.Where(x => x.MealOptions.Any(opt => filteringData.SizeFilter.Contains(opt.MealSizeOption)));
        }

        mealsQuery = filteringData.SortBy != null ? ApplyOrderBy(filteringData.SortBy, mealsQuery) : mealsQuery.OrderBy(x => x.MealOptions.SelectMany(x => x.OrderItems).Sum(x => x.Quantity)); ;

        mealsQuery = mealsQuery
            .Skip(filteringData.PageSize * (filteringData.PageNumber - 1))
            .Take(filteringData.PageSize);

        return mealsQuery;
    }

    private static IQueryable<Meal> ApplyOrderBy(SortBy? sortBy, IQueryable<Meal> mealsQuery)
    {
        switch (sortBy)
        {
            case SortBy.BestSelling:
                return mealsQuery.OrderBy(x => x.MealOptions.SelectMany(x => x.OrderItems).Sum(x => x.Quantity));

            case SortBy.NewlyAdded:
                return mealsQuery.OrderBy(x => x.CreatedDate);

            case SortBy.PriceAsc:
                return mealsQuery.OrderBy(x => x.MealOptions.Min(option => option.Price));

            case SortBy.PriceDesc:
                return mealsQuery.OrderByDescending(x => x.MealOptions.Max(option => option.Price));

            case SortBy.Discount:
                return mealsQuery;

            default:
                return mealsQuery;
        }
    }

    private static ICollection<MealSideDishOption> MapDTOToDomin(ICollection<AddMealSideDishOption> mealSideDishOptions, Guid MealSideDishID)
    {
        var mealSideDishesOptions = new List<MealSideDishOption>();
        foreach (var requestSideDishOption in mealSideDishOptions)
        {
            //mealSideDishesOptions.Add(new MealSideDishOption() { SideDishID = requestSideDishOption.SideDishID, SideDishSizeOption = requestSideDishOption.SideDishSizeOption, MealSideDishID = MealSideDishID });
            //MealSideDishOption.SideDishID = requestSideDishOption.SideDishID;
            //MealSideDishOption.SideDishSizeOption = requestSideDishOption.SideDishSizeOption;
            //MealSideDishOption.MealSideDishID = MealSideDish.ID;
        }
        return mealSideDishesOptions;
    }



    public async Task<bool> DeleteMeal(Guid ID)
    {
        var meal = await _context.Meals.AsTracking().FirstOrDefaultAsync(x => x.ID == ID);

        if (meal == null) return false;

        meal.IsAvailable = false;

        await _context.SaveChangesAsync();

        return true;
    }



    public Task<bool> DeleteMealOption()
    {
        throw new NotImplementedException();
    }

    public async Task<ListResult<GetCartMealRequest>> GetCartMeals(List<Guid> MealOptionIDs)
    {
        var ValidMealOptionsCount = await _context.MealOptions.CountAsync(x => MealOptionIDs.Contains(x.ID));

        if (ValidMealOptionsCount != MealOptionIDs.Count)
        {
            return ListResult<GetCartMealRequest>.Failure([("One or more meal option are not correct")], HttpStatusCode.NotFound);
        }
        else
        {
            List<GetCartMealRequest> mealOptions = await _context.MealOptions.Where(x => MealOptionIDs.Contains(x.ID)).Select(x => new GetCartMealRequest
            {
                MealID = x.MealID,
                Title = x.Meal.Name,
                Rating = x.Meal.Rating,
                MealCategory = x.Meal.MealCategory,
                MealSpiceLevel = x.Meal.MealSpiceLevel,
                MealStyle = x.Meal.MealStyle,
                MealOptionID = x.ID,
                AvailableQuantity = (int)x.AvailableQuantity,
                Price = x.Price,
                Image = x.ThumbnailImage,
            }).ToListAsync();

            return ListResult<GetCartMealRequest>.Success(mealOptions);
        }
    }

    public async Task<SingleResult<bool>> DisableMeal(Guid MealID, Guid ChiefID)
    {
        var Meal = await _context.Meals.AsTracking().FirstOrDefaultAsync(x => x.ID == MealID && x.ChiefID == ChiefID.ToString());
        if (Meal != null)
        {
            Meal.IsAvailable = false;
            await _context.SaveChangesAsync();
            return SingleResult<bool>.Success(true);
        }
        return SingleResult<bool>.Failure(["please try refreashing your page and try again"], HttpStatusCode.NotFound);
    }

    public async Task<ListResult<GetMealAnalysis>> GetMealAnalyses(string UserID)
    {
        var UserRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == UserID);

        if (UserRole == null)
            return ListResult<GetMealAnalysis>.Failure(["you do not have the accsess to this resource"], HttpStatusCode.Forbidden);

        if (UserRole.RoleId == "fb229fa6-3a88-4b19-8292-2ad318c2d82c")
        {
            var meals = await _context.MealOptions.Select(mealOption => new GetMealAnalysis()
            {
                MealID = mealOption.ID,
                MealOptionID = mealOption.ID,
                MealSizeOption = mealOption.MealSizeOption,
                Name = mealOption.Meal.Name,
                Image = mealOption.ThumbnailImage,
                TotalCost = mealOption.OrderItems.Sum(x => x.TotalCost),
                TotalRevenue = mealOption.OrderItems.Sum(x => x.TotalAmount),
                SoldAmount = mealOption.OrderItems.Sum(x => x.Quantity)
            }).ToListAsync();

            return ListResult<GetMealAnalysis>.Success(meals);
        }
        else if (UserRole.RoleId == "799e083d-076f-4812-b9db-527e85d3911d")
        {
            var meals = await _context.MealOptions.Where(x => x.Meal.ChiefID == UserID).Select(mealOption => new GetMealAnalysis()
            {
                MealID = mealOption.ID,
                MealOptionID = mealOption.ID,
                MealSizeOption = mealOption.MealSizeOption,
                Name = mealOption.Meal.Name,
                Image = mealOption.ThumbnailImage,
                TotalCost = mealOption.OrderItems.Sum(x => x.TotalCost),
                TotalRevenue = mealOption.OrderItems.Sum(x => x.TotalAmount),
                SoldAmount = mealOption.OrderItems.Sum(x => x.Quantity)
            }).ToListAsync();

            return ListResult<GetMealAnalysis>.Success(meals);
        }

        //var customerOrders = await _context.Orders
        //    .Include(o => o.Customer)
        //    .Include(o => o.OrderItems)
        //        .ThenInclude(oi => oi.MealOption)
        //                    .GroupBy(o => o.Customer)
        //    .Select(group => new
        //    {
        //        Customer = group.Key,
        //        TotalCost = group.Sum(order => order.OrderItems.Sum(item =>
        //            (item.MealOption.MealOptionIngredients.Sum(ingredient =>
        //                (ingredient.ChiefIngredient.CostPerKilo / 1000) * ingredient.AmountInGrams)) * item.Quantity)),
        //        TotalRevenue = group.Sum(order => order.OrderItems.Sum(item => item.MealOption.Price * item.Quantity))
        //    })
        //    .ToListAsync();

        //var customerCostRevenue = customerOrders
        //    .GroupBy(o => o.Customer)
        //    .Select(group => new
        //    {
        //        Customer = group.Key,
        //        TotalCost = group.Sum(order => order.OrderItems.Sum(item =>
        //            (item.MealOption.MealOptionIngredients.Sum(ingredient =>
        //                (ingredient.ChiefIngredient.CostPerKilo / 1000) * ingredient.AmountInGrams)) * item.Quantity)),
        //        TotalRevenue = group.Sum(order => order.OrderItems.Sum(item => item.MealOption.Price * item.Quantity))
        //    })
        //    .ToList();


        return ListResult<GetMealAnalysis>.Failure(["you do not have the accses for this resource"], HttpStatusCode.Forbidden);
    }

    public async Task<ListResult<GetCustomerAnalsis>> GetCustomerAnalsis(string UserID)
    {
        var UserRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == UserID);

        if (UserRole == null)
            return ListResult<GetCustomerAnalsis>.Failure(["you do not have the accsess to this resource"], HttpStatusCode.Forbidden);

        if (UserRole.RoleId == "fb229fa6-3a88-4b19-8292-2ad318c2d82c")
        {
            //var customers = await _context.Customers.Select(customer => new GetCustomerAnalsis()
            //{
            //    CustomerID = customer.Id,
            //    CustomerName = customer.FirstName + " " + customer.LastName,
            //    CustomerImage = customer.UserImage,
            //    TotalOrders = customer.orders.Select(x => x.OrderItems).Count(),
            //    TotalCost = customer.orders.Select(x => x.OrderItems.Sum(item =>
            //        item.MealOption.MealOptionIngredients.Sum(Ingredient => Ingredient.AmountInGrams * (Ingredient.ChiefIngredient.CostPerKilo / 1000))
            //    )).Sum() * customer.orders.Select(x => x.OrderItems.Select(item => item.Quantity))

            //}).ToListAsync();
            var customers = await _context.Customers.Select(customer => new GetCustomerAnalsis()
            {
                CustomerID = customer.Id,
                CustomerName = customer.FirstName + " " + customer.LastName,
                CustomerImage = customer.UserImage,
                TotalOrders = customer.orders.Select(x => x.OrderItems).Count(),
                TotalCost = customer.orders.SelectMany(x => x.OrderItems).Sum(item => item.TotalCost),
                TotalRevenue = customer.orders.SelectMany(x => x.OrderItems).Sum(item => item.TotalAmount)
            }).ToListAsync();

            return ListResult<GetCustomerAnalsis>.Success(customers);
        }
        else if (UserRole.RoleId == "799e083d-076f-4812-b9db-527e85d3911d")
        {

            //var customers = await _context.Customers.Where(x => x.orders.Select(x => x.OrderItems.Any(y => y.MealOption.Meal.ChiefID == UserID))).Select(customer => new GetCustomerAnalsis()
            //{
            //    CustomerID = customer.Id,
            //    CustomerName = customer.FirstName + " " + customer.LastName,
            //    CustomerImage = customer.UserImage,
            //    TotalOrders = customer.orders.Select(x => x.OrderItems).Count(),
            //    TotalCost = customer.orders.SelectMany(x => x.OrderItems).Sum(item => item.TotalCost),
            //    TotalRevenue = customer.orders.SelectMany(x => x.OrderItems).Sum(item => item.TotalAmount)
            //}).ToListAsync();

            var customers = await _context.Customers
                .Where(customer => customer.orders.Any(order => order.OrderItems.Any(item => item.MealOption.Meal.ChiefID == UserID)))
                .Select(customer => new GetCustomerAnalsis()
                {
                    CustomerID = customer.Id,
                    CustomerName = customer.FirstName + " " + customer.LastName,
                    CustomerImage = customer.UserImage,
                    TotalOrders = customer.orders.Where(order => order.OrderItems.Any(item => item.MealOption.Meal.ChiefID == UserID)).Count(),
                    TotalCost = customer.orders.Where(order => order.OrderItems.Any(item => item.MealOption.Meal.ChiefID == UserID))
                                               .SelectMany(order => order.OrderItems).Sum(item => item.TotalCost),
                    TotalRevenue = customer.orders.Where(order => order.OrderItems.Any(item => item.MealOption.Meal.ChiefID == UserID))
                                                  .SelectMany(order => order.OrderItems).Sum(item => item.TotalAmount)
                })
                .ToListAsync();



            //    var customers = await _context.Customers.Include(x => x.orders).ThenInclude(x => x.OrderItems).ThenInclude(x => x.MealOption).ThenInclude(x => x.MealOptionIngredients).ThenInclude(x => x.ChiefIngredient).Select(customer => new
            //    {
            //        Customer = customer,
            //        Orders = customer.orders
            //.Where(x => x.OrderItems.Any(y => y.MealOption.Meal.ChiefID == UserID))
            //    }).ToListAsync();

            //    var customerAnalysis = customers.Select(c =>
            //    {
            //        var totalCost = 0.0;
            //        foreach (var order in c.Orders)
            //        {
            //            foreach (var orderItem in order.OrderItems)
            //            {
            //                totalCost += orderItem.MealOption.MealOptionIngredients.Sum(ingredient =>
            //                    ingredient.AmountInGrams * (ingredient.ChiefIngredient.CostPerKilo / 1000)) * orderItem.Quantity;
            //            }
            //        }

            //        return new GetCustomerAnalsis()
            //        {
            //            CustomerID = c.Customer.Id,
            //            CustomerName = c.Customer.FirstName + " " + c.Customer.LastName,
            //            CustomerImage = c.Customer.UserImage,
            //            TotalOrders = c.Orders.Count(),
            //            TotalCost = totalCost,
            //            TotalRevenue = c.Orders.Select(x => x.TotalAmount).Sum()
            //        };
            //    }).ToList();

            return ListResult<GetCustomerAnalsis>.Success(customers);
        }

        return ListResult<GetCustomerAnalsis>.Failure(["you do not have the accsess to this resource"], HttpStatusCode.Forbidden);
    }

    public async Task<ListResult<GetIngredientAnalysis>> GetIngredientAnalysis(string ChiefID)
    {
        if (!await _context.Chiefs.AnyAsync(x => x.Id == ChiefID))
            return ListResult<GetIngredientAnalysis>.Failure(["you do not have the accsess to this resource"], HttpStatusCode.Forbidden);

        var Ingredients = await _context.ChiefIngredients.Where(x => x.ChiefID == ChiefID).Select(ingredient => new GetIngredientAnalysis()
        {
            Ingredient = ingredient.FoodIngredient,
            UsedAmountInGrams = ingredient.MealOptionIngredients.Sum(x => x.AmountInGrams),
            CostPerKilo = ingredient.CostPerKilo
        }).ToArrayAsync();

        return ListResult<GetIngredientAnalysis>.Success(Ingredients);
    }

    public async Task<ListResult<GetChiefAnalyticsRequest>> GetChiefAnalytics()
    {
        var Chiefs = await _context.OrderItems
            .GroupBy(orderItem => orderItem.MealOption.Meal.Chief.Id)
            .Select(group => new GetChiefAnalyticsRequest()
            {
                ChiefID = group.Key,
                ChiefName = group.First().MealOption.Meal.Chief.FirstName + " " + group.First().MealOption.Meal.Chief.LastName,
                Image = group.First().MealOption.Meal.Chief.ChiefFullScreenImage ?? "",
                TotalCost = group.Sum(orderItem => orderItem.TotalCost),
                TotalRevenue = group.Sum(orderItem => orderItem.TotalAmount)
            }).ToArrayAsync();

        if (Chiefs == null)
            return ListResult<GetChiefAnalyticsRequest>.Failure(["No result found"], HttpStatusCode.NotFound);

        return ListResult<GetChiefAnalyticsRequest>.Success(Chiefs);
    }

    private async Task<List<GetMealRequest>?> GetSimilarMeals(Meal nmeal)
    {
        var meals = await _context.Meals.ToListAsync();

        var ids = meals.Select(meal => new { Meal = meal, Similarity = nmeal.CalculateSimilarity(meal) })
            .OrderByDescending(x => x.Similarity)
            .Select(x => x.Meal.ID)
            .Take(5)
            .ToList();

        if (ids != null && ids.Count > 0)
        {
            var similarMeals = await _context.Meals.Where(x => ids.Contains(x.ID))
        .Select(x => new GetMealRequest()
        {
            ChiefID = x.ChiefID,
            ChiefName = x.Chief.FirstName + " " + x.Chief.LastName,
            ChiefImage = x.Chief.UserImage ?? "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png",
            ChiefDescription = x.Chief.Description ?? "",
            ChiefOrderCount = x.Chief.Meals.SelectMany(m => m.MealOptions).SelectMany(o => o.OrderItems).Count(),
            MealID = x.ID,
            CreatedDate = x.CreatedDate,
            Title = x.Name,
            MealCategory = x.MealCategory,
            MealSpiceLevel = x.MealSpiceLevel,
            MealStyle = x.MealStyle,
            Description = x.Description,
            Rating = x.MealReviews.Any() ? x.MealReviews.Average(x => x.Rating) : 0,
            ReviewCount = x.MealReviews.Count,
            GetMealReviewsRequest = x.MealReviews.Take(4).Select(x => new GetMealReview()
            {
                ReviewText = x.Title,
                CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                CustomerImage = x.Customer.UserImage ?? "https://static-00.iconduck.com/assets.00/profile-default-icon-1024x1023-4u5mrj2v.png",
                Rating = (int)x.Rating,
                ReviewDate = x.CreatedAt,
            }).ToArray(),
            MealTags = x.MealTags.Select(x => x.Tag).ToArray(),
            GetMealOptionsRequest = x.MealOptions.Select(x => new GetMealOptionRequest()
            {
                MealOptionID = x.ID,
                MealSizeOption = x.MealSizeOption,
                IsAvailable = x.IsAvailable,
                Quantity = (int)(x.AvailableQuantity == null ? 0 : x.AvailableQuantity),
                SaveQuantity = (x.DailyQuantity != null),
                Price = x.Price,
                ThumbnailImage = x.ThumbnailImage,
                FullScreenImage = x.FullScreenImage,
                UsedIngredients = x.MealOptionIngredients.Select(ingredient => new UsedIngredient()
                {
                    Ingredient = ingredient.FoodIngredient,
                    AmountInGrams = ingredient.AmountInGrams,
                }).ToArray(),
                GetMealSideDishesRequest = x.MealSideDishes.Select(y => new GetMealSideDishRequest()
                {
                    MealSideDishID = y.ID,
                    IsFree = y.IsFree,
                    IsTopping = y.IsTopping,
                    GetMealSideDishOptionsRequest = y.MealSideDishOptions.Select(z => new GetMealSideDishOptionRequest()
                    {
                        SideDishID = z.SideDishID,
                        SideDishSizeOption = z.SideDishSizeOption,
                        Name = z.SideDishOption.SideDish.Name,
                        Price = z.SideDishOption.Price,
                        Quantity = z.SideDishOption.Quantity
                    }).ToArray()
                }).ToArray()
            }).ToArray(),
        })
        .ToListAsync();

            return similarMeals;
        }

        return null;
    }

    public async Task<ListResult<GetMealRequest>> GetSimilarMeals(Guid MealID)
    {
        var meal = await _context.Meals.FirstOrDefaultAsync(x => x.ID == MealID);

        if (meal == null)
            return ListResult<GetMealRequest>.Failure(["this meal does not exist"], HttpStatusCode.NotFound);

        var meals = await _context.Meals.ToListAsync();

        var ids = meals.Select(x => new { Meal = x, Similarity = meal.CalculateSimilarity(x) })
            .OrderByDescending(x => x.Similarity)
            .Select(x => x.Meal.ID)
            .Take(5)
            .ToList();

        var similarMeals = await _context.Meals.Where(x => ids.Contains(x.ID))
            .Select(x => new GetMealRequest()
            {
                ChiefID = x.ChiefID,
                ChiefName = x.Chief.FirstName + " " + x.Chief.LastName,
                ChiefImage = x.Chief.UserImage ?? "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png",
                ChiefDescription = x.Chief.Description ?? "",
                ChiefOrderCount = x.Chief.Meals.SelectMany(m => m.MealOptions).SelectMany(o => o.OrderItems).Count(),
                MealID = x.ID,
                CreatedDate = x.CreatedDate,
                Title = x.Name,
                MealCategory = x.MealCategory,
                MealSpiceLevel = x.MealSpiceLevel,
                MealStyle = x.MealStyle,
                Description = x.Description,
                Rating = x.MealReviews.Any() ? x.MealReviews.Average(x => x.Rating) : 0,
                ReviewCount = x.MealReviews.Count,
                GetMealReviewsRequest = x.MealReviews.Take(4).Select(x => new GetMealReview()
                {
                    ReviewText = x.Title,
                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                    CustomerImage = x.Customer.UserImage ?? "https://static-00.iconduck.com/assets.00/profile-default-icon-1024x1023-4u5mrj2v.png",
                    Rating = (int)x.Rating,
                    ReviewDate = x.CreatedAt,
                }).ToArray(),
                MealTags = x.MealTags.Select(x => x.Tag).ToArray(),
                GetMealOptionsRequest = x.MealOptions.Select(x => new GetMealOptionRequest()
                {
                    MealOptionID = x.ID,
                    MealSizeOption = x.MealSizeOption,
                    IsAvailable = x.IsAvailable,
                    Quantity = (int)(x.AvailableQuantity == null ? 0 : x.AvailableQuantity),
                    SaveQuantity = (x.DailyQuantity != null),
                    Price = x.Price,
                    ThumbnailImage = x.ThumbnailImage,
                    FullScreenImage = x.FullScreenImage,
                    UsedIngredients = x.MealOptionIngredients.Select(ingredient => new UsedIngredient()
                    {
                        Ingredient = ingredient.FoodIngredient,
                        AmountInGrams = ingredient.AmountInGrams,
                    }).ToArray(),
                    GetMealSideDishesRequest = x.MealSideDishes.Select(y => new GetMealSideDishRequest()
                    {
                        MealSideDishID = y.ID,
                        IsFree = y.IsFree,
                        IsTopping = y.IsTopping,
                        GetMealSideDishOptionsRequest = y.MealSideDishOptions.Select(z => new GetMealSideDishOptionRequest()
                        {
                            SideDishID = z.SideDishID,
                            SideDishSizeOption = z.SideDishSizeOption,
                            Name = z.SideDishOption.SideDish.Name,
                            Price = z.SideDishOption.Price,
                            Quantity = z.SideDishOption.Quantity
                        }).ToArray()
                    }).ToArray()
                }).ToArray()
            }).ToListAsync();

        similarMeals = similarMeals.OrderBy(x => ids.IndexOf(x.MealID)).ToList();


        return ListResult<GetMealRequest>.Success(similarMeals);
    }

    public async Task<ListResult<GetChartData>> GetMealChartData(Guid MealOptionID)
    {
        if (!await _context.MealOptions.AnyAsync(x => x.ID == MealOptionID))
            ListResult<GetChartData>.Failure(["this meal does not exist"], HttpStatusCode.NotFound);

        var MealChartData = await _context.OrderItems
            .Where(x => x.MealOptionID == MealOptionID)
            .Select(orderItem => new GetChartData()
            {
                OrderCount = orderItem.Quantity,
                Date = orderItem.Order.OrderDate.Date,
            })
            .GroupBy(item => item.Date)
            .OrderBy(group => group.Key)
            .Select(group => new GetChartData()
            {
                Date = group.Key,
                OrderCount = group.Sum(item => item.OrderCount)
            })
            .ToArrayAsync();


        return ListResult<GetChartData>.Success(MealChartData);
    }

    public async Task<ListResult<GetChartData>> GetCustomerChartData(string CustomerID, bool IsAdmin, string? ChiefID)
    {
        if (!await _context.Customers.AnyAsync(x => x.Id == CustomerID))
            ListResult<GetChartData>.Failure(["this customer does not exist"], HttpStatusCode.NotFound);
        if (IsAdmin)
        {
            var CustomerChartData = await _context.OrderItems
                .Where(x => x.Order.CustomerID == CustomerID)
                .Select(orderItem => new GetChartData()
                {
                    OrderCount = orderItem.Quantity,
                    Date = orderItem.Order.OrderDate.Date,
                })
                .GroupBy(item => item.Date)
                .OrderBy(group => group.Key)
                .Select(group => new GetChartData()
                {
                    Date = group.Key,
                    OrderCount = group.Sum(item => item.OrderCount)
                })
                .ToArrayAsync();
            return ListResult<GetChartData>.Success(CustomerChartData);

        }
        else
        {
            var CustomerChartData = await _context.OrderItems
                .Where(x => x.Order.CustomerID == CustomerID && x.MealOption.Meal.ChiefID == ChiefID)
                .Select(orderItem => new GetChartData()
                {
                    OrderCount = orderItem.Quantity,
                    Date = orderItem.Order.OrderDate.Date,
                })
                .GroupBy(item => item.Date)
                .OrderBy(group => group.Key)
                .Select(group => new GetChartData()
                {
                    Date = group.Key,
                    OrderCount = group.Sum(item => item.OrderCount)
                })
                .ToArrayAsync();

            return ListResult<GetChartData>.Success(CustomerChartData);

        }

    }

    public async Task<ListResult<GetChartData>> GetIngredientChartData(FoodIngredient Ingredient,string ChiefID)
    {
        if (!await _context.ChiefIngredients.AnyAsync(x => x.ChiefID == ChiefID && x.FoodIngredient == Ingredient))
            ListResult<GetChartData>.Failure(["this Ingredient does not exist"], HttpStatusCode.NotFound);

        //var IngredientChartData = await _context.OrderItems
        //    .Where(x => x.MealOption.Meal.ChiefID == ChiefID && x.MealOption.MealOptionIngredients.Any(x => x.FoodIngredient == Ingredient))
        //    .Select(orderItem => new GetChartData()
        //    {
        //        OrderCount = orderItem.MealOption.MealOptionIngredients.First(x => x.FoodIngredient == Ingredient).AmountInGrams,
        //        Date = orderItem.Order.OrderDate.Date,
        //    })
        //    .GroupBy(item => item.Date)
        //    .OrderBy(group => group.Key)
        //    .Select(group => new GetChartData()
        //    {
        //        Date = group.Key,
        //        OrderCount = group.Sum(item => item.OrderCount)
        //    })
        //    .ToArrayAsync();
        // Fetch the data from the database
        var orderItems = await _context.OrderItems
            .Where(x => x.MealOption.Meal.ChiefID == ChiefID && x.MealOption.MealOptionIngredients.Any(x => x.FoodIngredient == Ingredient))
            .Select(orderItem => new
            {
                OrderDate = orderItem.Order.OrderDate.Date,
                AmountInGrams = orderItem.MealOption.MealOptionIngredients
                    .Where(x => x.FoodIngredient == Ingredient)
                    .Select(x => x.AmountInGrams)
                    .FirstOrDefault(),
                Quantity = orderItem.Quantity
            })
            .ToListAsync();

        // Perform the aggregation in memory
        var IngredientChartData = orderItems
            .GroupBy(item => item.OrderDate)
            .Select(group => new GetChartData()
            {
                Date = group.Key,
                OrderCount = group.Sum(item => item.AmountInGrams * item.Quantity)
            })
            .OrderBy(chartData => chartData.Date)
            .ToArray();



        return ListResult<GetChartData>.Success(IngredientChartData);
    }

    public async Task<ListResult<GetChartData>> GetChiefChartData(string ChiefID)
    {
        if (!await _context.Chiefs.AnyAsync(x => x.Id == ChiefID))
            ListResult<GetChartData>.Failure(["this chief does not exist"], HttpStatusCode.NotFound);

        var ChiefChartData = await _context.OrderItems
            .Where(x => x.MealOption.Meal.ChiefID == ChiefID)
            .Select(orderItem => new GetChartData()
            {
                OrderCount = orderItem.Quantity,
                Date = orderItem.Order.OrderDate.Date,
            })
            .GroupBy(item => item.Date)
            .OrderBy(group => group.Key)
            .Select(group => new GetChartData()
            {
                Date = group.Key,
                OrderCount = group.Sum(item => item.OrderCount)
            })
            .ToArrayAsync();


        return ListResult<GetChartData>.Success(ChiefChartData);
    }
}

//y.MealSideDishOptions.Select(z => new GetMealSideDishOptionRequest()
//{
//    MealSideDishID = z.MealSideDishID,
//                        SideDishSizeOption = z.SideDishSizeOption,
//                        Price = z.SideDishOption.Price,
//                        Quantity = z.SideDishOption.Quantity
//                    }).ToList()