﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MicaForEveryone.Interfaces
{
    public interface IStartupService
    {
        void SetEnabled(bool state);
        bool GetEnabled();
    }
}
