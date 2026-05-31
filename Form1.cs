using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Reflection;

namespace SwordCollection
{
    public partial class Form1 : Form
    {
        // Campi della classe
        private Panel sidebarInserimento;
        private FlowLayoutPanel pannelloVisualizzazione;
        private RadioButton rbSpada, rbKatana;
        private TextBox txtOrigine, txtMateriale, txtInfo, txtTecnica;
        private NumericUpDown numLunghezza, numPeso, numValore;
        private Button btnAggiungi, btnSalvaFile, btnCaricaFile, btnElimina, btnModifica;
        private Label lblTecnica;
        // Dichiarazione Card
        private Panel card;
        private Panel cardSelezionata = null;

        // oggetto contenitore
        public CollezioneSpade Spade;
        public Form1()
        {
            
            ConfiguraEsteticaForm();
            InizializzaComponentiGrafici();
            Spade = new CollezioneSpade();
            Spade.datiNonSalvati = false;
            this.FormClosing += Form1_FormClosing;
            Spade.Carica();
            AggiornaVisualizzazione();
        }
        private void AggiornaVisualizzazione()
        {
            pannelloVisualizzazione.Controls.Clear();
            foreach (var spada in Spade.Spade)
            {
                Panel card = CreaCardSpada(spada);
                pannelloVisualizzazione.Controls.Add(card);
            }
        }
        private Panel CreaCardSpada(Spada spada)
        {
            Panel card = new Panel
            {
                Width = 250,
                Height = 200,
                BackColor = Color.Gray,
                Margin = new Padding(10)
            };
            card.Tag = spada;
           double potenza = spada.CalcolaPotenza();

            // Crea una stringa con tutte le info principali
            string dettagli =
                             $"Origine: {spada.Origine}\n" +
                             $"Materiale: {spada.Materiale}\n" +
                             $"Lunghezza: {spada.Lunghezza} cm\n" +
                             $"Peso: {spada.Peso} kg\n" +
                             $"Potenza: {potenza} \n" +
                             $"Valore: {spada.Valore} €\n" +
                             $"Info: {spada.Info}\n";

            // Se la spada è una Katana, aggiungi la tecnica
            if (spada is Katana katana)
                dettagli += $"\nTecnica: {katana.TecnicaForgiatura}";

            Label lbl = new Label
            {
                Text = dettagli,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                AutoSize = false
            };
            card.Controls.Add(lbl);

            card.Click += Card_Click;
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += Card_Click; // Propaga il click anche alle label
            }
            return card;
        }

