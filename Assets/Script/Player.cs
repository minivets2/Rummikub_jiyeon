using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("player info")]
    [SerializeField] private int playerIndex;
    [SerializeField] private Image playerImage;
    [SerializeField] private TMP_Text playerID;

    [SerializeField][Unity.Collections.ReadOnly] private bool startTurn;
    [SerializeField][Unity.Collections.ReadOnly] private float countTime;
    [SerializeField] private GameObject countTimePanel;
    [SerializeField] private TMP_Text textNumber;
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerPlace playerPlace;
    [SerializeField] private Button newCardButton;
    
    private PhotonView _photonView;
    
    public delegate void EndTurnEvent();
    public static EndTurnEvent endTurnEvent;

    private void Update()
    {
        if (startTurn)
        {
            countTime += Time.deltaTime;
            slider.value = Mathf.FloorToInt(countTime);
            textNumber.text = Mathf.FloorToInt(countTime).ToString();

            if (countTime > slider.maxValue)
            {
                EndTurn();
                endTurnEvent?.Invoke();
            }
        }
    }

    public void InitPlayerInfo(int playerIndex)
    {
        this.playerIndex = playerIndex;
        playerID.text = RoomManager.Instance.playerId;
        playerImage.sprite = RoomManager.Instance.playerImage;
        PlaceManager.Instance.InitPlayerPlace(playerPlace);
        startTurn = false;
        slider.maxValue = 10;
    }

    public void StartTurn()
    {
        countTime = 0;
        startTurn = true;
        countTimePanel.SetActive(true);
        textNumber.text = Mathf.FloorToInt(countTime).ToString();
        slider.value = Mathf.FloorToInt(countTime);

    }
    
    public void EndTurn()
    {
        startTurn = false;
        countTimePanel.SetActive(false);
        newCardButton.interactable = false;
    }
    
    
}
