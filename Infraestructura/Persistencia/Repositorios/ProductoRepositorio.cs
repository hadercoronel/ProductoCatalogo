using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Persistencia.Repositorios
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly AplicacionDbContext _context;
        public ProductoRepositorio(AplicacionDbContext context)
        {
            _context = context;
        }
        public async Task<Producto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Productos.FindAsync(new object[] { id }, cancellationToken);
        }
        public async Task<(IEnumerable<Producto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await _context.Productos.CountAsync(cancellationToken);
            var items = await _context.Productos
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return (items, totalCount);
        }
        public async Task AddAsync(Producto producto, CancellationToken cancellationToken = default)
        {
            await _context.Productos.AddAsync(producto, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Producto producto, CancellationToken cancellationToken = default)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
