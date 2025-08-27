using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float spacing = 0.1f;
    [SerializeField] private float padding = 0.5f;

    [Header("Cell Prefab")]
    [SerializeField] public GameObject cellPrefab;

    public Color evenColor = Color.red;    // Четные - красные
    public Color oddColor = Color.green;   // Нечетные - зеленые

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        float availableWidth = screenWidth - 2 * padding;
        float availableHeight = screenHeight - 2 * padding;

        float totalSpacingX = spacing * (gridWidth - 1);
        float totalSpacingY = spacing * (gridHeight - 1);

        float maxCellSizeX = (availableWidth - totalSpacingX) / gridWidth;
        float maxCellSizeY = (availableHeight - totalSpacingY) / gridHeight;
        float cellSize = Mathf.Min(maxCellSizeX, maxCellSizeY);

        float gridTotalWidth = (cellSize * gridWidth) + (spacing * (gridWidth - 1));
        float gridTotalHeight = (cellSize * gridHeight) + (spacing * (gridHeight - 1));

        float startX = -gridTotalWidth / 2f;
        float startY = -gridTotalHeight / 2f;

        int cellNumber = 1;

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Vector2 cellPosition = new Vector2(
                    startX + x * (cellSize + spacing) + cellSize / 2f,
                    startY + y * (cellSize + spacing) + cellSize / 2f
                );

                GameObject cell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
                cell.transform.localScale = new Vector3(cellSize, cellSize, 1f);
                cell.name = $"Cell_{cellNumber}";

                // Добавляем коллайдер если нет
                if (cell.GetComponent<Collider2D>() == null)
                {
                    cell.AddComponent<BoxCollider2D>();
                }

                // Получаем или добавляем ClickController
                ClickController clickController = cell.GetComponent<ClickController>();
                if (clickController == null)
                {
                    clickController = cell.AddComponent<ClickController>();
                }

                // Инициализируем контроллер
                clickController.Initialize(cellNumber, this);

                // Устанавливаем начальный белый цвет
                SpriteRenderer renderer = cell.GetComponent<SpriteRenderer>();
                renderer.color = Color.white;

                cellNumber++;
            }
        }
    }

    public Color GetColorForNumber(int number)
    {
        return number % 2 == 0 ? evenColor : oddColor;
    }
}

