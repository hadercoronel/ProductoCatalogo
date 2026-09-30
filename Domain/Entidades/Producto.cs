namespace Domain.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; } = DateTime.UtcNow;
        private Producto() { }

        public Producto(string nombre, string descripcion, decimal precio, int inicialStock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del producto es obligatorio.", nameof(nombre));
            if (precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(precio));
            if (inicialStock < 0)
                throw new ArgumentException("El stock inicial no puede ser negativo.", nameof(inicialStock));

            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Stock = inicialStock;
        }

        public void AdjustStock(int cantidadCambio)
        {
            if (Stock + cantidadCambio < 0)
                Stock = 0;
            else
                Stock += cantidadCambio;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
