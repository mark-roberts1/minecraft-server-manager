﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class ToggleUserLockResponse
    {
        public bool IsUserLocked { get; set; }
    }
}