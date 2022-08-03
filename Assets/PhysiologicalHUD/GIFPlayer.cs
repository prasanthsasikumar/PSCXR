using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIFPlayer : MonoBehaviour
{
    public int materialIndex;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {

    }
    public Sprite[] frames;
    public int framesPerSecond = 10;

    void Update()
    {
        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        image.sprite = frames[index];
    }
}