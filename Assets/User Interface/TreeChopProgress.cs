using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeChopProgress : MonoBehaviour
{
    public ProgressBar TreeHealthBar;
    public Label BankUI;
    public FerrySelling ferrySelling;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        TreeHealthBar = root.Q<ProgressBar>("TreeHealthBar");
        ferrySelling = FindObjectOfType<FerrySelling>();
        BankUI = root.Q<Label>("Bank");
        TreeHealthBar.visible = false;
    }
    private void Update()
    {
        if (ferrySelling != null)
        {
            BankUI.text = ferrySelling.total.ToString();
        }
        
    }
}
