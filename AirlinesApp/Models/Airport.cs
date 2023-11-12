using System;
using System.Collections.Generic;

namespace AirlinesApp.Models;

public partial class Airport
{
    public int Id { get; set; }

    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Flight> FlightAirportFroms { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightAirportTos { get; set; } = new List<Flight>();
}
