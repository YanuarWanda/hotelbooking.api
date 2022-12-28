namespace hotelbooking.api.SharedKernel.Interfaces;

public interface IBaseReadRepository<T> where T : class
{
	Task<IReadOnlyList<T>> GetAllAsync(int page, int size, CancellationToken cancellationToken);
	Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken);
	Task<bool> IsExistsAsync(string id, CancellationToken cancellationToken);
}