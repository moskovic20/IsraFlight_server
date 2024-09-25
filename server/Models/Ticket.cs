using System;
using System.Collections.Generic;

namespace ServicesGatway.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int FlightId { get; set; }

    public int CustomerId { get; set; }

    public DateTime PurchaseDate { get; set; }

    //public virtual Customer Customer { get; set; } = null!;

    //public virtual Flight Flight { get; set; } = null!;
}
