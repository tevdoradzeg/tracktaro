namespace TrackTaro.Api.Helpers;

public class UploadHandler
{
    private List<string> _validExtensions = [".png", ".jpg"];
    private const long sizeLimit = 5 * 1024 * 1024;

    public string Upload(IFormFile file)
    {
        // Check for empty file
        if (file == null)
        {
            return "Invalid: No file uploaded.";
        }

        if (file.Length == 0)
        {
            return "Invalid: File is empty.";
        }

        // Check extension
            string inpExtension = Path.GetExtension(file.FileName);
        if (!_validExtensions.Contains(inpExtension)) { return $"Invalid file extension: {inpExtension}"; }

        // Check size
        long size = file.Length;
        if (size > sizeLimit) { return $"Invalid file size: {sizeLimit}"; }

        // Generate name
        string fileName = Guid.NewGuid().ToString() + inpExtension;
        return fileName;
    }
}