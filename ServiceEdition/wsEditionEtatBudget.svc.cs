﻿using System;
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
     //REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatBudget" à la fois dans le code, le fichier svc et le fichier de configuration.
     //REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatBudget.svc ou wsEditionEtatBudget.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsEditionEtatBudget : IwsEditionEtatBudget
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsEditionEtatBudgetWSBLL clsEditionEtatBudgetWSBLL = new clsEditionEtatBudgetWSBLL();

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
        public string pvgInsertIntoDatasetEtatBudget(clsEditionEtatBudget Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsEditionEtatBudget clsEditionEtatDepot = new ZenithWebServeur.BOJ.clsEditionEtatBudget();
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
                 Objet.AG_CODEAGENCE,Objet.OP_CODEOPERATEURSAISIE,Objet.OP_CODEOPERATEURVALIDATION,Objet.OP_CODEOPERATEUREDITION,Objet.BT_CODETYPEBUDGET,Objet.BU_CODEBUDGET,Objet.BG_CODEPOSTEBUDGETAIRE,Objet.SR_CODESERVICE,Objet.MONTANT1,Objet.MONTANT2,Objet.DATEVALIDATION1,Objet.DATEVALIDATION2,Objet.DATEDEBUT,Objet.DATEFIN,Objet.TYPEETAT
                };
                
                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                //}
                DataSet = clsEditionEtatBudgetWSBLL.pvgInsertIntoDatasetEtatBudget(clsDonnee, clsObjetEnvoi);
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

        //    ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        //    ///<param name="Objet">Collection de clsInput </param>
        //    ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        //    ///<author>Home Technology</author>
        //    public string pvgChargerDansDataSet(ZenithWebServeur.DTO.clsEditionEtatBudget Objet)
        //    {
        //        DataSet DataSet = new DataSet();
        //        DataTable dt = new DataTable("TABLE");
        //        dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //        dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //        dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //        string json = "";

        //        List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //        List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

        //        ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //        clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //        clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //        clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //        clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


        //        //for (int Idx = 0; Idx < Objet.Count; Idx++)
        //        //{
        //            //--TEST DES CHAMPS OBLIGATOIRES
        //            DataSet = TestChampObligatoireListe(Objet);
        //            //--VERIFICATION DU RESULTAT DU TEST
        //            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //            //--TEST CONTRAINTE
        //            //DataSet = TestTestContrainteListe(Objet);
        //            //--VERIFICATION DU RESULTAT DU TEST
        //            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //        //}

        //    clsObjetEnvoi.OE_PARAM = new string[] {  };

        //        try
        //        {
        //            clsDonnee.pvgDemarrerTransaction();
        //            DataSet = clsEditionEtatBudgetWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
        //             if (DataSet.Tables[0].Rows.Count > 0)
        //            {
        //                DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //                DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //                DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //                for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
        //                {
        //                    DataSet.Tables[0].Rows[i]["SL_CODEMESSAGE"] = "00";
        //                    DataSet.Tables[0].Rows[i]["SL_RESULTAT"] = "TRUE";
        //                    DataSet.Tables[0].Rows[i]["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
        //                }

        //                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //            }
        //            else
        //            {

        //                DataSet = new DataSet();
        //                DataRow dr = dt.NewRow();
        //                dr["SL_CODEMESSAGE"] = "99";
        //                dr["SL_RESULTAT"] = "FALSE";
        //                dr["SL_MESSAGE"] = "Aucun enregistrement n'a été trouvé";
        //                dt.Rows.Add(dr);
        //                DataSet.Tables.Add(dt);
        //                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //            }
        //        }
        //         catch (SqlException SQLEx)
        //        {
        //            DataSet = new DataSet();
        //            DataRow dr = dt.NewRow();
        //            dr["SL_CODEMESSAGE"] = "99";
        //            dr["SL_RESULTAT"] = "FALSE";
        //            dr["SL_MESSAGE"] = (SQLEx.Number == 2601 || SQLEx.Number == 2627) ? clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0003").MS_LIBELLEMESSAGE : SQLEx.Message;
        //            dt.Rows.Add(dr);
        //            DataSet.Tables.Add(dt);
        //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //            //Execution du log
        //            Log.Error(SQLEx.Message, null);
        //        }
        //        catch (Exception SQLEx)
        //        {
        //            DataSet = new DataSet();
        //            DataRow dr = dt.NewRow();
        //            dr["SL_CODEMESSAGE"] = "99";
        //            dr["SL_RESULTAT"] = "FALSE";
        //            dr["SL_MESSAGE"] = SQLEx.Message;
        //            dt.Rows.Add(dr);
        //            DataSet.Tables.Add(dt);
        //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //            //Execution du log
        //            Log.Error(SQLEx.Message, null);

        //        }


        //        finally
        //        {
        //            clsDonnee.pvgTerminerTransaction(true);
        //        }
        //        return json;
        //    }

        //}
    }

}
