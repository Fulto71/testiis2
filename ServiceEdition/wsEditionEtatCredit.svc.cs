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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatCredit" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatCredit.svc ou wsEditionEtatCredit.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsEditionEtatCredit : IwsEditionEtatCredit
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsEditionEtatCreditWSBLL clsEditionEtatCreditWSBLL = new clsEditionEtatCreditWSBLL();

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
        ///<author>Home Technology</author>
        public string pvgInsertIntoDatasetConventionCredit(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                clsEditionEtatCredit.CR_CODECREDIT = Objet.CR_CODECREDIT.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatCreditWSBLL.pvgInsertIntoDatasetConventionCredit(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;
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

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<author>Home Technology</author>
        public string pvgInsertIntoDatasetConventionCredit2(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                clsEditionEtatCredit.CR_CODECREDIT = Objet.CR_CODECREDIT.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                /*   clsObjetRetour.SetValue(true, clsEditionEtatCreditWSBLL.pvgInsertIntoDatasetConventionCredit(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi));
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
                   }*/


                DataSet = clsEditionEtatCreditWSBLL.pvgInsertIntoDatasetConventionCredit(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;
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

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<author>Home Technology</author>
        public string pvgInsertIntoDataSetCredit(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatCredit.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatCredit.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatCredit.PD_CODETYPEPRODUIT = Objet.PD_CODETYPEPRODUIT.ToString();
                clsEditionEtatCredit.PT_CODEPRODUIT = Objet.PT_CODEPRODUIT.ToString();
                clsEditionEtatCredit.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.TERMERETARD = Objet.TERMERETARD.ToString();
                clsEditionEtatCredit.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                clsEditionEtatCredit.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatCredit.GR_CODEGROUPEAPPARTENANCE = Objet.GR_CODEGROUPEAPPARTENANCE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatCredit.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatCredit.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatCredit.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatCredit.OF_CODEOBJETFINANCEMENT = Objet.OF_CODEOBJETFINANCEMENT.ToString();
                clsEditionEtatCredit.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatCredit.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatCredit.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatCredit.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                clsEditionEtatCredit.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                clsEditionEtatCredit.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatCredit.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatCredit.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatCredit.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                clsEditionEtatCredit.PREFIXE = Objet.PREFIXE.ToString();
                clsEditionEtatCredit.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatCredit.OP_AGENTCREDIT = Objet.OP_AGENTCREDIT.ToString();
                clsEditionEtatCredit.ST_CODESTATUTCLIENT = Objet.ST_CODESTATUTCLIENT.ToString();
                

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatCreditWSBLL.pvgInsertIntoDataSetCredit(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;
                    string exportFilename = "";
                    string URL_ETAT = "";

                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "Opération réalisée avec succès !!!";
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
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé !!!";
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
                clsDonnee.pvgTerminerTransaction(true);
            }
            return json;
        }


        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<author>Home Technology</author>
        public string pvgInsertIntoDataSetCreditGarentieAvalListe(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatCredit.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatCredit.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatCredit.PD_CODETYPEPRODUIT = Objet.PD_CODETYPEPRODUIT.ToString();
                clsEditionEtatCredit.PT_CODEPRODUIT = Objet.PT_CODEPRODUIT.ToString();
                clsEditionEtatCredit.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.TERMERETARD = Objet.TERMERETARD.ToString();
                clsEditionEtatCredit.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                clsEditionEtatCredit.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatCredit.GR_CODEGROUPEAPPARTENANCE = Objet.GR_CODEGROUPEAPPARTENANCE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatCredit.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatCredit.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatCredit.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatCredit.OF_CODEOBJETFINANCEMENT = Objet.OF_CODEOBJETFINANCEMENT.ToString();
                clsEditionEtatCredit.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatCredit.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatCredit.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatCredit.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                clsEditionEtatCredit.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                clsEditionEtatCredit.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatCredit.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatCredit.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatCredit.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                clsEditionEtatCredit.PREFIXE = Objet.PREFIXE.ToString();
                clsEditionEtatCredit.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatCredit.OP_AGENTCREDIT = Objet.OP_AGENTCREDIT.ToString();


                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatCreditWSBLL.pvgInsertIntoDataSetCreditGarentieAvalListe(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;
                    string exportFilename = "";
                    string URL_ETAT = "";

                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "Opération réalisée avec succès !!!";
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
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé !!!";
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
                clsDonnee.pvgTerminerTransaction(true);
            }
            return json;
        }



        public string pvgInsertIntoDataSetCredit_second(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                clsDonnee.pvgConnectionBase();
                //clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatCredit.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatCredit.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatCredit.PD_CODETYPEPRODUIT = Objet.PD_CODETYPEPRODUIT.ToString();
                clsEditionEtatCredit.PT_CODEPRODUIT = Objet.PT_CODEPRODUIT.ToString();
                clsEditionEtatCredit.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.TERMERETARD = Objet.TERMERETARD.ToString();
                clsEditionEtatCredit.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                clsEditionEtatCredit.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatCredit.GR_CODEGROUPEAPPARTENANCE = Objet.GR_CODEGROUPEAPPARTENANCE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatCredit.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatCredit.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatCredit.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatCredit.OF_CODEOBJETFINANCEMENT = Objet.OF_CODEOBJETFINANCEMENT.ToString();
                clsEditionEtatCredit.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatCredit.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatCredit.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatCredit.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                clsEditionEtatCredit.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                clsEditionEtatCredit.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatCredit.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatCredit.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatCredit.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                clsEditionEtatCredit.PREFIXE = Objet.PREFIXE.ToString();
                clsEditionEtatCredit.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatCredit.OP_AGENTCREDIT = Objet.OP_AGENTCREDIT.ToString();
                clsEditionEtatCredit.ST_CODESTATUTCLIENT = Objet.ST_CODESTATUTCLIENT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                clsObjetRetour.SetValue(true, clsEditionEtatCreditWSBLL.pvgInsertIntoDataSetCredit(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    if(clsEditionEtatCredit.SUPPRIMERTABLEINTERMEDIAIRE == "O")
                    {
                        DataSet = new DataSet();
                        DataSet = clsObjetRetour.OR_DATASET;

                        DataRow dr = dt.NewRow();
                        dr["SL_CODEMESSAGE"] = "99";
                        dr["SL_RESULTAT"] = "FALSE";
                        dr["SL_MESSAGE"] = "";
                        dt.Rows.Add(dr);
                        DataSet.Tables.Add(dt);
                        json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    } else
                    {
                        DataSet = new DataSet();
                        DataRow dr = dt.NewRow();
                        dr["SL_CODEMESSAGE"] = "00";
                        dr["SL_RESULTAT"] = "TRUE";
                        dr["SL_MESSAGE"] = "Opération réalisée avec succès !!!";
                        dt.Rows.Add(dr);
                        DataSet.Tables.Add(dt);
                        json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    }
                    // }
                }
                else
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Aucun enregistrement trouvé !!!";
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
        ///<author>Home Technology</author>
        public string pvgMAJETATCREDIT(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatCredit.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatCredit.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatCredit.PD_CODETYPEPRODUIT = Objet.PD_CODETYPEPRODUIT.ToString();
                clsEditionEtatCredit.PT_CODEPRODUIT = Objet.PT_CODEPRODUIT.ToString();
                clsEditionEtatCredit.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.TERMERETARD = Objet.TERMERETARD.ToString();
                clsEditionEtatCredit.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                clsEditionEtatCredit.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatCredit.GR_CODEGROUPEAPPARTENANCE = Objet.GR_CODEGROUPEAPPARTENANCE.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatCredit.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatCredit.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatCredit.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatCredit.OF_CODEOBJETFINANCEMENT = Objet.OF_CODEOBJETFINANCEMENT.ToString();
                clsEditionEtatCredit.OP_AGENTCREDIT = Objet.OP_AGENTCREDIT.ToString();
                clsEditionEtatCredit.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatCredit.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatCredit.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatCredit.MONTANT2 = Objet.MONTANT2.ToString();

                //clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsEditionEtatCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                //clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                //clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                //clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                //clsEditionEtatCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                //clsEditionEtatCredit.MONTANT1 = Objet.MONTANT1.ToString();
                //clsEditionEtatCredit.MONTANT2 = Objet.MONTANT2.ToString();
                //clsEditionEtatCredit.TYPEECRAN = Objet.TYPEECRAN.ToString();
                //clsEditionEtatCredit.PREFIXE = Objet.PREFIXE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatCreditWSBLL.pvgMAJETATCREDIT(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi);
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

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<author>Home Technology</author>
        public string pvgMAJETATCREDIT1(clsEditionEtatCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatCredit clsEditionEtatCreditDTO in Objet)
                //{

                clsEditionEtatCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatCredit.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatCredit.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatCredit.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatCredit.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatCredit.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatCredit.PREFIXE = Objet.PREFIXE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatCreditWSBLL.pvgMAJETATCREDIT(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi);
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


        public List<clsEditionEtatCreditRetoursHistogramme> pvgInsertIntoDataSetCreditHistogramme(string AG_CODEAGENCE, string PV_CODEPOINTVENTE, string TM_CODEMEMBRE, string SX_CODESEXE, string PD_CODETYPEPRODUIT, string PT_CODEPRODUIT, string PS_CODESOUSPRODUIT,
           string DATEDEBUT, string DATEFIN, string TERMERETARD, string TI_IDTIERS, string GR_CODEGROUPE, string GR_CODEGROUPEAPPARTENANCE, string OP_CODEOPERATEUREDITION,
           string TYPEETAT, string TYPERETOUR, string TA_CODETYPEACTIVITE, string AT_CODEACTIVITE, string AC_CODEACTIVITE, string OF_CODEOBJETFINANCEMENT, string MONTANT1,
           string MONTANT2, string CM_IDCOMMERCIAL, string OP_AGENTDECOLLECTEETDECREDIT, string OP_GESTIONNAIRECOMPTE, string TYPEECRAN, string SC_CODEGROUPE, string GM_CODESEGMENT,
           string GT_CODETYPECLIENT, string OP_AGENTCREDIT)

        //"@OP_AGENTDECOLLECTEETDECREDIT", "@OP_GESTIONNAIRECOMPTE", "@TYPEECRAN", "@SC_CODEGROUPE", "@GM_CODESEGMENT", "@GT_CODETYPECLIENT", "@CODEDECRYPTAGE", "@OP_AGENTCREDIT"
        {
            List<clsEditionEtatCreditRetoursHistogramme> clsEditionEtatCreditRetourss = new List<clsEditionEtatCreditRetoursHistogramme>();

            ZenithWebServeur.BOJ.clsObjetRetour clsObjetRetour = new ZenithWebServeur.BOJ.clsObjetRetour();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatCredit clsEditionEtatCredit = new ZenithWebServeur.BOJ.clsEditionEtatCredit();
            clsEditionEtatCredit.AG_CODEAGENCE = AG_CODEAGENCE;
            clsEditionEtatCredit.PV_CODEPOINTVENTE = PV_CODEPOINTVENTE;
            clsEditionEtatCredit.TM_CODEMEMBRE = TM_CODEMEMBRE;
            clsEditionEtatCredit.SX_CODESEXE = SX_CODESEXE;
            clsEditionEtatCredit.PD_CODETYPEPRODUIT = PD_CODETYPEPRODUIT;
            clsEditionEtatCredit.PT_CODEPRODUIT = PT_CODEPRODUIT;
            clsEditionEtatCredit.PS_CODESOUSPRODUIT = PS_CODESOUSPRODUIT;
            clsEditionEtatCredit.DATEDEBUT = DATEDEBUT;
            clsEditionEtatCredit.DATEFIN = DATEFIN;
            clsEditionEtatCredit.TERMERETARD = TERMERETARD;
            clsEditionEtatCredit.TI_IDTIERS = TI_IDTIERS;
            clsEditionEtatCredit.GR_CODEGROUPE = GR_CODEGROUPE;
            clsEditionEtatCredit.GR_CODEGROUPEAPPARTENANCE = GR_CODEGROUPEAPPARTENANCE;
            clsEditionEtatCredit.OP_CODEOPERATEUREDITION = OP_CODEOPERATEUREDITION;
            clsEditionEtatCredit.TYPEETAT = TYPEETAT;
            clsEditionEtatCredit.TYPERETOUR = TYPERETOUR;
            clsEditionEtatCredit.TA_CODETYPEACTIVITE = TA_CODETYPEACTIVITE;
            clsEditionEtatCredit.AT_CODEACTIVITE = AT_CODEACTIVITE;
            clsEditionEtatCredit.AC_CODEACTIVITE = AC_CODEACTIVITE;
            clsEditionEtatCredit.OF_CODEOBJETFINANCEMENT = OF_CODEOBJETFINANCEMENT;
            clsEditionEtatCredit.MONTANT1 = MONTANT1;
            clsEditionEtatCredit.MONTANT2 = MONTANT2;
            clsEditionEtatCredit.CM_IDCOMMERCIAL = CM_IDCOMMERCIAL;

            clsEditionEtatCredit.OP_AGENTDECOLLECTEETDECREDIT = OP_AGENTDECOLLECTEETDECREDIT;
            clsEditionEtatCredit.OP_GESTIONNAIRECOMPTE = OP_GESTIONNAIRECOMPTE;
            clsEditionEtatCredit.TYPEECRAN = TYPEECRAN;
            clsEditionEtatCredit.SC_CODEGROUPE = SC_CODEGROUPE;
            clsEditionEtatCredit.GM_CODESEGMENT = GM_CODESEGMENT;
            clsEditionEtatCredit.GT_CODETYPECLIENT = GT_CODETYPECLIENT;
            clsEditionEtatCredit.OP_AGENTCREDIT = OP_AGENTCREDIT;
            try
            {
                clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
                clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
                clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
                clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;
                clsDonnee.pvgDemarrerTransaction();

                clsObjetRetour.SetValue(true, clsEditionEtatCreditWSBLL.pvgInsertIntoDataSetCreditHistogramme(clsDonnee, clsEditionEtatCredit, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee,
                   "GNE0069").MS_LIBELLEMESSAGE);
                if (clsObjetRetour.OR_BOOLEEN)
                {

                    foreach (DataRow row in clsObjetRetour.OR_DATASET.Tables[0].Rows)
                    {

                        //private string _ET_LIBELLEMOIS = "";
                        //private string _ET_LIBELLEANNEE = "";
                        //private string _ET_NOMBRECOMPTE = "0";

                        clsEditionEtatCreditRetoursHistogramme clsEditionEtatCreditRetours = new clsEditionEtatCreditRetoursHistogramme();
                        clsEditionEtatCreditRetours.AT_CODEACTIVITE = row["AT_CODEACTIVITE"].ToString();
                        clsEditionEtatCreditRetours.AT_LIBELLE = row["AT_LIBELLE"].ToString();
                        clsEditionEtatCreditRetours.AC_LIBELLE = row["AT_LIBELLE"].ToString();
                        if (row["AC_VALEUR"].ToString() != "")
                            clsEditionEtatCreditRetours.AC_VALEUR = Math.Abs(double.Parse(row["AC_VALEUR"].ToString())).ToString();
                        if (row["AC_VALEURTOTAL"].ToString() != "")
                            clsEditionEtatCreditRetours.AC_VALEURTOTAL = Math.Abs(double.Parse(row["AC_VALEURTOTAL"].ToString())).ToString();
                        if (row["AC_TAUX"].ToString() != "")
                            clsEditionEtatCreditRetours.AC_TAUX = double.Parse(row["AC_TAUX"].ToString()).ToString();
                        clsEditionEtatCreditRetours.SL_CODEMESSAGE = "0000";
                        clsEditionEtatCreditRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                        clsEditionEtatCreditRetours.SL_RESULTAT = "TRUE";
                        clsEditionEtatCreditRetourss.Add(clsEditionEtatCreditRetours);

                    }
                }
                else
                {
                    clsEditionEtatCreditRetoursHistogramme clsEditionEtatCreditRetours = new clsEditionEtatCreditRetoursHistogramme();
                    clsEditionEtatCreditRetours.SL_CODEMESSAGE = "9999";
                    clsEditionEtatCreditRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                    clsEditionEtatCreditRetours.SL_RESULTAT = "FALSE";
                    clsEditionEtatCreditRetourss.Add(clsEditionEtatCreditRetours);
                    return clsEditionEtatCreditRetourss;
                }
                // clsObjetRetour.OR_DATASET

            }

            catch (SqlException SQLEx)
            {
                clsObjetRetour.SetValueMessage(false, SQLEx.Message);
                clsEditionEtatCreditRetoursHistogramme clsEditionEtatCreditRetours = new clsEditionEtatCreditRetoursHistogramme();
                clsEditionEtatCreditRetours.SL_CODEMESSAGE = "9999";
                clsEditionEtatCreditRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                clsEditionEtatCreditRetours.SL_RESULTAT = "FALSE";
                clsEditionEtatCreditRetourss.Add(clsEditionEtatCreditRetours);
                return clsEditionEtatCreditRetourss;
            }
            finally
            {
                clsDonnee.pvgTerminerTransaction(!clsObjetRetour.OR_BOOLEEN);
            }
            return clsEditionEtatCreditRetourss;

        }
    }
}
