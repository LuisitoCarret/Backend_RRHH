namespace Modulo.Reclutamiento.Domain.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadPdfAsync(Stream fileStream, string fileName);
    }
}
