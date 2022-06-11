using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;
using Lean.Touch;

public class PointEvent : MonoBehaviour
{
    public void ChessOnMove()
    {
        GameObject.Find("Chesses").SendMessage("ChessOnMove", transform.parent.gameObject.name);
    }
}
