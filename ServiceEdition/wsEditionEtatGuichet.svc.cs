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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatGuichet" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatGuichet.svc ou wsEditionEtatGuichet.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsEditionEtatGuichet : IwsEditionEtatGuichet
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsEditionEtatGuichetWSBLL clsEditionEtatGuichetWSBLL = new clsEditionEtatGuichetWSBLL();

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
        public string pvgInsertIntoDatasetBilletage(ZenithWebServeur.DTO.clsEditionEtatGuichet Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";
            
            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatGuichet clsEditionEtatGuichet = new ZenithWebServeur.BOJ.clsEditionEtatGuichet();
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

            clsEditionEtatGuichet.AG_CODEAGENCE = Objet.AG_CODEAGENCE;
            clsEditionEtatGuichet.DATEDEBUT = Objet.DATEDEBUT;
            clsEditionEtatGuichet.DATEFIN = Objet.DATEFIN;
            clsEditionEtatGuichet.PL_CODENUMCOMPTE = Objet.PL_CODENUMCOMPTE;
            clsEditionEtatGuichet.CB_CODECOUPURE = Objet.CB_CODECOUPURE;
            clsEditionEtatGuichet.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR;
            clsEditionEtatGuichet.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION;
            clsEditionEtatGuichet.TYPEETAT = Objet.TYPEETAT;

            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBilletage(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi);
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
        public string pvgInsertIntoDatasetBilletage1(ZenithWebServeur.DTO.clsEditionEtatGuichet Objet)
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
            ZenithWebServeur.BOJ.clsEditionEtatGuichet clsEditionEtatGuichet = new ZenithWebServeur.BOJ.clsEditionEtatGuichet();
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
            
            clsEditionEtatGuichet.AG_CODEAGENCE = Objet.AG_CODEAGENCE;
            clsEditionEtatGuichet.DATEDEBUT = DateTime.Parse(Objet.DATEDEBUT).ToShortDateString();
            clsEditionEtatGuichet.DATEFIN = DateTime.Parse(Objet.DATEFIN).ToShortDateString();
            clsEditionEtatGuichet.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION;
            clsEditionEtatGuichet.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1;
            clsEditionEtatGuichet.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2;
            clsEditionEtatGuichet.TS_CODETYPESCHEMACOMPTABLE = "";
            clsEditionEtatGuichet.JO_CODEJOURNAL = "";
            clsEditionEtatGuichet.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR;
            clsEditionEtatGuichet.TYPEETAT = "R";
            clsEditionEtatGuichet.TYPERETOUR = "";
            clsEditionEtatGuichet.TYPEECRAN = "";
            clsEditionEtatGuichet.SUPPRIMERTABLEINTERMEDIAIRE = "";
            clsEditionEtatGuichet.CB_CODECOUPURE = Objet.CB_CODECOUPURE;
            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBilletage(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi);
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
        public string pvgInsertIntoDatasetBrouillardCaisse(ZenithWebServeur.DTO.clsEditionEtatGuichet Objet)
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
            ZenithWebServeur.BOJ.clsEditionEtatGuichet clsEditionEtatGuichet = new ZenithWebServeur.BOJ.clsEditionEtatGuichet();
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

            clsEditionEtatGuichet.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatGuichet.DATEDEBUT = Objet.DATEDEBUT.ToString();
            clsEditionEtatGuichet.DATEFIN = Objet.DATEFIN.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2.ToString();
            clsEditionEtatGuichet.CB_CODECOUPURE = Objet.CB_CODECOUPURE.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatGuichet.TYPEETAT = Objet.TYPEETAT.ToString();
            clsEditionEtatGuichet.ET_CODETYPEETAT = Objet.ET_CODETYPEETAT.ToString();


            try
            {
                clsDonnee.pvgConnectionBase();
                DataSet = clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBrouillardCaisse(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi);
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

        public string pvgInsertIntoDatasetBrouillardCaisse_second_1(ZenithWebServeur.DTO.clsEditionEtatGuichet Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            dt.Columns.Add(new DataColumn("DATASET", typeof(string)));
            
            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatGuichet clsEditionEtatGuichet = new ZenithWebServeur.BOJ.clsEditionEtatGuichet();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();

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

            clsEditionEtatGuichet.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatGuichet.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
            clsEditionEtatGuichet.DATEDEBUT = Objet.DATEDEBUT.ToString();
            clsEditionEtatGuichet.DATEFIN = Objet.DATEFIN.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2.ToString();
            clsEditionEtatGuichet.MC_NUMPIECE = Objet.MC_NUMPIECE.ToString();
            clsEditionEtatGuichet.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
            clsEditionEtatGuichet.JO_CODEJOURNAL = Objet.JO_CODEJOURNAL.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatGuichet.ST_CODESTATUTLISTE = Objet.ST_CODESTATUTLISTE.ToString();
            clsEditionEtatGuichet.NT_CODENATURE = Objet.NT_CODENATURE.ToString();
            clsEditionEtatGuichet.ET_CODETYPEETAT = Objet.ET_CODETYPEETAT.ToString();
            clsEditionEtatGuichet.TYPEETAT = Objet.TYPEETAT.ToString();
            clsEditionEtatGuichet.TYPERETOUR = Objet.TYPERETOUR.ToString();
            clsEditionEtatGuichet.TYPEECRAN = Objet.TYPEECRAN.ToString();
            clsEditionEtatGuichet.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
            clsEditionEtatGuichet.MC_STATUTGLVRE = Objet.MC_STATUTGLVRE.ToString();


            try
            {
                clsDonnee.pvgConnectionBase();
                //DataSet = clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBrouillardCaisse(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi);
                clsObjetRetour.SetValue(true, clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBrouillardCaisse(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["DATASET"] = clsObjetRetour.OR_DATASET;
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                //if (DataSet.Tables[0].Rows.Count > 0)
                //{
                //    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                //    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                //    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                //    string exportFilename = "";
                //    string URL_ETAT = "";

                //    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                //    DataSet = new DataSet();
                //    DataRow dr = dt.NewRow();
                //    dr["SL_CODEMESSAGE"] = "00";
                //    dr["SL_RESULTAT"] = "TRUE";
                //    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                //    dr["DATASET"] = DataSet;
                //    dr["URL_ETAT"] = URL_ETAT;
                //    dt.Rows.Add(dr);
                //    DataSet.Tables.Add(dt);
                //    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                //    // }
                //}
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

        public string pvgInsertIntoDatasetBrouillardCaisse_second_2(ZenithWebServeur.DTO.clsEditionEtatGuichet Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            dt.Columns.Add(new DataColumn("DATASET", typeof(string)));

            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatGuichet clsEditionEtatGuichet = new ZenithWebServeur.BOJ.clsEditionEtatGuichet();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();

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

            clsEditionEtatGuichet.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatGuichet.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
            clsEditionEtatGuichet.DATEDEBUT = Objet.DATEDEBUT.ToString();
            clsEditionEtatGuichet.DATEFIN = Objet.DATEFIN.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2.ToString();
            clsEditionEtatGuichet.MC_NUMPIECE = Objet.MC_NUMPIECE.ToString();
            clsEditionEtatGuichet.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
            clsEditionEtatGuichet.JO_CODEJOURNAL = Objet.JO_CODEJOURNAL.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatGuichet.ST_CODESTATUTLISTE = Objet.ST_CODESTATUTLISTE.ToString();
            clsEditionEtatGuichet.NT_CODENATURE = Objet.NT_CODENATURE.ToString();
            clsEditionEtatGuichet.ET_CODETYPEETAT = Objet.ET_CODETYPEETAT.ToString();
            clsEditionEtatGuichet.TYPEETAT = Objet.TYPEETAT.ToString();
            clsEditionEtatGuichet.TYPERETOUR = Objet.TYPERETOUR.ToString();
            clsEditionEtatGuichet.TYPEECRAN = Objet.TYPEECRAN.ToString();
            clsEditionEtatGuichet.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
            clsEditionEtatGuichet.MC_STATUTGLVRE = Objet.MC_STATUTGLVRE.ToString();


            try
            {
                clsDonnee.pvgConnectionBase();
                //DataSet = clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBrouillardCaisse(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi);
                clsObjetRetour.SetValue(true, clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBrouillardCaisse(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
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
                    dr["DATASET"] = clsObjetRetour.OR_DATASET;
                    dr["URL_ETAT"] = URL_ETAT;
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                //if (DataSet.Tables[0].Rows.Count > 0)
                //{
                //    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                //    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                //    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                //    string exportFilename = "";
                //    string URL_ETAT = "";

                //    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                //    DataSet = new DataSet();
                //    DataRow dr = dt.NewRow();
                //    dr["SL_CODEMESSAGE"] = "00";
                //    dr["SL_RESULTAT"] = "TRUE";
                //    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                //    dr["DATASET"] = DataSet;
                //    dr["URL_ETAT"] = URL_ETAT;
                //    dt.Rows.Add(dr);
                //    DataSet.Tables.Add(dt);
                //    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                //    // }
                //}
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

        public string pvgInsertIntoDatasetBrouillardCaisse_second_3(clsEditionEtatGuichet Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            dt.Columns.Add(new DataColumn("DATASET", typeof(string)));

            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatGuichet clsEditionEtatGuichet = new ZenithWebServeur.BOJ.clsEditionEtatGuichet();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();

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

            clsEditionEtatGuichet.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
            clsEditionEtatGuichet.DATEDEBUT = Objet.DATEDEBUT.ToString();
            clsEditionEtatGuichet.DATEFIN = Objet.DATEFIN.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE1 = Objet.PL_CODENUMCOMPTE1.ToString();
            clsEditionEtatGuichet.PL_CODENUMCOMPTE2 = Objet.PL_CODENUMCOMPTE2.ToString();
            clsEditionEtatGuichet.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
            clsEditionEtatGuichet.JO_CODEJOURNAL = Objet.JO_CODEJOURNAL.ToString();
            clsEditionEtatGuichet.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
            clsEditionEtatGuichet.ST_CODESTATUTLISTE = Objet.ST_CODESTATUTLISTE.ToString();
            clsEditionEtatGuichet.NT_CODENATURE = Objet.NT_CODENATURE.ToString();
            clsEditionEtatGuichet.TYPEETAT = Objet.TYPEETAT.ToString();
            clsEditionEtatGuichet.TYPERETOUR = Objet.TYPERETOUR.ToString();
            clsEditionEtatGuichet.TYPEECRAN = Objet.TYPEECRAN.ToString();
            clsEditionEtatGuichet.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
            
            try
            {
                clsDonnee.pvgConnectionBase();
                clsObjetRetour.SetValue(true, clsEditionEtatGuichetWSBLL.pvgInsertIntoDatasetBrouillardCaisse(clsDonnee, clsEditionEtatGuichet, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
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
    }
}
