using Application.Services.PaymentServices.PayPal;

namespace Application.Services.PaymentServices.UnitWork
{
    public interface IUnitOfWork
    {
        IPaypalServices PaypalServices { get; }
    }
}
