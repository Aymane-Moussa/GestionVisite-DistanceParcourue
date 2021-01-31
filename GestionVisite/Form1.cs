using GestionVisite.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionVisite
{
    public partial class RésultatCalcDistPar : Form
    {
        DB db = new DB();
        Personne Commercial;
        List<int> dejaVisité;
        List<Personne> ordre;

        public RésultatCalcDistPar()
        {
            InitializeComponent();
        }

        private void btnAfficher_Click(object sender, EventArgs e)
        {
            grd.DataSource = db.Clients.Select(c => new { c.Id, c.Nom, c.Position.X, c.Position.Y }).ToList(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSaisirComm_Click(object sender, EventArgs e)
        {
            Commercial = new Personne(txtId.Text, txtNom.Text, txtX.Text, txtY.Text);
            dejaVisité  = new List<int>();
            ordre       = new List<Personne>();
            Visiter(Commercial, db.Clients);
            grdVisite.DataSource = ordre.Select(c => new { c.Id, c.Nom, c.Position.X, c.Position.Y }).ToList(); ;
        }

        private void Visiter(Personne commercial, List<Personne> clients)
        {
            Personne plusProche = trouverPlusProche(commercial, clients, dejaVisité);
            if(plusProche!=null)
            {
                ordre.Add(plusProche);
                dejaVisité.Add(plusProche.Id);
                Visiter(plusProche, clients);
            }
        }

        private Personne trouverPlusProche(Personne source, List<Personne> clients, List<int> dejaVisité)
        {
            Personne proche  = null;
            double   minDist =  double.MaxValue;

            foreach (Personne cli in clients)
            {
                if (!dejaVisité.Contains(cli.Id))
                {
                    double dist = Helper.Distance(source, cli);
                    if(dist < minDist)
                    {
                        minDist = dist;
                        proche = cli;
                    }

                }
            }
            return proche;
        }
        private double CalculDistanceParcourue(Personne source, List<Personne> clients, List<int> dejaVisité)
        {
            double calcul = Helper.Distance(source, ordre[0]);
            int i;
            for(i=1;i<ordre.Count; i++)
            {
                double dist = Helper.Distance(ordre[i-1], ordre[i]);
                calcul += dist;   
            }
            
            return calcul;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CalculDistParc_Click(object sender, EventArgs e)
        {
            double resultat;
            Commercial = new Personne(txtId.Text, txtNom.Text, txtX.Text, txtY.Text);
            dejaVisité = new List<int>();
            ordre = new List<Personne>();
            Visiter(Commercial, db.Clients);
            resultat = CalculDistanceParcourue(Commercial, db.Clients, dejaVisité);
            TxtBoxResultat.Text = resultat.ToString();

        }

        

        private void grdCalcDistParc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }
    }
}
