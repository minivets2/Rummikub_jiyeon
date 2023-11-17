using System;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField][ReadOnly] private bool startTurn;
    [SerializeField][ReadOnly] private float countTime;
    [SerializeField] private GameObject countTimePanel;
    [SerializeField] private TMP_Text textNumber;
    [SerializeField] private Slider slider;
    
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

    public void StartTurn()
    {
        countTime = 0;
        startTurn = true;
        countTimePanel.SetActive(true);
        textNumber.text = Mathf.FloorToInt(countTime).ToString();
        slider.maxValue = 30;
        slider.value = Mathf.FloorToInt(countTime);
    }
    
    
}
