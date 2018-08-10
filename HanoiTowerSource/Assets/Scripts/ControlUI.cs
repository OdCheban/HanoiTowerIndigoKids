using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour {
    public ControlGame controlGame;
    public Text textField;
    public Text errorUI;
    public InputField Input;
    public Button btnStart;
    int n;

    bool InputValidate()
    {
        if (!Int32.TryParse(Input.text, out n))
        {
            StartCoroutine(ShowError("введите число"));
            return false;
        }
        if (n < 3)
        {
            StartCoroutine(ShowError("дисков должно быть > 3"));
            return false;
        }
        if (n == 0)
        {
            StartCoroutine(ShowError("введите число(>0)"));
            return false;
        }
        if (n > 1000)
        {
            StartCoroutine(ShowError("нужно повысить дальность отрисовки(ClippingPlanes->Far)"));
            return false;
        }

        return true;
    }
    public void ReadyGame()
    {
        if (InputValidate())
        {
            controlGame.StopGame();
            controlGame.StartGame(n);
        }
    }

    IEnumerator ShowError(string e)
    {
        errorUI.text = e;
        yield return new WaitForSeconds(1.0f);
        errorUI.text = "";
        yield return null;
    }
}
