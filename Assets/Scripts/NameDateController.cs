using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class NameDateController
{
    private const string ReservationNameFieldName = "ReservationNameInput";
    private const string DateYearDropdownName = "YearDropdown";
    private const string DateMonthDropdownName = "MonthDropdown";
    private const string DateDayDropdownName = "DayDropdown";

    private TextField m_NameField;
    private DropdownField m_YearDropdown;
    private DropdownField m_MonthDropdown;
    private DropdownField m_DayDropdown;

    private string m_ReservationName;
    private DateTime m_SelectedDate;
    private int m_CurrentYear;
    private DateTimeFormatInfo m_DateTimeFormat;

    public NameDateController(VisualElement panel)
    {
        m_NameField = panel.Q<TextField>(ReservationNameFieldName);
        m_YearDropdown = panel.Q<DropdownField>(DateYearDropdownName);
        m_MonthDropdown = panel.Q<DropdownField>(DateMonthDropdownName);
        m_DayDropdown = panel.Q<DropdownField>(DateDayDropdownName);

        m_ReservationName = m_NameField.value;
        m_NameField.RegisterCallback<ChangeEvent<string>>(evt => 
        {
            m_ReservationName = evt.newValue;
        });

        m_SelectedDate = DateTime.Now.Date;
        m_CurrentYear = m_SelectedDate.Year;

        m_DateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;
        PopulateDate();

        m_YearDropdown.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            SelectDate(m_YearDropdown.index + m_CurrentYear, m_SelectedDate.Month, m_SelectedDate.Day);
        });

        m_MonthDropdown.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            SelectDate(m_SelectedDate.Year, m_MonthDropdown.index + 1, m_SelectedDate.Day);
        });

        m_DayDropdown.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            SelectDate(m_SelectedDate.Year, m_SelectedDate.Month, m_DayDropdown.index + 1);
        });
    }

    private void SelectDate(int year, int month, int day)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var newDay = day <= daysInMonth ? day : daysInMonth;
        m_SelectedDate = new(year, month, newDay);

        PopulateDate();
    }

    private void PopulateDate()
    {
        PopulateYearDropdown();
        PopulateMonthDropdown();
        PopulateDayDropdown();
    }

    private void PopulateYearDropdown()
    {
        var yearList = new List<string>();
        yearList.Add($"{m_CurrentYear}");
        yearList.Add($"{m_CurrentYear + 1}");

        m_YearDropdown.choices = yearList;
        m_YearDropdown.index = m_SelectedDate.Year - m_CurrentYear;
    }

    private void PopulateMonthDropdown()
    {
        var monthList = new List<string>();
        for (int month = 1; month <= 12; month++)
        {
            monthList.Add(m_DateTimeFormat.GetMonthName(month));
        }

        m_MonthDropdown.choices = monthList;
        m_MonthDropdown.index = m_SelectedDate.Month - 1;
    }

    private void PopulateDayDropdown()
    {
        var days = DateTime.DaysInMonth(m_SelectedDate.Year, m_SelectedDate.Month);
        var dayList = new List<string>();
        for (int day = 1; day <= days; day++)
        {
            dayList.Add($"{day}");
        }

        m_DayDropdown.choices = dayList;
        m_DayDropdown.index = m_SelectedDate.Day - 1;
    }
}
