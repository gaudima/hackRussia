using UnityEngine;
using System.Collections;
using System.IO;
using Vuforia;

public class ControlsScript : MonoBehaviour {

    public GameObject cameraCanvas;
    public GameObject shareCanvas;
    public UnityEngine.UI.Image img;
    private Sprite screenSprite;

    private string lastFile;

    string screenName()
    {
        string basePath = "/sdcard/Ilyich";

        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }
        return string.Format("{0}/{1}.png", basePath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void takeScreenShot()
    {
        int resWidth = Camera.main.pixelWidth;
        int resHeight = Camera.main.pixelHeight;
        Debug.Log("screenshot");
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = screenName();
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        lastFile = filename;
        screenSprite = Sprite.Create(screenShot, new Rect(0, 0, resWidth, resHeight), new Vector2(0.5f, 0.5f), 1.0f);
        img.sprite = screenSprite;
        cameraCanvas.SetActive(false);
        shareCanvas.SetActive(true);
    }

    public void switchCamera()
    {
        CameraDevice cd = CameraDevice.Instance;
        cd.Stop();
        cd.Deinit();
        if (cd.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_BACK)
        {
            cd.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
        }
        else
        {
            cd.Init(CameraDevice.CameraDirection.CAMERA_BACK);
        }
        cd.Start();
    }

    public void openShareDialog()
    {
        AndroidJavaClass intentC = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentO = new AndroidJavaObject("android.content.Intent");
        intentO.Call<AndroidJavaObject>("setAction", intentC.GetStatic<string>("ACTION_SEND"));
        intentO.Call<AndroidJavaObject>("setType", "image/png");
        AndroidJavaObject file = new AndroidJavaObject("java.io.File", lastFile);
        AndroidJavaClass uriC = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriO = uriC.CallStatic<AndroidJavaObject>("fromFile", file);
        intentO.Call<AndroidJavaObject>("putExtra", intentC.GetStatic<string>("EXTRA_STREAM"), uriO);
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject jChooser = intentC.CallStatic<AndroidJavaObject>("createChooser", intentO, "Share");
        currentActivity.Call("startActivity", jChooser);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && cameraCanvas.activeInHierarchy == false)
        {
            Debug.Log("Back pressed");
            cameraCanvas.SetActive(true);
            shareCanvas.SetActive(false);
        }
    }
}
