using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCast : MonoBehaviour
{
    public float coneAngle = 55f;
    public int rayCount = 10;
    public float rayDistance = 5f;

    [Header("Detection Settings")]
    public LayerMask detectionMask;
    [Header("UI Prefab à afficher")]
    public GameObject objectToShow; // Le prefab d'UI à afficher (non présent dans la scène)

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
                GameScore.Instance.lost = true;
                Debug.Log("Player détecté !");
            }
        }

        if (objectToShow != null && playerDetected)
        {
            // Vérifie s’il existe déjà dans la scène
            if (!GameObject.Find(objectToShow.name + "(Clone)"))
            {
                // Trouve le Canvas (celui de la Main Camera)
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    // Instancie le prefab comme enfant du Canvas
                    GameObject uiInstance = Instantiate(objectToShow, canvas.transform);
                    uiInstance.transform.localPosition = Vector3.zero; // Centre sur l’écran
                    Debug.Log("UI instancié dans le Canvas de la caméra !");
                }
                else
                {
                    Debug.LogWarning("Aucun Canvas trouvé dans la scène !");
                }
            }
        }
    }
}
