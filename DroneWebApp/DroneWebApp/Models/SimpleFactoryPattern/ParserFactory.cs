﻿using DroneWebApp.Models.SimpleFactoryPattern.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern
{
    public static class ParserFactory
    {
        public static IParser MakeParser(string parseType)
        {
            IParser parser = null;

            if (parseType.Equals(".txt", StringComparison.OrdinalIgnoreCase))
            {
                parser = new TXTParser();
            }
            else if (parseType.Equals(".xyz", StringComparison.OrdinalIgnoreCase))
            {
                parser = new XYZParser();
            }
            else if (parseType.Equals(".tfw", StringComparison.OrdinalIgnoreCase))
            {
                parser = new TFWParser();
            }
            else if (parseType.Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                parser = new CSVParser();
            }
            else if (parseType.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                parser = new PDFParser();
            }
            else if (parseType.Equals(".dat", StringComparison.OrdinalIgnoreCase))
            {
                parser = new DATParser();
            }

            return parser;
        }

    }
}