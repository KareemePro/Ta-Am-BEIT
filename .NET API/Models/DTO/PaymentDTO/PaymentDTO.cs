namespace FoodDelivery.Models.DTO.PaymentDTO;

public class PaymentDTO
{
    public Guid OrderID { get; set; }
    public Guid BuildingID { get; set; }
    public string FloorNo { get; set; }
    public string ApartmentNo { get; set; }
    public int TotalAmountInPennies { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}
