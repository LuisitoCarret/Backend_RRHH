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
> Nota: se omiten carpetas de build (`bin/`, `obj/`) para claridad.

### 2.1 Árbol de solución (real)

Sistema de Gestion de RRHH.sln
├─ .github/
│ └─ workflows/
├─ Modulo.Asistencias.Application/
│ ├─ Class1.cs
│ └─ Modulo.Asistencias.Application.csproj
├─ Modulo.Asistencias.Domain/
│ ├─ Class1.cs
│ └─ Modulo.Asistencias.Domain.csproj
├─ Modulo.Asistencias.Infrastructure/
│ ├─ Class1.cs
│ └─ Modulo.Asistencias.Infrastructure.csproj
├─ Modulo.Contratos.Application/
│ ├─ Class1.cs
│ └─ Modulo.Contratos.Application.csproj
├─ Modulo.Contratos.Domain/
│ ├─ Class1.cs
│ └─ Modulo.Contratos.Domain.csproj
├─ Modulo.Contratos.Infrastructure/
│ ├─ Class1.cs
│ └─ Modulo.Contratos.Infrastructure.csproj
├─ Modulo.Empelados.Application/ ← (typo: debería ser Modulo.Empleados.Application)
│ ├─ Class1.cs
│ └─ Modulo.Empelados.Application.csproj
├─ Modulo.Empleados.Domain/
│ ├─ Class1.cs
│ └─ Modulo.Empleados.Domain.csproj
├─ Modulo.Empleados.Infrastructure/
│ ├─ Class1.cs
│ └─ Modulo.Empleados.Infrastructure.csproj
├─ Modulo.Evaluaciones.Application/
│ ├─ Class1.cs
│ └─ Modulo.Evaluaciones.Application.csproj
├─ Modulo.Evaluaciones.Domain/
│ ├─ Class1.cs
│ └─ Modulo.Evaluaciones.Domain.csproj
├─ Modulo.Evaluaciones.Infrastructure/
│ ├─ Class1.cs
│ └─ Modulo.Evaluaciones.Infrastructure.csproj
├─ Modulo.Reclutamiento.Application/
│ ├─ Class1.cs
│ └─ Modulo.Reclutamiento.Application.csproj
├─ Modulo.Reclutamiento.Domain/
│ ├─ Class1.cs
│ └─ Modulo.Reclutamiento.Domain.csproj
├─ Modulo.Reclutamiento.Infrastructure/
│ ├─ Class1.cs
│ └─ Modulo.Reclutamiento.Infrastructure.csproj
└─ Sistema de Gestion de RRHH/ # Web API (Gateway)
├─ Controllers/
│ └─ WeatherForecastController.cs
├─ Program.cs
├─ appsettings.json
├─ appsettings.Development.json
├─ Properties/
│ └─ launchSettings.json
└─ Sistema de Gestion de RRHH.csproj



### 2.2 ¿Qué contiene cada proyecto/carpeta?

**Sistema de Gestion de RRHH (Gateway / Web API)**  
- `Controllers/` → Endpoints HTTP (actualmente `WeatherForecastController` de plantilla).  
- `Program.cs` → configuración de DI, Swagger, Auth/JWT (cuando se agregue), CORS, Logging.  
- `appsettings*.json` → configuración por entorno.  

**Modulo.*.Domain (negocio puro)**  
- Aquí van **Entities** (reglas/invariantes del dominio) y **Interfaces** (Repositories/Services).  
- Sin dependencias de infraestructura.  

**Modulo.*.Application (casos de uso)**  
- Aquí van **Services** (orquestación de lógica), **DTOs** (Request/Response) y **Validators** (FluentValidation).  
- Puede usarse MediatR para Commands/Queries.  

**Modulo.*.Infrastructure (infraestructura por módulo)**  
- Aquí se agregan los `DbContext` y configuraciones EF Core.  
- Implementaciones concretas de repositorios y **Migrations**.  

### 2.3 Observaciones y mejoras rápidas

- **Typo detectado:** renombrar `Modulo.Empelados.Application` → `Modulo.Empleados.Application` para mantener consistencia.  
- **Nombre del Gateway:** se recomienda renombrar `Sistema de Gestion de RRHH` → `Gateway.Api` para seguir el estándar de Clean Architecture.  
- **Estructura futura:** a medida que avances, crea las carpetas internas típicas:  
  - Domain → `Entities/`, `Interfaces/Repositories/`, `Interfaces/Services/`  
  - Application → `Services/`, `DTOs/`, `Validators/`  
  - Infrastructure → `DataContexts/`, `Persistence/Repositories/`, `Migrations/`  
