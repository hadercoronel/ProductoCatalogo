using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aplicacion.Descuentos.ProductoDescuentos;

namespace Aplicacion.Servicios
{
    public interface IProductoServicio
    {
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto, CancellationToken ct = default);
        Task<ProductResponseDto?> GetProductByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResultDto<ProductResponseDto>> GetPagedProductsAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<ProductResponseDto> AdjustStockAsync(int id, UpdateStockDto dto, CancellationToken ct = default);
    }
}
