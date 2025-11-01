using System.Collections;

using UnityEngine;

public class DisappearOnInteract2D : MonoBehaviour
{
    [Header("Paramètres du carré")]
    public float delayBeforeDisappear = 2f;
    public Color highlightColor = Color.yellow;

    [Header("Barre de progression")]
    public Transform progressBar;
    public GameObject barBackground;

    [Header("Apparence")]
    public Sprite[] staticSprites; // 6 sprites cadavre_1 à cadavre_6
    public Animator animator;      // Animator pour le cadavre_0 animé

    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 initialScale;
    private bool playerIsNear = false;
    private bool isDisappearing = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

        // Collider setup
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // Progress bar setup
        if (progressBar != null)
        {
            initialScale = progressBar.localScale;
            progressBar.gameObject.SetActive(false);
        }
        if (barBackground != null)
            barBackground.SetActive(false);

        // Player ref
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();



        int index = Random.Range(0, 7); // 0–6 inclus

        if (index == 0)
        {
            if (animator != null)
                animator.enabled = true;
        }
        else
        {
            sr.sprite = staticSprites[index - 1];
            if (animator != null)
                animator.enabled = false;
        }




    }

    void Update()
    {
        if (playerIsNear && Input.GetKey(KeyCode.Space) && !isDisappearing)
        {
            if (!playerMovement.IsMoving())
            {
                StartCoroutine(DisappearAfterDelay());
            }
            else
            {
                ResetProgressBar();
            }
        }

        if (isDisappearing && !Input.GetKey(KeyCode.Space))
        {
            ResetProgressBar();
        }
    }

    private IEnumerator DisappearAfterDelay()
    {
        isDisappearing = true;
        float elapsed = 0f;

        if (barBackground != null)
            barBackground.SetActive(true);

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.localScale = new Vector3(0f, initialScale.y, initialScale.z);
        }

        while (elapsed < delayBeforeDisappear)
        {
            if (playerMovement.IsMoving() || !Input.GetKey(KeyCode.Space))
            {
                ResetProgressBar();
                yield break;
            }

            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / delayBeforeDisappear);

            if (progressBar != null)
                progressBar.localScale = new Vector3(progress * initialScale.x, initialScale.y, initialScale.z);

            yield return null;
        }

        GameManager.Instance.AddScore(1);
        Destroy(gameObject);
    }

    private void ResetProgressBar()
    {
        isDisappearing = false;
        if (progressBar != null)
            progressBar.localScale = new Vector3(0f, initialScale.y, initialScale.z);

        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        if (barBackground != null)
            barBackground.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
            if (sr != null)
            sr.color = highlightColor;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
            if (sr != null)
                sr.color = originalColor;

            ResetProgressBar();
        }
    }
}
