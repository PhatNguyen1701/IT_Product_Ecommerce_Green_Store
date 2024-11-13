namespace ITProductECommerce.Services.EmailServices
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
