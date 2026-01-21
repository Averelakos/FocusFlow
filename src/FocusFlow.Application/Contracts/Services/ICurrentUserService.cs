public interface ICurrentUserService
{
    long? GetUserId();
    string? GetUserEmail();
    string? GetUsername();
}
