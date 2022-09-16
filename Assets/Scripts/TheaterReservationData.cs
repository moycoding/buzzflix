using System.Collections;
using System.Collections.Generic;

readonly public struct TheaterReservationData
{
    public TheaterReservationData(List<List<bool>> reservedSeats)
    {
        this.ReservedSeats = reservedSeats;
    }

    public List<List<bool>> ReservedSeats { get; }
}