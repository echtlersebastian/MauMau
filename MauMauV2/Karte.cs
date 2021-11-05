using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauMauV2
{
    public class Karte
    {
        public enum Farbe
        {
            Eichel,
            Laub,
            Herz,
            Schell
        };
        public enum Typ
        {
            Ass,
            König,
            Ober,
            Unter,
            Zehner,
            Neuner,
            Achter,
            Siebener,
            Sechser
        }

        public Farbe Kartenfarbe;
        public Typ Kartentyp;

        public Karte(Typ typ, Farbe farbe)
        {
            Kartentyp = typ;
            Kartenfarbe = farbe;
        }
    }
}
