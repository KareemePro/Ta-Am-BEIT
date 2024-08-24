
using FoodDelivery.Data;
using FoodDelivery.Models.DTO.AdminDTO;
using FoodDelivery.Models.DTO.CheifDTO;
using FoodDelivery.Services.Common;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Net;

namespace FoodDelivery.Services.Admin;

public class AdminService(DBContext context) : IAdminService
{
    private readonly DBContext _context = context;

    public async Task<SingleResult<bool>> DesableUser(string UserID)
    {
        var User = await _context.Users.AsTracking().FirstOrDefaultAsync(x => x.Id == UserID);

        if (User == null)
            return SingleResult<bool>.Failure(["this user does not exist"]);

        User.LockoutEnabled = true;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<SingleResult<bool>> EnableUser(string UserID)
    {
        var User = await _context.Users.AsTracking().FirstOrDefaultAsync(x => x.Id == UserID);

        if (User == null)
            return SingleResult<bool>.Failure(["this user does not exist"]);

        User.IsEnabled = true;

        await _context.SaveChangesAsync();

        return SingleResult<bool>.Success(true);
    }

    public async Task<ListResult<GetWaitingUserList>> GetWaitingUserList()
    {
        var Users = await _context.Users.Where(x => x.IsEnabled == false).Select(user => new GetWaitingUserList()
        {
            UserID = user.Id,
            Name = user.FirstName + " " + user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            SignedUpDate = (DateTime)user.SignupDate
        }).ToArrayAsync();

        return ListResult<GetWaitingUserList>.Success(Users);
    }

    public async Task<SingleResult<GetChiefProfileDataRequest>> GetWaitingUser(string ChiefID)
    {
        var ChiefData = await _context.Chiefs.Include(x => x.Building)
            .ThenInclude(x => x.Street).Where(x => x.Id == ChiefID).Select(x => new GetChiefProfileDataRequest()
            {
                ChiefID = x.Id,
                DistrictID = x.Building.Street.DistrictID,
                StreetID = x.Building.StreetID,
                BuildingID = x.BuildingID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Description = x.Description,
                PhoneNumber = x.PhoneNumber,
                CoverImage = x.CoverImage,
                ChiefImage = x.ChiefFullScreenImage,
                HealthCertImage = x.HealthCertImage,
                FloorNumber = x.FloorNumber,
                ApartmentNumber = x.ApartmentNumber,
                StartTime = x.OpeningTime,
                CloseTime = x.ClosingTime,
                GovernmentID = x.GovernmentID
            }).FirstOrDefaultAsync();

        if (ChiefData == null)
            return SingleResult<GetChiefProfileDataRequest>.Failure(["please try to refreash your page"], HttpStatusCode.NotFound);

        return SingleResult<GetChiefProfileDataRequest>.Success(ChiefData);

    }
}
