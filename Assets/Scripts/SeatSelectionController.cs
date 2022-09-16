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
    private const string SelectedSeatButtonClass = "selected-seat";

    private VisualElement m_SeatRows;

    private List<List<bool>> m_ReservedSeats;
    private List<List<Button>> m_Buttons;
    private int m_SelectedRow = -1;
    private int m_SelectedSeat = -1;

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
            new() {false, false, false, false, false, false, false, false},
            new() {false, false, true, true, false, false, false, false},
            new() {false, false, false, false, true, false, false, false},
            new() {false, false, false, false, false, false, false, false}
        };
    }

    private void PopulateView()
    {
        m_Buttons = new();

        for (int row = 0; row < m_ReservedSeats.Count; row++)
        {
            var rowData = m_ReservedSeats[row];
            var buttonList = new List<Button>();

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
                else
                {
                    var localRow = row;
                    var localSeat = seat;
                    button.clicked += () => SelectSeat(localRow, localSeat);
                }

                buttonList.Add(button);
                rowElement.Add(button);
            }
            
            m_Buttons.Add(buttonList);
            m_SeatRows.Add(rowElement);
        }
    }

    private void SelectSeat(int row, int seat)
    {
        if (m_SelectedRow != -1)
        {
            var prevButton = m_Buttons[m_SelectedRow][m_SelectedSeat];
            prevButton.RemoveFromClassList(SelectedSeatButtonClass);
        }

        m_SelectedRow = row;
        m_SelectedSeat = seat;

        var currButton = m_Buttons[m_SelectedRow][m_SelectedSeat];
        currButton.AddToClassList(SelectedSeatButtonClass);
    }
}
