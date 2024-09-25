using System;
using System.Collections.Generic;

namespace ServicesGatway.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool IsManager { get; set; } = false;

    // public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
