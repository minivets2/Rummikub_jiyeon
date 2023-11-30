using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private List<Slot> slots;
    [SerializeField] private int row;
    
    public List<Slot> Slots => slots;
    public int Row => row;
    
    private void Start()
    {
        row = FindChildIndex();
        SetSlotRow();
    }

    private void SetSlotRow()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetRow(row);
        }
    }
    
    private int FindChildIndex()
    {
        int index = 0;

        if (transform.parent.GetComponent<SharePlace>() || transform.parent.GetComponent<PlayerPlace>())
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) == transform)
                {
                    index = i;
                    break;
                }
            }
        }

        return index;
    }
}
