using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class InterfaceTextFieldHandler : MonoBehaviour
{
    TMP_InputField levelSizeWidthTextField;
    TMP_InputField levelSizeHeightTextField;
    public TMP_InputField ammunitiontTextField;

    LevelCreator levelCreator;

    void Start()
    {
        levelCreator = LevelCreator.GetInstance();
    }

    public void OnLevelSizeChanged(TMP_InputField textField)
    {
        if (textField.text == "")
        {
            textField.text = "30";
            return;
        }

        int size = int.Parse(textField.text);
        if (size <= 0)
            textField.text = "30";
    }

    public void OnAmmunitionChanged()
    {
        int amount = int.Parse(ammunitiontTextField.text);
        if (amount <= 0)
        {
            amount = 10;
            ammunitiontTextField.text = "10";
        }
        levelCreator.OnAmmunitionChanged(amount);
    }
}
