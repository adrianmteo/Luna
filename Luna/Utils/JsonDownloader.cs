using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Luna.Utils
{
    public class JsonDownloader
    {
        public WebClient Client { get; private set; }

        public JsonDownloader()
        {
            Client = new WebClient();
            Client.Headers.Add("User-Agent", Assembly.GetExecutingAssembly().FullName);
        }

        public async Task<T> GetObject<T>(string url)
        {
            string json = await Client.DownloadStringTaskAsync(url);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<string> GetTempFile(string url, string filename)
        {
            string extension = Path.GetExtension(filename);
            string path = Path.ChangeExtension(Path.GetTempFileName(), extension);

            await Client.DownloadFileTaskAsync(url, path);

            return path;
        }
    }
}
