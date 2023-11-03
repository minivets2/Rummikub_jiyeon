using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform deckTransform;
    [SerializeField] private Card cardPrefab;
    
    public void CreateCard()
    {
        CardManager cardManager = CardManager.Instance;
        var card = Instantiate(cardPrefab, deckTransform);
        string cardStatus = cardManager.GetRandomCardStatus();
        
        string number = cardStatus.Substring(1, cardStatus.Length - 1);
        string color = cardStatus.Substring(0, 1);
        
        card.SetCardStatus(int.Parse(number), color);
        
        card.transform.localScale = Vector3.one;
    }
}
