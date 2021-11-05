using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MauMauV2
{
    class Program
    {
        public static int anzahlSpieler = 3;
        public static int anzahlHandkarten = 5;


        public static Spieler[] spieler = new Spieler[anzahlSpieler];
        public static KartenDeck kartenDeck = new KartenDeck();


        public static List<Spieler> Rangliste = new List<Spieler>();


        static void Main(string[] args)
        {

            //hier passiert alle magie
            Console.WriteLine("Das Spiel startet jetzt, schnallt euch alle an!" + Environment.NewLine);

            spielerInitializieren();
            kartenAusteilen();
            spielerAnkündigen();

            spielen();

        }

        private static void spielen()
        {
            int aktuellerSpieler = 1; //bei eins starten, weil der 0te spieler eine zufällige karte legt.
            Karte aktuellGelegteKarte = spieler[0].startKarteLegen(); //der erste Spieler muss jetzt erstmal die Erste Karten legen.
            bool spielVorbei = false;
            bool einerHatnochNeKArte = false;
            bool einZweiterHatNochNeKArte = false;
            bool hatJemandAKarteGlegt = true;

            while (!spielVorbei)
            {
                if (!hatJemandAKarteGlegt)
                {
                    showRanking();
                    return;

                }


                hatJemandAKarteGlegt = false;
                for (int i = aktuellerSpieler; i < anzahlSpieler; i++)
                {
                    if (spieler[i].hatHandkarten())
                    {
                        if (!einerHatnochNeKArte)
                        {
                            einerHatnochNeKArte = true;
                        }
                        else if (!einZweiterHatNochNeKArte)
                        {
                            einZweiterHatNochNeKArte = true;
                        }

                        Karte karteDieErLegenWill;
                        karteDieErLegenWill = spieler[i].legeKarte(aktuellGelegteKarte);

                        if (karteDieErLegenWill == null)
                        {
                            spielerMussStrafkarteZiehen(spieler[i]);

                            if(kartenDeck.deck.Count != 0)
                            {
                                hatJemandAKarteGlegt = true;
                            }
                        }
                        else if (spieler[i].hatHandkarten() == false) //wenn er vorhin noch ne karte hatte, aber jetzt nimmer, dann hat er wohl gewonnen.
                        {
                            Rangliste.Add(spieler[i]);
                        }
                        else
                        {
                            WriteColor($"[{spieler[i].Name}] hat die [{karteDieErLegenWill.Kartenfarbe} {karteDieErLegenWill.Kartentyp}] gelegt.", ConsoleColor.Yellow);
                            aktuellGelegteKarte = karteDieErLegenWill;
                            hatJemandAKarteGlegt = true;
                        }

                    }


                }
                aktuellerSpieler = 0;
                if (!einerHatnochNeKArte && !einZweiterHatNochNeKArte)
                {
                    spielVorbei = true;
                };

            }

        }

        private static void showRanking()
        {
            for (int i = 0; i < spieler.Count(); i++)
            {
                if (!Rangliste.Contains(spieler[i]))
                {
                    Rangliste.Add(spieler[i]);
                }
            }
            Console.WriteLine(Environment.NewLine);
            int rang = 1;
            foreach (Spieler s in Rangliste)
            {
                if(rang == 1)
                {
                    WriteColor($"[{s.Name}] hat den [ERSTEN] Platz erreicht! Herzlichen Glückwunsch von der Regie.", ConsoleColor.Magenta);

                }else if(rang == Rangliste.Count){
                    WriteColor($"[{s.Name}] hat den [LETZTEN] Platz erreicht, der [IDIOT]! Herzlichen Glückwunsch von der Regie.", ConsoleColor.Magenta);

                }
                else
                {

                WriteColor($"[{s.Name}] hat den {rang} erreicht. ", ConsoleColor.DarkMagenta);
                }
                rang++;
            }


        }

        public static void spielerMussStrafkarteZiehen(Spieler s)
        {
            WriteColor($"[{s.Name}] Muss eine Strafkarte ziehen! Das führt zu noch mehr [{s.Stimmung}]", ConsoleColor.Red);
            if (kartenDeck.deck.Count > 0)
            {
                s.Handkarten.Add(kartenDeck.deck.First());
                kartenDeck.deck.Remove(kartenDeck.deck.First());
            }
            else
            {
                WriteColor($"Es gibt keine Strafkarten mehr! Da hat [{s.Name}] aber Glück gehabt!", ConsoleColor.Green);
            }

        }
        private static void spielerAnkündigen()
        {
            Console.WriteLine($"Die Karten sind jetzt ausgeteilt, heute spielen mit uns die folgenden {anzahlSpieler} Legenden:" + Environment.NewLine);

            int currentShoutout = 0;
            foreach (Spieler s in spieler)
            {
                WriteColor($"Legende Nummer {currentShoutout + 1}: [{s.Name}], heute ganz besonders [{s.Stimmung}]!", ConsoleColor.Green);

                currentShoutout++;
            };
            WriteColor(Environment.NewLine + $"Da jetzt alles klar ist gehts auch direkt los! [{spieler[0].Name}] darf anfangen!", ConsoleColor.Green);
        }

        private static void spielerInitializieren()
        {
            Random rnd = new Random();
            for (int i = 0; i < anzahlSpieler; i++)
            {
                spieler[i] = new Spieler();
            }
        }

        private static void kartenAusteilen()
        {
            int i = 0;
            List<Karte> kartenDieAusgeteiltWurden = new List<Karte>();
            foreach (Karte k in kartenDeck.deck)
            {
                //jetzt teile ich einfach der reihe nach die karten aus.
                if (spieler[i].Handkarten.Count < anzahlHandkarten) //gib dem dude nur so viele bis er die gewünschte anzahl an karten hat.
                {
                    spieler[i].Handkarten.Add(k);
                    kartenDieAusgeteiltWurden.Add(k);
                    i++;
                    if (i >= anzahlSpieler) //damit man dann quasi wieder beim ersten spieler anfängt, wenn man dem letzten ne karte gegeben hat.
                    {
                        i = i - anzahlSpieler;
                    }
                }

            }
            //jetzt müsste jeder spieler seine max.anzahl an karten haben, dann löschen wir die ausgeteilten karten jetzt aus dem array raus
            foreach (Karte k in kartenDieAusgeteiltWurden)
            {
                kartenDeck.deck.Remove(k);
            }
        }

        static void WriteColor(string message, ConsoleColor color)
        {
            var pieces = Regex.Split(message, @"(\[[^\]]*\])");

            for (int i = 0; i < pieces.Length; i++)
            {
                string piece = pieces[i];

                if (piece.StartsWith("[") && piece.EndsWith("]"))
                {
                    Console.ForegroundColor = color;
                    piece = piece.Substring(1, piece.Length - 2);
                }

                Console.Write(piece);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}
