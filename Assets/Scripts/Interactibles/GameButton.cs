using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameButton : Interactible
{
    public UnityEvent action;
    
    public override void Interact()
    {
        base.Interact();
        action.Invoke();
    }
}
