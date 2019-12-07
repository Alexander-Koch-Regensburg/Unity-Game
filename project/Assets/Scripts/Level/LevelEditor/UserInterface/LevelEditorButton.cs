using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEditorButton : MonoBehaviour
{
    private string text;

    public void SetupPrefab(string text, Transform panel)
    {
        float height = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();

        //rectTransform.localPosition = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, panel.childCount);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);


        SetText(text);
    }

    public void SetupConfigLevelButton(string text, Transform panel)
    {
        //float height = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();

        //rectTransform.localPosition = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
        //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        
        SetText(text);
    }

    private void SetText(string text)
    {
        this.text = text;
        TextMeshProUGUI textMeshPro = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = text;
    }

    public void RemovePressedColor()
    {
        SetButtonColor(Color.white);
    }

    public void SetPressedColor()
    {
        SetButtonColor(new Color32(5, 85, 167, 255));
        //SetButtonColor(Color.red);
    }

    private void SetButtonColor(Color color)
    {
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
            image.color = color;
    }
}
