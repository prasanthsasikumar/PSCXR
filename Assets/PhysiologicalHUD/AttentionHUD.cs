using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttentionHUD : MonoBehaviour, IHUDElement<float>
{
    public TextMeshProUGUI attentionText;

    public float fakeAttention;

    public float minAttention;
    public float maxAttention;

    public void UpdateHUD(float value)
    {
        float normal = Mathf.InverseLerp(minAttention, maxAttention, value);
        attentionText.text = $"Attention: {Mathf.Round(value)}";
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHUD(fakeAttention);
    }
}
