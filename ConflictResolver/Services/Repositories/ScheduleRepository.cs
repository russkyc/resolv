// MIT License
// 
// Copyright (c) 2024 John Russell C. Camo (Russkyc)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Net.Http.Json;
using ConflictResolver.Models;

namespace ConflictResolver.Services.Repositories;

public class ScheduleRepository
{
    private readonly HttpClient _httpClient;

    public ScheduleRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CourseSchedule>> GetSchedulesAsync(IEnumerable<string>? blocks = null,
        IEnumerable<string>? courses = null, string? day = null,
        bool conflicts = false)
    {
        var schedules = await _httpClient.GetFromJsonAsync<List<CourseSchedule>>("data/class_schedules.json");

        if (blocks != null)
        {
            schedules = schedules?
                .Where(course => blocks.Any(block => course.Block.Equals(block)))
                .ToList();
        }

        if (courses != null)
        {
            schedules = schedules?
                .Where(course => courses.Any(title => course.CourseTitle.Equals(title)))
                .ToList();
        }

        if (day != null)
        {
            if (!day.Equals("All"))
            {
                schedules = schedules?.Where(course => blocks.Any(block => course.Block.Equals(block))
                                                       && course.Days.Split(",").Any()
                        ? course.Days.Split(",").Any(dayId => dayId.Equals(day switch
                        {
                            "Mon" => "M",
                            "Tue" => "T",
                            "Wed" => "W",
                            "Thu" => "TH",
                            "Fri" => "F",
                            "Sat" => "S"
                        }, StringComparison.OrdinalIgnoreCase))
                        : course.Days.Equals(day switch
                        {
                            "Mon" => "M",
                            "Tue" => "T",
                            "Wed" => "W",
                            "Thu" => "TH",
                            "Fri" => "F",
                            "Sat" => "S"
                        }, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        if (conflicts)
        {
            var referenceSchedules = schedules;
            foreach (var courseSchedule in referenceSchedules)
            {
                var conflictSchedules = schedules.Where(schedule => courseSchedule
                                                                        .Days
                                                                        .Split(",")
                                                                        .Any(scheduleDay =>
                                                                            schedule
                                                                                .Days
                                                                                .Split(",")
                                                                                .Any(referenceDay =>
                                                                                    referenceDay.Equals(scheduleDay)))
                                                                    && courseSchedule.Start >= schedule.Start
                                                                    && courseSchedule.Start < schedule.End
                                                                    && !courseSchedule.Course.Equals(schedule.Course));
                if (conflictSchedules.Any())
                {
                    foreach (var schedule in schedules)
                    {
                        if (schedule.Course.Equals(courseSchedule.Course))
                        {
                            schedule.Conflicts = true;
                            schedule.CourseConflicts = conflictSchedules.Select(schedule => schedule.Course);
                        }
                    }
                }
            }
        }

        return schedules;
    }
}