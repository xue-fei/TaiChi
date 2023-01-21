using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelStory : MonoBehaviour
{
    private string host = "https://txt80.com/";
    private StoryType storyType = StoryType.dushi;
    public TMP_Dropdown dropdown;
    public StoryScrollView scrollView;
    public StoryItem storyItemPrefab;
    public List<GameObject> storyItems;
    private List<StoryInfo> storyInfos;
    public Button buttonLast;
    public Button buttonNext;
    /// <summary>
    /// 当前页
    /// </summary>
    private int nowPage = 1;

    // Start is called before the first frame update
    void Start()
    {
        storyInfos = new List<StoryInfo>();
        storyItems = new List<GameObject>();
        scrollView = transform.Find("Scroll View").GetComponent<StoryScrollView>();
        dropdown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnSelect);
        storyType = StoryType.dushi;
        buttonLast = transform.Find("ButtonLast").GetComponent<Button>();
        buttonNext = transform.Find("ButtonNext").GetComponent<Button>();
        buttonLast.onClick.AddListener(PageLast);
        buttonNext.onClick.AddListener(PageNext);
        Loom.RunAsync(() =>
        {
            RequestStory();
        });
    }

    private void OnSelect(int index)
    {
        switch (index)
        {
            case 0:
                storyType = StoryType.dushi;
                break;
            case 1:
                storyType = StoryType.yanqing;
                break;
            case 2:
                storyType = StoryType.xuanhuan;
                break;
            case 3:
                storyType = StoryType.wuxia;
                break;
            case 4:
                storyType = StoryType.wangyou;
                break;
            case 5:
                storyType = StoryType.junshi;
                break;
            case 6:
                storyType = StoryType.kehuan;
                break;
            case 7:
                storyType = StoryType.danmei;
                break;
            case 8:
                storyType = StoryType.wenxue;
                break;
            case 9:
                storyType = StoryType.qita;
                break;
            default:
                storyType = StoryType.dushi;
                break;
        }
        Loom.RunAsync(() =>
        {
            RequestStory();
        });
    }

    private void PageLast()
    {
        nowPage--;
        if (nowPage < 1)
        {
            nowPage = 1;
        }
        Loom.RunAsync(() =>
        {
            RequestStory();
        });
    }

    private void PageNext()
    {
        nowPage++;
        Loom.RunAsync(() =>
        {
            RequestStory();
        }); 
    }

    private void RequestStory()
    {
        storyInfos.Clear();
        Loom.QueueOnMainThread(() =>
        {
            foreach (GameObject go in storyItems)
            {
                Destroy(go);
            }
        });
        Loom.QueueOnMainThread(() =>
        {
            scrollView.UpdateData(storyInfos);
        });
        string index;
        if (nowPage == 1)
        {
            index = "/index.html";
        }
        else
        {
            index = "/index_" + nowPage + ".html";
        }
        string storyUrl = host + storyType.ToString() + index;
        string html1 = RequestHtml(storyUrl);
        Debug.LogWarning(storyUrl);
        if (string.IsNullOrEmpty(html1))
        {
            RequestStory();
        }
        HtmlDocument doc1 = new HtmlDocument();
        doc1.LoadHtml(html1);
        string Name;
        string ImgUrl;
        var hrefNodes = doc1.DocumentNode.SelectNodes("//div[@class='pic']//a");
        foreach (HtmlNode node in hrefNodes)
        {
            HtmlNode cnode = node.SelectSingleNode("./img[@src]");
            ImgUrl = cnode.Attributes["src"].Value;
            Debug.LogWarning(ImgUrl);
            Name = cnode.Attributes["alt"].Value.Replace("图片", "");
            Debug.LogWarning(Name);

            string herf1 = "https://txt80.com" + node.Attributes["href"].Value;
            Debug.LogWarning(herf1);
            string html2 = RequestHtml(herf1);
            HtmlDocument doc2 = new HtmlDocument();
            doc2.LoadHtml(html2);
            HtmlNode dnode = doc2.DocumentNode.SelectSingleNode("//div[@class='downlinks']//a[@href]");

            string herf2 = "https://txt80.com" + dnode.Attributes["href"].Value;
            Debug.LogWarning(herf2);
            string html3 = RequestHtml(herf2);
            HtmlDocument doc3 = new HtmlDocument();
            doc3.LoadHtml(html3);
            HtmlNode enode = doc3.DocumentNode.SelectSingleNode("//div[@class='downlist']//a[@href]");
            string DownUrl = enode.Attributes["href"].Value;
            Debug.LogWarning(DownUrl);
            StoryInfo storyInfo = new StoryInfo();
            storyInfo.Name = Name;
            storyInfo.ImgUrl = ImgUrl;
            storyInfo.DownUrl = DownUrl;
            storyInfos.Add(storyInfo);

            Loom.QueueOnMainThread(() =>
            {
                scrollView.UpdateData(storyInfos);
            });
        }
    }

    private string RequestHtml(string url)
    {
        string html = null;
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Timeout = 30000;
        webRequest.Method = "GET";
        webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";
        webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        // 如果使用了GZip则先解压
        if (webResponse.ContentEncoding.ToLower() == "gzip")
        {
            using (Stream stream = webResponse.GetResponseStream())
            {
                using (var Zip_Stream = new GZipStream(stream, CompressionMode.Decompress))
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
            using (Stream stream = webResponse.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(stream, Encoding.Default))
                {
                    html = streamReader.ReadToEnd();
                }
            }
        }
        return html;
    }
}

public class StoryInfo
{
    public string Name;
    public string ImgUrl;
    public string DownUrl;
}