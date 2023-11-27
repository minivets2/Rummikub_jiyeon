using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("player info")]
    [SerializeField] private Image playerImage;
    [SerializeField] private TMP_Text playerID;

    [SerializeField][Unity.Collections.ReadOnly] private bool startTurn;
    [SerializeField][Unity.Collections.ReadOnly] private float countTime;
    [SerializeField] private GameObject countTimePanel;
    [SerializeField] private TMP_Text textNumber;
    [SerializeField] private Slider slider;
    [SerializeField] private PlaceManager placeManager;
    
    private void Update()
    {
        if (startTurn)
        {
            countTime += Time.deltaTime;
            slider.value = Mathf.FloorToInt(countTime);
            textNumber.text = Mathf.FloorToInt(countTime).ToString();

            if (countTime > slider.maxValue)
            {
                startTurn = false;
                countTimePanel.SetActive(false);

                PlaceManager placeManager = PlaceManager.Instance;
                /*
                if (!placeManager.SharePlace.CheckComplete())
                {
                    placeManager.ResetCardList();
                }
                else
                {
                    placeManager.SaveCardList();
                }
                */
            }
        }
    }

    public void InitPlayerInfo()
    {
        playerID.text = RoomManager.Instance.playerId;
        playerImage.sprite = RoomManager.Instance.playerImage;
        placeManager.InitPlayerPlace();
        startTurn = false;
    }

    public void PlayerTurn()
    {
        countTime = 0;
        startTurn = true;
        countTimePanel.SetActive(true);
        textNumber.text = Mathf.FloorToInt(countTime).ToString();
        slider.maxValue = 30;
        slider.value = Mathf.FloorToInt(countTime);

    }
    
    
}
