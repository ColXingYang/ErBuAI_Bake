using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldView : MonoBehaviour {

    //InputField

    public InputField.SubmitEvent OnEndEdit;

    public InputField.SubmitEvent OnLeftClick;

    public InputField.SubmitEvent OnRightClick;


    public void OnInputFieldEndEdit(string value)
    {
        OnEndEdit.Invoke(value);
        //Debug.Log("OnInputFieldEndEdit");
    }

    public void OnLeftButtonClick()
    {
        OnLeftClick.Invoke("0.01");
        //Debug.Log("OnLeftClick");
    }

    public void OnRightButtonClick()
    {
        OnRightClick.Invoke("-0.01");
        //Debug.Log("OnRightClick");
    }

}
