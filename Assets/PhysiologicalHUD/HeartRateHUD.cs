using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartRateHUD : MonoBehaviour, IHUDElement<float>
{
    public TextMeshProUGUI bpmText;

    public float fakeBPM;

    public float minGifSpeed;
    public float maxGifSpeed;

    public float minBPM;
    public float maxBPM;

    public GIFPlayer gifPlayer;
    public void UpdateHUD(float value)
    {
        float normal = Mathf.InverseLerp(minBPM, maxBPM, value);
        float bValue = Mathf.Lerp(minGifSpeed, maxGifSpeed, normal);

        bpmText.text = $"BPM: {Mathf.Round(value)}";

        gifPlayer.framesPerSecond = (int)bValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHUD(fakeBPM);
    }
}
