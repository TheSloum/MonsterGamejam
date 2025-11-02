using UnityEngine;
using UnityEngine.UI;

public class LoseButton : MonoBehaviour
{
    void Start()
    {
        // On récupère le composant Button du prefab
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(OnLoseButtonClicked);
        }
        else
        {
            Debug.LogWarning("Aucun composant Button trouvé sur " + gameObject.name);
        }
    }

    void OnLoseButtonClicked()
    {
        Debug.Log("Bouton cliqué ! Le joueur a perdu !");
        GameScore.Instance.lost = true;
    }
}
