-- 1. Crear la base de datos (si no existe) y seleccionar el esquema
CREATE DATABASE IF NOT EXISTS `catalogo_db` 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE `catalogo_db`;

-- 2. Eliminar la tabla si ya existe (para reejecutar el script desde cero)
DROP TABLE IF EXISTS `Productos`;

-- 3. Crear la tabla 'Productos' alineada con las convenciones de EF Core y tu Dominio
CREATE TABLE `Productos` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Nombre` VARCHAR(150) NOT NULL,
    `Descripcion` VARCHAR(500) NOT NULL,
    `Precio` DECIMAL(18, 2) NOT NULL,
    `Stock` INT NOT NULL DEFAULT 0,
    `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT `PK_Productos` PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 4. Inserción de datos iniciales para pruebas
INSERT INTO `Productos` (`Nombre`, `Descripcion`, `Precio`, `Stock`, `CreatedAt`) VALUES
('Laptop Gamer ASUS TUF A15', 'Pantalla 15.6 FHD 144Hz, Ryzen 7, 16GB RAM, SSD 512GB, RTX 4050', 4200000.00, 10, NOW()),
('Monitor LG UltraGear 27"', 'Monitor IPS FHD 165Hz 1ms, compatible con FreeSync y G-Sync', 950000.00, 15, NOW()),
('Teclado Mecánico Redragon Kumara K552', 'Teclado TKL switches Red, retroiluminación RGB, distribución español', 210000.00, 25, NOW()),
('Mouse Inalámbrico Logitech G305', 'Sensor HERO 12K DPI, tecnología Lightspeed 1ms, color negro', 180000.00, 30, NOW()),
('Disco Duro Externo Adata HD710 Pro 2TB', 'Disco todoterreno resistente a caídas y agua IP68, USB 3.2', 340000.00, 18, NOW()),
('Memoria RAM Corsair Vengeance LPX 16GB', 'Kit 2x8GB DDR4 3200MHz C16, disipador de aluminio negro', 220000.00, 40, NOW()),
('SSD NVMe M.2 Kingston FURY Renegade 1TB', 'Velocidad de lectura hasta 7300 MB/s, interfaz PCIe 4.0', 410000.00, 12, NOW()),
('Fuente de Poder Corsair RM750e 750W', 'Certificación 80 Plus Gold, completamente modular, ATX 3.0', 580000.00, 8, NOW()),
('Gabinete Gamer NZXT H5 Flow', 'Chasis Mid-Tower con panel frontal perforado y cristal templado', 430000.00, 5, NOW()),
('Impresora Multifuncional HP Smart Tank 580', 'Inyección térmica de tinta con tanques continuos, Wi-Fi y USB', 690000.00, 7, NOW());

-- 5. Consulta para verificar los datos cargados
SELECT * FROM `Productos`;