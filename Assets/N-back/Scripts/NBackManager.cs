using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NBackSettings
{
    public int n = 2;
    public int totalStimuli = 10;
}

public class NBackManager : MonoBehaviour
{
    [Range(1, 9)]

    bool missed = false;
    bool playing = false;
    public int range;
    public NBackSettings settings;
    bool answered = false;
    public int correctCount = 0;
    public int wrongCount = 0;
    public int missedCount = 0;

    public float maxPresentTime = 0.76f;
    public float newStimuliTime = 2.76f;

    float timer = 0f;
    public int currentIndex = -1;
    public List<int> sequence;

    public GameObject nBackDisplayGO;
    INBackDisplay nbackDisplay;

    private GameLogic gameLogic;

    private void Awake()
    {
        gameLogic = GameObject.FindObjectOfType<GameLogic>();
    }

    public void StartTest()
    {
        playing = true;
        nbackDisplay = nBackDisplayGO.GetComponent<INBackDisplay>();
        sequence = new List<int>();
        currentIndex = -1;
        timer = 0f;

        nbackDisplay.DisplayStart();

        NewStimuli();
        if (gameLogic) gameLogic.StartSampling();
    }

    public void Reset()
    {
        nbackDisplay.DisplayReset();
    }

    void NewStimuli()
    {
        missed = false;
        answered = false;
        int number = Random.Range(0, range);

        sequence.Add(number);
        currentIndex++;
        timer = 0f;
        nbackDisplay.DisplayStimuli(number);

        if (currentIndex + 1 == settings.totalStimuli)
        {
            playing = false;
            nbackDisplay.DisplayEnd();
            nbackDisplay.DisplayScore(correctCount, wrongCount, missedCount);
            if (gameLogic && settings.n==1) gameLogic.CalculateMedium();
            else if (gameLogic && settings.n == 2) gameLogic.CalculateHigh();
        }
    }

    void HideStimuli()
    {
        nbackDisplay.HideStimuli();
    }

    void CorrectAnswer()
    {
        correctCount += 1;
        nbackDisplay.DisplayCorrect();
    }

    void WrongAnswer()
    {
        wrongCount += 1;
        nbackDisplay.DisplayWrong();
    }

    void MissedAnswer()
    {
        missed = true;
        missedCount += 1;
        nbackDisplay.MissedAnswer();
    }

    bool HasMatch()
    {
        int currentNumber = sequence[currentIndex];
        bool correctMatch = false;
        if (currentIndex - this.settings.n >= 0)
        {
            if (currentNumber == sequence[currentIndex - this.settings.n])
            {
                correctMatch = true;
            }
        }
        return correctMatch;
    }
    // Call when player thinks there is a match
    public void CallMatch()
    {
        if (answered) return;

        answered = true;
       
        if(HasMatch())
        {

            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!playing)
        {
            return;
        }

        timer += Time.deltaTime;
        if(timer >= newStimuliTime)
        {
            NewStimuli();
        }
        else if(timer >= maxPresentTime)
        {
            if (!answered && HasMatch() && !missed)
            {
                MissedAnswer();
            }
            HideStimuli();
        }
    }
}
