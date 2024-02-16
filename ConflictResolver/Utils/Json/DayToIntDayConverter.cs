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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConflictResolver.Utils.Json;

public class DayToIntDayConverter : JsonConverter<IEnumerable<int>>
{
    public override IEnumerable<int>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = (string)reader.GetString();

        if (str.Contains(","))
        {
            return str.Split(",").Select(day => day switch
            {
                "M" => 1,
                "T" => 2,
                "W" => 3,
                "TH" => 4,
                "F" => 5,
                "S" => 6
            });
        }

        return new int[]
        {
            str switch
            {
                "M" => 1,
                "T" => 2,
                "W" => 3,
                "TH" => 4,
                "F" => 5,
                "S" => 6
            }
        };
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<int> value, JsonSerializerOptions options)
    {
        var values = value.Select(day => day switch
        {
            1 => "M",
            2 => "T",
            3 => "W",
            4 => "TH",
            5 => "F",
            6 => "S"
        });
        var val = string.Join(",", values);
        writer.WriteStringValue(val);
    }
}