using UnityEngine;

public abstract class ColorFlashObject : Mover
{
    protected Color spriteColor;

    float intervalTimer;
    protected ShortColorFlash shortColorFlash;
    protected LongColorFlash LongColorFlash => longColorFlash;
    LongColorFlash longColorFlash;

    protected override void Awake()
    {
        base.Awake();
        longColorFlash = new();
    }

    protected void ColorFlash()
    {
        Color tempColor;
        if(shortColorFlash.duration > 0)
        {
            tempColor = shortColorFlash.Color;
            shortColorFlash.duration-= Time.deltaTime;
            if(shortColorFlash.duration <= 0)
            {
                tempColor = Color.white;
                IsSameColor(tempColor);
                shortColorFlash = default;
            }
            else
            {
                IsSameColor(tempColor);
                if(!longColorFlash.IsEmpty())
                    longColorFlash.ReduceDurations(Time.deltaTime);
                return;
            }
        }

        if(!longColorFlash.IsEmpty())
        {
            if(intervalTimer <= longColorFlash.Interval)
                intervalTimer+= Time.deltaTime;
            else
                intervalTimer = 0;

            tempColor = longColorFlash.GetColor(intervalTimer);
            IsSameColor(tempColor);
            longColorFlash.ReduceDurations(Time.deltaTime);

            return;
        }

        tempColor = Color.white;
        IsSameColor(tempColor);
    }

    void IsSameColor(Color newColor) 
    {
        if(spriteColor != newColor)
        {
            spriteColor = newColor;
            UpdateColor(newColor);
        }
    }

    protected abstract void UpdateColor(Color color);
}
