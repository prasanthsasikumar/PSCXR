using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INBackDisplay
{
    void DisplayStimuli(int stimuli);

    void HideStimuli();

    void DisplayCorrect();

    void DisplayWrong();

    void MissedAnswer();

    void DisplayEnd();

    void DisplayStart();

    void DisplayScore(int correct, int wrong, int missed);

    void DisplayReset();
}
