## 1) Resumen del Proyecto

**Nombre de la solución:** RRHH  
**Tipo:** ASP.NET Core Web API (Gateway) + librerías por módulo bajo enfoque de **Clean Architecture**.  

**Módulos iniciales implementados:**
- Empleados  
- Asistencias  
- Contratos  
- Reclutamiento  
- Evaluaciones  

**Objetivo de este documento:**  
Definir claramente, desde el inicio del proyecto, la **estructura de la solución**, las **dependencias necesarias**, el **flujo de ramas en Git** y los **estándares de desarrollo** que guiarán el trabajo en equipo.  
Esto permitirá mantener un código **organizado, escalable y mantenible**.  


## 2) Estructura de Carpetas y Proyectos

> Rama: `develop/sprint1`  

### 2.1 Árbol de solución (visual)

![Estructura del proyecto](Sistema%20de%20Gestion%20de%20RRHH/docs/estructura.jpg)
> Nota: se corrigió el typo en **Modulo.Empleados.Application**.  
> Se muestran todos los módulos (Asistencias, Contratos, Empleados, Evaluaciones y Reclutamiento) con sus capas **Domain, Application e Infrastructure**, además del proyecto **Gateway** (`Sistema de Gestion de RRHH`).

### 2.2 ¿Qué contiene cada proyecto/carpeta?

**Gateway (Sistema de Gestion de RRHH / Web API)**  
- `Controllers/` → Endpoints HTTP; delegan a la capa Application.  
- `Program.cs` → configuración de inyección de dependencias (DI), Swagger, autenticación, CORS y logging.  
- `appsettings*.json` → configuración por entorno.  

**Modulo.*.Domain (negocio puro)**  
- Entidades (Entities) con reglas de negocio.  
- Interfaces para repositorios y servicios (sin dependencias de infraestructura).  

**Modulo.*.Application (casos de uso)**  
- Casos de uso y lógica de aplicación (Services, Commands/Queries).  
- DTOs para Request/Response.  
- Validators con FluentValidation.  

**Modulo.*.Infrastructure (infraestructura por módulo)**  
- DbContext y configuraciones de EF Core.  
- Implementaciones concretas de repositorios.  
- Migrations para la base de datos.  

### 2.3 Observaciones

- Se recomienda renombrar el proyecto **Sistema de Gestion de RRHH** a `Gateway.Api` para mayor claridad y alineación con Clean Architecture.  
- Mantener consistencia en nombres de módulos (ya corregido el typo en *Empleados*).  
