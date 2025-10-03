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

### 2.2 ¿Qué contiene Clean Architecture?

**Gateway (Presentacion / Web API)**  
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

## 3) ¿Qué contiene cada proyecto/carpeta?

**Gateway.Api (Web API / Presentación)**
- `Controllers/` → Endpoints HTTP; delegan a *Application*.
- `Program.cs` → DI (inyección de dependencias), Swagger, Auth/JWT, CORS, Logging.

**Modulo.X.Domain (negocio puro)**
- `Entities/` → Clases que representan los objetos principales del negocio.
- `Interfaces/Repositories` → Contratos de acceso a datos (sin EF).
- `Interfaces/Services` → Servicios de dominio (si aplica).

**Modulo.X.Application (casos de uso)**
- `Services/` → Aquí van las clases que contienen la lógica para ejecutar acciones del sistema.
- `DTOs/` → Request/Response hacia/desde la capa de presentación.
- `Validators/` → Validaciones (FluentValidation) a nivel de caso de uso.

**Modulo.X.Infrastructure (infraestructura por módulo)**
- `DataContexts/` → `DbContext(s)` EF Core y configuraciones.
- `Persistence/` → Repositorios concretos (implementan contratos de Domain/Application) y `Migrations/`.

> Resumen de responsabilidades:
> - **Domain**: el *qué* (reglas de negocio).
> - **Application**: el *cómo* (casos de uso).
> - **Infrastructure**: detalles técnicos por módulo (persistencia, EF Core).
> - **Gateway.Api**: expone la API y configura el host.

> Dependencias (dirección):
> `Gateway.Api → Application → Domain`
> y `Infrastructure` implementa contratos definidos en Domain/Application.

## 4) Ramas de Git (flujo y uso)

### 4.1 Ramas principales

- *main*  
  - Rama de *producción*.  
  - Solo recibe merges desde release o hotfix.  

- *develop*  
  - Rama de *integración general*.  
  - Base donde se integran los sprints.  
  - Nunca se hace commit directo (solo Pull Request).  

### 4.2 Ramas de trabajo

- *sprint1, sprint2, ...*  
  - Ramas de cada *sprint*.  
  - Se crean a partir de develop.  
  - Sirven como base para las ramas personales de cada integrante.  

- *sprint1_integranteX*  
  - Ramas personales o por módulo dentro de un sprint.  
  - Se mergean hacia la rama del sprint correspondiente.  

- *feature/nombre-corto* (opcional)  
  - Ramas específicas por funcionalidad.  
  - Se integran a develop o al sprint activo.  

- *release/x.y.z*  
  - Rama de *estabilización/QA* antes de pasar a producción.  
  - Una vez mergeada a main y develop, se elimina.  

- *hotfix/fix-descriptivo*  
  - Ramas para *parches urgentes* desde producción (main).  
  - Se mergean tanto a main como a develop para no perder cambios.  

### 4.3 Reglas de flujo

- No se permiten commits directos en main ni en develop.  
- Todo cambio debe entrar por *Pull Request* y revisión.  
- Borrar ramas temporales (sprint, feature, release, hotfix) después del merge.  
- Mantener ramas actualizadas con git pull --rebase o con “Update branch” en el PR.
