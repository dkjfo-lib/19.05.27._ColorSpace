using UnityEngine;

[System.Serializable]
public class SelfHeldTrigonometricColor
{
    public TrigonometricColor colorFlow;
    public Color currentColor;
    public float ciclesPerSecond = .5f;
    private float color_degree;

    public void Tick()
    {
        float deltaDegree = 2 * Mathf.PI * ciclesPerSecond;
        color_degree += deltaDegree * Time.deltaTime;
        if (color_degree > 2 * Mathf.PI)
        {
            color_degree -= 2 * Mathf.PI;
        }
        currentColor = colorFlow.GetColor(color_degree);
    }
}

[System.Serializable]
public class TrigonometricColor
{
    public Color color_high;
    public Color color_low;

    public Color GetColor(float color_degree)
    {
        Color deltaColor = color_high - color_low;
        return deltaColor * Mathf.Sin(color_degree) + color_low;
    }
}