using System;
using System.Collections;
using GamepadInput;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class Ritual
{
    private static int CurrentId = 0;

    public int RitualId;
    public GamePad.Button[] Sequence;
    public string Name;

    private static Random _rand = new Random();

    public Ritual()
    {
        RitualId = CurrentId++;
    }

    public GamePad.Button this[int i]
    {
        get
        {
            if (Sequence != null && i >= 0 && i < 4)
            {
                return Sequence[i];
            }
            throw new Exception("Not initialized OR Index out of bounds (Length always 4).");
        }
        set
        {
            if (Sequence == null)
            {
                Sequence = new GamePad.Button[4];
            }
            if (i >= 0 && i < 4)
            {
                Sequence[i] = value;
                return;
            }
            throw new Exception("Index out of bounds (Length always 4).");
        }
    }

    public void RandomizeSequence()
    {
        Sequence = new GamePad.Button[4];
        for (int i = 0; i < Sequence.Length; i++)
        {
            GamePad.Button button = GamePad.Button.A;
            switch (_rand.Next(4))
            {
                case 0:
                    button = GamePad.Button.A;
                    break;
                case 1:
                    button = GamePad.Button.B;
                    break;
                case 2:
                    button = GamePad.Button.X;
                    break;
                case 3:
                    button = GamePad.Button.Y;
                    break;
            }
            Sequence[i] = button;
        }
    }

    public override string ToString()
    {
        return Sequence[0] + ", " + Sequence[1] + ", " + Sequence[2] + ", " + Sequence[3];
    }
}
