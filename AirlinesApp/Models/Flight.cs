using System;
using System.Collections.Generic;

namespace AirlinesApp.Models;

public partial class Flight
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int AirportFromId { get; set; }

    public int AirportToId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public double Price { get; set; }

    public virtual Airport AirportFrom { get; set; } = null!;

    public virtual Airport AirportTo { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
