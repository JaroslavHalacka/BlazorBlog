using Microsoft.AspNetCore.Components.Forms;

namespace BlazorBlog.Dtos
{
    public class UploadFilesInfo
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string LastModified { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
