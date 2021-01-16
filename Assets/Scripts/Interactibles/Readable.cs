using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Readable : Interactible
{
    [TextArea]
    public string text = "Nothing interesting";


    public override void Interact()
    {
        //SpeechBubble.Instance.SetText(text);
    }
}
