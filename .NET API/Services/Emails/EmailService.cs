using FoodDelivery.Models.DominModels.Auth;
using FoodDelivery.Models.DTO.AuthDTO;
using FoodDelivery.Services.Common;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json.Linq;
using System.Net;

namespace FoodDelivery.Services.Emails;

public class EmailService : IEmailService
{
    private readonly IMailjetClient _mailjetClient;

    public EmailService(IMailjetClient mailjetClient)
    {
        _mailjetClient = mailjetClient;
    }

    public async Task<SingleResult<bool>> SendEmailConfirmation(EmailData email)
    {
        var sendEmail = new TransactionalEmailBuilder()
         .WithFrom(new SendContact("support@taambeit.live", "Ta'am Biet"))
         .WithSubject(email.Subject)
         .WithHtmlPart(email.Body)
         .WithTo(new SendContact(email.To))
         .Build();

        var response = await _mailjetClient.SendTransactionalEmailAsync(sendEmail);
        if (response.Messages != null)
        {
            if (response.Messages[0].Status == "success")
            {
                return SingleResult<bool>.Success(true);
            }
        }

        return SingleResult<bool>.Failure(["We are unable to vertify your email right now please try again later"],
            HttpStatusCode.BadRequest);
    }
}
