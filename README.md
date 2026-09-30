# API Catálogo de Productos

API REST en .NET 9 para la gestión del catálogo de productos (crear, consultar, paginar, ajustar stock y eliminar), con MySQL (Pomelo EF Core) y documentación Swagger.

## 1. Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download) (`dotnet --version` → `9.x`)
- MySQL 8 local corriendo en `localhost:3306` (MySQL Installer o XAMPP)
- Git

## 2. Clonar y restaurar

```powershell
git clone https://github.com/hadercoronel/ProductoCatalogo.git
cd ProductoCatalogo
dotnet restore ProductoCatalogo.sln
```

## 3. Crear la base de datos local

Opción A — con el script (incluye 10 productos de prueba):

```sql
-- Ejecutar en MySQL Workbench o consola:
SOURCE CrearDatabase.sql;
```

Opción B — con migraciones (crea el esquema vacío):

```powershell
dotnet ef database update --project Infraestructura --startup-project ProductoCatalogo
```

(Ambas crean la base `catalogo_db`. No uses las dos: son alternativas.)

## 4. Configurar la conexión local (sin modificar archivos)

En la misma terminal donde correrás todo:

```powershell
$env:ConnectionStrings__ProductCatalog = "Server=localhost;Port=3306;Database=catalogo_db;User=root;Password=TU_PASSWORD;AllowPublicKeyRetrieval=True;SslMode=None;"
```

> El `appsettings.json` trae una conexión de ejemplo; la variable de entorno tiene prioridad y la reemplaza solo en tu sesión.

## 5. Ejecutar la API

```powershell
dotnet run --project ProductoCatalogo
```

Abrirá en `http://localhost:5225`. Verifica:

- Salud: `GET http://localhost:5225/health` → `{"status":"healthy"}`
- Swagger: `http://localhost:5225/swagger`

## 6. Probar los endpoints

**Crear producto** `POST /api/ControladorProductos` → `201`:

```json
{ "nombre": "Teclado", "descripcion": "Mecánico RGB", "precio": 210000, "inicialStock": 25 }
```

**Validación** (mismo endpoint, precio y stock negativos) → `400`:

```json
{ "nombre": "Prueba", "descripcion": "Prueba", "precio": -5, "inicialStock": -1 }
```

Respuesta: errores `"El precio no puede ser negativo."` y `"El stock inicial no puede ser negativo."`

**Resto:**

| Método | Ruta | Respuesta |
|---|---|---|
| `GET` | `/api/ControladorProductos?pageNumber=1&pageSize=10` | `200` paginado |
| `GET` | `/api/ControladorProductos/1` | `200` o `404` |
| `PATCH` | `/api/ControladorProductos/1/stock` + `{"cantidadCambio": 5}` | `200` |
| `DELETE` | `/api/ControladorProductos/1` | `204` o `404` |

## 7. Compilar todo (verificación)

```powershell
dotnet build ProductoCatalogo.sln -c Release
```

## Estructura de la solución

| Proyecto | Contenido |
|---|---|
| `ProductoCatalogo` | API (controladores, middleware, configuración) |
| `Aplicacion` | Servicios y DTOs (casos de uso) |
| `Domain` | Entidades y reglas de negocio |
| `Infraestructura` | EF Core (`AplicacionDbContext`), repositorios y migraciones |
