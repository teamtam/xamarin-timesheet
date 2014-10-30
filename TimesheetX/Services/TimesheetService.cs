﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimesheetX.Models;

namespace TimesheetX.Services
{
    public class TimesheetService
    {
        public static async Task<IEnumerable<TimesheetEntry>> GetTimesheetEntries()
        {
            await Task.Delay(2000); // NOTE: just to simulate a HTTP request over the wire
            List<TimesheetEntry> dates = new List<TimesheetEntry>();
            DateTime current = DateTime.Today;
            for (int i = 0; i < 5; i++)
            {
                if (current.DayOfWeek == DayOfWeek.Saturday)
                    current = current.AddDays(-1);
                else if (current.DayOfWeek == DayOfWeek.Sunday)
                    current = current.AddDays(-2);
                dates.Insert(0, CreateTimesheetEntry(current));
                current = current.AddDays(-1);
            }
            return dates;
        }

        private static TimesheetEntry CreateTimesheetEntry(DateTime date)
        {
            return new TimesheetEntry()
            {
                TimesheetId = new Guid("7faef0a6-f977-493f-95a1-b14a9f90e8dd"),
                Date = date,
                Customer = "Some Customer",
                Project = "Some Project"
            };
        }

        public static async Task SubmitTimesheetEntry(TimesheetEntry timesheet)
        {
            await Task.Delay(2000); // NOTE: just to simulate a HTTP request over the wire
            if (DateTime.Now.Second < 10)
                throw new Exception("Some exception");
            return;
        }
    }
}
