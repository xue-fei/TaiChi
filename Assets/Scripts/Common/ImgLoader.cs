using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// 图片加载器
/// </summary>
public class ImgLoader : MonoBehaviour
{
    /// <summary>
    /// 图片缓存路径 Application.persistentDataPath + "/Images";
    /// </summary>
    private string imagePath;

    public static ImgLoader Instance { get; private set; }

    public void Init()
    {
        imagePath = Application.persistentDataPath + "/Images";
        if (!Directory.Exists(imagePath))
        {
            Directory.CreateDirectory(imagePath);
        }
        Instance = this;
    }

    public void DownLoad(Image image, string url, string savePath, string fileName)
    {
        if (url != "")
        {
            StartCoroutine(LoadTexture(image, url, savePath, fileName));
        }
        else
        {
            Debug.LogWarning("url:" + url);
        }
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private IEnumerator LoadTexture(Image image, string url, string savePath, string fileName)
    {
        if (url == "")
        {
            yield break;
        } 
        bool ready = false;
        string filePath = savePath + "/" + fileName + ".jpg";
        if (File.Exists(filePath))
        {
            url = "file:///" + filePath;
            ready = true;
            Debug.LogWarning("本地已缓存");
        }

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("网络错误");
                uwr.Dispose();
                yield break;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                if (!ready)
                {
                    File.WriteAllBytesAsync(filePath, uwr.downloadHandler.data);
                    Debug.LogWarning("缓存到本地");
                }
                if (image != null)
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }

    public void DownLoad(Image image, string url)
    {
        if (url != "")
        {
            StartCoroutine(LoadTexture(image, url));
        }
        else
        {
            Debug.LogWarning("url:" + url);
        }
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private IEnumerator LoadTexture(Image image, string url)
    {
        if (url == "")
        {
            yield break;
        }
        string md5 = GetMD5WithString(url);
        bool ready = false;
        string filePath = imagePath + "/" + md5 + ".jpg";
        if (File.Exists(filePath))
        {
            url = "file:///" + filePath;
            ready = true;
            Debug.LogWarning("本地已缓存");
        }

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("网络错误");
                uwr.Dispose();
                yield break;
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                if (!ready)
                {
                    File.WriteAllBytesAsync(filePath, uwr.downloadHandler.data);
                    Debug.LogWarning("缓存到本地"); 
                }
                if (image != null)
                {
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }

    /// <summary>
    /// 计算字符串的Md5值 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private string GetMD5WithString(string input)
    {
        MD5 md5Hash = MD5.Create();
        // 将输入字符串转换为字节数组并计算哈希数据  
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        // 创建一个 Stringbuilder 来收集字节并创建字符串  
        StringBuilder str = new StringBuilder();
        // 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
        for (int i = 0; i < data.Length; i++)
        {
            str.Append(data[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
        }
        // 返回十六进制字符串  
        return str.ToString();
    }
}