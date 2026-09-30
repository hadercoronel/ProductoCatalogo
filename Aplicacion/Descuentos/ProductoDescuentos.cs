namespace Aplicacion.Descuentos
{
    public class ProductoDescuentos
    {
        public record CreateProductDto(string Nombre, string Descripcion, decimal Precio, int InicialStock);

        public record UpdateStockDto(int cantidadCambio);

        public record ProductResponseDto(int Id, string Nombre, string Descripcion, decimal Precio, int Stock, DateTime CreatedAt, DateTime? UpdatedAt);

        public record PagedResultDto<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount, int TotalPages);
    }
}
