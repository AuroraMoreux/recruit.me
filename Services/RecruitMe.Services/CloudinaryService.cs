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

            using MemoryStream memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            destinationImage = memoryStream.ToArray();

            using MemoryStream stream = new MemoryStream(destinationImage);
            name = name.Replace("&", "And");
            var fileDescription = new FileDescription(name, stream);

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = fileDescription,
                PublicId = name,
            };

            ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUri.AbsoluteUri;
        }

        public static async Task<string> UploadRawFileAsync(Cloudinary cloudinary, IFormFile file, string name)
        {
            byte[] destinationFile;

            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            destinationFile = memoryStream.ToArray();

            using MemoryStream stream = new MemoryStream(destinationFile);
            name = name.Replace("&", "And");
            var fileDescription = new FileDescription(name, stream);

            RawUploadParams uploadParams = new RawUploadParams()
            {
                File = fileDescription,
                PublicId = name,
            };

            RawUploadResult uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUri.AbsoluteUri;
        }

        public static void DeleteFile(Cloudinary cloudinary, string name)
        {
            DelResParams delParams = new DelResParams()
            {
                PublicIds = new List<string>() { name },
                Invalidate = true,
            };

            cloudinary.DeleteResources(delParams);
        }
    }
}
