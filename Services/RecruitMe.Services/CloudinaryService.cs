namespace RecruitMe.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public static class CloudinaryService
    {
        public static async Task<string> UploadImageAsync(Cloudinary cloudinary, IFormFile image, string name)
        {
            byte[] destinationImage;

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            destinationImage = memoryStream.ToArray();

            using var stream = new MemoryStream(destinationImage);
            name = name.Replace("&", "And");
            var fileDescription = new FileDescription(name, stream);

            var uploadParams = new ImageUploadParams()
            {
                File = fileDescription,
                PublicId = name,
            };

            var uploadResult = cloudinary.Upload(uploadParams);
            if (uploadResult.SecureUri == null)
            {
                return null;
            }

            return uploadResult.SecureUri.AbsoluteUri;
        }

        public static async Task<string> UploadRawFileAsync(Cloudinary cloudinary, IFormFile file, string name)
        {
            byte[] destinationFile;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            destinationFile = memoryStream.ToArray();

            using var stream = new MemoryStream(destinationFile);
            name = name.Replace("&", "And");
            var fileDescription = new FileDescription(name, stream);

            var uploadParams = new RawUploadParams()
            {
                File = fileDescription,
                PublicId = name,
            };

            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUri.AbsoluteUri;
        }

        public static void DeleteFile(Cloudinary cloudinary, string name)
        {
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
            };

            cloudinary.DeleteResources(delParams);
        }
    }
}
