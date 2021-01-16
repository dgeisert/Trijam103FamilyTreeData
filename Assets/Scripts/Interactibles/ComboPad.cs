using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ComboPad : MonoBehaviour
{
    [SerializeField] string code = "0000";
    string attempt = "";
    [SerializeField] UnityEvent onCorrect;
    AudioSource audioSource;
    [SerializeField] AudioClip correct, wrong, click;
    [SerializeField] TextMeshPro text;
    [SerializeField] MeshRenderer[] checks;

    void Awake()
    {
        onCorrect.AddListener(PlayCorrect);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        for (int i = 0; i < checks.Length; i++)
        {
            checks[i].material.SetColor("_Color", Color.black);
        }
    }

    void PlayCorrect()
    {
        text.color = Color.green;
        audioSource.clip = correct;
        audioSource.Play();
    }

    public void AddToCode(string add)
    {
        if (checks != null && checks.Length > attempt.Length)
        {
            if (add[0] == code[attempt.Length])
            {
                checks[attempt.Length].material.SetColor("_Color", Color.green);
            }
            else if (code.Contains(add))
            {
                checks[attempt.Length].material.SetColor("_Color", Color.yellow);
            }
            else
            {
                checks[attempt.Length].material.SetColor("_Color", Color.red);
            }
            if (attempt.Length == 0)
            {
                for (int i = 1; i < checks.Length; i++)
                {
                    checks[i].material.SetColor("_Color", Color.black);
                }
            }
        }
        audioSource.clip = click;
        attempt += add;
        text.color = Color.white;
        text.text = attempt;
        if (attempt.Length >= code.Length)
        {
            if (attempt == code)
            {
                onCorrect.Invoke();
            }
            else
            {
                audioSource.clip = wrong;
                attempt = "";
                text.color = Color.red;
            }
        }
        audioSource.Play();
    }
}