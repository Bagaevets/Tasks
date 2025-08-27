using UnityEngine;
using UnityEngine.EventSystems;

public class ClickController : MonoBehaviour, IPointerClickHandler
{
    private int cellNumber;
    private GridGenerator gridGenerator;
    private bool isColored = false;
    private SpriteRenderer spriteRenderer;

    public void Initialize(int number, GridGenerator generator)
    {
        cellNumber = number;
        gridGenerator = generator;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ToggleColor();
        }
    }

    private void ToggleColor()
    {
        if (!isColored)
        {
            // Первый клик - устанавливаем цвет по четности
            Color targetColor = gridGenerator.GetColorForNumber(cellNumber);
            spriteRenderer.color = targetColor;
            isColored = true;
            Debug.Log($"Клетка {cellNumber} ({(cellNumber % 2 == 0 ? "четная" : "нечетная")}) окрашена");
        }
        // Последующие клики игнорируются - цвет фиксирован
    }

   
}
