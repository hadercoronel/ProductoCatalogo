using System.ComponentModel.DataAnnotations;

namespace Aplicacion.Descuentos
{
    public class ProductoDescuentos
    {
        public record CreateProductDto(
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [MinLength(1, ErrorMessage = "El nombre no puede estar vacío.")]
            string Nombre,
            string Descripcion,
            [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "El precio no puede ser negativo.")]
            decimal Precio,
            [Range(0, int.MaxValue, ErrorMessage = "El stock inicial no puede ser negativo.")]
            int InicialStock);

        public record UpdateStockDto(int cantidadCambio);

        public record ProductResponseDto(int Id, string Nombre, string Descripcion, decimal Precio, int Stock, DateTime CreatedAt, DateTime? UpdatedAt);

        public record PagedResultDto<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount, int TotalPages);
    }
}
