using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private List<Slot> slots;

    public List<Slot> Slots => slots;
}
