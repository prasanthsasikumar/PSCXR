using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBackInput : MonoBehaviour
{
    public KeyCode answerKey;
    public KeyCode startKey;
    public NBackManager nBackManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(answerKey))
        {

            nBackManager.CallMatch();
        }  
        
        if(Input.GetKeyDown(startKey))
        {

            nBackManager.StartTest();
        }
    }
}
