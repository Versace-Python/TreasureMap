using UnityEngine;
using UnityEngine.UI;

public class GridClickHandler : MonoBehaviour
{
    public int gridBlockSize = 2; // Size of each block in the grid

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 for left click
        {
            RegisterClick();
        }
    }

    void RegisterClick()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, Camera.main, out Vector2 localPoint))
        {
            Vector2 normalizedPoint = Rect.PointToNormalized(rectTransform.rect, localPoint);
            int gridX = Mathf.FloorToInt(normalizedPoint.x * rectTransform.rect.width / gridBlockSize);
            int gridY = Mathf.FloorToInt(normalizedPoint.y * rectTransform.rect.height / gridBlockSize);

            Debug.Log($"Grid X: {gridX}, Grid Y: {gridY}");
            OnGuess(gridX, gridY);
        }
    }

    // Assuming this method is in GridClickHandler or a similar script
    void OnGuess(int x, int y)
    {
        // Find the PixelGridDrawer component and call ShadeGuessedCell
        PixelGridDrawer drawer = FindObjectOfType<PixelGridDrawer>();
        if (drawer != null)
        {
            drawer.ShadeGuessedCell(x * 2, y * 2); // Multiply by 2 because each cell is 2x2
        }
    }
}
