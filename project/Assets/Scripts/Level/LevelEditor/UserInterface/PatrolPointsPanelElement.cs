using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PatrolPointsPanelElement : MonoBehaviour
{
    private string preText = "P";

    public void Setup(int count, Vector2 position)
    {
        SetHeaderText(count);
        SetXCoordinate(position);
        SetYCoordinate(position);

        float height = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();

        rectTransform.pivot = new Vector2(0, count);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);


    }

    private void SetHeaderText(int count)
    {
        Transform text = transform.Find("Text");
        SetText(text, preText + count.ToString());
    }

    private void SetXCoordinate(Vector2 position)
    {
        Transform xCoordinate = transform.Find("X_Coordinate");
        Transform value = xCoordinate.Find("Value");
        SetText(value, position.x.ToString("F2"));

    }

    private void SetYCoordinate(Vector2 position)
    {
        Transform yCoordinate = transform.Find("Y_Coordinate");
        Transform value = yCoordinate.Find("Value");
        SetText(value, position.y.ToString("F2"));
    }

    private void SetText(Transform transform, string value)
    {
        TextMeshProUGUI textMeshPro = transform.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = value;
    }
}
