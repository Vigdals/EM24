using System;
using System.Collections.Generic;

public static class Euro2024Teams
{
    private static readonly Dictionary<string, string> lag = new Dictionary<string, string>
    {
        { "Tyskland", "TYS" },
        { "Skottland", "SKO" },
        { "Ungarn", "UNG" },
        { "Sveits", "SVE" },
        { "Spania", "SPA" },
        { "Italia", "ITA" },
        { "Kroatia", "KRO" },
        { "Albania", "ALB" },
        { "England", "ENG" },
        { "Danmark", "DAN" },
        { "Serbia", "SER" },
        { "Slovenia", "SLO" },
        { "Frankrike", "FRA" },
        { "Nederland", "NED" },
        { "Polen", "POL" },
        { "Østerrike", "ØST" },
        { "Belgia", "BEL" },
        { "Portugal", "POR" },
        { "Romania", "ROM" },
        { "Slovakia", "SVK" },
        { "Tyrkia", "TYR" },
        { "Tsjekkia", "TSJ" },
        { "Ukraina", "UKR" },
        { "Georgia", "GEO" }
    };

    public static string HentForkortelse(string lagNavn)
    {
        if (lag.TryGetValue(lagNavn, out string forkortelse))
        {
            return forkortelse;
        }
        else
        {
            return lagNavn;
        }
    }
}