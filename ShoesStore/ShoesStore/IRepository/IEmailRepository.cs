namespace ShoesStore.IRepository
{
    public interface IEmailRepository
    {
        public Task SendEmail(string receptor, string subject, string body);
    }
}
