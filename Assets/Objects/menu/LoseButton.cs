using UnityEngine;
using UnityEngine.UI;

public class LoseButton : MonoBehaviour
{
    
public GameObject gameOver; 
    void Start()
    {
        // On r�cup�re le composant Button du prefab
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(OnLoseButtonClicked);
        }
        else
        {
            Debug.LogWarning("Aucun composant Button trouv� sur " + gameObject.name);
        }
    }

    void OnLoseButtonClicked()
    {
    }
}
