using UnityEngine;

public class BusSeat : MonoBehaviour
{
    [Header("Seat Settings")]
    [SerializeField] private Transform seatAnchor;
    [SerializeField] private bool seatOccupied = false;

    public Transform SeatAnchor => seatAnchor;
    public bool IsOccupied => seatOccupied;

    public bool TrySit()
    {
        if (seatOccupied) return false;

        seatOccupied = true;
        return true;
    }

    public void LeaveSeat()
    {
        seatOccupied = false;
    }
}