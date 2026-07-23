using System;
using UnityEngine;

public class MoralityManager : MonoBehaviour
{
    // Point Trackers (default starting at 50)
    [Header("Morality Bars")]
    public int heavenBar = 50;
    public int hellBar = 50;

    // Warning Flags
    [Header("Status Warnings")]
    public bool warningHeaven = false;
    public bool warningHell = false;

    // Warning Event Callbacks
    public enum WarningType { None, HeavensLament, HellsGrumble }
    public event Action<WarningType> onWarningTriggered;
    public event Action<WarningType> onWarningFaded;

    // Ending Options
    public enum GameEnding
    {
        None,
        DeathHeaven,
        DeathHell,
        IdealEnd,
        PerfectEnd
    }

    [Header("Current Ending Result")]
    public GameEnding currentEnding = GameEnding.None;

    public enum Destination { Heaven, Hell }
    public enum Mask { Justice, Judgement }
    public enum SoulType { GoodPerson, BadPerson }

    // Processes player choice, updates HeavenBar / HellBar scores, and checks warnings.

    public void ProcessVerdict(Destination destination, Mask mask, SoulType soul)
    {
        int pointChange = CalculatePoints(destination, mask, soul);

        if (destination == Destination.Heaven)
        {
            heavenBar += pointChange;
            heavenBar = Mathf.Clamp(heavenBar, 0, 100);
        }
        else if (destination == Destination.Hell)
        {
            hellBar += pointChange;
            hellBar = Mathf.Clamp(hellBar, 0, 100);
        }

        EvaluateWarnings(pointChange > 0);
    }

    private int CalculatePoints(Destination destination, Mask mask, SoulType soul)
    {
        if (destination == Destination.Heaven)
        {
            if (soul == SoulType.GoodPerson && mask == Mask.Justice) return 20;
            if (soul == SoulType.GoodPerson && mask == Mask.Judgement) return 5;
            if (soul == SoulType.BadPerson && mask == Mask.Justice) return -5;
            if (soul == SoulType.BadPerson && mask == Mask.Judgement) return -20;
        }
        else if (destination == Destination.Hell)
        {
            if (soul == SoulType.BadPerson && mask == Mask.Judgement) return 20;
            if (soul == SoulType.BadPerson && mask == Mask.Justice) return 5;
            if (soul == SoulType.GoodPerson && mask == Mask.Judgement) return -5;
            if (soul == SoulType.GoodPerson && mask == Mask.Justice) return -20;
        }

        return 0;
    }

    private void EvaluateWarnings(bool isCorrectChoice)
    {
        // --- CHECK HEAVEN WARNING ---
        if (heavenBar < 20 && !warningHeaven)
        {
            warningHeaven = true;
            onWarningTriggered?.Invoke(WarningType.HeavensLament);
        }
        else if (warningHeaven && isCorrectChoice && heavenBar >= 20)
        {
            warningHeaven = false;
            onWarningFaded?.Invoke(WarningType.HeavensLament);
        }

        // --- CHECK HELL WARNING ---
        if (hellBar < 20 && !warningHell)
        {
            warningHell = true;
            onWarningTriggered?.Invoke(WarningType.HellsGrumble);
        }
        else if (warningHell && isCorrectChoice && hellBar >= 20)
        {
            warningHell = false;
            onWarningFaded?.Invoke(WarningType.HellsGrumble);
        }
    }

    // Evaluates ending state based on current HeavenBar, HellBar, and Warning flags.
    public GameEnding EvaluateEnding()
    {
        // Death Endings
        if (warningHeaven && heavenBar == 0)
        {
            currentEnding = GameEnding.DeathHeaven;
        }
        else if (warningHell && hellBar == 0)
        {
            currentEnding = GameEnding.DeathHell;
        }
        // Ideal Endings
        else if ((warningHeaven && heavenBar > 20) || (warningHell && hellBar > 20))
        {
            currentEnding = GameEnding.IdealEnd;
        }
        // Perfect Endings
        else if (heavenBar > 25 || hellBar > 25)
        {
            currentEnding = GameEnding.PerfectEnd;
        }

        Debug.Log($"[MoralityManager] Ending: {currentEnding}");
        return currentEnding;
    }
}