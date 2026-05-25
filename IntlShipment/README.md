# Backend - API REST de Gestión de Envíos Internacionales (IntlShipment)

## 1. Descripción Corta del Proyecto
Este directorio contiene el núcleo lógico (Backend) de la solución **IntlShipment**. Consiste en una API RESTful desarrollada en .NET 8 encargada de procesar las reglas de negocio logísticas, gestionar la seguridad del sistema mediante la autenticación de usuarios y garantizar el almacenamiento estructurado y la integridad de los datos de las guías de transporte internacionales.

---

## 2. Tecnologías Utilizadas y Versiones
* **Framework Principal:** .NET 8
* **Acceso a Datos y ORM:** Entity Framework Core v8.x (Mapeo de base de datos a objetos C#)
* **Seguridad y Cifrado:** JWT (JwtBearer Authentication v8.x) para protección de endpoints y hashing seguro para contraseñas.
* **Motor de Base de Datos:** Microsoft SQL Server 2022 (o superior)
* **Documentación de API:** Swagger / OpenAPI (Integrado de forma nativa en .NET 8)

---

## 3. Arquitectura Propuesta, Patrones y Estructura de Carpetas
El sistema implementa una **Arquitectura en Capas (N-Tier Architecture)** apoyada en el patrón **Model-View-Controller (MVC)** para la exposición de servicios REST. Adicionalmente, aplica el patrón de **Inyección de Dependencias** mediante el desacoplamiento de interfaces (`I{Service}`) y el uso de **DTOs (Data Transfer Objects)** para asegurar que las entidades directas de la base de datos no se expongan innecesariamente al cliente.

La estructura real del proyecto se distribuye de la siguiente manera:

text
IntlShipment/
├── Controllers/         # Controladores que exponen los endpoints REST (Auth, Shipments)
├── Data/                # Contexto de Entity Framework (DbContext) y configuración de base de datos
├── DTOs/                # Objetos de Transferencia de Datos (Data Transfer Objects) para peticiones y respuestas
├── Helpers/             # Clases de utilidad general (e.g., utilidades de cifrado o mapeo)
├── Migrations/          # Historial de migraciones automáticas de Entity Framework Core hacia SQL Server
├── Models/              # Entidades del modelo de dominio relacional (User, Shipment)
├── Services/            # Capa de Lógica de Negocio (Abstraída mediante Interfaces y Servicios)
│   ├── IAuthService.cs       -> Interfaz de autenticación
│   ├── AuthService.cs        -> Implementación de lógica de login y tokens
│   ├── IShipmentService.cs   -> Interfaz de lógica de envíos
│   └── ShipmentService.cs    -> Implementación de operaciones CRUD logísticas
├── Validators/          # Clases encargadas de la validación de reglas de entrada y datos de DTOs
├── appsettings.json     # Configuración global de la app (Cadenas de conexión y parámetros JWT)
├── Program.cs           # Punto de entrada de la aplicación, configuración de middlewares y pipeline HTTP
└── README.md            # Este archivo de documentación técnica

---
## 4. Arquitectura Propuesta, Patrones y Estructura de Carpetas
La base de datos relacional en SQL Server interactúa con el sistema mediante los siguientes modelos definidos en
User:Contiene la información de acceso de los operadores del sistema.
campos clave : Id, Email, Password, Rol
Shipment: Registra el histórico y estado físico de los paquetes.
campos clave: 
Identificador Único (PK): Id (Llave primaria autoincremental).

Número de Rastreo (Unique): NumeroGuia (El identificador logístico del paquete).

Ruta Logística: PaisOrigen, PaisDestino, CiudadOrigen, CiudadDestino.

Actores: NombreRemitente, NombreDestinatario.

Detalles de Carga: DescripcionMercancia, PesoKg (Decimal).

Control de Estado: Estado (Crucial para los filtros de tu app).

Auditoría y Trazabilidad: FechaCreacion, FechaEstimadaEntrega.


---

## 5. Pasos para Crear o Migrar la Base de Datos
Para desplegar la base de datos localmente utilizando Migraciones de Entity Framework Core, siga estos pasos:


- Abra la consola de comandos o la consola de administración de paquetes NuGet en la raíz de la carpeta del backend.
- **Realizar restauración de paquetes
  dotnet restore


-**Instalar la herramienta de forma global** (solo se hace una vez en el computador):
   dotnet tool install --global dotnet-ef

- **Aplicar la Migración a SQL Server**
dotnet ef database update


## 6. Explicación de la Consulta SQL Incluida
Como requerimiento analítico del proyecto, se ha optimizado la siguiente consulta de agregación dentro del sistema para reportes del negocio:
SELECT Estado, COUNT(*) AS Total, SUM(PesoKg) AS PesoTotal
FROM Shipments
GROUP BY Estado
ORDER BY Total DESC;

Esta consulta agrupa (GROUP BY) todos los registros de la tabla de envíos según su estado operativo actual (ej. Pendiente, En Tránsito, Entregado). Aplica la función de agregación COUNT(*) para obtener la cantidad de paquetes en cada fase y SUM(PesoKg) para calcular la carga total acumulada en kilogramos que se está movilizando. Finalmente, ordena los resultados de mayor a menor volumen (ORDER BY Total DESC), lo que permite a la gerencia identificar cuellos de botella en la bodega de distribución de forma inmediata.


## 6. Configuración appsettings.Development.json
Abra el archivo appsettings.json y configure la cadena de conexión de SQL Server junto con la firma secreta de JWT:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=Nombre_Intancia_Local + \\SQLEXPRESS;Database=Shipment;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "Clave minimo 30 caracteres",
    "Issuer": "Shipment",
    "Audience": "Angular"
  }
}


