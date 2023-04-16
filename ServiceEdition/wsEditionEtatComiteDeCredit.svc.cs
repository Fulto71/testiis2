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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatComiteDeCredit" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatComiteDeCredit.svc ou wsEditionEtatComiteDeCredit.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsEditionEtatComiteDeCredit : IwsEditionEtatComiteDeCredit
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsEditionEtatComiteDeCreditWSBLL clsEditionEtatComiteDeCreditWSBLL = new clsEditionEtatComiteDeCreditWSBLL();

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
        public string pvgMajEtatComiteCredit(clsEditionEtatComiteDeCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";
            
            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            ZenithWebServeur.BOJ.clsEditionEtatComiteDeCredit clsEditionEtatComiteDeCredit = new ZenithWebServeur.BOJ.clsEditionEtatComiteDeCredit();

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

            try
            {
                clsDonnee.pvgDemarrerTransaction();

                clsEditionEtatComiteDeCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatComiteDeCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable1 = Objet.DateJourneeComptable1.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable2 = Objet.DateJourneeComptable2.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable3 = Objet.DateJourneeComptable3.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable4 = Objet.DateJourneeComptable4.ToString();
                clsEditionEtatComiteDeCredit.TypeAffichage = Objet.TypeAffichage.ToString();
                clsEditionEtatComiteDeCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatComiteDeCredit.TYPEETAT = Objet.TYPEETAT.ToString();

                clsObjetRetour.SetValue(true, clsEditionEtatComiteDeCreditWSBLL.pvgMajEtatComiteCredit(clsDonnee, clsEditionEtatComiteDeCredit, clsObjetEnvoi));
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


        public string pvgInsertIntoDataSetDocumentEtatComiteDeCredit(clsEditionEtatComiteDeCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            ZenithWebServeur.BOJ.clsEditionEtatComiteDeCredit clsEditionEtatComiteDeCredit = new ZenithWebServeur.BOJ.clsEditionEtatComiteDeCredit();

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

            try
            {
                clsDonnee.pvgDemarrerTransaction();

                clsEditionEtatComiteDeCredit.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatComiteDeCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatComiteDeCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatComiteDeCredit.PD_CODETYPEPRODUIT = Objet.PD_CODETYPEPRODUIT.ToString();
                clsEditionEtatComiteDeCredit.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable1 = Objet.DateJourneeComptable1.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable2 = Objet.DateJourneeComptable2.ToString();
                clsEditionEtatComiteDeCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatComiteDeCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatComiteDeCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();
                
                DataSet = clsEditionEtatComiteDeCreditWSBLL.pvgInsertIntoDataSetDocumentEtatComiteDeCredit(clsDonnee, clsEditionEtatComiteDeCredit, clsObjetEnvoi);
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
                clsDonnee.pvgTerminerTransaction(true);
            }
            return json;
        }

        public string pvgInsertIntoDataSetDocumentEtatComiteDeCredit2(clsEditionEtatComiteDeCredit Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            ZenithWebServeur.BOJ.clsEditionEtatComiteDeCredit clsEditionEtatComiteDeCredit = new ZenithWebServeur.BOJ.clsEditionEtatComiteDeCredit();

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

            try
            {
                clsDonnee.pvgDemarrerTransaction();

                clsEditionEtatComiteDeCredit.SUPPRIMERTABLEINTERMEDIAIRE = Objet.SUPPRIMERTABLEINTERMEDIAIRE.ToString();
                clsEditionEtatComiteDeCredit.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsEditionEtatComiteDeCredit.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsEditionEtatComiteDeCredit.PD_CODETYPEPRODUIT = Objet.PD_CODETYPEPRODUIT.ToString();
                clsEditionEtatComiteDeCredit.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable1 = Objet.DateJourneeComptable1.ToString();
                clsEditionEtatComiteDeCredit.DateJourneeComptable2 = Objet.DateJourneeComptable2.ToString();
                clsEditionEtatComiteDeCredit.OP_CODEOPERATEUREDITION = Objet.OP_CODEOPERATEUREDITION.ToString();
                clsEditionEtatComiteDeCredit.TYPEETAT = Objet.TYPEETAT.ToString();
                clsEditionEtatComiteDeCredit.TYPERETOUR = Objet.TYPERETOUR.ToString();


                clsObjetRetour.SetValue(true, clsEditionEtatComiteDeCreditWSBLL.pvgInsertIntoDataSetDocumentEtatComiteDeCredit(clsDonnee, clsEditionEtatComiteDeCredit, clsObjetEnvoi));
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
               /* DataSet = clsEditionEtatComiteDeCreditWSBLL.pvgInsertIntoDataSetDocumentEtatComiteDeCredit(clsDonnee, clsEditionEtatComiteDeCredit, clsObjetEnvoi);
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
                clsDonnee.pvgTerminerTransaction(true);
            }
            return json;
        }


    }
}
