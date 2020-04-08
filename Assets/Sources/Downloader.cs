using System.IO;
using System.Net;

public class Downloader
{
    public static string HttpDownloadFile(string url, string path)
    {
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        Stream responseStream = response.GetResponseStream();
        Stream stream = new FileStream(path, FileMode.Create);
        byte[] bArr = new byte[1024];
        int size = responseStream.Read(bArr, 0, (int)bArr.Length);
        while (size > 0)
        {
            stream.Write(bArr, 0, size);
            size = responseStream.Read(bArr, 0, (int)bArr.Length);
        }
        stream.Close();
        responseStream.Close();
        return path;
    }
}
