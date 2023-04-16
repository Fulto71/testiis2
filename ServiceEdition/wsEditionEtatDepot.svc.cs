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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatDepot" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatDepot.svc ou wsEditionEtatDepot.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsEditionEtatDepot : IwsEditionEtatDepot
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsEditionEtatDepotWSBLL clsEditionEtatDepotWSBLL = new clsEditionEtatDepotWSBLL();

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
        //public string pvgInsertIntoDataSetTiers(clsEditionEtatDepot Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    //for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    //{
        //    //--TEST DES CHAMPS OBLIGATOIRES
        //    //DataSet = TestChampObligatoireInsertpvgProvisionDebiteursDiversReprise(Objet);
        //    ////--VERIFICATION DU RESULTAT DU TEST
        //    //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    ////--TEST DES TYPES DE DONNEES
        //    //DataSet = TestTypeDonnee(Objet);
        //    ////--VERIFICATION DU RESULTAT DU TEST
        //    //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    ////--TEST CONTRAINTE
        //    //DataSet = TestTestContrainteListe(Objet);
        //    ////--VERIFICATION DU RESULTAT DU TEST
        //    //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //}

        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        //clsDonnee.pvgConnectionBase();
        //        clsDonnee.pvgDemarrerTransaction();
        //        clsObjetEnvoi.OE_PARAM = new string[] {

        //        };

        //        //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
        //        //{

        //        clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
        //        clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
        //        clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
        //        clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
        //        clsEditionEtatDepot.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
        //        clsEditionEtatDepot.MC_DATEPIECE = Objet.MC_DATEPIECE.ToString();
        //        clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
        //        //}
        //        clsObjetRetour.SetValue(true, clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetTiers(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi));
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            DataSet = new DataSet();
        //            DataRow dr = dt.NewRow();
        //            dr["SL_CODEMESSAGE"] = "00";
        //            dr["SL_RESULTAT"] = "TRUE";
        //            dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
        //            dt.Rows.Add(dr);
        //            DataSet.Tables.Add(dt);
        //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //        }
        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        DataSet = new DataSet();
        //        DataRow dr = dt.NewRow();
        //        dr["SL_CODEMESSAGE"] = "99";
        //        dr["SL_RESULTAT"] = "FALSE";
        //        dr["SL_MESSAGE"] = SQLEx.Message;
        //        dt.Rows.Add(dr);
        //        DataSet.Tables.Add(dt);
        //        json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        DataSet = new DataSet();
        //        DataRow dr = dt.NewRow();
        //        dr["SL_CODEMESSAGE"] = "99";
        //        dr["SL_RESULTAT"] = "FALSE";
        //        dr["SL_MESSAGE"] = SQLEx.Message;
        //        dt.Rows.Add(dr);
        //        DataSet.Tables.Add(dt);
        //        json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //    }

        //    finally
        //    {
        //        bool OR_BOOLEEN = true;
        //        if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE")
        //        {
        //            OR_BOOLEEN = false;
        //        }
        //        clsDonnee.pvgTerminerTransaction(!OR_BOOLEEN);
        //        //clsDonnee.pvgDeConnectionBase();
        //    }

        //    return json;
        //}


        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<author>Home Technology</author>
        public string pvgInsertIntoDataSetCompte(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.AG_CODEAGENCE,
                    Objet.PV_CODEPOINTVENTE,
                    Objet.OP_CODEOPERATEUR,
                    Objet.TM_CODEMEMBRE,
                    Objet.SX_CODESEXE,
                    Objet.PS_CODESOUSPRODUIT,
                    Objet.DATEDEBUT,
                    Objet.DATEFIN,
                    Objet.MONTANT1,
                    Objet.MONTANT2,
                    Objet.OP_CODEOPERATEUREDITION,
                    Objet.TYPEETAT,
                    Objet.CM_IDCOMMERCIAL,
                    Objet.GR_CODEGROUPE,
                    Objet.OP_AGENTDECOLLECTEETDECREDIT,
                    Objet.OP_GESTIONNAIRECOMPTE,
                    Objet.TS_CODETYPESCHEMACOMPTABLE,
                    Objet.SC_CODEGROUPE,
                    Objet.GM_CODESEGMENT,
                    Objet.GT_CODETYPECLIENT
                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatDepot.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = "1";
                clsEditionEtatDepot.TYPEECRAN = "";
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatDepot.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                clsEditionEtatDepot.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                //clsEditionEtatDepot.PREFIXE = Objet.PREFIXE.ToString();

                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = clsEditionEtatDepot.PS_CODESOUSPRODUIT;
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatDepot.ST_CODESTATUTCLIENT = Objet.ST_CODESTATUTCLIENT.ToString();
                clsEditionEtatDepot.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                
                //  clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.Replace("/", "-");
                //  clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.Replace("/", "-");

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCompte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
        public string pvgInsertIntoDataSetTrancheParDepot1(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                //clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                //clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                //clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                //clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                //clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                //clsEditionEtatDepot.TYPEECRAN = Objet.TYPEECRAN.ToString();
                //clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                //clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                //clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                //clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = "";
                clsEditionEtatDepot.SX_CODESEXE = "";
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatDepot.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = "";
                clsEditionEtatDepot.TYPEECRAN = "I";
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.DATEOPERATION = clsEditionEtatDepot.DATEOPERATION;
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = clsEditionEtatDepot.PS_CODESOUSPRODUIT;
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetTrancheParDepot(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
        public string pvgInsertIntoDataSetCompte1(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                //clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                //clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                //clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                //clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                //clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                //clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                //clsEditionEtatDepot.MONTANT1 = Objet.MONTANT1.ToString();
                //clsEditionEtatDepot.MONTANT2 = Objet.MONTANT2.ToString();
                //clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                //clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                //clsEditionEtatDepot.TYPERETOUR = "1";
                //clsEditionEtatDepot.TYPEECRAN = "";
                //clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                //clsEditionEtatDepot.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                //clsEditionEtatDepot.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                //clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                //clsEditionEtatDepot.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                //clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                //clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                //clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                ////clsEditionEtatDepot.PREFIXE = Objet.PREFIXE.ToString();

                //clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                //clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                //clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = clsEditionEtatDepot.PS_CODESOUSPRODUIT;//Objet.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT.ToString();
                //clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                //clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                //clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();


                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = "";
                clsEditionEtatDepot.SX_CODESEXE = "";
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatDepot.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = "1";
                clsEditionEtatDepot.TYPEECRAN = "I";
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.DATEOPERATION = clsEditionEtatDepot.DATEOPERATION;
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = clsEditionEtatDepot.PS_CODESOUSPRODUIT;
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCompte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
        public string pvgInsertIntoDataSetCompteGenerer(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsDonnee.pvgConnectionBase();
                clsObjetEnvoi.OE_PARAM = new string[] {

                };




                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = "";
                clsEditionEtatDepot.SX_CODESEXE = "";
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatDepot.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = "1";
                clsEditionEtatDepot.TYPEECRAN = "";
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.DATEOPERATION = clsEditionEtatDepot.DATEOPERATION;
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = clsEditionEtatDepot.PS_CODESOUSPRODUIT;
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatDepot.ST_CODESTATUTCLIENT = Objet.ST_CODESTATUTCLIENT.ToString();
                clsEditionEtatDepot.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                clsObjetRetour.SetValue(true, clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCompte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi));
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
                bool OR_BOOLEEN = true;
                if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE")
                {
                    OR_BOOLEEN = false;
                }
                //clsDonnee.pvgTerminerTransaction(!OR_BOOLEEN);
                clsDonnee.pvgDeConnectionBase();
            }

            return json;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<author>Home Technology</author>
        public string pvgInsertIntoDataSetCompte2(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.AG_CODEAGENCE,
                    Objet.PV_CODEPOINTVENTE,
                    Objet.OP_CODEOPERATEUR,
                    Objet.TM_CODEMEMBRE,
                    Objet.SX_CODESEXE,
                    Objet.PS_CODESOUSPRODUIT,
                    Objet.DATEDEBUT,
                    Objet.DATEFIN,
                    Objet.MONTANT1,
                    Objet.MONTANT2,
                    Objet.OP_CODEOPERATEUREDITION,
                    Objet.TYPEETAT,
                    Objet.CM_IDCOMMERCIAL,
                    Objet.GR_CODEGROUPE,
                    Objet.OP_AGENTDECOLLECTEETDECREDIT,
                    Objet.OP_GESTIONNAIRECOMPTE,
                    Objet.TS_CODETYPESCHEMACOMPTABLE,
                    Objet.SC_CODEGROUPE,
                    Objet.GM_CODESEGMENT,
                    Objet.GT_CODETYPECLIENT
                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.MONTANT1 = Objet.MONTANT1.ToString();
                clsEditionEtatDepot.MONTANT2 = Objet.MONTANT2.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = "1";
                clsEditionEtatDepot.TYPEECRAN = "";
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsEditionEtatDepot.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                clsEditionEtatDepot.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                //clsEditionEtatDepot.PREFIXE = Objet.PREFIXE.ToString();

                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = Objet.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT.ToString();
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();


                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                clsObjetRetour.SetValue(true, clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCompte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi));
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
               /* DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCompte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
                }*/
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
        public string pvgMAJETATDEPOT(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatDepot.TYPEECRAN = Objet.TYPEECRAN.ToString();
                //clsEditionEtatDepot.PREFIXE = Objet.PREFIXE.ToString();

                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgMAJETATDEPOT(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
        public string pvgInsertIntoDataSetCollecte(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCollecte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
        public string pvgInsertIntoDataSetDecompositionComptePm(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.DATEDEBUT = Objet.DATEDEBUT.ToString();
                clsEditionEtatDepot.DATEFIN = Objet.DATEFIN.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatDepot.TYPERETOUR = Objet.TYPERETOUR.ToString();
                clsEditionEtatDepot.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                clsEditionEtatDepot.ST_CODESTATUTCLIENT = Objet.ST_CODESTATUTCLIENT.ToString();
                
                //clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                //clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                //clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = Objet.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT.ToString();
                //clsEditionEtatDepot.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                //clsEditionEtatDepot.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                //clsEditionEtatDepot.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                //clsEditionEtatDepot.TYPEETAT = Objet.TYPEETAT.ToString();
                //clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                //clsEditionEtatDepot.TYPEECRAN = Objet.TYPEECRAN.ToString();
                //clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                //clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                //clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetDecompositionComptePm(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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
        public string pvgInsertIntoDataSetTrancheParDepot(clsEditionEtatDepot Objet)
        {
            DataSet DataSet1 = new DataSet();
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot1 = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatDepot.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatDepot.DATEOPERATION = Objet.DATEOPERATION.ToString();
                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatDepot.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsEditionEtatDepot.SX_CODESEXE = Objet.SX_CODESEXE.ToString();
                clsEditionEtatDepot.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatDepot.TYPEECRAN = Objet.TYPEECRAN.ToString();
                clsEditionEtatDepot.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsEditionEtatDepot.SC_CODEGROUPE = Objet.SC_CODEGROUPE.ToString();
                clsEditionEtatDepot.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                clsEditionEtatDepot.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                clsEditionEtatDepot.ST_CODESTATUTCLIENT = Objet.ST_CODESTATUTCLIENT.ToString();

                clsEditionEtatDepot1.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
               
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetTrancheParDepot(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
                DataSet1 = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetTrancheDepotSE(clsDonnee, clsEditionEtatDepot1, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0 && DataSet1.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;
                    string exportFilename = "";
                    string URL_ETAT = "";
                    string[] vppFichierSousEtat = new string[] { "TrancheDepotSE.rpt" };
                    DataSet[] vppDataSetSousEtat = new DataSet[] { DataSet1 };
                    
                    URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, vppFichierSousEtat, vppDataSetSousEtat, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);

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
        public string pvgInsertIntoDataSetTrancheDepotSE(clsEditionEtatDepot Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
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
                clsObjetEnvoi.OE_PARAM = new string[] {

                };

                //foreach (ZenithWebServeur.DTO.clsEditionEtatDepot clsEditionEtatDepotDTO in Objet)
                //{

                clsEditionEtatDepot.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                //clsEditionEtatDepot.PREFIXE = Objet.PREFIXE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetTrancheDepotSE(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi);
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


        public List<clsEditionEtatDepotRetoursHistogramme> pvgInsertIntoDataSetCompteHistogramme(string AG_CODEAGENCE, string PV_CODEPOINTVENTE, string OP_CODEOPERATEUR, string TM_CODEMEMBRE, string SX_CODESEXE, string PS_CODESOUSPRODUIT, string DATEDEBUT, string DATEFIN, string MONTANT1, string MONTANT2, string OP_CODEOPERATEUREDITION, string TYPEETAT, string TYPERETOUR, string TYPEECRAN, string CM_IDCOMMERCIAL, string GR_CODEGROUPE, string OP_AGENTDECOLLECTEETDECREDIT, string OP_GESTIONNAIRECOMPTE, string TS_CODETYPESCHEMACOMPTABLE, string SC_CODEGROUPE, string GM_CODESEGMENT, string GT_CODETYPECLIENT, string SUPPRIMERTABLEINTERMEDIAIRE)
        {
            List<clsEditionEtatDepotRetoursHistogramme> clsEditionEtatDepotRetourss = new List<clsEditionEtatDepotRetoursHistogramme>();

            ZenithWebServeur.BOJ.clsObjetRetour clsObjetRetour = new ZenithWebServeur.BOJ.clsObjetRetour();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
            clsEditionEtatDepot.AG_CODEAGENCE = AG_CODEAGENCE;
            clsEditionEtatDepot.PV_CODEPOINTVENTE = PV_CODEPOINTVENTE;
            clsEditionEtatDepot.OP_CODEOPERATEUR = OP_CODEOPERATEUR;
            clsEditionEtatDepot.TM_CODEMEMBRE = TM_CODEMEMBRE;
            clsEditionEtatDepot.SX_CODESEXE = SX_CODESEXE;
            clsEditionEtatDepot.PS_CODESOUSPRODUIT = PS_CODESOUSPRODUIT;
            clsEditionEtatDepot.DATEDEBUT = DATEDEBUT;
            clsEditionEtatDepot.DATEFIN = DATEFIN;
            clsEditionEtatDepot.MONTANT1 = MONTANT1;
            clsEditionEtatDepot.MONTANT2 = MONTANT2;
            clsEditionEtatDepot.OP_CODEOPERATEUR = OP_CODEOPERATEUR;
            clsEditionEtatDepot.OP_CODEOPERATEUREDITION = OP_CODEOPERATEUREDITION;
            clsEditionEtatDepot.TYPEETAT = TYPEETAT;
            clsEditionEtatDepot.TYPERETOUR = TYPERETOUR;
            clsEditionEtatDepot.TYPEETAT = TYPEETAT;
            clsEditionEtatDepot.TYPERETOUR = TYPERETOUR;
            clsEditionEtatDepot.TYPEECRAN = TYPEECRAN;
            clsEditionEtatDepot.TYPEECRAN = TYPEECRAN;
            clsEditionEtatDepot.CM_IDCOMMERCIAL = CM_IDCOMMERCIAL;
            clsEditionEtatDepot.GR_CODEGROUPE = GR_CODEGROUPE;
            clsEditionEtatDepot.OP_AGENTDECOLLECTEETDECREDIT = OP_AGENTDECOLLECTEETDECREDIT;
            clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = OP_GESTIONNAIRECOMPTE;
            clsEditionEtatDepot.TS_CODETYPESCHEMACOMPTABLE = TS_CODETYPESCHEMACOMPTABLE;
            clsEditionEtatDepot.SC_CODEGROUPE = SC_CODEGROUPE;
            clsEditionEtatDepot.GM_CODESEGMENT = GM_CODESEGMENT;
            clsEditionEtatDepot.GT_CODETYPECLIENT = GT_CODETYPECLIENT;
            clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = SUPPRIMERTABLEINTERMEDIAIRE;
            try
            {
                clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
                clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
                clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
                clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;
                clsDonnee.pvgDemarrerTransaction();

                clsObjetRetour.SetValue(true, clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCompteHistogramme(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee,
                   "GNE0069").MS_LIBELLEMESSAGE);
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    if (clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE == "O")
                    {
                        foreach (DataRow row in clsObjetRetour.OR_DATASET.Tables[0].Rows)
                        {

                            //private string _ET_LIBELLEMOIS = "";
                            //private string _ET_LIBELLEANNEE = "";
                            //private string _ET_NOMBRECOMPTE = "0";

                            clsEditionEtatDepotRetoursHistogramme clsEditionEtatDepotRetours = new clsEditionEtatDepotRetoursHistogramme();
                            clsEditionEtatDepotRetours.ET_LIBELLEMOIS = row["ET_LIBELLEMOIS"].ToString();
                            clsEditionEtatDepotRetours.ET_LIBELLEANNEE = row["ET_LIBELLEANNEE"].ToString();
                            if (row["ET_NOMBRECOMPTE"].ToString() != "")
                                clsEditionEtatDepotRetours.ET_NOMBRECOMPTE = double.Parse(row["ET_NOMBRECOMPTE"].ToString()).ToString();
                            clsEditionEtatDepotRetours.SL_CODEMESSAGE = "0000";
                            clsEditionEtatDepotRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                            clsEditionEtatDepotRetours.SL_RESULTAT = "TRUE";
                            clsEditionEtatDepotRetourss.Add(clsEditionEtatDepotRetours);

                        }
                    }
                    else
                    {
                        clsEditionEtatDepotRetoursHistogramme clsEditionEtatDepotRetours = new clsEditionEtatDepotRetoursHistogramme();
                        clsEditionEtatDepotRetours.SL_CODEMESSAGE = "9999";
                        clsEditionEtatDepotRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                        clsEditionEtatDepotRetours.SL_RESULTAT = "FALSE";
                        clsEditionEtatDepotRetourss.Add(clsEditionEtatDepotRetours);
                    }

                }
                // clsObjetRetour.OR_DATASET

            }

            catch (SqlException SQLEx)
            {
                clsObjetRetour.SetValueMessage(false, SQLEx.Message);
                clsEditionEtatDepotRetoursHistogramme clsEditionEtatDepotRetours = new clsEditionEtatDepotRetoursHistogramme();
                clsEditionEtatDepotRetours.SL_CODEMESSAGE = "9999";
                clsEditionEtatDepotRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                clsEditionEtatDepotRetours.SL_RESULTAT = "FALSE";
                clsEditionEtatDepotRetourss.Add(clsEditionEtatDepotRetours);
                return clsEditionEtatDepotRetourss;
            }
            finally
            {
                clsDonnee.pvgTerminerTransaction(!clsObjetRetour.OR_BOOLEEN);
            }
            return clsEditionEtatDepotRetourss;

        }


        public List<clsEditionEtatDepotRetoursTableaudeBord> pvgInsertIntoDataSetTableaudeBord(string AG_CODEAGENCE, string PV_CODEPOINTVENTE, string OP_CODEOPERATEUR, string DATEDEBUT, string DATEFIN, string OP_CODEOPERATEUREDITION, string TYPEETAT)
        {
            List<clsEditionEtatDepotRetoursTableaudeBord> clsEditionEtatDepotRetourss = new List<clsEditionEtatDepotRetoursTableaudeBord>();

            ZenithWebServeur.BOJ.clsObjetRetour clsObjetRetour = new ZenithWebServeur.BOJ.clsObjetRetour();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatDepot clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatDepot();
            clsEditionEtatDepot.AG_CODEAGENCE = AG_CODEAGENCE;
            clsEditionEtatDepot.PV_CODEPOINTVENTE = PV_CODEPOINTVENTE;
            clsEditionEtatDepot.OP_CODEOPERATEUR = OP_CODEOPERATEUR;
            //clsEditionEtatDepot.TM_CODEMEMBRE = TM_CODEMEMBRE;
            //clsEditionEtatDepot.SX_CODESEXE = SX_CODESEXE;
            //clsEditionEtatDepot.PS_CODESOUSPRODUIT = PS_CODESOUSPRODUIT;
            clsEditionEtatDepot.DATEDEBUT = DATEDEBUT;
            clsEditionEtatDepot.DATEFIN = DATEFIN;
            //clsEditionEtatDepot.GR_CODEGROUPE = GR_CODEGROUPE;
            //clsEditionEtatDepot.MONTANT1 = MONTANT1;
            //clsEditionEtatDepot.MONTANT2 = MONTANT2;
            clsEditionEtatDepot.OP_CODEOPERATEUREDITION = OP_CODEOPERATEUREDITION;
            clsEditionEtatDepot.TYPEETAT = TYPEETAT;
            //clsEditionEtatDepot.TYPERETOUR = TYPERETOUR;
            //clsEditionEtatDepot.TYPEECRAN = TYPEECRAN;
            //clsEditionEtatDepot.TYPERETOUR = TYPERETOUR;
            //clsEditionEtatDepot.TYPEECRAN = TYPEECRAN;
            //clsEditionEtatDepot.CM_IDCOMMERCIAL = CM_IDCOMMERCIAL;
            //clsEditionEtatDepot.OP_AGENTDECOLLECTEETDECREDIT = OP_AGENTDECOLLECTEETDECREDIT;
            //clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = OP_GESTIONNAIRECOMPTE;
            //clsEditionEtatDepot.TS_CODETYPESCHEMACOMPTABLE = TS_CODETYPESCHEMACOMPTABLE;
            //clsEditionEtatDepot.OP_GESTIONNAIRECOMPTE = OP_GESTIONNAIRECOMPTE;
            //clsEditionEtatDepot.TS_CODETYPESCHEMACOMPTABLE = TS_CODETYPESCHEMACOMPTABLE;
            //clsEditionEtatDepot.DATEOPERATION = DATEOPERATION;
            //clsEditionEtatDepot.PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT = PS_CODESOUSPRODUITouPD_CODETYPEPRODUIT;
            clsEditionEtatDepot.TA_CODETYPEACTIVITE = "";
            clsEditionEtatDepot.AT_CODEACTIVITE = "";
            //clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = SUPPRIMERTABLEINTERMEDIAIRE;
            //clsEditionEtatDepot.SC_CODEGROUPE = SC_CODEGROUPE;
            //clsEditionEtatDepot.GM_CODESEGMENT = GM_CODESEGMENT;
            //clsEditionEtatDepot.GT_CODETYPECLIENT = GT_CODETYPECLIENT;

            try
            {
                clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
                clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
                clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
                clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;
                clsDonnee.pvgDemarrerTransaction();

                clsObjetRetour.SetValue(true, clsEditionEtatDepotWSBLL.pvgInsertIntoDataSetCollecte(clsDonnee, clsEditionEtatDepot, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee,
                   "GNE0069").MS_LIBELLEMESSAGE);
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE = "O";
                    if (clsEditionEtatDepot.SUPPRIMERTABLEINTERMEDIAIRE == "O")
                    {
                        foreach (DataRow row in clsObjetRetour.OR_DATASET.Tables[0].Rows)
                        {

                            //private string _ET_LIBELLEMOIS = "";
                            //private string _ET_LIBELLEANNEE = "";
                            //private string _ET_NOMBRECOMPTE = "0";

                            clsEditionEtatDepotRetoursTableaudeBord clsEditionEtatDepotRetours = new clsEditionEtatDepotRetoursTableaudeBord();
                            clsEditionEtatDepotRetours.CODEPOSTE = row["CODEPOSTE"].ToString();
                            clsEditionEtatDepotRetours.LIBELLEPOSTE = row["LIBELLEPOSTE"].ToString();
                            clsEditionEtatDepotRetours.CODEDETAIL = row["CODEDETAIL"].ToString();
                            clsEditionEtatDepotRetours.LIBELLEDETAIL = row["LIBELLEDETAIL"].ToString();

                            if (row["SITUATIONJOURREEL"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONJOURREEL = double.Parse(row["SITUATIONJOURREEL"].ToString()).ToString();
                            if (row["SITUATIONJOUROBJECTIF"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONJOUROBJECTIF = double.Parse(row["SITUATIONJOUROBJECTIF"].ToString()).ToString();
                            if (row["SITUATIONJOURTXREEL"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONJOURTXREEL = double.Parse(row["SITUATIONJOURTXREEL"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEMOISREEL"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEMOISREEL = double.Parse(row["SITUATIONCUMULEEMOISREEL"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEMOISOBJECTIF"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEMOISOBJECTIF = double.Parse(row["SITUATIONCUMULEEMOISOBJECTIF"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEMOISTXREEL"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEMOISTXREEL = double.Parse(row["SITUATIONCUMULEEMOISTXREEL"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEANNEEREEL"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEANNEEREEL = double.Parse(row["SITUATIONCUMULEEANNEEREEL"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEANNEEOBJECTIF"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEANNEEOBJECTIF = double.Parse(row["SITUATIONCUMULEEANNEEOBJECTIF"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEANNEETXREEL"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEANNEETXREEL = double.Parse(row["SITUATIONCUMULEEANNEETXREEL"].ToString()).ToString();
                            if (row["SITUATIONJOURENTREES"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONJOURENTREES = double.Parse(row["SITUATIONJOURENTREES"].ToString()).ToString();
                            if (row["SITUATIONJOURSORTIES"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONJOURSORTIES = double.Parse(row["SITUATIONJOURSORTIES"].ToString()).ToString();
                            if (row["SITUATIONJOURSOLDE"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONJOURSOLDE = double.Parse(row["SITUATIONJOURSOLDE"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEMOISENTREES"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEMOISENTREES = double.Parse(row["SITUATIONCUMULEEMOISENTREES"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEMOISSORTIES"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEMOISSORTIES = double.Parse(row["SITUATIONCUMULEEMOISSORTIES"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEMOISSOLDE"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEMOISSOLDE = double.Parse(row["SITUATIONCUMULEEMOISSOLDE"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEANNEEENTREES"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEANNEEENTREES = double.Parse(row["SITUATIONCUMULEEANNEEENTREES"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEANNEESORTIES"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEANNEESORTIES = double.Parse(row["SITUATIONCUMULEEANNEESORTIES"].ToString()).ToString();
                            if (row["SITUATIONCUMULEEANNEESOLDE"].ToString() != "")
                                clsEditionEtatDepotRetours.SITUATIONCUMULEEANNEESOLDE = double.Parse(row["SITUATIONCUMULEEANNEESOLDE"].ToString()).ToString();
                            clsEditionEtatDepotRetours.SL_CODEMESSAGE = "0000";
                            clsEditionEtatDepotRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                            clsEditionEtatDepotRetours.SL_RESULTAT = "TRUE";
                            clsEditionEtatDepotRetourss.Add(clsEditionEtatDepotRetours);

                        }
                    }
                    else
                    {
                        clsEditionEtatDepotRetoursTableaudeBord clsEditionEtatDepotRetours = new clsEditionEtatDepotRetoursTableaudeBord();
                        clsEditionEtatDepotRetours.SL_CODEMESSAGE = "0000";
                        clsEditionEtatDepotRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                        clsEditionEtatDepotRetours.SL_RESULTAT = "TRUE";
                        clsEditionEtatDepotRetourss.Add(clsEditionEtatDepotRetours);
                    }

                }
                // clsObjetRetour.OR_DATASET

            }

            catch (SqlException SQLEx)
            {
                clsObjetRetour.SetValueMessage(false, SQLEx.Message);
                clsEditionEtatDepotRetoursTableaudeBord clsEditionEtatDepotRetours = new clsEditionEtatDepotRetoursTableaudeBord();
                clsEditionEtatDepotRetours.SL_CODEMESSAGE = "9999";
                clsEditionEtatDepotRetours.SL_MESSAGE = clsObjetRetour.OR_MESSAGE;
                clsEditionEtatDepotRetours.SL_RESULTAT = "FALSE";
                clsEditionEtatDepotRetourss.Add(clsEditionEtatDepotRetours);
                return clsEditionEtatDepotRetourss;
            }
            finally
            {
                clsDonnee.pvgTerminerTransaction(!clsObjetRetour.OR_BOOLEEN);
            }
            return clsEditionEtatDepotRetourss;

        }
    }
}
