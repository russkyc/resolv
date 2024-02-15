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

using System.Text.Json.Serialization;
using ConflictResolver.Utils.Json;

namespace ConflictResolver.Models;

public record CourseSchedule
{
    public string Block { get; init; }
    public string Course { get; init; }
    public string CourseTitle { get; init; }
    public string ClassType { get; init; }
    public double Units { get; init; }
    public string Days { get; init; }

    [JsonConverter(typeof(TimeOnlyConverter))]
    public TimeOnly Start { get; init; }

    [JsonConverter(typeof(TimeOnlyConverter))]
    public TimeOnly End { get; init; }

    public string Room { get; init; }
    public string FacultyName { get; init; }
    [JsonIgnore] public bool Conflicts { get; set; }
    [JsonIgnore] public IEnumerable<string>? CourseConflicts { get; set; }
}