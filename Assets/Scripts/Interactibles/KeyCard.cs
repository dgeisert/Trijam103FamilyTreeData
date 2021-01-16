using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : Interactible
{
    [SerializeField] int key = 1;
    public override void Interact()
    {
        //Player.Instance.key[key] = true;
        gameObject.SetActive(false);
    }
}