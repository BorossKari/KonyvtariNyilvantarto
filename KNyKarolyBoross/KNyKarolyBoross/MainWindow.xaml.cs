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
        public Konyvek()
        {

        }
    }
    class Tagok
    {
        public int OlvasoID { get; set; }
        public string Nev { get; set; }
        public string Szuletes { get; set; }
        public int Iranyitoszam { get; set; }
        public string Telepules { get; set; }
        public string Lakcim { get; set; }
        public Tagok(string sor)
        {
            string[] resz = sor.Split(';');
            OlvasoID = Convert.ToInt32(resz[0]);
            Nev = resz[1];
            Szuletes = resz[2];
            Iranyitoszam = Convert.ToInt32(resz[3]);
            Telepules = resz[4];
            Lakcim = resz[5];
        }
        public Tagok()
        {

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
        public Kolcson()
        {

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
            KeresesTabla.DataContext = konyvek;
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
            Konyv_hozzaadas_mehet();
        }
        private void cim_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_hozzaadas_mehet();
        }
        private void kiadasev_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_hozzaadas_mehet();
        }
        private void kiado_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Konyv_hozzaadas_mehet();
        }
        private void Konyv_hozzaadas_mehet()
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
        private void KonyvekTabla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KonyvekTabla.SelectedItems.Count != 0)
            {
                konyv_torles.IsEnabled = true;
            }
            else
            {
                konyv_torles.IsEnabled = false;
            }
        }
        private void konyv_hozzaadas_Click(object sender, RoutedEventArgs e)
        {
            KonyvekTabla.DataContext = null;
            konyvek.Add(new Konyvek() { KonyvID = konyvek[konyvek.Count - 1].KonyvID + 1, Szerzo = szerzo_adatbe.Text, Cim = cim_adatbe.Text, KiadasEv = kiadasev_adatbe.Text, Kiado = kiado_adatbe.Text, Kolcson = (bool)kolcson_adatbe.IsChecked });
            KonyvekTabla.DataContext = konyvek;
        }

        private void KonyvekTabla_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            konyvek[KonyvekTabla.SelectedIndex] = (Konyvek)KonyvekTabla.SelectedItem;
        }

        private void konyv_torles_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in KonyvekTabla.SelectedItems)
            {
                konyvek.Remove((Konyvek)item);
            }
            KonyvekTabla.DataContext = null;
            KonyvekTabla.DataContext = konyvek;
        }

        private void konyv_mentes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult valasz = MessageBox.Show("Biztos, hogy felül akarja írni az eredeti fájl tartalmait? Ez nem vonhaó vissza.", "Megerősítés", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (valasz == MessageBoxResult.OK)
            {
                StreamWriter sw = new StreamWriter("konyvek.txt");
                foreach (var item in konyvek)
                {
                    sw.WriteLine(item.KonyvID + ";" + item.Szerzo + ";" + item.Cim + ";" + item.KiadasEv + ";" + item.Kiado + ";" + item.Kolcson);
                }
                sw.Close();
                sw.Dispose();
            }
        }

        private void tag_hozzaadas_Click(object sender, RoutedEventArgs e)
        {
            TagokTabla.DataContext = null;
            tagok.Add(new Tagok() { OlvasoID = tagok[tagok.Count - 1].OlvasoID + 1, Nev = nev_adatbe.Text, Szuletes = szuletes_adatbe.Text, Iranyitoszam = Convert.ToInt32(irszam_adatbe.Text), Telepules = telepules_adatbe.Text, Lakcim = utcahsz_adatbe.Text });
            TagokTabla.DataContext = tagok;
        }

        private void tag_torles_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in TagokTabla.SelectedItems)
            {
                tagok.Remove((Tagok)item);
            }
            TagokTabla.DataContext = null;
            TagokTabla.DataContext = tagok;
        }

        private void tag_mentes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult valasz = MessageBox.Show("Biztos, hogy felül akarja írni az eredeti fájl tartalmait? Ez nem vonhaó vissza.", "Megerősítés", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (valasz == MessageBoxResult.OK)
            {
                StreamWriter sw = new StreamWriter("tagok.txt");
                foreach (var item in tagok)
                {
                    sw.WriteLine(item.OlvasoID + ";" + item.Nev + ";" + item.Szuletes + ";" + item.Iranyitoszam + ";" + item.Telepules + ";" + item.Lakcim);
                }
                sw.Close();
                sw.Dispose();
            }
        }

        private void nev_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tag_hozzaadas_mehet();
        }

        private void szuletes_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tag_hozzaadas_mehet();
        }

        private void irszam_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tag_hozzaadas_mehet();
        }

        private void telepules_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tag_hozzaadas_mehet();
        }

        private void utcahsz_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tag_hozzaadas_mehet();
        }
        public void Tag_hozzaadas_mehet()
        {
            if (nev_adatbe.Text != "" && szuletes_adatbe.Text != "" && irszam_adatbe.Text != "" && telepules_adatbe.Text != "" && utcahsz_adatbe.Text != "")
            {
                tag_hozzaadas.IsEnabled = true;
            }
            else
            {
                tag_hozzaadas.IsEnabled = false;
            }
        }

        private void TagokTabla_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            tagok[TagokTabla.SelectedIndex] = (Tagok)TagokTabla.SelectedItem;
        }

        private void TagokTabla_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TagokTabla.SelectedItems.Count != 0)
            {
                tag_torles.IsEnabled = true;
            }
            else
            {
                tag_torles.IsEnabled = false;
            }
        }

        private void kolcsonolvaso_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolcson_hozzaadas_mehet();
        }

        private void kolcsonkonyv_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolcson_hozzaadas_mehet();
        }

        private void kolcsondatum_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolcson_hozzaadas_mehet();
        }

        private void kolcsonhatarido_adatbe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolcson_hozzaadas_mehet();
        }

        private void kolcson_hozzaadas_Click(object sender, RoutedEventArgs e)
        {
            KolcsonzesekTabla.DataContext = null;
            kolcson.Add(new Kolcson() { KolcsID = kolcson[kolcson.Count - 1].KolcsID + 1, OlvasoID = Convert.ToInt32(kolcsonolvaso_adatbe.Text), KonyvID = Convert.ToInt32(kolcsonkonyv_adatbe.Text), KolcsDatum = kolcsondatum_adatbe.Text, VisszaDatum = kolcsonhatarido_adatbe.Text });
            KolcsonzesekTabla.DataContext = kolcson;
        }

        private void kolcson_torles_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in KolcsonzesekTabla.SelectedItems)
            {
                kolcson.Remove((Kolcson)item);
            }
            KolcsonzesekTabla.DataContext = null;
            KolcsonzesekTabla.DataContext = kolcson;
        }

        private void kolcson_mentes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult valasz = MessageBox.Show("Biztos, hogy felül akarja írni az eredeti fájl tartalmait? Ez nem vonhaó vissza.", "Megerősítés", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (valasz == MessageBoxResult.OK)
            {
                StreamWriter sw = new StreamWriter("kolcsonzesek.txt");
                foreach (var item in kolcson)
                {
                    sw.WriteLine(item.KolcsID + ";" + item.OlvasoID + ";" + item.KonyvID + ";" + item.KolcsDatum + ";" + item.VisszaDatum);
                }
                sw.Close();
                sw.Dispose();
            }
        }
        private void Kolcson_hozzaadas_mehet()
        {
            if (kolcsonolvaso_adatbe.Text != "" && kolcsonkonyv_adatbe.Text != "" && kolcsondatum_adatbe.Text != "" && kolcsonhatarido_adatbe.Text != "")
            {
                kolcson_hozzaadas.IsEnabled = true;
            }
            else
            {
                kolcson_hozzaadas.IsEnabled = false;
            }
        }

        private void KolcsonzesekTabla_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            kolcson[KolcsonzesekTabla.SelectedIndex] = (Kolcson)KolcsonzesekTabla.SelectedItem;
        }

        private void kereses_gomb_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
