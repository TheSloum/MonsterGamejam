using UnityEngine;

public class menu : MonoBehaviour
{
    public PlayerMovement player;
    public SpriteRenderer startButtonSprite;
    public SpriteRenderer quitButtonSprite;

    void Start()
    {
        if (player != null)
            player.canMove = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == startButtonSprite.gameObject)
                {
                    StartGame();
                }
                else if (hit.collider.gameObject == quitButtonSprite.gameObject)
                {
                    QuitGame();
                }
            }
        }
    }

    void StartGame()
    {
        Debug.Log("▶️ Start cliqué !");
        if (player != null)
            player.canMove = true;
        gameObject.SetActive(false);
    }

    void QuitGame()
    {
        Debug.Log("⛔ Quit cliqué !");
        Application.Quit();
    }
}
