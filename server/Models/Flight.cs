using System;
using System.Collections.Generic;

namespace ServicesGatway.Models;

public partial class Flight
{
    public int FlightId { get; set; }

    public int AirplaneId { get; set; }

    public string DepartureCity { get; set; } = null!;

    public string DepartureAirport { get; set; } = null!;

    public string DepartureCountry { get; set; } = null!;

    public string ArrivalCity { get; set; } = null!;

    public string ArrivalAirport { get; set; } = null!;

    public string ArrivalCountry { get; set; } = null!;

    public DateTime DepartureDateTime { get; set; }

    public DateTime ArrivalDateTime { get; set; }

    public bool? IsSabbathLanding { get; set; }

    public int? AvailableSeats { get; set; }

    // הוספת השדה מחיר
    public decimal Price { get; set; }
}
