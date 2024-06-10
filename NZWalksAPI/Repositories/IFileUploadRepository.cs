using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IFileUploadRepository
    {
        Task<Image> upload(Image image);
    }
}
