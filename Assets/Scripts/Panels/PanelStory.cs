using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using UnityEngine;

public class PanelStory : MonoBehaviour
{
    private string host = "https://txt80.com/";
    public StoryType storyType = StoryType.xuanhuan;

    // Start is called before the first frame update
    void Start()
    {
        string url = host+ storyType.ToString();
        string html;
        HttpWebRequest Web_Request = (HttpWebRequest)WebRequest.Create(url);
        Web_Request.Timeout = 30000;
        Web_Request.Method = "GET";
        Web_Request.UserAgent = "Mozilla/4.0";
        Web_Request.Headers.Add("Accept-Encoding", "gzip, deflate");
         
        HttpWebResponse Web_Response = (HttpWebResponse)Web_Request.GetResponse();

        if (Web_Response.ContentEncoding.ToLower() == "gzip")  // 如果使用了GZip则先解压
        {
            Debug.LogWarning("gzip");
            using (Stream Stream_Receive = Web_Response.GetResponseStream())
            {
                using (var Zip_Stream = new GZipStream(Stream_Receive, CompressionMode.Decompress))
                {
                    using (StreamReader Stream_Reader = new StreamReader(Zip_Stream, Encoding.Default))
                    {
                        html = Stream_Reader.ReadToEnd();
                    }
                }
            }
        }
        else
        {
            using (Stream Stream_Receive = Web_Response.GetResponseStream())
            {
                using (StreamReader Stream_Reader = new StreamReader(Stream_Receive, Encoding.Default))
                {
                    html = Stream_Reader.ReadToEnd();
                }
            }
        }
        Debug.LogWarning(html);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
