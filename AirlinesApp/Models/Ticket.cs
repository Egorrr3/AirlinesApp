using System;
using System.Collections.Generic;

namespace AirlinesApp.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int FlightId { get; set; }

    public int PassengerId { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;
}
