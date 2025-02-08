using CloudinaryDotNet.Actions;

namespace RunGroupWebApp.Services.Interfaces
{
    public interface IPhotoservice
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
