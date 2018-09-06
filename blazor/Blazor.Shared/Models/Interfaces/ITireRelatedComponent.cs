﻿using System;
using System.Collections.Generic;
using System.Text;

using KenticoCloud.Delivery;

namespace Blazor.Shared.Models
{
    public interface ITireRelatedComponent
    {
        decimal? MinimumTireThicknessInches { get; set; }
        decimal? MaximumTireThicknessInches { get; set; }
    }
}
