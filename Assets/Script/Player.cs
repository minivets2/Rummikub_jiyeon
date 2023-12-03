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
    
    [Header("UI")]
    [SerializeField] private GameObject countTimePanel;
    [SerializeField] private TMP_Text textNumber;
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerPlace playerPlace;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button newCardButton;

    private PhotonView _photonView;
    private bool _startTurn;
    private float _countTime;
    
    public delegate void StartTurnEvent();
    public static StartTurnEvent startTurnEvent;

    public delegate void ResetEvent();
    public static ResetEvent resetEvent;
    
    public delegate void CheckCompleteEvent();
    public static CheckCompleteEvent checkCompleteEvent;
    
    public delegate void OtherPlayerSettingEvent(int playerIndex, int playerImageIndex, string playerId);
    public static OtherPlayerSettingEvent otherPlayerSettingEvent;

    private void Update()
    {
        if (_startTurn)
        {
            _countTime += Time.deltaTime;
            slider.value = Mathf.FloorToInt(_countTime);
            textNumber.text = Mathf.FloorToInt(_countTime).ToString();

            if (_countTime > slider.maxValue)
            {
                NewCardButtonClick();
            }
        }
    }

    public void InitPlayerInfo(int playerIndex)
    {
        this.playerIndex = playerIndex;
        playerID.text = RoomManager.Instance.playerId;
        playerImage.sprite = RoomManager.Instance.playerImage;
        PlayerPlaceManager.Instance.InitPlayerPlace(playerPlace);
        _startTurn = false;
        slider.maxValue = 30;
        otherPlayerSettingEvent?.Invoke(playerIndex, RoomManager.Instance.playerImageIndex, RoomManager.Instance.playerId);
    }

    public void StartTurn()
    {
        _countTime = 0;
        _startTurn = true;
        countTimePanel.SetActive(true);
        textNumber.text = Mathf.FloorToInt(_countTime).ToString();
        slider.value = Mathf.FloorToInt(_countTime);
        resetButton.gameObject.SetActive(true);
        newCardButton.interactable = true;
        startTurnEvent?.Invoke();
        SharePlaceManager.Instance.SaveCardList();
    }
    
    public void EndTurn()
    {
        _countTime = 0;
        _startTurn = false;
        countTimePanel.SetActive(false);
        slider.value = Mathf.FloorToInt(_countTime);
        resetButton.gameObject.SetActive(false);
        newCardButton.interactable = false;
    }

    public void NewCardButtonClick()
    {
        EndTurn();
        checkCompleteEvent?.Invoke();
    }
    
    public void ResetButtonClick()
    {
        resetEvent?.Invoke();
    }
    
    
}
