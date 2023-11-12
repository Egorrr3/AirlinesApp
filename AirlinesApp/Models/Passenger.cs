using System;
using System.Collections.Generic;

namespace AirlinesApp.Models;

public partial class Passenger
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
