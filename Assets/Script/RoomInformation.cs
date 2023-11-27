using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInformation : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private List<Sprite> playerImages;
    [SerializeField] private TMP_Text playersCount;
    [SerializeField] private List<Button> buttons;

    private int _playerImageIndex;
    private int _playersCount;

    public int PlayerImageIndex => _playerImageIndex;
    public int PlayersCount => _playersCount;

    public Sprite GetPlayerImage()
    {
        return playerImages[PlayerImageIndex];
    }

    private void Start()
    {
        _playerImageIndex = 0;
        _playersCount = 1;
        playersCount.text = _playersCount + "명";
    }

    public void SetButton(bool value)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = value;
        }
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
        _playerImageIndex--;

        if (_playerImageIndex == -1) _playerImageIndex = 0;

        playerImage.sprite = playerImages[_playerImageIndex];
    }
    
    public void CharacterInfoRightButtonClick()
    {
        _playerImageIndex++;

        if (_playerImageIndex == playerImages.Count) _playerImageIndex = playerImages.Count - 1;

        playerImage.sprite = playerImages[_playerImageIndex];
    }
}
