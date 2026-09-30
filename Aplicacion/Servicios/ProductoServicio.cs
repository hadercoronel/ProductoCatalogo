using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Descuentos;
using Domain.Entidades;
using Infraestructura.Excepciones;
using Infraestructura.Persistencia.Repositorios;
using static Aplicacion.Descuentos.ProductoDescuentos;

namespace Aplicacion.Servicios
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly IProductoRepositorio _repositorio;

        public ProductoServicio(IProductoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new DomainExcepciones("El nombre del producto es obligatorio.");
            if (dto.Precio < 0)
                throw new DomainExcepciones("El precio no puede ser negativo.");
            if (dto.InicialStock < 0)
                throw new DomainExcepciones("El stock inicial no puede ser negativo.");

            var producto = new Producto(dto.Nombre, dto.Descripcion, dto.Precio, dto.InicialStock);
            await _repositorio.AddAsync(producto, ct);
            return MapToDto(producto);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var product = await _repositorio.GetByIdAsync(id, ct);
            return product == null ? null : MapToDto(product);
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken ct = default)
        {
            return await _repositorio.DeleteAsync(id, ct);
        }

        public async Task<PagedResultDto<ProductResponseDto>> GetPagedProductsAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var (items, totalCount) = await _repositorio.GetPagedAsync(pageNumber, pageSize, ct);
            var dtos = items.Select(MapToDto);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResultDto<ProductResponseDto>(dtos, pageNumber, pageSize, totalCount, totalPages);
        }

        public async Task<ProductResponseDto> AdjustStockAsync(int id, UpdateStockDto dto, CancellationToken ct = default)
        {
            var producto = await _repositorio.GetByIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"El producto con ID {id} no fue encontrado.");

            producto.AdjustStock(dto.cantidadCambio); // Aplica la regla de negocio del dominio
            await _repositorio.UpdateAsync(producto, ct);

            return MapToDto(producto);
        }

        private static ProductResponseDto MapToDto(Producto p) =>
            new(p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.CreatedAt, p.UpdatedAt);
    }
}
