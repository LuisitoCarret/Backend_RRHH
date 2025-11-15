using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Modulo.Reclutamiento.Infrastructure.Persistence
{
    public sealed class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinaryOptions> opt)
        {
            var cfg = opt.Value;
            _cloudinary = new Cloudinary(new Account(
                cfg.CloudName,
                cfg.ApiKey,
                cfg.ApiSecret
            ));
        }

        public async Task<string> UploadPdfAsync(Stream fileStream, string fileName)
        {
            var parameters = new RawUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "cv-postulaciones",
                PublicId = Path.GetFileNameWithoutExtension(fileName),
                AccessMode = "public"
            };

            var uploadResult = await _cloudinary.UploadAsync(parameters);
            return uploadResult?.SecureUrl?.ToString() ?? "";
        }
    }
}
