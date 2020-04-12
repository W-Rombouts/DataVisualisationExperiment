using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Datapoint
{
    
    public int Year { get; set; }
    public string Quarter { get; set; }
    public string Month { get; set; }
    public int Day { get; set; }
    public string Time { get; set; }
    /// <summary>
    /// Hoeveelheid ijzer toegevoegd aan vat AT1 in liters
    /// </summary>
    public double doseerDebietAT1 { get; set; }
    /// <summary>
    /// Meting fosfaatwaarden in AT1 in mg/l
    /// </summary>
    public double fosfaatMetingAT1 { get; set; }
    /// <summary>
    /// Hoeveelheid ijzer toegevoegd aan vat AT2 in liters
    /// </summary>
    public double doseerDebietAT2 { get; set; }
    /// <summary>
    /// Meting fosfaatwaarden in AT2 in mg/l
    /// </summary>
    public double fosfaatMetingAT2 { get; set; }
    /// <summary>
    /// Hoeveelheid zuiver water gegenereerd in 1000 liters
    /// </summary>
    public double effluentDebiet { get; set; }
    /// <summary>
    /// Stationummer waar de observatie gedaan is,
    /// </summary>
    public double STN { get; set; }
    /// <summary>
    /// Windrichting (in graden) gemiddeld over de laatste 10 minuten van het afgelopen uur (360=noord, 90=oost, 180=zuid, 270=west, 0=windstil 990=veranderlijk.
    /// </summary>
    public double DD { get; set; }
    /// <summary>
    /// Uurgemiddelde windsnelheid (in 0.1 m/s).
    /// </summary>
    public double FH { get; set; }
    /// <summary>
    /// Windsnelheid (in 0.1 m/s) gemiddeld over de laatste 10 minuten van het afgelopen uur
    /// </summary>
    public double FF { get; set; }
    /// <summary>
    /// Hoogste windstoot (in 0.1 m/s) over het afgelopen uurvak
    /// </summary>
    public double FX { get; set; }
    /// <summary>
    /// Temperatuur (in 0.1 graden Celsius) op 1.50 m hoogte tijdens de waarneming
    /// </summary>
    public double T { get; set; }
    /// <summary>
    /// Minimumtemperatuur (in 0.1 graden Celsius) op 10 cm hoogte in de afgelopen 6 uur
    /// </summary>
    public double T10 { get; set; }
    /// <summary>
    /// Dauwpuntstemperatuur (in 0.1 graden Celsius) op 1.50 m hoogte tijdens de waarneming
    /// </summary>
    public double TD { get; set; }
    /// <summary>
    /// Duur van de zonneschijn (in 0.1 uren) per uurvak, berekend uit globale straling  (-1 for <0.05 uur)
    /// </summary>
    public double SQ { get; set; }
    /// <summary>
    /// Globale straling (in J/cm2) per uurvak
    /// </summary>
    public double Q { get; set; }
    /// <summary>
    /// Duur van de neerslag (in 0.1 uur) per uurvak
    /// </summary>
    public double DR { get; set; }
    /// <summary>
    /// Uursom van de neerslag (in 0.1 mm) (-1 voor <0.05 mm)
    /// </summary>
    public double RH { get; set; }
    /// <summary>
    /// Luchtdruk (in 0.1 hPa) herleid naar zeeniveau, tijdens de waarneming
    /// </summary>
    public double P { get; set; }
    /// <summary>
    /// Horizontaal zicht tijdens de waarneming (0=minder dan 100m, 1=100-200m, 2=200-300m,..., 49=4900-5000m, 50=5-6km, 56=6-7km, 57=7-8km, ..., 79=29-30km, 80=30-35km, 81=35-40km,..., 89=meer dan 70km)
    /// </summary>
    public double VV { get; set; }
    /// <summary>
    /// Bewolking (bedekkingsgraad van de bovenlucht in achtsten), tijdens de waarneming (9=bovenlucht onzichtbaar)
    /// </summary>
    public double N { get; set; }
    /// <summary>
    /// Relatieve vochtigheid (in procenten) op 1.50 m hoogte tijdens de waarneming
    /// </summary>
    public double U { get; set; }
    /// <summary>
    /// Weercode (00-99), visueel(WW) of automatisch(WaWa) waargenomen, voor het actuele weer of het weer in het afgelopen uur.
    /// </summary>
    public double WW { get; set; }
    /// <summary>
    /// Weercode indicator voor de wijze van waarnemen op een bemand of automatisch station (1=bemand gebruikmakend van code uit visuele waarnemingen, 2,3=bemand en weggelaten (geen belangrijk weersverschijnsel, geen gegevens), 4=automatisch en opgenomen (gebruikmakend van code uit visuele waarnemingen), 5,6=automatisch en weggelaten (geen belangrijk weersverschijnsel, geen gegevens), 7=automatisch gebruikmakend van code uit automatische waarnemingen)
    /// </summary>
    public double IX { get; set; }
    /// <summary>
    /// Mist 0=niet voorgekomen, 1=wel voorgekomen in het voorgaande uur en/of tijdens de waarneming
    /// </summary>
    public double M { get; set; }
    /// <summary>
    /// Regen 0=niet voorgekomen, 1=wel voorgekomen in het voorgaande uur en/of tijdens de waarneming
    /// </summary>
    public double R { get; set; }
    /// <summary>
    /// Sneeuw 0=niet voorgekomen, 1=wel voorgekomen in het voorgaande uur en/of tijdens de waarneming
    /// </summary>
    public double S { get; set; }
    /// <summary>
    /// Onweer 0=niet voorgekomen, 1=wel voorgekomen in het voorgaande uur en/of tijdens de waarneming
    /// </summary>
    public double O { get; set; }
    /// <summary>
    /// IJsvorming 0=niet voorgekomen, 1=wel voorgekomen in het voorgaande uur en/of tijdens de waarneming
    /// </summary>
    public double Y { get; set; }
}

