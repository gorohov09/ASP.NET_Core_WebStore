namespace WebStore.Interfaces.Services
{
    public interface IDbInitializer
    {
        Task<bool> RemoveAsync(CancellationToken cancel = default);

        Task InitializeAsync(bool RemoveBefore = false, CancellationToken cancel = default);
    }
}
