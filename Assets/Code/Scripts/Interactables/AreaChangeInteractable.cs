using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaChangeInteractable : Interactable
{
    [SerializeField] string targetLocation;
    [SerializeField] bool unlocked;
    string message = "Press E to Enter ";
    public override string GetPromptMessage()
    {
       return message+targetLocation;
    }

    public override void Interact()
    {
        if (PlayerProfile.gameData.currentArea.HasKey() || unlocked)
        {
            message += targetLocation;
            PlayerProfile.SetCurrentArea(targetLocation);
            SceneManager.LoadScene(targetLocation);
        }
        else 
        {
            message = "You need a key to enter the ";
        }
    }
}
