using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController
{
    public event Action<DateTime> onDateChanged;
    public event Action<int> onMovieIndexChanged;
    public event Action<int> onTimeIndexChanged;
    public event Action<int> onRowChanged;
    public event Action<int> onSeatChanged;

    public string ReservationName { get; set; }

    public DateTime Date
    {
        get => m_Date;
        set
        {
            if (m_Date != value)
            {
                m_Date = value;
                MovieIndex = -1;
                TimeIndex = -1;
                onDateChanged?.Invoke(value);
            }
        }
    }

    public int MovieIndex
    {
        get => m_MovieIndex;
        set
        {
            if (m_MovieIndex != value)
            {
                m_MovieIndex = value;
                onMovieIndexChanged?.Invoke(value);
            }
        }
    }

    public int TimeIndex
    {
        get => m_TimeIndex;
        set
        {
            if (m_TimeIndex != value)
            {
                m_TimeIndex = value;
                Row = -1;
                Seat = -1;
                onTimeIndexChanged?.Invoke(value);
            }
        }
    }

    public int Row
    {
        get => m_Row;
        set
        {
            if (m_Row != value)
            {
                m_Row = value;
                onRowChanged?.Invoke(value);
            }
        }
    }

    public int Seat
    {
        get => m_Seat;
        set
        {
            if (m_Seat != value)
            {
                m_Seat = value;
                onSeatChanged?.Invoke(value);
            }
        }
    }

    private DateTime m_Date = DateTime.Today;
    private int m_MovieIndex = -1;
    private int m_TimeIndex = -1;
    private int m_Row = -1;
    private int m_Seat = -1;
}
