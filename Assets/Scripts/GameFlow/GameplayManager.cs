using UnityEngine;
using System.Collections.Generic;

public class GameplayManager : BJ.SingletonGameObject<GameplayManager>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        GameObject gameplayManager = new GameObject("GameplayManager");
        gameplayManager.AddComponent<GameplayManager>();
    }

    private Dictionary<string, bool> flags;
    [SerializeField]
    private float stimulation;
    [SerializeField]
    private float energy;

    protected override void Awake()
    {
        base.Awake();

        flags = new Dictionary<string, bool>();
    }

    public void ModifyRange(RangeType rangeType, MathOperation operation, float value)
    {
        float range = rangeType == RangeType.STIMULATION ? stimulation : energy;

        if (operation == MathOperation.SET)
        {
            range = value;
        }
        else if (operation == MathOperation.ADD)
        {
            range += value;
        }
        else if (operation == MathOperation.SUB)
        {
            range -= value;
        }
        else if (operation == MathOperation.MUL)
        {
            range *= value;
        }

        // after operation write
        if (rangeType == RangeType.STIMULATION)
        {
            stimulation = range;
        }
        else
        {
            energy = range;
        }
    }

    public void SetFlag(string flag, bool value)
    {
        flags[flag] = value;
    }

    public bool EvaluateCondition(string query)
    {
        if (query.Length == 0)
        {
            return true;
        }

        string[] conds = query.Split("&");

        bool result = true;

        for (int i = 0; i < conds.Length; i++)
        {
            string cond = conds[i];

            if (cond.StartsWith("stim"))
            {
                if (cond.IndexOf('>') != -1)
                {
                    string[] stimCond = cond.Split(">");
                    result &= stimulation > int.Parse(stimCond[1]);
                    Debug.Log("Condition Ran: " + result);
                }
                else if (cond.IndexOf('<') != -1)
                {
                    string[] stimCond = cond.Split("<");
                    result &= stimulation < int.Parse(stimCond[1]);
                }
            }
            else if (cond.StartsWith("ener"))
            {
                if (cond.IndexOf('>') != -1)
                {
                    string[] stimCond = cond.Split(">");
                    result &= energy > int.Parse(stimCond[1]);
                }
                else if (cond.IndexOf('<') != -1)
                {
                    string[] stimCond = cond.Split("<");
                    result &= energy < int.Parse(stimCond[1]);
                }
            }
            else
            {
                bool not = false;
                if (cond.StartsWith ('!'))
                {
                    cond = cond.Substring(1);
                }

                result &= flags.GetValueOrDefault(cond, false) ^ not;
            }
        }
        Debug.Log("got here");
        Debug.Log(result);
        return result;
    }
}
