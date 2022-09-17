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
    private SelectionController m_SelectionController;
    private ApiService m_ApiService;

    public SeatSelectionController(
        VisualElement panel,
        SelectionController selectionController,
        ApiService apiService)
    {
        m_SelectionController = selectionController;
        m_ApiService = apiService;

        LoadSeatData();

        m_SeatRows = panel.Q<VisualElement>(SeatRowsContainerName);

        PopulateView();

        apiService.onReservationsUpdated += () =>
        {
            LoadSeatData();
            PopulateView();
        };
    }

    private void LoadSeatData()
    {
        m_ReservedSeats = null;
        if (m_SelectionController.TimeIndex != -1)
        {
            var data = new TheaterReservationData(m_ApiService.Reservations);
            m_ReservedSeats = data.ReservedSeats;
        }
    }

    private void PopulateView()
    {
        m_Buttons = new();
        m_SeatRows.Clear();

        if (m_ReservedSeats == null) return;

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
        if (m_SelectionController.Row != -1)
        {
            var prevButton = m_Buttons[m_SelectionController.Row][m_SelectionController.Seat];
            prevButton.RemoveFromClassList(SelectedSeatButtonClass);
        }

        m_SelectionController.Row = row;
        m_SelectionController.Seat = seat;

        if (row != -1 && seat != -1)
        {
            var currButton = m_Buttons[m_SelectionController.Row][m_SelectionController.Seat];
            currButton.AddToClassList(SelectedSeatButtonClass);
        }
    }
}
