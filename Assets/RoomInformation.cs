using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInformation : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private List<Sprite> playerImages;
    [SerializeField] private TMP_Text playersCount;

    private int _index;
    private int _playersCount;

    private void Start()
    {
        _index = 0;
        _playersCount = 1;
        playersCount.text = _playersCount + "명";
    }

    public void PlayersInfoLeftButtonClick()
    {
        _playersCount--;

        if (_playersCount == 0) _playersCount = 1;

        playersCount.text = _playersCount + "명";
    }
    
    public void PlayersInfoRightButtonClick()
    {
        _playersCount++;

        if (_playersCount == 5) _playersCount = 4;

        playersCount.text = _playersCount + "명";
    }

    public void CharacterInfoLeftButtonClick()
    {
        _index--;

        if (_index == -1) _index = 0;

        playerImage.sprite = playerImages[_index];
    }
    
    public void CharacterInfoRightButtonClick()
    {
        _index++;

        if (_index == playerImages.Count) _index = playerImages.Count - 1;

        playerImage.sprite = playerImages[_index];
    }
}
