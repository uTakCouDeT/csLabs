﻿using System;
using System.Collections.Generic;

namespace lab_10;

public partial class Ticker
{
    public long Id { get; set; }

    public string? Ticker1 { get; set; }

    public virtual ICollection<Price> Prices { get; } = new List<Price>();

    public virtual ICollection<TodaysCondition> TodaysConditions { get; } = new List<TodaysCondition>();
}
