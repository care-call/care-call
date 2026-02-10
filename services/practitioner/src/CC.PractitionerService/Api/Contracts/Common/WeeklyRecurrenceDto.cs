using System.Text.Json.Serialization;

namespace CC.PractitionerService.Api.Contracts.Common;

public sealed record WeeklyRecurrenceDto(
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    DayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime);