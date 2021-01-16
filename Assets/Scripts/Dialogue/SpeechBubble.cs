using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    bool typing = false;
    [SerializeField] private TextMeshProUGUI text, name;
    [SerializeField] private Image portrait;
    [SerializeField] private string data;
    Coroutine typingCoroutine;

    void Update()
    {
        if (Controls.Shake)
        {
            Next();
        }
    }

    public void Next()
    {
        if (typing)
        {
            typing = false;
            text.text = data;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }
        else
        {
            typingCoroutine = StartCoroutine(Type());
        }
    }

    IEnumerator Type()
    {
        text.text = "";
        typing = true;
        for (int i = 0; i < data.Length; i++)
        {
            text.text += data[i];
            yield return null;
            yield return null;
        }
        typing = false;
    }
}