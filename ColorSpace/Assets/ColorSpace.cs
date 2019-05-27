using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpace : MonoBehaviour
{
    public SpriteRenderer sprite_prefab;
    public new Camera camera;
    public UpdateColorFlow colorFlow;
    private Color currentColor;
    private Vector3 colorPosition;

    void Update()
    {
        if (CheckValues())
        {
            colorFlow.Update();
            UpdateColorValues();
            DestroyChildren();
            RiseChildren();
        }
    }


    bool CheckValues()
    {
        if (camera == null)
        {
            Debug.LogWarning("Little accident occurred and value was not set");
            return false;
        }
        if (sprite_prefab == null)
        {
            Debug.LogWarning("Little accident occurred and value was not set");
            return false;
        }
        return true;
    }

    private void UpdateColorValues()
    {
        currentColor = colorFlow.GetColor();
        //camera.backgroundColor = currentColor;
        colorPosition = new Vector3(currentColor.r, currentColor.g, currentColor.b);
    }

    private void DestroyChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void RiseChildren()
    {
        colorPosition.Scale(transform.localScale);
        Vector3 spritePosition =
            transform.position + colorPosition;
        SpriteRenderer sr = Instantiate(
            sprite_prefab,
            spritePosition,
            transform.rotation,
            transform);
        sr.color = currentColor;
        sr.transform.localScale = transform.localScale * .25f;
        Vector3 deltaCameraVector = camera.transform.position - spritePosition;
        sr.transform.forward = deltaCameraVector;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + transform.localScale * .5f, transform.localScale);
    }
}

[System.Serializable]
public class UpdateColorFlow
{
    private const float standartCiclesPerSecond = .25f;
    private const float standartColorPercent = 0f;

    public ColorFlow colorFlow;
    public float ciclesPerSecond = standartCiclesPerSecond;
    [Range(0, 1)] public float colorPercent = standartColorPercent;
    private int direction = 1;

    public Color GetColor()
    {
        return colorFlow.GetColor(colorPercent);
    }

    public void Update()
    {
        if (CheckValues())
        {
            UpdateColorPercent();
        }
    }

    bool CheckValues()
    {
        if (colorPercent < 0 || colorPercent > 1)
        {
            Debug.LogWarning("Little accident occurred and value was out of bounds");
            colorPercent = standartColorPercent;
            return false;
        }
        if (ciclesPerSecond < 0)
        {
            Debug.LogWarning("Little accident occurred and value was out of bounds");
            ciclesPerSecond = standartCiclesPerSecond;
            return false;
        }
        return true;
    }

    void UpdateColorPercent()
    {
        colorPercent += direction * ciclesPerSecond * Time.deltaTime;
        if (colorPercent > 1)
        {
            colorPercent = 1 - (colorPercent % 1);
            direction = -1;
        }
        if (colorPercent < 0)
        {
            colorPercent = -(colorPercent % 1);
            direction = 1;
        }
    }
}

[System.Serializable]
public class ColorFlow
{
    public enum FlowMode
    {
        Trigonometric,
        Linear
    }

    public Color color_high = Color.white;
    public Color color_low = Color.black;
    public FlowMode mode = FlowMode.Linear;

    public Color GetColor(float flowPercent)
    {
        Color deltaColor = color_high - color_low;
        switch (mode)
        {
            case FlowMode.Trigonometric:
                float color_degree = 2 * Mathf.PI / flowPercent;
                return deltaColor * (.5f * Mathf.Sin(color_degree) + .5f) + color_low;
            case FlowMode.Linear:
                return deltaColor * flowPercent + color_low;
            default:
                Debug.LogWarning("Little accident occurred and mode was not found");
                return Color.white;
        }
    }
}