using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDynamicText : MonoBehaviour
{
    [SerializeField]
    private string label;
    [SerializeField]
    private TextMeshProUGUI text;

    private void OnValidate()
    {
        UpdateContent(0);
    }

    public void UpdateContent(string content)
    {
        text.text = string.Format("{0}{1}", label, content);
    }

    public void UpdateContent(int content)
    {
        UpdateContent(content.ToString());
    }

    public void UpdateContent(float content)
    {
        UpdateContent(content.ToString());
    }
}
