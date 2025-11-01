using System.Collections;
using UnityEngine;

public class DisappearOnInteract2D : MonoBehaviour
{
    [Header("Paramètres du carré")]
    public float delayBeforeDisappear = 2f;       // Durée avant disparition
    public Color highlightColor = Color.yellow;  // Couleur quand le joueur est proche
    public Vector2 hitboxSize = new Vector2(1.5f, 1.5f);

    [Header("Barre de progression")]
    public Transform progressBar;    // La barre verte
    public GameObject barBackground; // Fond noir derrière la barre

    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 initialScale;
    private bool playerIsNear = false;
    private bool isDisappearing = false;

    void Start()
    {
        // SpriteRenderer du carré
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

        // Ajuster la taille du collider
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = hitboxSize;
            collider.isTrigger = true;
        }

        // Initialiser la barre verte
        if (progressBar != null)
        {
            initialScale = progressBar.localScale;
            progressBar.gameObject.SetActive(false); // cachée au départ
        }

        // Cacher le fond noir
        if (barBackground != null)
            barBackground.SetActive(false);
    }

    void Update()
    {
        // Lancer la destruction si le joueur est proche et appuie sur Espace
        if (playerIsNear && Input.GetKeyDown(KeyCode.Space) && !isDisappearing)
        {
            StartCoroutine(DisappearAfterDelay());
        }
    }

    private IEnumerator DisappearAfterDelay()
    {
        isDisappearing = true;
        float elapsed = 0f;

        // Activer le fond noir
        if (barBackground != null)
            barBackground.SetActive(true);

        // Activer et réinitialiser la barre verte
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.localScale = new Vector3(0f, initialScale.y, initialScale.z);
        }

        // Remplir la barre progressivement
        while (elapsed < delayBeforeDisappear)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / delayBeforeDisappear);

            if (progressBar != null)
                progressBar.localScale = new Vector3(progress * initialScale.x, initialScale.y, initialScale.z);

            yield return null;
        }

        // Détruire le carré
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
            if (sr != null)
                sr.color = highlightColor; // Changer couleur quand proche
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
            if (sr != null)
                sr.color = originalColor;

            // Si la barre n’a pas encore commencé, cacher barre et fond
            if (!isDisappearing)
            {
                if (progressBar != null)
                    progressBar.gameObject.SetActive(false);

                if (barBackground != null)
                    barBackground.SetActive(false);
            }
        }
    }
}
