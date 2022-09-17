using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Reservations
{
    public Theater theater;
    public List<ReservationData> reserved_seats;
}
