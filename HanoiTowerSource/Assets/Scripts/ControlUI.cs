using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour {
    public ControlGame controlGame;
    public Text textField;
    public InputField Input;
    public Button btnStart;
    int n;

    public void StartGame()
    {
        controlGame.Kdisk = n;
    }

    public void ReadField()
    {
        try
        {
            n = Int32.Parse(Input.text);
            if (n == 0) throw new Exception();
            btnStart.interactable = true;
        }
        catch 
        {
            btnStart.interactable = false;
            Input.text = "";
        }
    }
}
