using Domain.Entidades;

namespace Infraestructura.Persistencia.Repositorios
{
    public interface IProductoRepositorio
    {
        Task<Producto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Producto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task AddAsync(Producto producto, CancellationToken cancellationToken = default);
        Task UpdateAsync(Producto producto, CancellationToken cancellationToken = default);
    }
}
