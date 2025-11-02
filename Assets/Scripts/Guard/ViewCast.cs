using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCast : MonoBehaviour
{
    [Header("Vision Settings")]
    public float coneAngle = 55f;
    public int rayCount = 10;
    public float rayDistance = 5f;

    [Header("Detection Settings")]
    public LayerMask detectionMask;

    [Header("Prefab à afficher (Sprite animé)")]
    public GameObject objectToShow;

    private float timer = 0f;
    private bool coold = true;
    private bool playerDetected = false;

    void Update()
    {
        if (coold)
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                coold = false;
            }
        }

        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        Vector2 forward = transform.up;
        float halfAngle = coneAngle / 2f;
        float startAngle = -halfAngle;
        float angleStep = coneAngle / (rayCount - 1);
        playerDetected = false;

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;
            Vector2 dir = Quaternion.Euler(0, 0, currentAngle) * forward;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, detectionMask);
            Vector2 endPoint = hit ? hit.point : (Vector2)transform.position + dir * rayDistance;
            Debug.DrawLine(transform.position, endPoint, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerDetected = true;
            }
        }

        if (objectToShow != null && playerDetected)
        {
            if (!GameObject.Find(objectToShow.name + "(Clone)"))
            {
                GameObject container = GameObject.Find("InGame");


                if (container != null)
                {
                    GameObject instance = Instantiate(objectToShow, container.transform);
                    Debug.Log("Sprite animé instancié dans 'InGame' !");
                }
                else
                {
                    Debug.LogWarning("Conteneur 'InGame' introuvable dans la scène !");
                }
            }
        }
    }
}