        private void ConfiguraEsteticaForm()
        {
            this.Text = "Sword Collection";
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.MinimumSize = new Size(900, 600);
            this.MaximumSize = new Size(900, 600); // Imposta anche la dimensione massima
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Bordo fisso
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void InizializzaComponentiGrafici()
        {
            // --- SIDEBAR (FlowLayoutPanel verticale) ---
            sidebarInserimento = new Panel
            {
                Dock = DockStyle.Left,
                Width = 300,
                BackColor = Color.FromArgb(45, 45, 45),
                Padding = new Padding(20),

            };


            Splitter splitterRicerca = new Splitter
            {
                Dock = DockStyle.Right,
                Width = 4,
                BackColor = Color.Goldenrod
            };


            // Titolo
            Label titolo = new Label
            {
                Text = "SWORD COLLECTION",
                ForeColor = Color.Goldenrod,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Height = 30,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Width = 260
            };

            sidebarInserimento.Controls.Add(titolo);

            // Selettore tipo spada (GroupBox con RadioButton e campo Tecnica)
            AggiungiSelettoreTipo();

            int yStart = 180;
            int campoHeight = 40;
            // Campi di input
            txtOrigine = AggiungiCampoTesto("Origine:", yStart);
            txtMateriale = AggiungiCampoTesto("Materiale:", yStart + campoHeight * 1);
            numLunghezza = AggiungiCampoNumerico("Lunghezza (cm):", yStart + campoHeight * 2);
            numPeso = AggiungiCampoNumerico("Peso (kg):", yStart + campoHeight * 3);
            numValore = AggiungiCampoNumerico("Valore(€)", yStart + campoHeight * 4);
            txtInfo = AggiungiCampoTesto("Info:", yStart + campoHeight * 5);



            // Pulsanti
            btnAggiungi = new Button
            {
                Text = "AGGIUNGI SPADA",
                Height = 40,
                BackColor = Color.Goldenrod,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 20, 0, 0),
                Width = 250,
                Location = new Point(20, 420)
            };
            sidebarInserimento.Controls.Add(btnAggiungi);

            // Primo bottone a sinistra, prima riga
            btnElimina = new Button
            {
                Text = "Elimina",
                Height = 30,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Width = 120,
                Location = new Point(20, 470)
            };
            sidebarInserimento.Controls.Add(btnElimina);

            // Secondo bottone a destra, prima riga
            btnModifica = new Button
            {
                Text = "Modifica",
                Height = 30,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Width = 120,
                Location = new Point(150, 470)
            };
            sidebarInserimento.Controls.Add(btnModifica);

            // Terzo bottone a sinistra, seconda riga
            btnSalvaFile = new Button
            {
                Text = "Salva su Disco",
                Height = 30,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Width = 120,
                Location = new Point(20, 500)
            };
            sidebarInserimento.Controls.Add(btnSalvaFile);

            // Quarto bottone a destra, seconda riga
            btnCaricaFile = new Button
            {
                Text = "Carica da Disco",
                Height = 30,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Width = 120,
                Location = new Point(150, 500)
            };
            sidebarInserimento.Controls.Add(btnCaricaFile);

            sidebarInserimento.Controls.Add(btnCaricaFile);

            this.Controls.Add(sidebarInserimento);





            // --- AREA CARD ---
            pannelloVisualizzazione = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoScroll = true,
                BackColor = Color.FromArgb(20, 20, 20),
                Height = 500,
                Width = 600,
                Padding = new Padding(15),
                WrapContents = true
            };

