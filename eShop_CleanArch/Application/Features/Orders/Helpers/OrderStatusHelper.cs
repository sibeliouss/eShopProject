using Domain.Enums;

namespace Application.Features.Orders.Helpers;

public static class OrderStatusHelper
{
    public static string GetLocalizedStatus(OrderStatusEnum status, string language)
    {
        var translations = new Dictionary<OrderStatusEnum, (string English, string Turkish)>
        {
            { OrderStatusEnum.AwaitingApproval, ("Awaiting Approval", "Onay Bekliyor") },
            { OrderStatusEnum.BeingPrepared, ("Being Prepared", "Hazırlanıyor") },
            { OrderStatusEnum.InTransit, ("In Transit", "Yolda") },
            { OrderStatusEnum.Delivered, ("Delivered", "Teslim Edildi") },
            { OrderStatusEnum.Rejected, ("Rejected", "Reddedildi") },
            { OrderStatusEnum.Returned, ("Returned", "İade Edildi") },
        };

        if (translations.TryGetValue(status, out var translation))
        {
            return language.ToLower() switch
            {
                "tr" => translation.Turkish,
                "en" => translation.English,
                _ => translation.English
            };
        }

        return "Unknown";
    }
}