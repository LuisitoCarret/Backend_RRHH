namespace Modulo.Reclutamiento.Application.Services
{
    public class PostulacionesService
    {
        private readonly IPostulacionRepository _repo;
        private readonly ICloudinaryService _cloudinary;

        public PostulacionesService(IPostulacionRepository repo,ICloudinaryService cloudinary)
        {
            _repo = repo;
            _cloudinary = cloudinary;
        }

        public async Task<PostulacionDetailDto> CreateAsync(CreatePostulacionRequest req, CancellationToken ct)
        {
            // Validaciones
            if (req.VacanteId <= 0)
                throw new BusinessRuleException("No fue posible registrar la postulación.");

            if (string.IsNullOrWhiteSpace(req.NombreContacto))
                throw new BusinessRuleException("El nombre de contacto es obligatorio.");

            string? cvUrl = null;

            // SUBIR PDF A CLOUDINARY
            if (req.CvFile is not null)
            {
                using var stream = req.CvFile.OpenReadStream();
                cvUrl = await _cloudinary.UploadPdfAsync(stream, req.CvFile.FileName);
            }

            // Crear entidad
            var entity = new PostulacionCreate
            {
                VacanteId = req.VacanteId,
                NombreContacto = req.NombreContacto,
                EmailContacto = req.EmailContacto,
                TelefonoContacto = req.TelefonoContacto,
                CvUrl = cvUrl,
                Estatus = req.Estatus ?? "recibida",
                Observacion = req.Observacion,
                FechaPostulacion = req.FechaPostulacion ?? DateTime.Now
            };

            // Guardar en BD
            var created = await _repo.CreateAsync(entity, ct);

            if (created is null)
                throw new BusinessRuleException("No se pudo crear la postulación.");

            // Mapear DTO final
            return new PostulacionDetailDto
            {
                PostulacionId = created.PostulacionId,
                VacanteId = created.VacanteId,
                NombreContacto = created.NombreContacto,
                EmailContacto = created.EmailContacto,
                TelefonoContacto = created.TelefonoContacto,
                CvUrl = created.CvUrl,
                Estatus = created.Estatus,
                Observacion = created.Observacion,
                FechaPostulacion = created.FechaPostulacion
            };
        }

        public async Task<PostulacionListResponseDto> ListAsync(string? vacanteNombre, string? estatus, int page, int pageSize, CancellationToken ct)
        {
            var data = await _repo.ListAsync(vacanteNombre, estatus, page, pageSize, ct);

            return new PostulacionListResponseDto
            {
                Total = data.Total,
                Page = data.Page,
                PageSize = data.PageSize,
                Items = data.Items.Select(x => new PostulacionListItemDto
                {
                    PostulacionId = x.PostulacionId,
                    VacanteId = x.VacanteId,
                    NombreVacante = x.NombreVacante,
                    NombreContacto = x.NombreContacto,
                    Estatus = x.Estatus,
                    FechaPostulacion = x.FechaPostulacion
                }).ToList()
            };
        }

        public async Task<PostulacionDetailsDto> GetDetalleAsync(int id, CancellationToken ct)
        {
            var data = await _repo.GetDetalleAsync(id, ct);

            if (data is null)
                throw new BusinessRuleException("La postulación no existe.");

            return new PostulacionDetailsDto
            {
                PostulacionId = data.PostulacionId,
                VacanteId = data.VacanteId,
                NombreVacante = data.NombreVacante,
                NombreContacto = data.NombreContacto,
                EmailContacto = data.EmailContacto,
                TelefonoContacto = data.TelefonoContacto,
                CvUrl = data.CvUrl,
                Estatus = data.Estatus,
                Observacion = data.Observacion,
                FechaPostulacion = data.FechaPostulacion
            };
        }

        public async Task<PostulacionUpdateResultDto> UpdateAsync(int id, UpdatePostulacionRequest req, CancellationToken ct)
        {
            var valid = new[] { "recibida", "aceptada", "rechazada" };

            if (string.IsNullOrWhiteSpace(req.Estatus) || !valid.Contains(req.Estatus))
                throw new BusinessRuleException("El estatus enviado no es válido. Solo: recibida, aceptada o rechazada.");

            // 2. Obtener detalle actual para hacer validaciones
            var detalleActual = await _repo.GetDetalleAsync(id, ct);
            if (detalleActual is null)
                throw new BusinessRuleException("La postulación no existe.");

            // 3. VALIDACIÓN: No permitir actualizar al mismo estatus
            if (detalleActual.Estatus == req.Estatus)
                throw new BusinessRuleException("La postulación ya tiene este estatus.");

            // 4. VALIDACIÓN: Si la vacante está cerrada, no permitir aceptar
            if (detalleActual.VacanteEstatus == "cerrada" && req.Estatus == "aceptada")
                throw new BusinessRuleException("La vacante ya está cerrada. No puedes aceptar la postulación.");

            if (req.Estatus == "rechazada" && string.IsNullOrWhiteSpace(req.Observacion))
                throw new BusinessRuleException("Debes ingresar una observación al rechazar la postulación.");

            var updated = await _repo.UpdateAsync(id, req.Estatus, req.Observacion, ct);

            return new PostulacionUpdateResultDto
            {
                PostulacionId = updated.PostulacionId,
                VacanteId = updated.VacanteId,
                Estatus = updated.Estatus,
                Observacion = updated.Observacion,
                VacanteEstatus = updated.VacanteEstatus,
                VacanteFechaCierre = updated.VacanteFechaCierre
            };
        }

    }
}
