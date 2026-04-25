namespace pftc_auth.interfaces;

public interface IBucketStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage);
    Task DeleteFileAsync(string fileName); 
}