using System;
using System.Collections.Generic;

namespace AirlinesApp.Models;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();
}
