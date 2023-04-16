using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using ZenithWebServeur.DTO;
using ZenithWebServeur.WSTOOLS;
using log4net;
using System.Reflection;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ZenithWebServeur.WSBLL;
using System.Data;
using ZenithWebServeur.Common;
using ZenithWebServeur.BOJ;

namespace ZenithWebServeur.WCF
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsWCFEditionDate" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsWCFEditionDate.svc ou wsWCFEditionDate.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class wsEditionDate : IwsEditionDate
    {
        private clsDonnee _clsDonnee = new clsDonnee();

        private clsEditionWSBLL clsEditionWSBLL = new clsEditionWSBLL();
        private clsMiccompteWSBLL clsMiccompteWSBLL = new clsMiccompteWSBLL();

        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        public clsDonnee clsDonnee
        {
            get { return _clsDonnee; }
            set { _clsDonnee = value; }
        }

        //Chargement de la période précédente n-1
        public clsEditionDate pvgPeriodicitePeriodePrecedenteDateDebutFin(string vppDateDebut, string vppDateFin, string ExerciceEncours, string Periodicite, string Periode, string JT_DATEJOURNEETRAVAIL)
        {
            List<clsEditionDate> clsEditionDates = new List<clsEditionDate>();
            BOJ.clsObjetRetour clsObjetRetour = new BOJ.clsObjetRetour();

            BOJ.clsObjetEnvoi clsObjetEnvoi = new BOJ.clsObjetEnvoi();
            clsEditionDate clsEditionDate = new clsEditionDate();

            try
            {

                vppDateDebut = vppDateFin = "";
                //DataSet vlpDataSet = new DataSet();
                string ExercicePrecedent = "0";
                string PeriodePrecedent = "0";
                switch (Periodicite)
                {

                    //Mensuel 
                    case "03":
                        if (Periode == "01")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else
                        {
                            ExercicePrecedent = ExerciceEncours;
                            if (double.Parse(Periode) <= 10)
                                PeriodePrecedent = "0" + (double.Parse(Periode) - 1).ToString();
                            if (double.Parse(Periode) > 10)
                                PeriodePrecedent = (double.Parse(Periode) - 1).ToString();
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        break;

                    //Trimestriel
                    case "05":

                        if (Periode == "01")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "10";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "02")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "11";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "03")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else
                        {
                            ExercicePrecedent = ExerciceEncours;
                            //if(double.Parse(Periode)>=10)
                            PeriodePrecedent = "0" + (double.Parse(Periode) - 3).ToString();
                            //if(double.Parse(Periode)<10)
                            //PeriodePrecedent=(double.Parse(Periode)-3).ToString();
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }

                        break;

                    //SEMESTRIEL
                    case "06":

                        if (Periode == "01")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "07";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "02")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "08";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "03")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "09";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "04")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "10";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "05")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "11";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "06")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }

                        else
                        {
                            ExercicePrecedent = ExerciceEncours;
                            //if(double.Parse(Periode)>=10)
                            PeriodePrecedent = "0" + (double.Parse(Periode) - 6).ToString();
                            //if(double.Parse(Periode)<10)
                            //PeriodePrecedent=(double.Parse(Periode)-3).ToString();
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }



                        break;


                    //Annuel
                    case "07":
                        if (Periode == "12")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }


                        break;

                    default:
                        break;


                }

            }

            catch (SqlException SQLEx)
            {
                clsEditionDate = new clsEditionDate();
                clsEditionDate.SL_RESULTAT = "FALSE";
                clsEditionDate.SL_MESSAGE = SQLEx.Message;
                clsEditionDate.SL_CODEMESSAGE = "9999";
                clsObjetRetour.SetValueMessage(false, SQLEx.Message);
            }
            finally
            {

            }
            return clsEditionDate;

        }


        //Chargement de la période précédente n-2
        public clsEditionDate pvgPeriodicitePeriodePrecedenteDateDebutFinn2(string vppDateDebut, string vppDateFin, string ExerciceEncours, string Periodicite, string Periode, string JT_DATEJOURNEETRAVAIL)
        {
            List<clsEditionDate> clsEditionDates = new List<clsEditionDate>();
            BOJ.clsObjetRetour clsObjetRetour = new BOJ.clsObjetRetour();

            BOJ.clsObjetEnvoi clsObjetEnvoi = new BOJ.clsObjetEnvoi();
            clsEditionDate clsEditionDate = new clsEditionDate();

            try
            {

                vppDateDebut = vppDateFin = "";
                //DataSet vlpDataSet = new DataSet();
                string ExercicePrecedent = "0";
                string PeriodePrecedent = "0";
                switch (Periodicite)
                {

                    //Mensuel 
                    case "03":
                        if (Periode == "01")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "11";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else if (Periode == "02")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        else
                        {
                            ExercicePrecedent = ExerciceEncours;
                            if (double.Parse(Periode) <= 10)
                                PeriodePrecedent = "0" + (double.Parse(Periode) - 2).ToString();
                            if (double.Parse(Periode) > 10)
                                PeriodePrecedent = (double.Parse(Periode) - 2).ToString();
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        break;

                    //Trimestriel
                    case "05":


                        if (Periode == "03")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "09";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }

                        else if (Periode == "06")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }


                        else
                        {
                            ExercicePrecedent = ExerciceEncours;
                            //if(double.Parse(Periode)>=10)
                            PeriodePrecedent = "0" + (double.Parse(Periode) - 6).ToString();
                            //if(double.Parse(Periode)<10)
                            //PeriodePrecedent=(double.Parse(Periode)-3).ToString();
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }

                        break;

                    //SEMESTRIEL
                    case "06":


                        if (Periode == "06")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "06";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }
                        if (Periode == "12")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 1).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }

                        //else
                        //{
                        //    ExercicePrecedent = ExerciceEncours;
                        //    //if(double.Parse(Periode)>=10)
                        //    PeriodePrecedent = "0" + (double.Parse(Periode) - 8).ToString();
                        //    //if(double.Parse(Periode)<10)
                        //    //PeriodePrecedent=(double.Parse(Periode)-3).ToString();
                        //    pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite);
                        //}

                        break;


                    //Annuel
                    case "07":
                        if (Periode == "12")
                        {
                            ExercicePrecedent = (double.Parse(ExerciceEncours) - 2).ToString();
                            PeriodePrecedent = "12";
                            clsEditionDate = pvgPeriodiciteDateDebutFin(vppDateDebut, vppDateFin, ExercicePrecedent, PeriodePrecedent, Periodicite, JT_DATEJOURNEETRAVAIL, "");
                        }


                        break;

                    default:
                        break;


                }

            }

            catch (SqlException SQLEx)
            {
                clsEditionDate = new clsEditionDate();
                clsEditionDate.SL_RESULTAT = "FALSE";
                clsEditionDate.SL_MESSAGE = SQLEx.Message;
                clsEditionDate.SL_CODEMESSAGE = "9999";
                clsObjetRetour.SetValueMessage(false, SQLEx.Message);
            }
            finally
            {

            }
            return clsEditionDate;

        }


        public clsEditionDate pvgPeriodiciteDateDebutFin(string vppDateDebut, string vppDateFin, string EX_EXERCICE, string MO_CODEMOIS, string PE_CODEPERIODICITE, string JT_DATEJOURNEETRAVAIL, string ecran)
        {
            BOJ.clsObjetEnvoi clsObjetEnvoi = new BOJ.clsObjetEnvoi();
            clsEditionDate clsEditionDate = new clsEditionDate();

            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;
            clsDonnee.pvgDemarrerTransaction();

            //string EX_EXERCICE = ""; string MO_CODEMOIS = ""; string PE_CODEPERIODICITE = ""; string JT_DATEJOURNEETRAVAIL = "";
            vppDateDebut = vppDateFin = "";
            DataSet vlpDataSet = new DataSet();
            clsEditionWSBLL clsEditionWSBLL = new clsEditionWSBLL();


            BOJ.clsObjetRetour clsObjetRetour = new BOJ.clsObjetRetour();
            clsObjetEnvoi.OE_PARAM = new string[] { EX_EXERCICE, MO_CODEMOIS, PE_CODEPERIODICITE };
            clsObjetRetour.SetValue(true, clsEditionWSBLL.pvgPeriodiciteDateDebutFin(clsDonnee, clsObjetEnvoi));
            vlpDataSet = clsObjetRetour.OR_DATASET;
            if (vlpDataSet.Tables.Count > 0)
            {
                if (vlpDataSet.Tables[0].Rows.Count > 0)
                {
                    clsEditionDate.EX_DATEDEBUT = DateTime.Parse(vlpDataSet.Tables[0].Rows[0][0].ToString()).ToShortDateString();
                    //
                    if (ecran == "")
                    {
                        if (DateTime.Parse(vlpDataSet.Tables[0].Rows[0][1].ToString()) > DateTime.Parse(JT_DATEJOURNEETRAVAIL))
                        {
                            clsEditionDate.EX_DATEFIN = DateTime.Parse(JT_DATEJOURNEETRAVAIL).ToShortDateString();
                            clsEditionDate.SL_RESULTAT = "TRUE";
                            clsEditionDate.SL_MESSAGE = "Opération réalisé avec succès !!!";
                            clsEditionDate.SL_CODEMESSAGE = "000";
                        }
                        else
                        {
                            clsEditionDate.EX_DATEFIN = DateTime.Parse(vlpDataSet.Tables[0].Rows[0][1].ToString()).ToShortDateString();
                            clsEditionDate.SL_RESULTAT = "TRUE";
                            clsEditionDate.SL_MESSAGE = "Opération réalisé avec succès !!!";
                            clsEditionDate.SL_CODEMESSAGE = "000";
                        }
                    }
                    // vppCritere[3] == "DATEFINSUPDATEJOUR"
                    else
                    {
                        clsEditionDate.EX_DATEFIN = DateTime.Parse(vlpDataSet.Tables[0].Rows[0][1].ToString()).ToShortDateString();
                        clsEditionDate.SL_RESULTAT = "TRUE";
                        clsEditionDate.SL_MESSAGE = "Opération réalisé avec succès !!!";
                        clsEditionDate.SL_CODEMESSAGE = "000";
                    }
                }
            }

            return clsEditionDate;
        }



    }
}
