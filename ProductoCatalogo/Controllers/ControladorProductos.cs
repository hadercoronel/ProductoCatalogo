using Aplicacion.Servicios;
using Microsoft.AspNetCore.Mvc;
using static Aplicacion.Descuentos.ProductoDescuentos;

namespace ProductoCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ControladorProductos : ControllerBase
    {
        private readonly IProductoServicio _productService;

        public ControladorProductos(IProductoServicio productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Crea un nuevo producto en el catálogo.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
        {
            var result = await _productService.CreateProductAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Consulta un producto por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _productService.GetProductByIdAsync(id, ct);
            if (result == null) return NotFound(new { message = $"Producto con ID {id} no encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// Consulta el catálogo de productos con paginación.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest(new { message = "El número de página y el tamaño deben ser mayores a cero." });

            var result = await _productService.GetPagedProductsAsync(pageNumber, pageSize, ct);
            return Ok(result);
        }

        /// <summary>
        /// Incrementa o disminuye el stock de un producto.
        /// </summary>
        [HttpPatch("{id:int}/stock")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AdjustStock(int id, [FromBody] UpdateStockDto dto, CancellationToken ct)
        {
            var result = await _productService.AdjustStockAsync(id, dto, ct);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _productService.DeleteProductAsync(id, ct);

            if (!deleted)
                return NotFound(new { message = $"Producto con ID {id} no encontrado." });

            return NoContent(); // 204 No Content: borrado exitoso sin cuerpo de respuesta
        }
    }
}
