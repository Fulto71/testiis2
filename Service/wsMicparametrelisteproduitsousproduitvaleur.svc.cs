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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicparametrelisteproduitsousproduitvaleur" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicparametrelisteproduitsousproduitvaleur.svc ou wsMicparametrelisteproduitsousproduitvaleur.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicparametrelisteproduitsousproduitvaleur : IwsMicparametrelisteproduitsousproduitvaleur
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicparametrelisteproduitsousproduitvaleurWSBLL clsMicparametrelisteproduitsousproduitvaleurWSBLL = new clsMicparametrelisteproduitsousproduitvaleurWSBLL();

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

        string vlpResultat = "";

        //physique
        string[] vppSegmentPersonnePhysique = null; // clsMicparametrelisteproduitsousproduitvaleurDTO.vppSegmentPersonnePhysique[0].ToString().Trim().Split(','); //cpsDevCheckedComboBoxEdit5.Properties.GetCheckedItems().ToString().Trim().Split(',');
                                                    //morale
        string[] vppSegmentPersonneMorale = null; //  clsMicparametrelisteproduitsousproduitvaleurDTO.vppSegmentPersonneMorale[0].ToString().Trim().Split(','); // // cpsDevCheckedComboBoxEdit6.Properties.GetCheckedItems().ToString().Trim().Split(',');

        string[] vppAgenceSelectionneesTypeMembrePersonnePhysique = null; //  clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesTypeMembrePersonnePhysique[0].ToString().Trim().Split(','); // //cpsDevCheckedComboBoxEdit3.Properties.GetCheckedItems().ToString().Trim().Split(',');
        string[] vppAgenceSelectionneesTypeMembrePersonneMorale = null; //  clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesTypeMembrePersonneMorale[0].ToString().Trim().Split(','); // //cpsDevCheckedComboBoxEdit4.Properties.GetCheckedItems().ToString().Trim().Split(',');
        string[] vppAgenceSelectionneesSexe = null; //  clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesSexe[0].ToString().Trim().Split(','); // //cpsDevCheckedComboBoxEdit1.Properties.GetCheckedItems().ToString().Trim().Split(',');
        string[] vppAgenceSelectionneesFormeJuridique = null; // clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesFormeJuridique[0].ToString().Trim().Split(','); // // null;


        //AJOUT
        public string pvgAjouterListeParametrageModel(List<clsMicparametrelisteproduitsousproduitvaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur> clsMicparametrelisteproduitsousproduitvaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                DataSet = TestChampObligatoireInsert(Objet[Idx]);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            DataSet = TestTypeDonnee(Objet[Idx]);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            DataSet = TestTestContrainteListe(Objet[Idx]);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
                    clsMicparametrelisteproduitsousproduitvaleur.PS_CODESOUSPRODUIT = clsMicparametrelisteproduitsousproduitvaleurDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.PL_CODEPARAMETRELISTE = clsMicparametrelisteproduitsousproduitvaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = clsMicparametrelisteproduitsousproduitvaleurDTO.GM_CODESEGMENT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMIN = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_BORNEMIN.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMAX = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_BORNEMAX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMINI = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMINI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMAXI = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMAXI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUX = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANT = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANT.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUXREMUNERATIONCOMMERCIAL = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUXREMUNERATIONCOMMERCIAL.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTREMUNERATIONCOMMERCIAL = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTREMUNERATIONCOMMERCIAL.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_VALEUR = clsMicparametrelisteproduitsousproduitvaleurDTO.LP_VALEUR.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelisteproduitsousproduitvaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TM_CODEMEMBRE = clsMicparametrelisteproduitsousproduitvaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE = clsMicparametrelisteproduitsousproduitvaleurDTO.SX_CODESEXE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TYPEOPERATION = int.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.TYPEOPERATION.ToString());

                    clsObjetEnvoi.OE_A = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelisteproduitsousproduitvaleurs.Add(clsMicparametrelisteproduitsousproduitvaleur);
                }
                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgAjouterListeParametrageModel(clsDonnee, clsMicparametrelisteproduitsousproduitvaleurs, clsObjetEnvoi));
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

        //AJOUT
        public string pvgAjouterListeParametrageModel_second(List<clsMicparametrelisteproduitsousproduitvaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur> clsMicparametrelisteproduitsousproduitvaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireInsert(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //    //--TEST DES TYPES DE DONNEES
            //    DataSet = TestTypeDonnee(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //    //--TEST CONTRAINTE
            //    DataSet = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
                    clsMicparametrelisteproduitsousproduitvaleur.PL_CODEPARAMETRELISTE = clsMicparametrelisteproduitsousproduitvaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TM_CODEMEMBRE = clsMicparametrelisteproduitsousproduitvaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE = clsMicparametrelisteproduitsousproduitvaleurDTO.SX_CODESEXE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = clsMicparametrelisteproduitsousproduitvaleurDTO.GM_CODESEGMENT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = clsMicparametrelisteproduitsousproduitvaleurDTO.GM_CODESEGMENT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = clsMicparametrelisteproduitsousproduitvaleurDTO.GM_CODESEGMENT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.PS_CODESOUSPRODUIT = clsMicparametrelisteproduitsousproduitvaleurDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMAXI = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMAXI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMINI = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMINI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUX = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANT = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANT.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelisteproduitsousproduitvaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TYPEOPERATION = int.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.TYPEOPERATION.ToString());

                    clsObjetEnvoi.OE_A = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelisteproduitsousproduitvaleurs.Add(clsMicparametrelisteproduitsousproduitvaleur);
                }
                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgAjouterListeParametrageModel(clsDonnee, clsMicparametrelisteproduitsousproduitvaleurs, clsObjetEnvoi));
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

        //AJOUT PARAMETRAGE SPECIFIQUE
        public string pvgAjouterListeParametrageSpecifique(List<clsMicparametrelisteproduitsousproduitvaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur> clsMicparametrelisteproduitsousproduitvaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                DataSet = TestChampObligatoireInsert(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST DES TYPES DE DONNEES
                DataSet = TestTypeDonnee(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST CONTRAINTE
                DataSet = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
                    clsMicparametrelisteproduitsousproduitvaleur.PS_CODESOUSPRODUIT = clsMicparametrelisteproduitsousproduitvaleurDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.PL_CODEPARAMETRELISTE = clsMicparametrelisteproduitsousproduitvaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = clsMicparametrelisteproduitsousproduitvaleurDTO.GM_CODESEGMENT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMIN = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_BORNEMIN.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMAX = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_BORNEMAX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMINI = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMINI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMAXI = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMAXI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUX = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANT = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANT.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUXREMUNERATIONCOMMERCIAL = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUXREMUNERATIONCOMMERCIAL.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTREMUNERATIONCOMMERCIAL = Decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTREMUNERATIONCOMMERCIAL.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_VALEUR = clsMicparametrelisteproduitsousproduitvaleurDTO.LP_VALEUR.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelisteproduitsousproduitvaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TM_CODEMEMBRE = clsMicparametrelisteproduitsousproduitvaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE = clsMicparametrelisteproduitsousproduitvaleurDTO.SX_CODESEXE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TYPEOPERATION = int.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.TYPEOPERATION.ToString());

                    clsObjetEnvoi.OE_A = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelisteproduitsousproduitvaleurs.Add(clsMicparametrelisteproduitsousproduitvaleur);
                }
                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgAjouterListeParametrageSpecifique(clsDonnee, clsMicparametrelisteproduitsousproduitvaleurs, clsObjetEnvoi));
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

        public string pvgAjouterListeParametrageSpecifique_second(List<clsMicparametrelisteproduitsousproduitvaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur> clsMicparametrelisteproduitsousproduitvaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireInsert(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //    //--TEST DES TYPES DE DONNEES
            //    DataSet = TestTypeDonnee(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //    //--TEST CONTRAINTE
            //    DataSet = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
                    clsMicparametrelisteproduitsousproduitvaleur.PL_CODEPARAMETRELISTE = clsMicparametrelisteproduitsousproduitvaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TM_CODEMEMBRE = clsMicparametrelisteproduitsousproduitvaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.PS_CODESOUSPRODUIT = clsMicparametrelisteproduitsousproduitvaleurDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMAXI = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMAXI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMINI = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMINI.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUX = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANT = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANT.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_TAUXREMUNERATIONCOMMERCIAL = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUXREMUNERATIONCOMMERCIAL.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTREMUNERATIONCOMMERCIAL = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTREMUNERATIONCOMMERCIAL.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMIN = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_BORNEMIN.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMAX = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_BORNEMAX.ToString());
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = clsMicparametrelisteproduitsousproduitvaleurDTO.GM_CODESEGMENT.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE = clsMicparametrelisteproduitsousproduitvaleurDTO.SX_CODESEXE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelisteproduitsousproduitvaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelisteproduitsousproduitvaleur.TYPEOPERATION = int.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.TYPEOPERATION.ToString());

                    clsObjetEnvoi.OE_A = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelisteproduitsousproduitvaleurs.Add(clsMicparametrelisteproduitsousproduitvaleur);
                }
                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgAjouterListeParametrageSpecifique(clsDonnee, clsMicparametrelisteproduitsousproduitvaleurs, clsObjetEnvoi));
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

        //MODIFICATION
        public string pvgModifier(clsMicparametrelisteproduitsousproduitvaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
           //DataSet = TestChampObligatoireUpdate(Objet);
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
                //Objet.PS_CODESOUSPRODUIT
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.PS_CODESOUSPRODUIT };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                //{

                //clsMicparametrelisteproduitsousproduitvaleur.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsMicparametrelisteproduitsousproduitvaleur.PL_CODEPARAMETRELISTE = Objet.PL_CODEPARAMETRELISTE.ToString();
                clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMIN = Decimal.Parse(Objet.LP_BORNEMIN.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMAX = Decimal.Parse(Objet.LP_BORNEMAX.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMINI = Decimal.Parse(Objet.LP_MONTANTMINI.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMAXI = Decimal.Parse(Objet.LP_MONTANTMAXI.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_TAUX = Decimal.Parse(Objet.LP_TAUX.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANT = Decimal.Parse(Objet.LP_MONTANT.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_TAUXREMUNERATIONCOMMERCIAL = Decimal.Parse(Objet.LP_TAUXREMUNERATIONCOMMERCIAL.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTREMUNERATIONCOMMERCIAL = Decimal.Parse(Objet.LP_MONTANTREMUNERATIONCOMMERCIAL.ToString());
                clsMicparametrelisteproduitsousproduitvaleur.LP_VALEUR = Objet.LP_VALEUR.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgModifier(clsDonnee, clsMicparametrelisteproduitsousproduitvaleur, clsObjetEnvoi));
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

        //SUPPRESSION
        public string pvgSupprimer(clsMicparametrelisteproduitsousproduitvaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireDelete(Objet);
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.PS_CODESOUSPRODUIT, Objet.PL_CODEPARAMETRELISTE };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
                //}
            }
             catch (SqlException SQLEx)
            {
                DataSet = new DataSet();
                DataRow dr = dt.NewRow();
                dr["SL_CODEMESSAGE"] = "99";
                dr["SL_RESULTAT"] = "FALSE";
                dr["SL_MESSAGE"] = (SQLEx.Number == 547) ? clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0003").MS_LIBELLEMESSAGE : SQLEx.Message;
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

        //LISTE
        public string pvgChargerDansDataSet(clsMicparametrelisteproduitsousproduitvaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.PS_CODESOUSPRODUIT,
                    Objet.AT_CODEACTIVITE,
                    Objet.TM_CODEMEMBRE,
                    Objet.FM_CODEFORMEJURIDIQUE,
                    Objet.SX_CODESEXE,
                    Objet.PL_CODEPARAMETRELISTE,
                    Objet.PL_TYPEPARAMETRE,
                    Objet.MONTANTENVOYER,
                    Objet.GM_CODESEGMENT
                };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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

        //LISTE SELON PLAGE MONTANT
        public string pvgChargerDansDataSetParametrage(clsMicparametrelisteproduitsousproduitvaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.PS_CODESOUSPRODUIT, Objet.PL_CODEPARAMETRELISTE,
                    Objet.LP_MONTANTMINI, Objet.LP_MONTANTMAXI, Objet.SX_CODESEXE, Objet.GM_CODESEGMENT
                };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSetParametrage(clsDonnee, clsObjetEnvoi);
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

        //LISTE SELON PLAGE MONTANT
        public string pvgChargerDansDataSetParametrageSegment(clsMicparametrelisteproduitsousproduitvaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.PS_CODESOUSPRODUIT, Objet.PL_CODEPARAMETRELISTE, Objet.LP_MONTANTMINI, Objet.LP_MONTANTMAXI, Objet.SX_CODESEXE, Objet.GM_CODESEGMENT };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSetParametrageSegment(clsDonnee, clsObjetEnvoi);
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

        //COMBO PLAGE DES MONTANTS
        public string pvgChargerDansDataSetPourComboMontantMinMax(clsMicparametrelisteproduitsousproduitvaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.PS_CODESOUSPRODUIT, Objet.PL_CODEPARAMETRELISTE, Objet.SX_CODESEXE };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgChargerDansDataSetPourComboMontantMinMax(clsDonnee, clsObjetEnvoi);
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



        private ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur pvpRecuperationParametreObjet(int vppIndex, int vppIndex1, int vppIndex2, int vppIndex3, clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO)
        {

            ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();

            //physique
            //string[] vppSegmentPersonnePhysique = clsMicparametrelisteproduitsousproduitvaleurDTO.vppSegmentPersonnePhysique; //cpsDevCheckedComboBoxEdit5.Properties.GetCheckedItems().ToString().Trim().Split(',');
            //                                                                                                                                               //morale
            //string[] vppSegmentPersonneMorale = clsMicparametrelisteproduitsousproduitvaleurDTO.vppSegmentPersonneMorale; // cpsDevCheckedComboBoxEdit6.Properties.GetCheckedItems().ToString().Trim().Split(',');

            //string[] vppAgenceSelectionneesTypeMembrePersonnePhysique = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesTypeMembrePersonnePhysique; //cpsDevCheckedComboBoxEdit3.Properties.GetCheckedItems().ToString().Trim().Split(',');
            //string[] vppAgenceSelectionneesTypeMembrePersonneMorale = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesTypeMembrePersonneMorale; //cpsDevCheckedComboBoxEdit4.Properties.GetCheckedItems().ToString().Trim().Split(',');
            //string[] vppAgenceSelectionneesSexe = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesSexe; //cpsDevCheckedComboBoxEdit1.Properties.GetCheckedItems().ToString().Trim().Split(',');
            //string[] vppAgenceSelectionneesFormeJuridique = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesFormeJuridique; // null;

            clsMicparametrelisteproduitsousproduitvaleur.PL_CODEPARAMETRELISTE = clsMicparametrelisteproduitsousproduitvaleurDTO.PL_CODEPARAMETRELISTE;
            clsMicparametrelisteproduitsousproduitvaleur.TM_CODEMEMBRE = vppAgenceSelectionneesTypeMembrePersonnePhysique[vppIndex2].ToString().Substring(0, 2);
            clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE = vppAgenceSelectionneesSexe[vppIndex].ToString().Substring(0, 2);

            clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = "";

            if (clsMicparametrelisteproduitsousproduitvaleurDTO.PP_MONTANTSGCL == "TRUE")
            {
                if (clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE == "03")
                {
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = vppSegmentPersonneMorale[vppIndex3].ToString().Substring(0, 4);
                }
                else
                {
                    clsMicparametrelisteproduitsousproduitvaleur.GM_CODESEGMENT = vppSegmentPersonnePhysique[vppIndex3].ToString().Substring(0, 4);
                }
            }

            clsMicparametrelisteproduitsousproduitvaleur.PS_CODESOUSPRODUIT = clsMicparametrelisteproduitsousproduitvaleurDTO.PS_CODESOUSPRODUIT;
            clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMAXI = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMAXI);
            clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANTMINI = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANTMINI);
            clsMicparametrelisteproduitsousproduitvaleur.LP_TAUX = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_TAUX.Replace("%", ""));
            clsMicparametrelisteproduitsousproduitvaleur.LP_MONTANT = decimal.Parse(clsMicparametrelisteproduitsousproduitvaleurDTO.LP_MONTANT);
            //clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMIN = decimal.Parse(cpsDevTextBoxD4.Text);
            //clsMicparametrelisteproduitsousproduitvaleur.LP_BORNEMAX = decimal.Parse(cpsDevTextBoxD4.Text);
            //clsMicparametrelisteproduitsousproduitvaleur.LP_VALEUR = cpsDevTextBoxD4.Text;
            if (vlpResultat == "F")
                clsMicparametrelisteproduitsousproduitvaleur.FM_CODEFORMEJURIDIQUE = vppAgenceSelectionneesFormeJuridique[vppIndex1].ToString().Substring(0, 2);

            clsMicparametrelisteproduitsousproduitvaleur.TYPEOPERATION = 0;

            return clsMicparametrelisteproduitsousproduitvaleur;
        }

        //AJOUT
        public string pvgAjouterListeParametrageModel_secondNEW(List<clsMicparametrelisteproduitsousproduitvaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur> clsMicparametrelisteproduitsousproduitvaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireInsert(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //    //--TEST DES TYPES DE DONNEES
            //    DataSet = TestTypeDonnee(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //    //--TEST CONTRAINTE
            //    DataSet = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleurDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur clsMicparametrelisteproduitsousproduitvaleur = new ZenithWebServeur.BOJ.clsMicparametrelisteproduitsousproduitvaleur();


                    //physique
                    vppSegmentPersonnePhysique = clsMicparametrelisteproduitsousproduitvaleurDTO.vppSegmentPersonnePhysique[0].ToString().Trim().Split(','); //cpsDevCheckedComboBoxEdit5.Properties.GetCheckedItems().ToString().Trim().Split(',');
                                                                                                                                                             //morale
                    vppSegmentPersonneMorale = clsMicparametrelisteproduitsousproduitvaleurDTO.vppSegmentPersonneMorale[0].ToString().Trim().Split(','); // // cpsDevCheckedComboBoxEdit6.Properties.GetCheckedItems().ToString().Trim().Split(',');

                    vppAgenceSelectionneesTypeMembrePersonnePhysique = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesTypeMembrePersonnePhysique[0].ToString().Trim().Split(','); // //cpsDevCheckedComboBoxEdit3.Properties.GetCheckedItems().ToString().Trim().Split(',');
                    vppAgenceSelectionneesTypeMembrePersonneMorale = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesTypeMembrePersonneMorale[0].ToString().Trim().Split(','); // //cpsDevCheckedComboBoxEdit4.Properties.GetCheckedItems().ToString().Trim().Split(',');
                    vppAgenceSelectionneesSexe = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesSexe[0].ToString().Trim().Split(','); // //cpsDevCheckedComboBoxEdit1.Properties.GetCheckedItems().ToString().Trim().Split(',');
                    vppAgenceSelectionneesFormeJuridique = clsMicparametrelisteproduitsousproduitvaleurDTO.vppAgenceSelectionneesFormeJuridique[0].ToString().Trim().Split(','); // // null;



                    if (vppAgenceSelectionneesTypeMembrePersonnePhysique != null)
                        for (int l = 0; l < vppAgenceSelectionneesTypeMembrePersonnePhysique.Length; l++)
                        {
                            //Travaux de verification des type membres personnes physiques autorisés en type membre personnes morale
                            string vppRecupValeurTypeMembrePersonnePhysique = vppAgenceSelectionneesTypeMembrePersonnePhysique[l].Trim();
                            string vppRecupValeurTypeMembrePersonneMorale = "";
                            bool vlpTrouverTypeMembrePersonneMorale = false;
                            //On vérifie si le type membre personne physique en cours est également un type membre personne morale autorisée
                            for (int a = 0; a < vppAgenceSelectionneesTypeMembrePersonneMorale.Length; a++)
                            {
                                vppRecupValeurTypeMembrePersonneMorale = vppAgenceSelectionneesTypeMembrePersonneMorale[a].Trim();
                                try
                                {
                                    if (vppRecupValeurTypeMembrePersonnePhysique.Substring(0, 2) == vppRecupValeurTypeMembrePersonneMorale.Substring(0, 2))
                                    {
                                        vlpTrouverTypeMembrePersonneMorale = true;
                                        break;
                                    }
                                }
                                catch { }
                            }

                            //
                            for (int i = 0; i < vppAgenceSelectionneesSexe.Length; i++)
                            {
                                vlpResultat = "S";
                                string vppRecupValeur = vppAgenceSelectionneesSexe[i].Trim();
                                if (vppRecupValeur.Substring(0, 2) == "01" || vppRecupValeur.Substring(0, 2) == "02")
                                {
                                    if (vppSegmentPersonnePhysique.Length > 0)
                                    {
                                        for (int m = 0; m < vppSegmentPersonnePhysique.Length; m++)
                                        {
                                            clsMicparametrelisteproduitsousproduitvaleurs.Add(pvpRecuperationParametreObjet(i, 0, l, m, clsMicparametrelisteproduitsousproduitvaleurDTO));
                                        }
                                    }
                                    else
                                        clsMicparametrelisteproduitsousproduitvaleurs.Add(pvpRecuperationParametreObjet(i, 0, l, 0, clsMicparametrelisteproduitsousproduitvaleurDTO));
                                }

                                if (vppRecupValeur.Substring(0, 2) == "03")
                                {
                                    //vppAgenceSelectionneesFormeJuridique = cpsDevCheckedComboBoxEdit2.Properties.GetCheckedItems().ToString().Split(',');

                                    for (int k = 0; k < vppAgenceSelectionneesFormeJuridique.Length; k++)
                                    {
                                        vlpResultat = "F";
                                        for (int j = 0; j < vppAgenceSelectionneesSexe.Length; j++)
                                        {
                                            vppRecupValeur = vppAgenceSelectionneesSexe[j].Trim();
                                            if (vppRecupValeur.Substring(0, 2) == "03" && vlpTrouverTypeMembrePersonneMorale == true)
                                                
                                                if (vppSegmentPersonneMorale.Length > 0)
                                                {
                                                    for (int o = 0; o < vppSegmentPersonneMorale.Length; o++)
                                                    {
                                                        clsMicparametrelisteproduitsousproduitvaleurs.Add(pvpRecuperationParametreObjet(j, k, l, o, clsMicparametrelisteproduitsousproduitvaleurDTO));
                                                    }
                                                }
                                                else
                                                    clsMicparametrelisteproduitsousproduitvaleurs.Add(pvpRecuperationParametreObjet(j, k, l, 0, clsMicparametrelisteproduitsousproduitvaleurDTO));
                                        }
                                    }
                                }
                            }
                        }
                    
                    clsObjetEnvoi.OE_A = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelisteproduitsousproduitvaleurDTO.clsObjetEnvoi.OE_Y;
                    
                }
                clsObjetRetour.SetValue(true, clsMicparametrelisteproduitsousproduitvaleurWSBLL.pvgAjouterListeParametrageModel(clsDonnee, clsMicparametrelisteproduitsousproduitvaleurs, clsObjetEnvoi));
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
