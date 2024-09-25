using System;
using System.Collections.Generic;

namespace ServicesGatway.Models;

public partial class Airplane
{
    public int AirplaneId { get; set; }

    public string Manufacturer { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public int YearOfManufacture { get; set; }

    public string? ImageUrl { get; set; }

    public int SeatCount { get; set; }

    //public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
