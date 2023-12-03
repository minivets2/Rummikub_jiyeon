using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    [SerializeField] private int index;
    [Header("UI")]
    [SerializeField] private Image playerImage;
    [SerializeField] private TMP_Text playerID;
    [SerializeField] private GameObject countTimePanel;
    [SerializeField] private TMP_Text textNumber;
    [SerializeField] private Slider slider;

    private bool _startTurn;
    private float _countTime;

    public int Index => index;
    
    private void Update()
    {
        if (_startTurn)
        {
            _countTime += Time.deltaTime;
            slider.value = Mathf.FloorToInt(_countTime);
            textNumber.text = Mathf.FloorToInt(_countTime).ToString();
        }
    }
    
    public void StartTurn()
    {
        _countTime = 0;
        _startTurn = true;
        countTimePanel.SetActive(true);
        textNumber.text = Mathf.FloorToInt(_countTime).ToString();
        slider.value = Mathf.FloorToInt(_countTime);
        slider.maxValue = 30;
    }

    public void EndTurn()
    {
        _countTime = 0;
        _startTurn = false;
        countTimePanel.SetActive(false);
        slider.value = Mathf.FloorToInt(_countTime);
    }

    public void SetProfile(Sprite playerImage, string playerID)
    {
        this.playerImage.sprite = playerImage;
        this.playerID.text = playerID;
    }

}
