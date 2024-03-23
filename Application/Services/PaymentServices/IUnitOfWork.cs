namespace Application.Services.PaymentServices
{
    public interface IUnitOfWork
    {
        IPaypalServices PaypalServices { get; }
    }
}
