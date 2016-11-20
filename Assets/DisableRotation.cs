using UnityEngine;
using System.Collections;

public class DisableRotation : MonoBehaviour {
    ScreenOrientation orient = ScreenOrientation.Landscape;
    public UnityEngine.UI.Image img;

    void setAnchors(UnityEngine.UI.Button btn, float x, float y)
    {
        btn.GetComponent<RectTransform>().anchorMax = new Vector2(x, y);
        btn.GetComponent<RectTransform>().anchorMin = new Vector2(x, y);
    }

    void setPos(UnityEngine.UI.Button btn, float x, float y)
    {
        btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    void setAnchorsAndPos(UnityEngine.UI.Button btn, float ax, float ay, float x, float y)
    {
        setAnchors(btn, ax, ay);
        setPos(btn, x, y);
    }

    void Update()
    {
        if(orient != Screen.orientation) {
            Debug.Log("Orientation changed");
            orient = Screen.orientation;
            UnityEngine.UI.Button[] chld = gameObject.GetComponentsInChildren<UnityEngine.UI.Button>();
            if (orient == ScreenOrientation.Portrait || orient == ScreenOrientation.PortraitUpsideDown)
            {
                setAnchorsAndPos(chld[0], 0.5f, 0.0f, 0, 139);
                setAnchorsAndPos(chld[1], 1.0f, 0.0f, -136, 139);
                gameObject.GetComponent<UnityEngine.UI.CanvasScaler>().matchWidthOrHeight = 0.5f;
                if (img)
                {
                    if (img.sprite.rect.width > img.sprite.rect.height)
                    {
                        img.GetComponent<RectTransform>().sizeDelta = new Vector2(1920 / 16 * 9, 1080 / 16 * 9);
                    }
                    else
                    {
                        img.GetComponent<RectTransform>().sizeDelta = new Vector3(1920 / 9 * 16, 1920);
                    }
                }
            }
            else if (orient == ScreenOrientation.LandscapeLeft || orient == ScreenOrientation.LandscapeRight)
            {
                setAnchorsAndPos(chld[0], 1.0f, 0.5f, -136, 0);
                setAnchorsAndPos(chld[1], 1.0f, 1.0f, -136, -139);
                gameObject.GetComponent<UnityEngine.UI.CanvasScaler>().matchWidthOrHeight = 0;
                if (img)
                {
                    if (img.sprite.rect.width <= img.sprite.rect.height)
                    {
                        img.GetComponent<RectTransform>().sizeDelta = new Vector2(1080 / 16 * 9, 1080);
                    }
                    else
                    {
                        img.GetComponent<RectTransform>().sizeDelta = new Vector3(1920, 1080);
                    }
                }
            }
        }
    }
}
