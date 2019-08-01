using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;
using Npgsql;
using DataRow = System.Data.DataRow;


namespace Client_Migrator_Medexy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillComboBox();
            BlockRect.Visibility = Visibility.Collapsed;
            BlockRect.Height = 1080;
            BlockRect.Width = 1920;

            worker.DoWork += worker_DoWork;
        }

        public int ReadVisitsCount = 0;

        public void DownloadFile()
        {
            
        }


        private readonly BackgroundWorker worker = new BackgroundWorker();

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Batch(*.bat)|*.bat|All(*.*)|*"
            };

            ReadVisitsCount = 0;

            if (dialog.ShowDialog() == true)
            {
                bool FirstLoop = true;

                #region String Snipets

                string EndOfLineForBatFile = " | \"C:\\Program Files\\PostgreSQL\\9.5\\bin\\psql\" -h localhost -p 5432 -U postgres -d test";

                string StartOfText = "( " + Environment.NewLine +
                                     "echo SET statement_timeout = 0; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET lock_timeout = 0; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET client_encoding = 'UTF8'; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET standard_conforming_strings = on; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SELECT pg_catalog.set_config('search_path', '', false); " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET check_function_bodies = false; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET client_min_messages = warning; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET row_security = off; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET default_tablespace = ''; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo SET default_with_oids = false; " + EndOfLineForBatFile + Environment.NewLine +
                                     "echo CREATE TABLE public.klientai ( " +
                                     "    id integer NOT NULL PRIMARY KEY, " +
                                     "    vardas character varying, " +
                                     "    lygis character varying, " +
                                     "    profesija character varying, " +
                                     "    darb_id_1 integer, " +
                                     "    darb_id_2 integer, " +
                                     "    darb_id_3 integer, " +
                                     "    tel_nr character varying, " +
                                     "    email character varying, " +
                                     "    gim_dat timestamp without time zone, " +
                                     "    pastabos character varying, " +
                                     "    mediket_prevent character varying, " +
                                     "    mediket_versi character varying, " +
                                     "    mediket_plus character varying, " +
                                     "    mediket_ictamo character varying, " +
                                     "    benemedio_301 character varying, " +
                                     "    vitella_extreme character varying, " +
                                     "    vitella_zn character varying, " +
                                     "    vitella_u character varying, " +
                                     "    vitella_oilbath character varying, " +
                                     "    vitella_nose character varying, " +
                                     "    vitella_ictogel character varying, " +
                                     "    vitella_ictamo character varying, " +
                                     "    vizituotojas character varying, " +
                                     "    stock_db_kodas character varying, " +
                                     "    adresas character varying, " +
                                     "    miestas character varying, " +
                                     "    redaguotas boolean, " +
                                     "    licenzija character varying, " +
                                     "    rusis character varying " +
                                     ");" + EndOfLineForBatFile + Environment.NewLine +
                                     "" +
                                     "echo CREATE TABLE public.vizitai( " +
                                     "    vizito_id integer NOT NULL PRIMARY KEY, " +
                                     "    kliento_id integer, " +
                                     "    data timestamp without time zone, " +
                                     "    vitella_ir_mediket_1 character varying, " +
                                     "    vitella_ir_mediket_2 character varying, " +
                                     "    vitella_ir_mediket_3 character varying, " +
                                     "    vitella_ir_mediket_4 character varying, " +
                                     "    vitella_ir_mediket_5 character varying, " +
                                     "    vitella_ir_mediket_6 character varying, " +
                                     "    vitella_ir_mediket_7 character varying, " +
                                     "    vitella_ir_mediket_8 character varying, " +
                                     "    vitella_ir_mediket_9 character varying, " +
                                     "    vitella_ir_mediket_10 character varying, " +
                                     "    darbovietes_id_klientu_vizitams character varying, " +
                                     "    kliento_id_darbovieciu_vizitams character varying, " +
                                     "    lankstukai boolean, " +
                                     "    kita character varying, " +
                                     "    komentarai character varying, " +
                                     "    pastabos character varying, " +
                                     "    vizituotojas character varying, " +
                                     "    vizito_tipas character varying, " +
                                     "    kliento_tipas character varying, " +
                                     "    darb_id integer, " +
                                     "    vizitas_redaguotas boolean     " +
                                     "); " + EndOfLineForBatFile + Environment.NewLine;



                string StartOfLine = "echo INSERT INTO public.klientai " +
                                     "(id, vardas, lygis, profesija, " +
                                     "tel_nr, email, gim_dat, pastabos, vizituotojas, " +
                                     "stock_db_kodas, adresas, miestas, " +
                                     "licenzija, rusis) VALUES (";

                string EndOfLine = "); " + EndOfLineForBatFile;

                string StartOfVisitLine = "echo INSERT INTO public.vizitai " +
                                          "(vizito_id, kliento_id, data, " +
                                          "darbovietes_id_klientu_vizitams, " +
                                          "kliento_id_darbovieciu_vizitams, lankstukai, " +
                                          "kita, komentarai, pastabos, vizituotojas, " +
                                          "vizito_tipas, kliento_tipas, darb_id) VALUES ( ";




                #endregion



                int ClientID = ClientOffset + 1;
                int VisitID = VisitOffset + 1;

                string fileText = "";

                fileText += StartOfText;

                //Loopina per klientus
                foreach (var row in ClientsToMoveDg.Items)
                {
                    fileText += Environment.NewLine + StartOfLine;

                    fileText += ClientID + ", ";

                    DataRowView rw = (DataRowView)row;

                    string ClientName = rw[1].ToString();
                    ClientName = ClientName.Replace(",", "");
                    ClientName = ClientName.Replace("'", "");

                    fileText += "'" + ClientName + "', ";
                    fileText += "'" + rw[2] + "', ";
                    fileText += "'" + rw[3] + "', ";
                    fileText += "'" + rw[7] + "', ";
                    fileText += "'" + rw[8] + "', ";

                    if (rw[9].ToString() == "")
                    {
                        fileText += "NULL, ";

                    }
                    else
                    {
                        fileText += "'" + rw[9] + "', ";
                    }

                    string Pastabos = rw[10].ToString();
                    Pastabos = Pastabos.Replace(",", "");
                    Pastabos = Pastabos.Replace("'", "");


                    fileText += "'" + Pastabos + "', ";

                    string text = "";
                    Dispatcher.Invoke(() => { text = NewVizituotojasTb.Text; });

                    fileText += "'" + text + "', ";

                    fileText += "'" + rw[24] + "', ";
                    fileText += "'" + rw[25] + "', ";
                    fileText += "'" + rw[26] + "', ";
                    fileText += "'" + rw[28] + "', ";
                    fileText += "'" + rw[29] + "' " + EndOfLine;


                    //loopina per vizitus vieno kliento


                    int ClientLocalId = Convert.ToInt32(rw[0]);
                    string ClientVIzituotojas = Convert.ToString(rw[23]);

                    string SelectAllVisits =
                        "select * from vizitai where kliento_id = @clientID and vizituotojas = @vizituotojas and kliento_tipas = 'klientas'";

                    NpgsqlCommand SelectAllVisitsCmd = new NpgsqlCommand(SelectAllVisits, Conn);

                    SelectAllVisitsCmd.Parameters.AddWithValue("@clientID", ClientLocalId);
                    SelectAllVisitsCmd.Parameters.AddWithValue("@vizituotojas", ClientVIzituotojas);

                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }

                    DataTable TAbleOfVisits = new DataTable();
                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(SelectAllVisitsCmd))
                    {
                        da.Fill(TAbleOfVisits);
                    }

                    DataView VisitsView = TAbleOfVisits.AsDataView();

                    foreach (DataRowView VisitRow in VisitsView)
                    {


                        fileText += Environment.NewLine + StartOfVisitLine;


                        fileText += "'" + VisitID + "', ";
                        fileText += "'" + ClientID + "', ";
                        fileText += "'" + VisitRow[2] + "', ";
                        fileText += "'" + VisitRow[13] + "', ";
                        fileText += "'" + ClientID + "', ";
                        fileText += "'" + VisitRow[15] + "', ";
                        fileText += "'" + VisitRow[16] + "', ";

                        string Komentarai = VisitRow[17].ToString();
                        Komentarai = Komentarai.Replace("'", "");

                        string bre = char.ConvertFromUtf32(10);

                        Komentarai = Komentarai.Replace(Environment.NewLine, " ");
                        Komentarai = Regex.Replace(Komentarai, bre, " ");

                        fileText += "'" + Komentarai + "', ";

                        string PastabosV = VisitRow[18].ToString();
                        PastabosV = PastabosV.Replace(",", "");
                        PastabosV = PastabosV.Replace("'", "");
                        PastabosV = PastabosV.Replace(";", "");
                        PastabosV = PastabosV.Replace(Environment.NewLine, " ");
                        PastabosV = Regex.Replace(PastabosV, bre, " ");

                        fileText += "'" + PastabosV + "', ";

                        string NaujasVizituotojas = "";
                        Dispatcher.Invoke(() => { NaujasVizituotojas = NewVizituotojasTb.Text; });

                        fileText += "'" + NaujasVizituotojas + "', ";
                        fileText += "'" + VisitRow[20] + "', ";
                        fileText += "'" + VisitRow[21] + "', ";

                        if (VisitRow[22].ToString() == "")
                        {
                            fileText += "NULL" + EndOfLine;
                        }
                        else
                        {
                            fileText += "'" + VisitRow[22] + "'" + EndOfLine;
                        }

                        VisitID++;
                        ReadVisitsCount++;
                        //update textbox to show progress

                        if(FirstLoop)
                        {
                            File.WriteAllText(dialog.FileName, fileText, Encoding.UTF8);
                            FirstLoop = false;
                        }
                        else
                        {
                            File.AppendAllText(dialog.FileName, fileText, Encoding.UTF8);
                        }

                        fileText = "";
                    }
                    ClientID++;
                }
                fileText += Environment.NewLine + ")";


                worker_RunWorkerCompleted();
            }

        }

        private void worker_RunWorkerCompleted()
        {
            MessageBox.Show("Baigta");
        }


        #region Variables

        public string VizituotojasLastSelected;

        public int ClientOffset = 0;
        public int VisitOffset = 0;

        #endregion

        #region Connection

        private static string ConnectionString =
            ""; /* Redacted */

        public NpgsqlConnection Conn = new NpgsqlConnection(ConnectionString);


        #endregion

        public void FillComboBox()
        {
            string SelectAlllStr = "select distinct(vizituotojas) from klientai";
            NpgsqlCommand SelectAllUsers = new NpgsqlCommand(SelectAlllStr, Conn);

            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }

            NpgsqlDataReader Dr = SelectAllUsers.ExecuteReader();
            while (Dr.Read())
            {
                OldVizituotojasComboBox.Items.Add(Dr["vizituotojas"].ToString());
            }

            Dr.Close();
        }

        private void QuitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateClientList()
        {
            string selectClientsStr = "select * from klientai where vizituotojas = @vizituotojas and" +
                                      " (Upper(vardas) like @search or Upper(adresas) like @search or Upper(miestas) like @search)";

            string searchStr = SearchTb.Text.ToUpper();

            NpgsqlCommand SelectClientsCmd = new NpgsqlCommand(selectClientsStr, Conn);
            SelectClientsCmd.Parameters.AddWithValue("@search", "%" + searchStr + "%");
            SelectClientsCmd.Parameters.AddWithValue("@vizituotojas", VizituotojasLastSelected);

            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }

            DataTable Dt = new DataTable();

            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(SelectClientsCmd))
            {
                da.Fill(Dt);
            }

            ListOfClientsDg.ItemsSource = Dt.AsDataView();

        }

        private void OldVizituotojasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VizituotojasLastSelected = OldVizituotojasComboBox.SelectedValue.ToString();
            UpdateClientList();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClientList();
        }

        private void MoveRightBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListOfClientsDg.SelectedItems.Count > 0)
            {
                foreach (var t in ListOfClientsDg.SelectedItems)
                {
                    ClientsToMoveDg.Items.Add(t);
                }
            }
        }

        private void MoveLeftBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void AutoOffsetCb_Click(object sender, RoutedEventArgs e)
        {
            if (AutoOffsetCb.IsChecked == true)
            {
                ClientIdOffsetTb.IsReadOnly = true;
                VisitIdOffsetTb.IsReadOnly = true;
            }

            else
            {
                ClientIdOffsetTb.IsReadOnly = false;
                VisitIdOffsetTb.IsReadOnly = false;
            }
        }

        private void SelectAllBrn_Click(object sender, RoutedEventArgs e)
        {
            if (ListOfClientsDg.Items.Count > 0)
            {
                foreach (var t in ListOfClientsDg.Items)
                {
                    ClientsToMoveDg.Items.Add(t);
                }
            }

        }

        private void CheckOffset() //Offset check for auto offset
        {
            string NaujasVizituotojas = NewVizituotojasTb.Text;

            string slctMaxKlientasStr = "select max(id_local) from klientai where vizituotojas = @naujasVizituotojas";
            string slctMaxVizitaiStr = "select max(vizito_id) from vizitai where vizituotojas = @naujasVizituotojas";

            NpgsqlCommand SlctMaxKlientas = new NpgsqlCommand(slctMaxKlientasStr, Conn);
            SlctMaxKlientas.Parameters.AddWithValue("@naujasVizituotojas", NaujasVizituotojas);
            NpgsqlCommand SlctMaxVizitas = new NpgsqlCommand(slctMaxVizitaiStr, Conn);
            SlctMaxVizitas.Parameters.AddWithValue("@naujasVizituotojas", NaujasVizituotojas);

            int maxKlientoId = 1;
            int maxVizitoId = 1;

            using (NpgsqlDataReader dr = SlctMaxKlientas.ExecuteReader())
            {
                while (dr.Read())
                {
                    try
                    {
                        maxKlientoId = Convert.ToInt32(dr[0].ToString());
                    }
                    catch (Exception exception)
                    {
                        maxKlientoId = 1;
                    }
                }
            }
            using (NpgsqlDataReader dr = SlctMaxVizitas.ExecuteReader())
            {
                while (dr.Read())
                {
                    try
                    {
                        maxVizitoId = Convert.ToInt32(dr[0].ToString());
                    }
                    catch (Exception exception)
                    {
                        maxVizitoId = 1;
                    }
                }
            }

            ClientIdOffsetTb.Text = maxKlientoId.ToString();
            VisitIdOffsetTb.Text = maxVizitoId.ToString();
        }

        private void CheckOffsetBtn_Click(object sender, RoutedEventArgs e)
        {
            string NaujasVizituotojas = NewVizituotojasTb.Text;

            string slctMaxKlientasStr = "select max(id_local) from klientai where vizituotojas = @naujasVizituotojas";
            string slctMaxVizitaiStr = "select max(vizito_id) from vizitai where vizituotojas = @naujasVizituotojas";

            NpgsqlCommand SlctMaxKlientas = new NpgsqlCommand(slctMaxKlientasStr, Conn);
            SlctMaxKlientas.Parameters.AddWithValue("@naujasVizituotojas", NaujasVizituotojas);
            NpgsqlCommand SlctMaxVizitas = new NpgsqlCommand(slctMaxVizitaiStr, Conn);
            SlctMaxVizitas.Parameters.AddWithValue("@naujasVizituotojas", NaujasVizituotojas);

            int maxKlientoId = 1;
            int maxVizitoId = 1;

            using (NpgsqlDataReader dr = SlctMaxKlientas.ExecuteReader())
            {
                while (dr.Read())
                {
                    try
                    {
                        maxKlientoId = Convert.ToInt32(dr[0].ToString());
                    }
                    catch (Exception exception)
                    {
                        maxKlientoId = 1;
                    }
                }
            }
            using (NpgsqlDataReader dr = SlctMaxVizitas.ExecuteReader())
            {
                while (dr.Read())
                {
                    try
                    {
                        maxVizitoId = Convert.ToInt32(dr[0].ToString());
                    }
                    catch (Exception exception)
                    {
                        maxVizitoId = 1;
                    }
                }
            }

            ClientIdOffsetTb.Text = maxKlientoId.ToString();
            VisitIdOffsetTb.Text = maxVizitoId.ToString();
        }

        private void NewVizituotojasTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoOffsetCb != null)
            {
                if (AutoOffsetCb.IsChecked == true)
                {
                    CheckOffset();
                }
            }
        }

        private void ExportCSVBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Connection
    {
        private static string ConnectionString =
            ""; /* Redacted */

        public NpgsqlConnection Conn = new NpgsqlConnection(ConnectionString);


    }

}