using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float duration = 5f;
    public Color startColor, endColor;

    public void Start()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<MeshCollider>().enabled = true;
        StartCoroutine(ChangeColor(this.GetComponent<MeshRenderer>().material));
    }

    IEnumerator ChangeColor(Material toChange)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            toChange.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;
        }
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<MeshCollider>().enabled = false;
    }

}
