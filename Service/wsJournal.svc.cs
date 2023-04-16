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


namespace ZenithWebServeur.WCF
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsJournal" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsJournal.svc ou wsJournal.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsJournal : IwsJournal
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsJournalWSBLL clsJournalWSBLL = new clsJournalWSBLL();

        public clsDonnee clsDonnee
        {
            get { return _clsDonnee; }
            set { _clsDonnee = value; }
        }

        //Déclaration du log
        log4net.ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsJournal> pvgAjouter(List<ZenithWebServeur.DTO.clsJournal> Objet)
        {
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsJournal> clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                clsJournals = TestChampObligatoireInsert(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
                //--TEST CONTRAINTE
                clsJournals = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
            }
            //clsObjetEnvoi.OE_PARAM = new string[] {};
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                clsDonnee.pvgConnectionBase();
                foreach (ZenithWebServeur.DTO.clsJournal clsJournalDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsJournal clsJournal = new ZenithWebServeur.BOJ.clsJournal();
                    clsJournal.JO_CODEJOURNAL = clsJournalDTO.JO_CODEJOURNAL.ToString();
                    clsJournal.JO_LIBELLE = clsJournalDTO.JO_LIBELLE.ToString();
                    clsJournal.JO_NUMEROORDRE =int.Parse(clsJournalDTO.JO_NUMEROORDRE.ToString());
                    clsObjetEnvoi.OE_A = clsJournalDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsJournalDTO.clsObjetEnvoi.OE_Y;
                    clsObjetRetour.SetValue(true, clsJournalWSBLL.pvgAjouter(clsDonnee, clsJournal, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

                }
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE = "00";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "TRUE";
                    clsJournal.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                    clsJournals.Add(clsJournal);
                }
                else
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsJournal.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
                    clsJournals.Add(clsJournal);
                }



            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                clsJournals.Add(clsJournal);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                clsJournals.Add(clsJournal);
            }

            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsJournals;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsJournal> pvgModifier(List<ZenithWebServeur.DTO.clsJournal> Objet)
        {
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsJournal> clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                clsJournals = TestChampObligatoireUpdate(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
                //--TEST CONTRAINTE
                clsJournals = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
            }
            
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                clsDonnee.pvgConnectionBase();
                foreach (ZenithWebServeur.DTO.clsJournal clsJournalDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsJournal clsJournal = new ZenithWebServeur.BOJ.clsJournal();
                    clsJournal.JO_CODEJOURNAL = clsJournalDTO.JO_CODEJOURNAL.ToString();
                    clsJournal.JO_LIBELLE = clsJournalDTO.JO_LIBELLE.ToString();
                    clsJournal.JO_NUMEROORDRE = int.Parse(clsJournalDTO.JO_NUMEROORDRE.ToString());
                    clsObjetEnvoi.OE_A = clsJournalDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsJournalDTO.clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_PARAM = new string[] { clsJournalDTO.JO_CODEJOURNAL };
                    clsObjetRetour.SetValue(true, clsJournalWSBLL.pvgModifier(clsDonnee, clsJournal, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

                }
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE = "00";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "TRUE";
                    clsJournal.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                    clsJournals.Add(clsJournal);
                }
                else
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsJournal.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
                    clsJournals.Add(clsJournal);
                }



            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                clsJournals.Add(clsJournal);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                clsJournals.Add(clsJournal);
            }

            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsJournals;
        }


        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsJournal> pvgSupprimer(List<ZenithWebServeur.DTO.clsJournal> Objet)
        {

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsJournal> clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                clsJournals = TestChampObligatoireDelete(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
                //--TEST CONTRAINTE
                clsJournals = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
            }


            clsObjetEnvoi.OE_PARAM = new string[] { Objet[0].JO_CODEJOURNAL };
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();

            try
            {
                clsDonnee.pvgConnectionBase();
                clsObjetEnvoi.OE_A = Objet[0].clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet[0].clsObjetEnvoi.OE_Y;
                clsObjetRetour.SetValue(true, clsJournalWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "VIT0002").MS_LIBELLEMESSAGE);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE = "00";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "TRUE";
                    clsJournal.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                    clsJournals.Add(clsJournal);
                }
                else
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsJournal.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
                    clsJournals.Add(clsJournal);
                }



            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                clsJournals.Add(clsJournal);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
                clsJournals.Add(clsJournal);
            }


            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsJournals;
        }


            ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
            ///<param name="Objet">Collection de clsInput </param>
            ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
            ///<author>Home Technology</author>
            public List<ZenithWebServeur.DTO.clsJournal> pvgChargerDansDataSet(List<ZenithWebServeur.DTO.clsJournal> Objet)
            {

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsJournal> clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
           
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    clsJournals = TestChampObligatoireListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
            //    //--TEST CONTRAINTE
            //    clsJournals = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsJournals[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsJournals;
            //}

      
            clsObjetEnvoi.OE_PARAM= new string[] {};
            DataSet DataSet = new DataSet();

            try
            {
            clsDonnee.pvgConnectionBase();
            DataSet = clsJournalWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
            clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
            if (DataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in DataSet.Tables[0].Rows)
                {
                    ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                    clsJournal.JO_CODEJOURNAL = row["JO_CODEJOURNAL"].ToString();
                    clsJournal.JO_LIBELLE = row["JO_LIBELLE"].ToString();
                    clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsJournal.clsObjetRetour.SL_CODEMESSAGE ="00";
                    clsJournal.clsObjetRetour.SL_RESULTAT = "TRUE";
                    clsJournal.clsObjetRetour.SL_MESSAGE ="L'opération s'est réalisée avec succès";
                    clsJournals.Add(clsJournal);
                }
            }
            else
            {
                ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
                clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsJournal.clsObjetRetour.SL_RESULTAT = "FALSE";
                clsJournal.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé";
                clsJournals.Add(clsJournal);
            }
                


            }
            catch (SqlException SQLEx)
            {
            ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
            clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
            clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
            clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
            clsJournal.clsObjetRetour.SL_RESULTAT ="FALSE";
            //Execution du log
            Log.Error(SQLEx.Message, null);
            clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
            clsJournals.Add(clsJournal);
            }
            catch (Exception SQLEx)
            {
            ZenithWebServeur.DTO.clsJournal clsJournal = new ZenithWebServeur.DTO.clsJournal();
            clsJournal.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
            clsJournal.clsObjetRetour.SL_CODEMESSAGE = "99";
            clsJournal.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
            clsJournal.clsObjetRetour.SL_RESULTAT ="FALSE";
            //Execution du log
            Log.Error(SQLEx.Message, null);
            clsJournals = new List<ZenithWebServeur.DTO.clsJournal>();
            clsJournals.Add(clsJournal);
            }


            finally
            {
            clsDonnee.pvgDeConnectionBase();
            }
            return clsJournals;
            }
        

        public string pvgChargerDansDataSetPourCombo(clsJournal Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsJournal clsJournal = new ZenithWebServeur.BOJ.clsJournal();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsJournal clsJournalDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsJournalWSBLL.pvgChargerDansDataSetPourCombo(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
                    for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
                    {
                        DataSet.Tables[0].Rows[i]["SL_CODEMESSAGE"] = "00";
                        DataSet.Tables[0].Rows[i]["SL_RESULTAT"] = "TRUE";
                        DataSet.Tables[0].Rows[i]["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    }

                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                else
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Aucun enregistrement n'a été trouvé";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                //}
            }
            catch (SqlException SQLEx)
            {
                DataSet = new DataSet();
                DataRow dr = dt.NewRow();
                dr["SL_CODEMESSAGE"] = "99";
                dr["SL_RESULTAT"] = "FALSE";
                dr["SL_MESSAGE"] = (SQLEx.Number == 2601 || SQLEx.Number == 2627) ? clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0003").MS_LIBELLEMESSAGE : SQLEx.Message;
                dt.Rows.Add(dr);
                DataSet.Tables.Add(dt);
                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                //Execution du log
                Log.Error(SQLEx.Message, null);
            }
            catch (Exception SQLEx)
            {
                DataSet = new DataSet();
                DataRow dr = dt.NewRow();
                dr["SL_CODEMESSAGE"] = "99";
                dr["SL_RESULTAT"] = "FALSE";
                dr["SL_MESSAGE"] = SQLEx.Message;
                dt.Rows.Add(dr);
                DataSet.Tables.Add(dt);
                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                //Execution du log
                Log.Error(SQLEx.Message, null);
            }

            finally
            {
                bool OR_BOOLEEN = true;
                if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE")
                {
                    OR_BOOLEEN = false;
                }
                clsDonnee.pvgTerminerTransaction(!OR_BOOLEEN);
                //clsDonnee.pvgDeConnectionBase();
            }

            return json;
        }


    }
}
