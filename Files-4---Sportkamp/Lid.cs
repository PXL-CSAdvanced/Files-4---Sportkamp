using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Files_4___Sportkamp
{
    public class Lid
    {
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Sportcode { get; set; }
        public int Weeknr { get; set; }
        public int KampVolgnr { get; set; }

        // Elk Lid hoeft niet apart een sportTabel of weekOverzicht in zich te hebben.
        // Eentje is genoeg, dus daarom maken we die static.
        // Namelijk 1 voor deze class Lid en niet per Lid object.
        private static Dictionary<string, Sport> _sportTabel = LeesSportTabel(@"..\..\..\Bestanden\Sporten.txt");
        private static WeekInschrijving[] _weekOverzicht = WeekInschrijving.WeekOverzicht(9);

        public string NaamVolledig => $"{Naam} {Voornaam}";
        public string SportNaam => _sportTabel[Sportcode].Omschrijving;
        public double KampPrijs => _sportTabel[Sportcode].Prijs;
        public double TeBetalen
        {
            get
            {
                double prijs = KampPrijs;
                double percentage = 1.0;
                if (KampVolgnr >= 5)
                    percentage = 0.9; // 10% korting, dus maar 90% betalen
                else if (KampVolgnr >= 2)
                    percentage = 0.95; // 5% korting, dus maar 95% betalen

                return prijs * percentage;
            }
        }

        public string InformatieVolledig()
            => $"{NaamVolledig,-35}{SportNaam,-15}{KampPrijs:c} - {KampVolgnr} - {TeBetalen:c}";
    
        private static Dictionary<string, Sport> LeesSportTabel(string bestand)
        {
            Dictionary<string, Sport> tabel = new Dictionary<string, Sport>();
            FileInfo fi = new FileInfo(bestand);
            if (!fi.Exists)
            {
                throw new FileNotFoundException();
            }
            using (StreamReader sr = fi.OpenText())
            {
                while (!sr.EndOfStream)
                {
                    string[] velden = sr.ReadLine().Split(';');
                    Sport sport = new Sport()
                    {
                        Code = velden[0].Replace("\"", ""),
                        Omschrijving = velden[1].Replace("\"", ""),
                        Prijs = double.Parse(velden[2])
                    };
                    tabel[sport.Code] = sport;
                }
            }
            return tabel;
        }

        public static List<Lid> LeesLeden(string bestand)
        {
            FileInfo fi = new FileInfo(bestand);
            if (!fi.Exists)
            {
                throw new FileNotFoundException();
            }
            List<Lid> leden = new List<Lid>();
            using (StreamReader sr = fi.OpenText())
            {
                while (!sr.EndOfStream)
                {
                    string lijn = sr.ReadLine();
                    string volledigeCode = lijn.Substring(60, 5);
                    Lid lid = new Lid()
                    {
                        Naam = lijn.Substring(0, 30).Trim(),
                        Voornaam = lijn.Substring(30, 30).Trim(),
                        Weeknr = (int)char.GetNumericValue(volledigeCode[0]),
                        Sportcode = volledigeCode.Substring(1, 3),
                        KampVolgnr = (int)char.GetNumericValue(volledigeCode[4])
                    };
                    leden.Add(lid);

                    // Update onze sporttabel na toevoeging van nieuw lid
                    _sportTabel[lid.Sportcode].Aantal++;
                    _sportTabel[lid.Sportcode].Totaalprijs += lid.TeBetalen;

                    // Update ons weekoverzicht na toevoeging van nieuw lid
                    int weekIndex = lid.Weeknr - 1;
                    _weekOverzicht[weekIndex].Aantal++;
                    _weekOverzicht[weekIndex].Bedrag += lid.TeBetalen;
                }
            }
            return leden;
        }

        public static string ToonLeden(List<Lid> leden)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Details van alle leden.\r\n");
            foreach (var lid in leden)
            {
                sb.AppendLine(lid.InformatieVolledig());
            }
            return sb.ToString();
        }

        public static string ToonWeekOverzicht()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Overzicht per week\r\n");
            for (int i = 0; i < _weekOverzicht.Length; i++)
            {
                sb.AppendLine($"Week {i + 1} - {_weekOverzicht[i].Aantal, 3} inschrijvingen: {_weekOverzicht[i].Bedrag, 20:c}");
            }

            return sb.ToString();
        }

        public static string ToonSporttakOverzicht()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Overzicht per Sporttak\r\n");
            foreach (var item in _sportTabel)
            {
                Sport sport = item.Value;
                sb.AppendLine($"{sport.Omschrijving, -12} - {sport.Aantal, 3} inschrijvingen: {sport.Totaalprijs, 12:c}");
            }
            return sb.ToString();
        }
    }
}
