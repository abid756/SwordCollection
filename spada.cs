using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwordCollection
{
    public class Spada
    {

        public string Origine { get; set; }
        public string Materiale { get; set; }
        public double Lunghezza { get; set; }
        public double Peso { get; set; }
        public float Valore { get; set; }

        public string Immagine { get; set; }
        public string Info { get; set; }

        public virtual double CalcolaPotenza()
        {
            double Pot;
            Pot = Lunghezza * Peso;
            return Pot;
        }
        public Spada(string origine, string materiale, double lunghezza, double peso, float valore, string info)
        {

            this.Origine = origine;
            this.Materiale = materiale;
            this.Lunghezza = lunghezza;
            this.Peso = peso;
            this.Valore = valore;
            this.Info = info; // Assegna il parametro alla pro
        }
    }
    public class Katana : Spada
    {
        public string TecnicaForgiatura { get; set; }
        public Katana(string origine, string materiale, double lunghezza, double peso, float valore, string info, string tecnicaForgiatura)
       :
        base(origine, materiale, lunghezza, peso, valore, info)
        {
            TecnicaForgiatura = tecnicaForgiatura;
        }

        public override double CalcolaPotenza()
        {
            return base.CalcolaPotenza() + 20;
        }
    }
}


