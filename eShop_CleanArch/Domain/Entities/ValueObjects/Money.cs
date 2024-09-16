namespace Domain.Entities.ValueObjects;

public class Money
{
    public Money(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public decimal Value { get; private set; }
    public string Currency { get; private set; } 
}