﻿using System;

namespace Sverto.General.Logging
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Info = 1,
        Debug = 2,
        Warning = 4,
        Error = 8
    }
}
