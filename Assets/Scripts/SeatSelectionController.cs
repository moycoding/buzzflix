using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SeatSelectionController
{
    private const string SeatRowsContainerName = "SeatRows";
    private const string SeatRowClass = "seat-row";
    private const string SeatButtonClass = "seat-button";
    private const string UnavailableSeatButtonClass = "unavailable-seat";

    private VisualElement m_SeatRows;

    private List<List<bool>> m_ReservedSeats;

    public SeatSelectionController(VisualElement panel)
    {
        LoadSeatData();

        m_SeatRows = panel.Q<VisualElement>(SeatRowsContainerName);

        PopulateView();
    }

    private void LoadSeatData()
    {
        m_ReservedSeats = new() {
            new() {false, false, false, false, false},
            new() {false, false, false, false, false},
            new() {false, false, true, true, false, false},
            new() {false, false, false, false, true, false},
            new() {false, false, false, false, false, false},
            new() {false, false, true, true, false, false, false, false},
            new() {false, false, false, false, true, false, false, false},
            new() {false, false, false, false, false, false, false, false}
        };
    }

    private void PopulateView()
    {
        for (int row = 0; row < m_ReservedSeats.Count; row++)
        {
            var rowData = m_ReservedSeats[row];

            var rowElement = new VisualElement();
            rowElement.AddToClassList(SeatRowClass);

            for (int seat = 0; seat < rowData.Count; seat++)
            {
                var button = new Button();
                button.AddToClassList(SeatButtonClass);
                if (rowData[seat])
                {
                    button.AddToClassList(UnavailableSeatButtonClass);
                    button.clickable = null;
                }
                rowElement.Add(button);
            }
            
            m_SeatRows.Add(rowElement);
        }
    }
}
