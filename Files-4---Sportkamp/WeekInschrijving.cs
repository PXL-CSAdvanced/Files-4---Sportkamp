using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files_4___Sportkamp
{
    public class WeekInschrijving
    {
        public int Aantal { get; set; }
        public double Bedrag { get; set; }

        public WeekInschrijving() : this(0, 0.0)
        {
        }

        public WeekInschrijving(int aantal, double bedrag)
        {
            Aantal = aantal;
            Bedrag = bedrag;
        }

        public static WeekInschrijving[] WeekOverzicht(int aantal)
        {
            WeekInschrijving[] weekOverzicht = new WeekInschrijving[aantal];
            for (int i = 0; i < weekOverzicht.Length; i++)
            {
                weekOverzicht[i] = new WeekInschrijving();
            }
            return weekOverzicht;
        }
    }
}
