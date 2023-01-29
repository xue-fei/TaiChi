using UnityEngine;

public class Driver : MonoBehaviour
{
    public static Driver Instance;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Log.Init(true, false, Application.persistentDataPath);
        Loom.Initialize();
        ImgLoader imgLoader = gameObject.AddComponent<ImgLoader>();
        imgLoader.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}