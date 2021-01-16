using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Shake(this Transform t, float duration = 0.3f, float magnitude = 1f)
    {
        Util.Instance?.StartCoroutine(Util.Instance?.Shake(t, duration, magnitude));
    }
    public static void SetEnabled(this CanvasGroup canvasGroup, bool enabled)
    {
        canvasGroup.blocksRaycasts = enabled;
        canvasGroup.alpha = enabled ? 1 : 0;
        canvasGroup.interactable = enabled;
    }
    public static T Random1<T>(this List<T> list)
    {
        return list[Mathf.FloorToInt(Random.value * list.Count)];
    }
    public static T Random1<T>(this T[] array)
    {
        return array[Mathf.FloorToInt(Random.value * array.Length)];
    }
}

public class Util : MonoBehaviour
{
    public static Util Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Shake(Transform t, float duration, float magnitude)
    {
        float time = 0;
        Vector3 shake = Vector3.zero;
        while (time < duration)
        {
            time += Time.deltaTime;
            shake = magnitude * 0.1f * new Vector3((Random.value - 0.5f), (Random.value - 0.5f), (Random.value - 0.5f));
            t.position += shake;
            yield return null;
            if (t == null)
            {
                yield break;
            }
            t.position -= shake;
        }
    }
}