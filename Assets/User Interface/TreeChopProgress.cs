using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeChopProgress : MonoBehaviour
{
    public ProgressBar TreeHealthBar;
    public Label BankUI;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        TreeHealthBar = root.Q<ProgressBar>("TreeHealthBar");
        BankUI = root.Q<Label>("Bank");
        TreeHealthBar.visible = false;
    }
}
