using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour {
    public ControlGame controlGame;
    public Text textField;
    public Text infoUI;
    public InputField Input;
    public Button btnStart;
    int n;

    bool InputValidate()
    {
        if (!Int32.TryParse(Input.text, out n))
        {
            StartCoroutine(ShowInfo("введите число"));
            return false;
        }
        if (n < 3)
        {
            StartCoroutine(ShowInfo("дисков должно быть > 3"));
            return false;
        }
        if (n == 0)
        {
            StartCoroutine(ShowInfo("введите число(>0)"));
            return false;
        }
        if (n > 1000)
        {
            StartCoroutine(ShowInfo("нужно повысить дальность отрисовки(ClippingPlanes->Far)"));
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

    IEnumerator ShowInfo(string e)
    {
        infoUI.text = e;
        yield return new WaitForSeconds(1.0f);
        infoUI.text = "";
        yield return null;
    }

    public void DoneTask()
    {
        StartCoroutine(ShowInfo("Задача выполнена"));
    }
}
