using System.Collections.Generic;
using UnityEngine;

public class SharePlace : Place
{
    private List<List<Card>> _previousPlayGround = new List<List<Card>>();
    private List<Card> element = new List<Card>();
    private bool _test;

    private void Start()
    {
        transform.parent.SetParent(GameObject.Find("Canvas").transform);
        transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(32, 56,0);
        transform.parent.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public bool CheckComplete()
    {
        _previousPlayGround.Clear();
        element.Clear();

        for (int i = 0; i < PlaceManager.Instance.SharedSlots.Length; i++)
        {
            if (PlaceManager.Instance.SharedSlots[i].transform.childCount > 0)
            {
                Transform child = PlaceManager.Instance.SharedSlots[i].transform.GetChild(0);

                if (child != null)
                {
                    element.Add(child.GetComponent<Card>());
                }
            }
            else if (PlaceManager.Instance.SharedSlots[i].transform.childCount == 0 || i == PlaceManager.Instance.SharedSlots.Length - 1)
            {
                if (element.Count > 0)
                {
                    if (element.Count < 3)
                    {
                        _test = false;
                        return _test;
                    }
   
                    if ((AllNumbersEqual(element) && AllColorsDifferent(element)) ||
                        AllColorsEqual(element) && AreConsecutive(element))
                    {
                        _previousPlayGround.Add(element);
                        element.Clear();
                    }
                }
            }
        }

        _test = true;
        return _test;
    }
}
