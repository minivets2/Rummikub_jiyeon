using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField][ReadOnly] private bool startTurn;
    [SerializeField][ReadOnly] private float countTime;
    [SerializeField] private GameObject countTimePanel;
    [SerializeField] private TMP_Text textNumber;
    
    private void Update()
    {
        if (startTurn)
        {
            countTime -= Time.deltaTime;
            textNumber.text = Mathf.FloorToInt(countTime).ToString();

            if (countTime < 0)
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

    public void StartTurn()
    {
        countTime = 30;
        startTurn = true;
        countTimePanel.SetActive(true);
        textNumber.text = Mathf.FloorToInt(countTime).ToString();
    }
    
    
}
