using System.Collections;
using UnityEngine;

public class DisappearOnInteract2D : MonoBehaviour
{
    [Header("Paramètres du carré")]
    public float delayBeforeDisappear = 2f;
    public Color highlightColor = Color.yellow;
    public Vector2 hitboxSize;

    [Header("Barre de progression")]
    public Transform progressBar;
    public GameObject barBackground;

    [Header("Apparence")]
    public Sprite[] staticSprites;
    public Animator animator;

    [Header("Audio")]
    public AudioClip pickupClip;
    [Range(0f, 1f)] public float pickupVolume = 1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 initialScale;
    private bool playerIsNear = false;
    private bool isDisappearing = false;
    private PlayerMovement playerMovement;
    private AudioSource currentPickupAudio; // 🎧 le son actif pendant le ramassage

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;

        // Collider setup
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = hitboxSize;
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

        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        int index = Random.Range(0, 7);
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
                StopPickupSound();
                ResetProgressBar();
            }
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            StopPickupSound();
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

        // 🔊 Démarre le son de ramassage pendant la progression
        PlayPickupSound();

        while (elapsed < delayBeforeDisappear)
        {
            if (playerMovement.IsMoving() || !Input.GetKey(KeyCode.Space))
            {
                StopPickupSound();
                ResetProgressBar();
                yield break;
            }

            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / delayBeforeDisappear);

            if (progressBar != null)
                progressBar.localScale = new Vector3(progress * initialScale.x, initialScale.y, initialScale.z);

            yield return null;
        }

        StopPickupSound();

        GameManager.Instance.AddScore(1);
        Destroy(gameObject);
    }

    private void PlayPickupSound()
    {
        if (pickupClip == null) return;

        // Si un son est déjà en train de jouer, ne pas le redémarrer
        if (currentPickupAudio != null && currentPickupAudio.isPlaying) return;

        GameObject tempAudio = new GameObject("PickupSound");
        tempAudio.transform.position = Camera.main.transform.position;

        currentPickupAudio = tempAudio.AddComponent<AudioSource>();
        currentPickupAudio.clip = pickupClip;
        currentPickupAudio.volume = pickupVolume;
        currentPickupAudio.loop = true; // 🔁 boucle pendant la progression
        currentPickupAudio.spatialBlend = 0f;
        currentPickupAudio.Play();
    }

    private void StopPickupSound()
    {
        if (currentPickupAudio != null)
        {
            currentPickupAudio.Stop();
            Destroy(currentPickupAudio.gameObject);
            currentPickupAudio = null;
        }
    }

    private void ResetProgressBar()
    {
        isDisappearing = false;

        if (progressBar != null)
        {
            progressBar.localScale = new Vector3(0f, initialScale.y, initialScale.z);
            progressBar.gameObject.SetActive(false);
        }

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

            StopPickupSound();
            ResetProgressBar();
        }
    }
}
