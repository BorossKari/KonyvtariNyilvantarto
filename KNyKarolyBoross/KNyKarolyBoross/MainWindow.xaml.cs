using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace KNyKarolyBoross
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    class Konyvek
    {
        public int KonyvID { get; set; }
        public string Szerzo { get; set; }
        public string Cim { get; set; }
        public string KiadasEv { get; set; }
        public string Kiado { get; set; }
        public bool Kolcson { get; set; }
        public Konyvek(string sor)
        {
            string[] resz = sor.Split(';');
            KonyvID = Convert.ToInt32(resz[0]);
            Szerzo = resz[1];
            Cim = resz[2];
            KiadasEv = resz[3];
            Kiado = resz[4];
            Kolcson = Convert.ToBoolean(resz[5]);
        }
    }
    class Tagok
    {
        public string OlvasoID { get; set; }
        public string Nev { get; set; }
        public string Szuletes { get; set; }
        public int Iranyitoszam { get; set; }
        public string Telepules { get; set; }
        public string Lakcim { get; set; }
        public Tagok(string sor)
        {
            string[] resz = sor.Split(';');
            OlvasoID = resz[0];
            Nev = resz[1];
            Szuletes = resz[2];
            Iranyitoszam = Convert.ToInt32(resz[3]);
            Telepules = resz[4];
            Lakcim = resz[5];
        }
    }
    class Kolcson
    {
        public int KolcsID { get; set; }
        public int OlvasoID { get; set; }
        public int KonyvID { get; set; }
        public string KolcsDatum { get; set; }
        public string VisszaDatum { get; set; }
        public Kolcson(string sor)
        {
            string[] resz = sor.Split(';');
            KolcsID = Convert.ToInt32(resz[0]);
            OlvasoID = Convert.ToInt32(resz[1]);
            KonyvID = Convert.ToInt32(resz[2]);
            KolcsDatum = resz[3];
            VisszaDatum = resz[4];
        }
}
    public partial class MainWindow : Window
    {
        List<Konyvek> konyvek = new List<Konyvek>();
        List<Tagok> tagok = new List<Tagok>();
        List<Kolcson> kolcson = new List<Kolcson>();
        public MainWindow()
        {
            InitializeComponent();
            KonyvekTabla.DataContext = konyvek;
            TagokTabla.DataContext = tagok;
            KolcsonzesekTabla.DataContext = kolcson;
            foreach (var item in File.ReadAllLines("konyvek.txt"))
            {
                konyvek.Add(new Konyvek(item));
            }
            foreach (var item in File.ReadAllLines("tagok.txt"))
            {
                tagok.Add(new Tagok(item));
            }
            foreach (var item in File.ReadAllLines("kolcsonzesek.txt"))
            {
                kolcson.Add(new Kolcson(item));
            }
        }
        private void szerzo_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_mentes_mehet();
        }
        private void cim_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_mentes_mehet();
        }
        private void kiadasev_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_mentes_mehet();
        }
        private void kiado_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_mentes_mehet();
        }
        private void Konyv_mentes_mehet()
        {
            if (szerzo_adatbe.Text != "" && cim_adatbe.Text != "" && kiadasev_adatbe.Text != "" && kiado_adatbe.Text != "")
            {
                konyv_hozzaadas.IsEnabled = true;
            }
            else
            {
                konyv_hozzaadas.IsEnabled = false;
            }
        }
    }
}
