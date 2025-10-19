using UnityEngine;

public class DropZone : MonoBehaviour
{
 public bool isTopZone;
    private bool occupied = false;

    public bool CanAccept(CardManager deck)
    {
        // Player can only play in their corresponding half
        if (occupied) return false;
        if (deck.IsPlayer && !isTopZone) { occupied = true; return true; }
        if (!deck.IsPlayer && isTopZone) { occupied = true; return true; }
        return false;
    }
}
