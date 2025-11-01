using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DisappearOnInteract2D : MonoBehaviour
{
    [Header("Paramètres du carré")]
    public float delayBeforeDisappear = 2f;
    public Color highlightColor = Color.yellow;
    public Vector2 hitboxSize = new Vector2(1.5f, 1.5f);
    public int scoreValue = 1;

    [Header("Barre de progression")]
    public Transform progressBar;
    public GameObject barBackground;

    [Header("Apparence")]
    public Sprite[] possibleSprites = new Sprite[6]; // Tableau de 6 sprites à assigner dans l’inspector


    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 initialScale;

    private bool playerIsNear = false;
    private bool isCollecting = false;
    private Coroutine collectionCoroutine;
    private PlayerMovement playerMovement;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = hitboxSize;
            collider.isTrigger = true;
        }

        if (progressBar != null)
        {
            initialScale = progressBar.localScale;
            progressBar.gameObject.SetActive(false);
        }

        if (barBackground != null)
            barBackground.SetActive(false);

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;

            // Choix aléatoire d’un sprite dans le tableau
            if (possibleSprites != null && possibleSprites.Length > 0)
            {
                int index = Random.Range(0, possibleSprites.Length); 
                sr.sprite = possibleSprites[index];
            }
        }
    }


    void Update()
    {
        if (!playerIsNear || playerMovement == null)
        {
            CancelCollection();
            return;
        }

        // Commence la collecte uniquement si Espace est maintenu
        if (Input.GetKey(KeyCode.Space))
        {
            // Annule si le joueur bouge
            if (playerMovement.IsMoving())
            {
                CancelCollection();
            }
            else if (!isCollecting)
            {
                collectionCoroutine = StartCoroutine(FillBar());
            }
        }
        else
        {
            CancelCollection();
        }
    }

    private IEnumerator FillBar()
    {
        isCollecting = true;

        if (barBackground != null)
            barBackground.SetActive(true);

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.localScale = new Vector3(0f, initialScale.y, initialScale.z);
        }


        float elapsed = 0f;
        while (elapsed < delayBeforeDisappear)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / delayBeforeDisappear);

            if (progressBar != null)
                progressBar.localScale = new Vector3(progress * initialScale.x, initialScale.y, initialScale.z);

            // Annule si le joueur bouge ou relâche Espace
            if (!Input.GetKey(KeyCode.Space) || playerMovement.IsMoving())
            {
                CancelCollection();
                yield break;
            }

            yield return null;
        }

        // Ajouter le score
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(scoreValue);

        Destroy(gameObject);
        isCollecting = false;
    }

    private void CancelCollection()
    {
        if (!isCollecting) return;

        // Réinitialise la barre
        if (progressBar != null)
            progressBar.localScale = new Vector3(0f, initialScale.y, initialScale.z);

        // On garde le fond visible si tu veux
        if (barBackground != null)
            barBackground.SetActive(true);

        if (collectionCoroutine != null)
        {
            StopCoroutine(collectionCoroutine);
            collectionCoroutine = null;
        }

        isCollecting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
            if (sr != null) sr.color = highlightColor;
            playerMovement = collision.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
            if (sr != null) sr.color = originalColor;
            CancelCollection();
        }
    }
}
