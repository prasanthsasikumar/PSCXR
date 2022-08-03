using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CognitiveLoadHUD : MonoBehaviour, IHUDElement<float>
{
    public float fakeLoad = 0.5f;
    public Image cognitiveLoadFill;
    public TextMeshProUGUI loadText;
    public void UpdateHUD(float value)
    {
        cognitiveLoadFill.fillAmount = value;

        string loadString = "";
        Color loadColor;

        if(value > 0.7f)
        {
            loadColor = Color.red;
            loadString = "High Cognitive Load";
        }
        else if(value > 0.4f)
        {
            loadColor = Color.red + Color.green;
            loadString = "Normal Cognitive Load";

        }
        else
        {
            loadColor = Color.blue;
            loadString = "Low Cognitive Load";

        }
        loadText.text = loadString;
        cognitiveLoadFill.color = Color.Lerp(cognitiveLoadFill.color, loadColor, Time.deltaTime * 10f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHUD(fakeLoad);
    }
}
