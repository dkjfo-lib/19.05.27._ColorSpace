using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLine : MonoBehaviour
{
    public readonly int standartLineLength = 4;
    public readonly int standartElementsCount = 4;

    public float lineLength = 4f;
    public int elementsCount = 4;

    public SpriteRenderer sprite_prefab;


    public TrigonometricColor colorFlow;
    public float ciclesPerSecond = .5f;
    [Range (0f,1f)]public float delta_degree = .125f;
    private float initial_degree;
    public float deltaMovement = 3f;

    void Start()
    {

    }

    void Update()
    {
        CheckValues();
        if (sprite_prefab != null)
        {
            DestroyChildren();
            RiseChildren();
        }
        else
        {
            Debug.LogWarning("Little accident occurred and value was not found");
        }
    }

    private void CheckValues()
    {
        if (elementsCount < 1)
        {
            Debug.LogWarning("Little accident occurred and value was changed");
            elementsCount = standartElementsCount;
        }
        if (lineLength < 0)
        {
            Debug.LogWarning("Little accident occurred and value was changed");
            lineLength = standartLineLength;
        }
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
        {
            float deltaDegree = 2 * Mathf.PI * ciclesPerSecond;
            initial_degree += deltaDegree * Time.deltaTime;
            if (initial_degree > 2 * Mathf.PI)
            {
                initial_degree -= 2 * Mathf.PI;
            }
        }
        {
            float initialPoint = -lineLength / 2;
            float deltaElementDistance = lineLength / elementsCount;
            for (int i = 0; i < elementsCount; i++)
            {
                float elementDegree = initial_degree + delta_degree * i * 2 * Mathf.PI;
                Vector3 elementPosition =
                    transform.position +
                    transform.right * (deltaElementDistance * i + initialPoint) +
                    transform.forward * deltaMovement * Mathf.Sin(elementDegree);
                SpriteRenderer sr = Instantiate(
                    sprite_prefab,
                    elementPosition,
                    transform.rotation,
                    transform);
                sr.color = colorFlow.GetColor(elementDegree);
            }
        }
    }

    private void OnDrawGizmos()
    {
        float initialPoint = -lineLength / 2;
        float deltaElementDistance = lineLength / elementsCount;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < elementsCount; i++)
        {
            Gizmos.DrawWireSphere(
                transform.position + transform.right * (deltaElementDistance * i + initialPoint), 0.25f);
        }
        Gizmos.DrawLine(
            transform.position + transform.right * initialPoint,
            transform.position + transform.right * (-initialPoint - deltaElementDistance));
    }
}
