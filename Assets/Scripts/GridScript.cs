using UnityEngine;
using UnityEngine.UI;

public class PixelGridDrawer : MonoBehaviour
{
    public int screenWidth = 1920;
    public int screenHeight = 1080;
    public Color blockColor; // Set this to a semi-transparent color in the Unity Editor
    public Color guessedColor = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent color for guessed cells
    private Texture2D screenTexture;

    void Start()
    {
        CreateGridTexture();
        ApplyTextureToUI();
    }

    void CreateGridTexture()
    {
        screenTexture = new Texture2D(screenWidth, screenHeight, TextureFormat.RGBA32, false);
        ClearTexture();

        // Draw your grid here (if necessary)
        // ...

        screenTexture.Apply();
    }

    void ClearTexture()
    {
        for (int x = 0; x < screenWidth; x++)
        {
            for (int y = 0; y < screenHeight; y++)
            {
                screenTexture.SetPixel(x, y, Color.clear); // Set all pixels to transparent
            }
        }
    }

    void ApplyTextureToUI()
    {
        Image screenImage = GetComponent<Image>();
        if (screenImage != null)
        {
            screenImage.sprite = Sprite.Create(screenTexture, new Rect(0.0f, 0.0f, screenWidth, screenHeight), new Vector2(0.5f, 0.5f));
        }
    }

    public void ShadeGuessedCell(int x, int y)
    {
        // Shade the guessed cell with the guessedColor
        for (int i = x; i < x + 2; i++)
        {
            for (int j = y; j < y + 2; j++)
            {
                if (i < screenWidth && j < screenHeight)
                {
                    Debug.Log($"Shading cell at: X={x}, Y={y}");
                    screenTexture.SetPixel(i, j, guessedColor);
                }
            }
        }
        screenTexture.Apply();
    }
}
