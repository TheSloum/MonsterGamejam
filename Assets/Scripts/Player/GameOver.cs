using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // <-- needed for scene reload

public class GameOver : MonoBehaviour
{
    public Transform rotateObject;       // Object to rotate 90 degrees
    public Transform moveObject;         // Object to move -80 on local X after rotation
    public GameObject bloodMistPrefab;   // Prefab for blood mist particles
    public int bloodMistCount = 3;

    public GameObject bullet;
    public GameObject surprise;
    public float rotationSpeed = 90f;   // degrees per second
    public float moveSpeed = 80f;       // units per second
    public float bloodMoveSpeed = 1f;   // speed blood mists move
    public float bloodFadeDuration = 1.5f; // time until invisible

    private bool triggered = false;

    void Update()
    {
        if (!triggered && GameScore.Instance != null && GameScore.Instance.lost)
        {
            surprise.SetActive(true);
            triggered = true;
            StartCoroutine(HandleLostSequence());
        }
    }

    private IEnumerator HandleLostSequence()
    {
        // Step 1: Rotate rotateObject 90 degrees smoothly
        yield return StartCoroutine(RotateObject90(rotateObject, rotationSpeed));

        // Step 2: Move moveObject -3300 units on its local X
        yield return StartCoroutine(MoveObjectLocalX(moveObject, -3300f, moveSpeed));

        // Step 3: Spawn blood mist particles
        for (int i = 0; i < bloodMistCount; i++)
        {
            SpawnBloodMist();
        }

        // Wait 1 second before resetting the scene
        yield return new WaitForSeconds(5f);

        GameScore.Instance.lost = false;
        GameScore.Instance.score = 0;
        
        GameScore.Instance.day = 1;
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator RotateObject90(Transform obj, float speed)
    {
        float targetAngle = obj.eulerAngles.z + 90f;
        float remaining = 90f;

        while (remaining > 0f)
        {
            float step = speed * Time.deltaTime;
            if (step > remaining) step = remaining;
            obj.Rotate(0, 0, step);
            remaining -= step;
            yield return null;
        }

        obj.eulerAngles = new Vector3(obj.eulerAngles.x, obj.eulerAngles.y, targetAngle);
    }

    private IEnumerator MoveObjectLocalX(Transform obj, float distance, float speed)
    {
        float remaining = Mathf.Abs(distance);
        int direction = distance > 0 ? 1 : -1;

        while (remaining > 0f)
        {
            float step = speed * Time.deltaTime;
            if (step > remaining) step = remaining;
            obj.Translate(Vector3.right * step * direction, Space.Self);
            remaining -= step;
            yield return null;
        }
    }

    private void SpawnBloodMist()
    {
        surprise.SetActive(false);             
        bullet.SetActive(false);

        GameObject mist = Instantiate(bloodMistPrefab, transform.position, Quaternion.identity, transform);

        Vector3 direction = Vector3.left + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
        StartCoroutine(MoveAndFadeMist(mist.transform, direction.normalized));
    }

    private IEnumerator MoveAndFadeMist(Transform mist, Vector3 direction)
    {
        SpriteRenderer sr = mist.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float elapsed = 0f;
        Vector3 moveVector = direction * bloodMoveSpeed;

        while (elapsed < bloodFadeDuration)
        {
            mist.localPosition += moveVector * Time.deltaTime;
            sr.color = new Color(1f, 0f, 0f, Mathf.Lerp(1f, 0f, elapsed / bloodFadeDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(mist.gameObject);
    }
}
