using UnityEngine;
using System.Collections;

public class SimpleFog : MonoBehaviour
{
    private Texture2D fog;
    private int sizeModifier = 128;

    private GameObject ground;
    private int height, width;

    // Use this for initialization
    void Awake()
    {
        Vector3 groundScale = GameObject.Find("Ground").transform.localScale;
        height = (int)(sizeModifier * groundScale.z * 2);
        width = (int)(sizeModifier * groundScale.x * 2);
        fog = new Texture2D(height, width);
        for (int pix_x = 0; pix_x < width; pix_x++)
        {
            for (int pix_y = 0; pix_y < height; pix_y++)
            {
                fog.SetPixel(pix_x, pix_y, Color.black);
            }
        }
        fog.Apply();
        // attach to material
    }

    public void DoFogCircle(Vector3 pos, float unityRange, float alpha)
    {
        DoFogCircle((int)(pos.x * sizeModifier * 2 + width), (int)(pos.z * sizeModifier * 2 + width), (int)(unityRange * sizeModifier), alpha);
    }

    public void DoFogCircle(int x, int y, float pixelRange, float alpha)
    {
        int upperBoundX = Mathf.Min(x + Mathf.CeilToInt(pixelRange), width);
        int upperBoundY = Mathf.Min(y + Mathf.CeilToInt(pixelRange), height);
        int lowerBoundX = Mathf.Max(x - Mathf.CeilToInt(pixelRange), 0);
        int lowerBoundY = Mathf.Max(y - Mathf.CeilToInt(pixelRange), 0);
        for (int x_fog = lowerBoundX; x_fog < upperBoundX; x_fog++)
        {
            for (int y_fog = lowerBoundY; y_fog < upperBoundY; y_fog++)
            {
                if ((x_fog - x) * (x_fog - x) + (y_fog - y) * (y_fog - y) < pixelRange * pixelRange)
                {
                    fog.SetPixel(x_fog, y_fog, new Color(0, 0, 0, alpha));
                }
            }
        }
        fog.Apply();
    }

    public bool IsFoggy(int x, int y)
    {
        return fog.GetPixel(x, y).a >= 0.5;
    }
}
