using System.Collections.Generic;
using UnityEngine;

struct MyStruct
{
    
}

public class PlayGround : MonoBehaviour
{
    public List<List<Card>> _previousPlayGround = new List<List<Card>>();
    public  List<Card> element = new List<Card>();

    [SerializeField] private bool test = true;
    
    public void CheckComplete()
    {
        GameManager gameManager = GameManager.Instance;
        
        _previousPlayGround.Clear();
        element.Clear();
        
        foreach (var slot in gameManager.PlayGroundSlot)
        {
            if (slot.transform.childCount > 0)
            {
                Transform child = slot.transform.GetChild(0);

                if (child != null)
                {
                    Debug.Log(child.GetComponent<Card>().Number);
                    element.Add(child.GetComponent<Card>());
                }
            }
            else
            {
                if (element.Count > 0)
                {
                    if (element.Count < 3)
                    {
                        test = false;
                        return;
                    }
                    
                    _previousPlayGround.Add(element);
                    element.Clear();   
                }
            }
        }
        
        
        if (element.Count > 0)
        {
            _previousPlayGround.Add(element);
            element.Clear();   
        }
        
        test = true;
    }
}
