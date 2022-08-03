using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NBackDisplay : MonoBehaviour, INBackDisplay
{
    public AudioClip wrongSound;
    public AudioClip correctSound;
    public AudioClip missedSound;

    public AudioSource audioSource;
    public TextMeshProUGUI feedbackText;    
    public TextMeshProUGUI stimuliText;

    public GameObject gameplayDisplay;
    public GameObject endDisplay;

    public void DisplayCorrect()
    {
        audioSource.PlayOneShot(correctSound);
    }

    public void DisplayStart()
    {
        gameplayDisplay.SetActive(true);
        endDisplay.SetActive(false);

    }
    public void DisplayEnd()
    {
        gameplayDisplay.SetActive(true);
        endDisplay.SetActive(true);
    }

    public void DisplayStimuli(int stimuli)
    {
        stimuliText.text = ""+stimuli;
    }

    public void DisplayWrong()
    {
        audioSource.PlayOneShot(wrongSound);
    }

    public void HideStimuli()
    {
        stimuliText.text = "";
    }

    public void MissedAnswer()
    {
        audioSource.PlayOneShot(missedSound);
    }

    public void DisplayScore(int correct, int wrong, int missed)
    {
        feedbackText.text = "Feedback: " + correct + " Wrong: " + wrong + " Missed: " + missed; 
    }

    public void DisplayReset()
    {
        stimuliText.text = "";
        feedbackText.text = "Press Trigger to respond.";
    }

    // Start is called before the first frame update
    void Start()
    {
        stimuliText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
