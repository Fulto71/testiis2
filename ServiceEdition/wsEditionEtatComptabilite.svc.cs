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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatComptabilite" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatComptabilite.svc ou wsEditionEtatComptabilite.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsEditionEtatComptabilite : IwsEditionEtatComptabilite
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsEditionEtatComptabiliteWSBLL clsEditionEtatComptabiliteWSBLL = new clsEditionEtatComptabiliteWSBLL();

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
        public string pvgInsertIntoDatasetDocument(ZenithWebServeur.DTO.clsEditionEtatComptabilite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";
            
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.BOJ.clsEditionEtatComptabilite();
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
                //--TEST CONTRAINTE
                //DataSet = TestTestContrainteListe(Objet);
                //--VERIFICATION DU RESULTAT DU TEST
                //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            clsObjetEnvoi.OE_PARAM = new string[] { Objet.OP_CODEOPERATEUR };

            clsEditionEtatComptabilite.AG_CODEAGENCE = Objet.AG_CODEAGENCE;
            clsEditionEtatComptabilite.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE;
            clsEditionEtatComptabilite.PL_ACTIF = "O";
            clsEditionEtatComptabilite.DateJourneeComptable1 = Objet.DateJourneeComptable1;
            clsEditionEtatComptabilite.DateJourneeComptable2 = Objet.DateJourneeComptable2;
            clsEditionEtatComptabilite.TypeAffichage = Objet.TypeAffichage;
            clsEditionEtatComptabilite.TYPEETAT = Objet.TYPEETAT;
            clsEditionEtatComptabilite.TypeListe = Objet.TypeListe;
            clsEditionEtatComptabilite.ConditionExecution = Objet.ConditionExecution;
            clsEditionEtatComptabilite.PL_TYPECOMPTE = Objet.PL_TYPECOMPTE;
            clsEditionEtatComptabilite.PL_NUMCOMPTE = Objet.PL_NUMCOMPTE;
            clsEditionEtatComptabilite.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1;
            clsEditionEtatComptabilite.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2;
            clsEditionEtatComptabilite.Typebalance = Objet.Typebalance;
            clsEditionEtatComptabilite.TailleCpte = Objet.TailleCpte;

            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatComptabiliteWSBLL.pvgInsertIntoDatasetDocument(clsDonnee, clsEditionEtatComptabilite, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    string exportFilename = "";
                    string URL_ETAT = "";

                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["URL_ETAT"] = URL_ETAT;
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    // }
                }
                else
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
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
                clsDonnee.pvgDeConnectionBase();
            }
            return json;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public string pvgETATIMMOBILISATIONAMORTISSEMENT(ZenithWebServeur.DTO.clsEditionEtatComptabilite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.BOJ.clsEditionEtatComptabilite();
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
            //--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            clsObjetEnvoi.OE_PARAM = new string[] {  };

            clsEditionEtatComptabilite.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatComptabilite.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
            clsEditionEtatComptabilite.DateJourneeComptable1 = Objet.DateJourneeComptable1.ToString();
            clsEditionEtatComptabilite.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatComptabilite.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatComptabilite.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
            clsEditionEtatComptabilite.TYPEETAT = Objet.TYPEETAT;
            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatComptabiliteWSBLL.pvgETATIMMOBILISATIONAMORTISSEMENT(clsDonnee, clsEditionEtatComptabilite, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    string exportFilename = "";
                    string URL_ETAT = "";

                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["URL_ETAT"] = URL_ETAT;
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    // }
                }
                else
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
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
                clsDonnee.pvgDeConnectionBase();
            }
            return json;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public string pvgETATMICEFFETCHEQUE(ZenithWebServeur.DTO.clsEditionEtatComptabilite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.BOJ.clsEditionEtatComptabilite();
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
            //--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            clsObjetEnvoi.OE_PARAM = new string[] { };

            clsEditionEtatComptabilite.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatComptabilite.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
            clsEditionEtatComptabilite.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatComptabilite.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatComptabilite.PL_CODENUMCOMPTE = Objet.PL_CODENUMCOMPTE.ToString();
            clsEditionEtatComptabilite.DateJourneeComptable1 = Objet.DateJourneeComptable1.ToString();
            clsEditionEtatComptabilite.DateJourneeComptable2 = Objet.DateJourneeComptable2.ToString();
            clsEditionEtatComptabilite.TYPEETAT = Objet.TYPEETAT.ToString();

            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatComptabiliteWSBLL.pvgETATMICEFFETCHEQUE(clsDonnee, clsEditionEtatComptabilite, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    string exportFilename = "";
                    string URL_ETAT = "";

                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["URL_ETAT"] = URL_ETAT;
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    // }
                }
                else
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
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
                clsDonnee.pvgDeConnectionBase();
            }
            return json;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public string pvgETATORDREVIREMENT(ZenithWebServeur.DTO.clsEditionEtatComptabilite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.BOJ.clsEditionEtatComptabilite();
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
            //--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            clsObjetEnvoi.OE_PARAM = new string[] { };

            clsEditionEtatComptabilite.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatComptabilite.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
            clsEditionEtatComptabilite.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatComptabilite.DateJourneeComptable1 = Objet.DateJourneeComptable1.ToString();
            clsEditionEtatComptabilite.DateJourneeComptable2 = Objet.DateJourneeComptable2.ToString();
            clsEditionEtatComptabilite.STATUT = Objet.STATUT.ToString();
            clsEditionEtatComptabilite.OV_NUMEROORDREVIREMENT = Objet.OV_NUMEROORDREVIREMENT.ToString();
            clsEditionEtatComptabilite.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatComptabilite.ET_TYPEETAT = Objet.ET_TYPEETAT.ToString();

            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatComptabiliteWSBLL.pvgETATORDREVIREMENT(clsDonnee, clsEditionEtatComptabilite, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    string exportFilename = "";
                    string URL_ETAT = "";

                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["URL_ETAT"] = URL_ETAT;
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    // }
                }
                else
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
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
                clsDonnee.pvgDeConnectionBase();
            }
            return json;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> pvgChargerDansDataSetPourComboAffichage(List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> Objet)
        {
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    clsEditionEtatComptabilites = TestChampObligatoireInsert(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsEditionEtatComptabilites[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsEditionEtatComptabilites;
            //    //--TEST CONTRAINTE
            //    clsEditionEtatComptabilites = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsEditionEtatComptabilites[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsEditionEtatComptabilites;
            //}
            //clsObjetEnvoi.OE_PARAM = new string[] {};
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            DataSet DataSet = new DataSet();

            try
            {
                string[] vlpChamp1 = new string[] { "OPTIONAFFICHAGECODE", "OPTIONAFFICHAGELIBELLE" };
                DataSet.Tables.Add("TABLE");
                for (int i = 0; i < vlpChamp1.Length; i++)
                {
                    DataSet.Tables[0].Columns.Add(vlpChamp1[i]);
                }
                DataSet.Tables[0].Rows.Add("S", "AFFICHAGE SIMPLE");
                DataSet.Tables[0].Rows.Add("M", "AFFICHAGE EN MILLIER");
                
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in DataSet.Tables[0].Rows)
                    {
                        ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                        clsEditionEtatComptabilite.OPTIONAFFICHAGECODE = row["OPTIONAFFICHAGECODE"].ToString();
                        clsEditionEtatComptabilite.OPTIONAFFICHAGELIBELLE = row["OPTIONAFFICHAGELIBELLE"].ToString();
                        clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                        clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "00";
                        clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "TRUE";
                        clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                        clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
                    }
                }
                else
                {
                    ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                    clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé";
                    clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
                }
            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
            }

            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsEditionEtatComptabilites;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> pvgChargerDansDataSetPourComboStatut(List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> Objet)
        {
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    clsEditionEtatComptabilites = TestChampObligatoireInsert(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsEditionEtatComptabilites[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsEditionEtatComptabilites;
            //    //--TEST CONTRAINTE
            //    clsEditionEtatComptabilites = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsEditionEtatComptabilites[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsEditionEtatComptabilites;
            //}
            //clsObjetEnvoi.OE_PARAM = new string[] {};
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            DataSet DataSet = new DataSet();

            try
            {
                string[] vlpChamp1 = new string[] { "STATUTCODE", "STATUTLIBELLE" };
                DataSet.Tables.Add("TABLE");
                for (int i = 0; i < vlpChamp1.Length; i++)
                {
                    DataSet.Tables[0].Columns.Add(vlpChamp1[i]);
                }
                DataSet.Tables[0].Rows.Add("CLOTURES", "CLOTURES");
                DataSet.Tables[0].Rows.Add("EN SUSPENSION", "EN SUSPENSION");
                DataSet.Tables[0].Rows.Add("EN COURS D'EXECUTION", "EN COURS D'EXECUTION");

                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in DataSet.Tables[0].Rows)
                    {
                        ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                        clsEditionEtatComptabilite.STATUTCODE = row["STATUTCODE"].ToString();
                        clsEditionEtatComptabilite.STATUTLIBELLE = row["STATUTLIBELLE"].ToString();
                        clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                        clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "00";
                        clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "TRUE";
                        clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                        clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
                    }
                }
                else
                {
                    ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                    clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé";
                    clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
                }
            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
            }

            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsEditionEtatComptabilites;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> pvgChargerDansDataSetPourComboGrandLivre(List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> Objet)
        {
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsEditionEtatComptabilite> clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    clsEditionEtatComptabilites = TestChampObligatoireInsert(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsEditionEtatComptabilites[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsEditionEtatComptabilites;
            //    //--TEST CONTRAINTE
            //    clsEditionEtatComptabilites = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsEditionEtatComptabilites[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsEditionEtatComptabilites;
            //}
            //clsObjetEnvoi.OE_PARAM = new string[] {};
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            DataSet DataSet = new DataSet();

            try
            {
                string[] vlpChamp1 = new string[] { "MV_STATUTGLVRE", "MV_STATUTGLVRELIBELLE" };
                DataSet.Tables.Add("TABLE");
                for (int i = 0; i < vlpChamp1.Length; i++)
                {
                    DataSet.Tables[0].Columns.Add(vlpChamp1[i]);
                }
                DataSet.Tables[0].Rows.Add("01", "LETTREES");
                DataSet.Tables[0].Rows.Add("02", "NON LETTREES");

                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in DataSet.Tables[0].Rows)
                    {
                        ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                        clsEditionEtatComptabilite.MV_STATUTGLVRE = row["MV_STATUTGLVRE"].ToString();
                        clsEditionEtatComptabilite.MV_STATUTGLVRELIBELLE = row["MV_STATUTGLVRELIBELLE"].ToString();
                        clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                        clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "00";
                        clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "TRUE";
                        clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                        clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
                    }
                }
                else
                {
                    ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                    clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé";
                    clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
                }
            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsEditionEtatComptabilite clsEditionEtatComptabilite = new ZenithWebServeur.DTO.clsEditionEtatComptabilite();
                clsEditionEtatComptabilite.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsEditionEtatComptabilite.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsEditionEtatComptabilite.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsEditionEtatComptabilite.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsEditionEtatComptabilites = new List<ZenithWebServeur.DTO.clsEditionEtatComptabilite>();
                clsEditionEtatComptabilites.Add(clsEditionEtatComptabilite);
            }

            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsEditionEtatComptabilites;
        }
    }
}
