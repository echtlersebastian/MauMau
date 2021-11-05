using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauMauV2
{
    public class KartenDeck
    {
        public List<Karte> deck = new List<Karte>();


        public KartenDeck()
        {
            initDeck();

        }

        private void initDeck()
        {
            int i = 0;
            foreach (Karte.Farbe farbe in Enum.GetValues(typeof(Karte.Farbe)))
            {
                foreach (Karte.Typ typ in Enum.GetValues(typeof(Karte.Typ)))
                {
                    deck.Add(new Karte(typ, farbe));
                    i++;
                }
            }

            //einmal gescheid mischeln
            Random rnd = new Random();  //System.Random ist nicht "thread save" also bitte nur zum spaß benutzen und nicht irgendwo produktiv...
            var gemischeltesDeck = deck.OrderBy(a => Guid.NewGuid()).ToList();
            deck = gemischeltesDeck; //darf man nicht gleichzeitig machen, weil System.Random nicht so ganz super ist... System.Cryptography (also der RNGCryptoServiceProvider) ist da viel besser. Fragt google, wenn ihr da mehr wissen wollt :)





        }
    }
}

