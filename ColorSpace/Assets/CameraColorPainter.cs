using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColorPainter : MonoBehaviour
{
    public SelfHeldTrigonometricColor color;
    public new Camera camera;

    void Update()
    {
        color.Tick();
        camera.backgroundColor = color.currentColor;
    }
}