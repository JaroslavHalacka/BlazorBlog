namespace BlazorBlog.Services.ImageService
{
    public interface IImageService
    {
        Task<ServiceResponse<bool>> AddUploadImage(UploadImageDto uploadImage);
        Task<ServiceResponse<List<UploadImageDto>>> GetListImageByArticleId(int articleId);
        Task<ServiceResponse<bool>> DeleteImage(int articleId, string nameImage, string fullPathImage);

    }
}
