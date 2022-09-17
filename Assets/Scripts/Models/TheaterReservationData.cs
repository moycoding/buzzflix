using System.Collections;
using System.Collections.Generic;

public class TheaterReservationData
{
    public TheaterReservationData(Reservations reservations)
    {
        var reservedSeats = new List<List<bool>>();
        List<int> seatsPerRow = reservations.theater.SeatsPerRow;
        for (int rowIndex = 0; rowIndex < seatsPerRow.Count; rowIndex++)
        {
            var seats = new List<bool>();
            for (int i = 0; i < seatsPerRow[rowIndex]; i++)
            {
                seats.Add(false);
            }
            reservedSeats.Add(seats);
        }

        for (int i = 0; i < reservations.reserved_seats.Count; i++)
        {
            var reservedSeat = reservations.reserved_seats[i];
            if (reservedSeat.row >= 0 && reservedSeat.row < reservedSeats.Count)
            {
                var row = reservedSeats[reservedSeat.row];
                if (reservedSeat.seat >= 0 && reservedSeat.seat < row.Count)
                {
                    row[reservedSeat.seat] = true;
                }
            }
        }

        this.ReservedSeats = reservedSeats;
    }

    public List<List<bool>> ReservedSeats { get; }
}