namespace Domain.Enums;

public enum OrderStatusEnum
{
    AwaitingApproval = 0, //Onay bekliyor
    BeingPrepared = 1, //Hazırlanıyor
    InTransit = 2, 
    Delivered = 3, 
    Rejected = 4, 
    Returned = 5 
}