            btnAggiungi.Click += BtnAggiungi_Click;
            btnElimina.Click += BtnElimina_Click;
            btnModifica.Click += BtnModifica_Click;
            btnSalvaFile.Click += BtnSalvaFile_Click;
            btnCaricaFile.Click += BtnCaricaFile_Click;
            this.Controls.Add(pannelloVisualizzazione);
        }

        private void AggiungiSelettoreTipo()
        {
            GroupBox gbTipo = new GroupBox
            {
                Text = "Tipo di Spada",
                ForeColor = Color.Goldenrod,
                Width = 260,
                Height = 100,
                Location = new Point(20, 70)
            };

            rbSpada = new RadioButton
            {
                Text = "Spada Comune",
                Checked = true,
                Location = new Point(10, 20),
                ForeColor = Color.White,
                AutoSize = true
            };
            rbKatana = new RadioButton
            {
                Text = "Katana",
                Location = new Point(10, 45),
                ForeColor = Color.White,
                AutoSize = true
            };

            lblTecnica = new Label
            {
                Text = "Tecnica Forgiatura:",
                ForeColor = Color.LightGray,
                Location = new Point(10, 70),
                Visible = false,
                AutoSize = true
            };
            txtTecnica = new TextBox
            {
                Location = new Point(120, 70),
                Width = 100,
                Visible = false
            };

            rbKatana.CheckedChanged += (s, e) =>
            {
                lblTecnica.Visible = txtTecnica.Visible = rbKatana.Checked;
            };

            gbTipo.Controls.Add(rbSpada);
            gbTipo.Controls.Add(rbKatana);
            gbTipo.Controls.Add(lblTecnica);
            gbTipo.Controls.Add(txtTecnica);

            sidebarInserimento.Controls.Add(gbTipo);
            gbTipo.Resize += (s, e) =>
            {
                int w = gbTipo.ClientSize.Width - 20;
                rbSpada.Width = w;
                rbKatana.Width = w;
                lblTecnica.Left = 10;
                txtTecnica.Left = lblTecnica.Right + 5;
                txtTecnica.Width = w - txtTecnica.Left - 10;
            };
        }

        // Helper per aggiungere un campo testo
        private TextBox AggiungiCampoTesto(string etichetta, int y)
        {
            Label lbl = new Label
            {
                Text = etichetta,
                ForeColor = Color.LightGray,
                Height = 15,
                Width = 260,
                Location = new Point(20, y)
            };
            TextBox tb = new TextBox
            {
                Width = 250,
                BackColor = Color.White,
                Location = new Point(20, y + 15)
            };
            sidebarInserimento.Controls.Add(lbl);
            sidebarInserimento.Controls.Add(tb);
            return tb;
        }

        // Helper per aggiungere un campo numerico
        private NumericUpDown AggiungiCampoNumerico(string etichetta, int y)
        {
            Label lbl = new Label
            {
                Text = etichetta,
                ForeColor = Color.LightGray,
                Height = 15,
                Width = 260,
                Location = new Point(20, y)

            };
            NumericUpDown n = new NumericUpDown
            {
                DecimalPlaces = 2,
                Maximum = 1000000000000000,
                Width = 250,
                Location = new Point(20, y + 15)
            };
            sidebarInserimento.Controls.Add(lbl);
            sidebarInserimento.Controls.Add(n);
            return n;

        }
        //eventi Card
        private void Card_Click(object sender, EventArgs e)
        {
            // Trova il pannello card
            Panel clickedCard = sender as Panel ?? ((Control)sender).Parent as Panel;
            if (cardSelezionata != null)
            {
                cardSelezionata.BackColor = Color.Gray; // Deseleziona la precedente
            }
            cardSelezionata = clickedCard;
            cardSelezionata.BackColor = Color.DarkGoldenrod; // Evidenzia la selezione

            // Riempi i campi di input con i dati della spada selezionata
            var spada = cardSelezionata.Tag as Spada;
            if (spada != null)
            {
                txtInfo.Text = spada.Info;
                txtOrigine.Text = spada.Origine;
                txtMateriale.Text = spada.Materiale;
                numLunghezza.Value = (decimal)spada.Lunghezza;
                numPeso.Value = (decimal)spada.Peso;
                numValore.Value = (decimal)spada.Valore;

                if (spada is Katana katana)
                {
                    rbKatana.Checked = true;
                    txtTecnica.Text = katana.TecnicaForgiatura;
                }
                else
                {
                    rbSpada.Checked = true;
                    txtTecnica.Text = "";
                }
            }
        }


        // Eventi pulsanti
        private void BtnAggiungi_Click(object sender, EventArgs e)
        {
            // Recupera i dati dai campi
            string info = txtInfo.Text;
            string origine = txtOrigine.Text;
            string materiale = txtMateriale.Text;
            double lunghezza = Convert.ToDouble(numLunghezza.Value);
            double peso = Convert.ToDouble(numPeso.Value);
            float valore = Convert.ToSingle(numValore.Value);
            string tecnica = rbKatana.Checked ? txtTecnica.Text : null;

           
            
            if (cardSelezionata != null)
            {
                MessageBox.Show("non puoi aggiungere le spade se ci sono spade selezionate .");
                return;
            }


            if (string.IsNullOrWhiteSpace(info) || string.IsNullOrWhiteSpace(origine) || string.IsNullOrWhiteSpace(materiale))
            {
                MessageBox.Show("tutti i campi sono obbligatori.");
                return;
            }

            if (lunghezza <= 0 || peso <= 0 || valore < 0)
            {
                MessageBox.Show("Lunghezza, peso e valore devono essere maggiori di zero.");
                return;
            }

            if (rbKatana.Checked)
            {
                Spade.AggiungiKatana(info, origine, materiale, lunghezza, peso, valore, tecnica);
            }
            else
            {
                Spade.AggiungiSpada(info, origine, materiale, lunghezza, peso, valore);
            }

            AggiornaVisualizzazione();



            // Pulisci i campi
            txtInfo.Text = "";
            txtOrigine.Text = "";
            txtMateriale.Text = "";
            numLunghezza.Value = 0;
            numPeso.Value = 0;
            numValore.Value = 0;
            txtTecnica.Text = "";
        }


        private void BtnElimina_Click(object sender, EventArgs e)
        {
            if (cardSelezionata == null)
            {
                MessageBox.Show("Seleziona una spada da eliminare.");
                return;
            }
            else
            {
                var spada = cardSelezionata.Tag as Spada;
                if (MessageBox.Show("sei sicuro?", "Conferma cancellazione", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Spade.RimuoviSpada(spada);
                    AggiornaVisualizzazione();
                }
            }
            cardSelezionata = null;
           
            txtInfo.Text = "";
            txtOrigine.Text = "";
            txtMateriale.Text = "";
            numLunghezza.Value = 0;
            numPeso.Value = 0;
            numValore.Value = 0;
            txtTecnica.Text = "";
            rbSpada.Checked = true;

        }

        private void BtnModifica_Click(object sender, EventArgs e)
        {
            if (cardSelezionata == null)
            {
                MessageBox.Show("Seleziona una spada da modificare.");
                return;
            }

            var spada = cardSelezionata.Tag as Spada;
           

            // Leggi valori dai campi
            string info = txtInfo.Text.Trim();
            string origine = txtOrigine.Text.Trim();
            string materiale = txtMateriale.Text.Trim();
            double lunghezza = Convert.ToDouble(numLunghezza.Value);
            double peso = Convert.ToDouble(numPeso.Value);
            float valore = Convert.ToSingle(numValore.Value);
            string tecnica = rbKatana.Checked ? txtTecnica.Text.Trim() : null;

            // Validazione
            if (string.IsNullOrWhiteSpace(info) || string.IsNullOrWhiteSpace(origine) || string.IsNullOrWhiteSpace(materiale))
            {
                MessageBox.Show("Tutti i campi sono obbligatori.");
                return;
            }
            if (lunghezza <= 0 || peso <= 0 || valore < 0)
            {
                MessageBox.Show("Lunghezza, peso e valore devono essere maggiori di zero.");
                return;
            }

            bool richiestaKatana = rbKatana.Checked;
            bool originaleEraKatana = spada is Katana;

            if (richiestaKatana && !originaleEraKatana)
            {
                // Trasforma Spada -> Katana: aggiungi nuova Katana e rimuovi la vecchia
                Spade.AggiungiKatana(info, origine, materiale, lunghezza, peso, valore, tecnica);
                Spade.RimuoviSpada(spada);
            }
            else if (!richiestaKatana && originaleEraKatana)
            {
                // Trasforma Katana -> Spada: aggiungi nuova Spada e rimuovi la vecchia
                Spade.AggiungiSpada(info, origine, materiale, lunghezza, peso, valore);
                Spade.RimuoviSpada(spada);
            }
            else
            {
                // Stesso tipo: aggiorna direttamente l'oggetto (reference)
                spada.Info = info;
                spada.Origine = origine;
                spada.Materiale = materiale;
                spada.Lunghezza = lunghezza;
                spada.Peso = peso;
                spada.Valore = valore;

                if (spada is Katana k)
                    k.TecnicaForgiatura = tecnica;
            }

            Spade.datiNonSalvati = true;
            AggiornaVisualizzazione();

            cardSelezionata = null;
            // (Opzionale) pulire i campi dopo la modifica
            txtInfo.Text = "";
            txtOrigine.Text = "";
            txtMateriale.Text = "";
            numLunghezza.Value = 0;
            numPeso.Value = 0;
            numValore.Value = 0;
            txtTecnica.Text = "";
            rbSpada.Checked = true;

            
        }
        private void BtnSalvaFile_Click(object sender, EventArgs e)
        {
            Spade.Salva();
        }

        private void BtnCaricaFile_Click(object sender, EventArgs e)
        {
            Spade.Carica();
            AggiornaVisualizzazione();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //se ci sono dati non salvati chiede se salvare
            if (Spade.datiNonSalvati == true)
            {
                if (MessageBox.Show("Ci sono dati non salvati, vuoi salvare?", "Conferma salvataggio", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Spade.Salva();
                }
            }
        }
    }
}



