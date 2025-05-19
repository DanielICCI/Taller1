# Taller 1 - Backend ASP.NET Core

Este proyecto corresponde al backend del Taller 1 y ha sido desarrollado utilizando el framework ASP.NET Core junto con Entity Framework Core, autenticación JWT y almacenamiento de imágenes en Cloudinary.

## Tecnologías utilizadas

* ASP.NET Core 8
* Entity Framework Core
* SQLite como base de datos
* JWT (JSON Web Tokens) para autenticación
* Cloudinary para subida de imágenes
* Postman para pruebas de API

## Instrucciones para ejecutar el proyecto

1. Clonar el repositorio:

```bash
git clone https://github.com/DanielICCI/Taller1.git
cd Taller1
```

2. Restaurar paquetes y compilar:

```bash
dotnet restore
dotnet build
```

3. Aplicar las migraciones y crear la base de datos:

```bash
dotnet ef database update
```

4. Ejecutar el proyecto:

```bash
dotnet run
```

## Usuario administrador por defecto

Se crea un usuario administrador automáticamente al iniciar la aplicación. Sus credenciales son:

* Correo electrónico: `admin@taller.com`
* Contraseña: `Admin123!`

## Endpoints principales

| Método | Endpoint             | Requiere autenticación | Descripción                     |
| ------ | -------------------- | ---------------------- | ------------------------------- |
| POST   | `/api/auth/register` | No                     | Registro de nuevos usuarios     |
| POST   | `/api/auth/login`    | No                     | Inicio de sesión (devuelve JWT) |
| GET    | `/api/product`       | Sí                     | Listado de productos            |
| POST   | `/api/product`       | Sí (rol Admin)         | Crear producto con imagen       |
| PUT    | `/api/product/{id}`  | Sí (rol Admin)         | Modificar producto              |
| DELETE | `/api/product/{id}`  | Sí (rol Admin)         | Eliminar producto               |

## Pruebas con Postman

Para probar la API se puede utilizar Postman. Se incluye una colección con los principales endpoints.


## Autor

Daniel Cruz

Entrega correspondiente al Taller 1 de la asignatura Desarrollo Web / Backend
