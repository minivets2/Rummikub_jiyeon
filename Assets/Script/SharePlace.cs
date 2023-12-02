using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SharePlace : Place
{
    private List<List<Card>> _previousPlayGround = new List<List<Card>>();
    private List<Card> element = new List<Card>();
    
    public delegate void EndGameEvent(int playerIndex, string winnerID);
    public static EndGameEvent endGameEvent;

    public delegate void NextTurnEvent(int playerIndex, bool newCard);
    public static NextTurnEvent nextTurnEvent;
    
    private void Start()
    {
        transform.parent.SetParent(GameObject.Find("Canvas").transform);
        transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(32, 56,0);
        transform.parent.GetComponent<RectTransform>().localScale = Vector3.one;
        
        SharePlaceManager.Instance.InitSharePlace(gameObject);
    }

    private void OnEnable()
    {
        Player.checkCompleteEvent += CheckComplete;
    }

    private void OnDisable()
    {
        Player.checkCompleteEvent -= CheckComplete;
    }

    public void CheckComplete()
    {
        element.Clear();
        bool isComplete = false;
        int cardCount = 0;

        List<Slot> slots = SharePlaceManager.Instance.GetAllSlots();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                Transform child = slots[i].transform.GetChild(0);

                if (child != null)
                {
                    element.Add(child.GetComponent<Card>());
                    cardCount++;
                }
            }
            else if (slots[i].transform.childCount == 0 || i == slots.Count - 1)
            {
                if (element.Count > 0)
                {
                    if (element.Count < 3)
                    {
                        isComplete = false;
                        break;
                    }
   
                    if ((AllNumbersEqual(element) && AllColorsDifferent(element)) ||
                        AllColorsEqual(element) && AreConsecutive(element))
                    {
                        isComplete = true;
                        element.Clear();
                    }
                }
            }
        }
        
        if (isComplete && PlayerPlaceManager.Instance.GetCardCount() == 0)
        {
            endGameEvent?.Invoke(PhotonNetwork.LocalPlayer.ActorNumber - 1, RoomManager.Instance.playerId);
            return;
        }

        if (isComplete)
        {
            if (cardCount != SharePlaceManager.Instance.PreviousCardCount)
                nextTurnEvent?.Invoke(PhotonNetwork.LocalPlayer.ActorNumber - 1, false);
            else
                nextTurnEvent?.Invoke(PhotonNetwork.LocalPlayer.ActorNumber - 1, true);
        }
        else
        {
            SharePlaceManager.Instance.ResetCardList();
            nextTurnEvent?.Invoke(PhotonNetwork.LocalPlayer.ActorNumber - 1, true);
        }
    }
}
