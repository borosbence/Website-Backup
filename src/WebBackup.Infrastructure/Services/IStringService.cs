namespace WebBackup.Infrastructure.Services
{
    public interface IStringResourceService
    {
        string? GetValue(string resourceKey);
    }
}
