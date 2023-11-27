using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    
    public void ReadyButtonClick()
    {
        readyButton.interactable = false;
        FindObjectOfType<GameReady>().SetReadyCount();
    }
}
