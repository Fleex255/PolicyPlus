using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PolicyPlus.csharp.Helpers
{
    internal static class Download
    {
        internal static async Task DownloadAndSave(string sourceFile, string destinationFolder, string destinationFileName)
        {
            var fileStream = await GetFileStream(sourceFile);

            if (fileStream != Stream.Null)
            {
                await SaveStream(fileStream, destinationFolder, destinationFileName);
            }
        }

        private static async Task<Stream> GetFileStream(string fileUrl)
        {
            var httpClient = new HttpClient();
            try
            {
                var fileStream = await httpClient.GetStreamAsync(fileUrl);
                return fileStream;
            }
            catch
            {
                return Stream.Null;
            }
        }

        private static async Task SaveStream(Stream fileStream, string destinationFolder, string destinationFileName)
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            var path = Path.Combine(destinationFolder, destinationFileName);

            using var outputFileStream = new FileStream(path, FileMode.CreateNew);
            await fileStream.CopyToAsync(outputFileStream);
        }
    }
}
