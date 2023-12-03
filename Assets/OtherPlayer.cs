using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Image playerImage;
    [SerializeField] private TMP_Text playerID;

    public int Index => index;

    public void SetProfile(Sprite playerImage, string playerID)
    {
        this.playerImage.sprite = playerImage;
        this.playerID.text = playerID;
    }

}
