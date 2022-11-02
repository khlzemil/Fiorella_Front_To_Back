namespace Fiorello_Front_To_Back.Helpers
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string webRootPath);
        void Delete(string fileName, string webRootPath);

        bool IsImage(IFormFile file);
        bool CheckSize(IFormFile file, int size);
    }
}
