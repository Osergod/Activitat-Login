using UnityEngine;
using UnityEngine.UI;

public class DarkRGBCanvasBackground : MonoBehaviour
{
    public Image backgroundImage;

    [Range(0.01f, 1f)]
    public float speed = 0.05f;

    [Range(0f, 1f)]
    public float saturation = 0.35f;

    [Range(0f, 1f)]
    public float brightness = 0.12f;

    private float hue;

    void Start()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    void Update()
    {
        hue += Time.deltaTime * speed;
        if (hue > 1f) hue = 0f;

        Color color = Color.HSVToRGB(hue, saturation, brightness);
        backgroundImage.color = color;
    }
}
