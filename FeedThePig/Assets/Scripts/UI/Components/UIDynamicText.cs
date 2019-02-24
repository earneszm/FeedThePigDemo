using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDynamicText : MonoBehaviour
{
    [SerializeField]
    private string label;
    [SerializeField]
    private string contentSeperator;
    [SerializeField]
    private string suffix;
    [SerializeField]
    private bool showDigitOnValidate = true;
    [SerializeField]
    private TextMeshProUGUI text;

    private void OnValidate()
    {
        if (showDigitOnValidate)
            UpdateContent(string.Format("{0}{1}{2}", 0, contentSeperator, string.IsNullOrEmpty(contentSeperator) ? "" : "0"));
        else
            UpdateContent("");
    }

    public void UpdateContent(string content)
    {
        text.text = string.Format("{0}{1}{2}", label, content, suffix);
    }

    public void UpdateContent(string content1, string content2)
    {
        text.text = string.Format("{0}{1}{2}{3}{4}", label, content1, contentSeperator, content2, suffix);
    }

    public void UpdateContent(int content)
    {
        UpdateContent(content.ToString());
    }

    public void UpdateContent(int content1, int content2)
    {
        UpdateContent(content1.ToString(), content2.ToString());
    }

    public void UpdateContent(float content)
    {
        UpdateContent(content.ToString());
    }

    public void UpdateContent(float content1, float content2)
    {
        UpdateContent(content1.ToString(), content2.ToString());
    }
}
