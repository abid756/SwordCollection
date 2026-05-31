using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwordCollection
{


    public class CollezioneSpade
    {
        public List<Spada> Spade = new List<Spada>();
        public bool datiNonSalvati = false;
        public string FSpade = "FileSpade.json";

        public CollezioneSpade()
        {
            Spade = new List<Spada>();
        }

        public void AggiungiSpada(string info, string origine, string materiale, double lunghezza, double peso, float valore)
        {

            Spada s = new Spada(origine, materiale, lunghezza, peso, valore, info);
            
                Spade.Add(s);
            datiNonSalvati = true;
        }

        public void AggiungiKatana(string info, string origine, string materiale, double lunghezza, double peso, float valore, string tecnica)
        {

            Katana s = new Katana(origine, materiale, lunghezza, peso, valore, info, tecnica);
            Spade.Add(s);
            datiNonSalvati = true;
        }
        public void RimuoviSpada(Spada s)
        {
            Spade.Remove(s);
            datiNonSalvati = true;
        }

        public string Convert2json()
        {
            return JsonSerializer.Serialize(Spade);
        }
        public void Salva()
        {

            string jsonString = Convert2json();
            System.IO.File.WriteAllText(FSpade, jsonString);
            datiNonSalvati = false;
        }

        public void Carica()
        {
            // todo aggiungere try catch e generare eventuali eccezioni
            // eliminare message box
            if (!System.IO.File.Exists(FSpade))
            {
                MessageBox.Show("File non trovato");
                return;
            }

            string jsonString = System.IO.File.ReadAllText(FSpade);

            Spade = JsonSerializer.Deserialize<List<Spada>>(jsonString);
            datiNonSalvati = false;
        }
        public void ModificaSpada(Spada spadaDaModificare, string nuovoInfo, string nuovaOrigine, string nuovoMateriale, double nuovaLunghezza, double nuovoPeso , float nuovovalore)
        {
            spadaDaModificare.Info = nuovoInfo;
            spadaDaModificare.Origine = nuovaOrigine;
            spadaDaModificare.Materiale = nuovoMateriale;
            spadaDaModificare.Lunghezza = nuovaLunghezza;
            spadaDaModificare.Peso = nuovoPeso;
        }


    }
}


