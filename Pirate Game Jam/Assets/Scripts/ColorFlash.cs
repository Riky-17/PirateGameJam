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

public struct LongColorFlash
{
    public readonly Color Color;
    public float duration;

    public LongColorFlash(Color color, float duration)
    {
        Color = color;
        this.duration = duration;
    }
}
