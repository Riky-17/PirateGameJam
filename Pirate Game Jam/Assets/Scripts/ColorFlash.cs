using System.Collections.Generic;
using UnityEngine;

public struct ShortColorFlash
{
    public readonly Color Color;
    public float duration;

    public ShortColorFlash(Color color)
    {
        Color = color;
        duration = .1f;
    }
}

public class LongColorFlash
{
    readonly List<Color> colors;
    readonly Dictionary<Color, float> durations;
    public float Interval => interval;
    float interval;

    public LongColorFlash()
    {
        colors = new();
        durations = new();
        interval = .2f;
    }

    public LongColorFlash(Color color, float duration)
    {
        colors = new() { color };
        durations = new() { { color, duration } };
        interval = .4f;
    }

    public LongColorFlash(Dictionary<Color, float> colors)
    {
        this.colors = new(colors.Keys);
        durations = colors;
        CalculateInterval();
    }

    public void ReduceDurations(float time)
    {
        List<Color> ColorsDup = new(colors);
        foreach (Color color in ColorsDup)
        {
            float duration = durations[color];
            duration-= time;

            if(duration <= 0)
            {
                colors.Remove(color);
                durations.Remove(color);
                CalculateInterval();
                continue;
            }

            durations[color] = duration;
        }
    }

    public Color GetColor(float timer)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            float indexInterval = .2f + (.2f * i);

            if(timer < indexInterval)
                return colors[i];
        }
        return Color.white;
    }

    public bool IsEmpty() => colors == null || colors.Count == 0;

    void CalculateInterval() => interval = (colors.Count * .2f) + .2f;

    public void AddColor(Color color, float duration)
    {
        if(colors.Contains(color))
        {
            float finalDuration = durations[color] + duration;
            durations[color] = finalDuration;
        }

        colors.Add(color);
        durations[color] = duration;
        CalculateInterval();
    }
}
