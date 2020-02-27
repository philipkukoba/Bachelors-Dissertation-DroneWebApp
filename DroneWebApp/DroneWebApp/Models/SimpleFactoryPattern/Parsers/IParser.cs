﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public interface IParser
    {
        void Parse(string path, string date_and_location);
    }
}