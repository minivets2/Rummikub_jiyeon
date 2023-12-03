using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerMessage;

    public void SetWinnerMessage(string winnerID)
    {
        transform.SetAsLastSibling();
        
        if (winnerID == "")
            winnerMessage.text = "당신이 승리했습니다!";
        else
            winnerMessage.text = "<" + winnerID + "> 이(가) 승리했습니다!";
    }

    public void RobbyButtonClick()
    {
        
    }
}
