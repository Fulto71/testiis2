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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicparametrelistevaleur" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicparametrelistevaleur.svc ou wsMicparametrelistevaleur.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicparametrelistevaleur : IwsMicparametrelistevaleur
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicparametrelistevaleurWSBLL clsMicparametrelistevaleurWSBLL = new clsMicparametrelistevaleurWSBLL();

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
        string[] vppAgenceSelectionneesTypeMembre = null;
        string[] vppAgenceSelectionneesSexe = null;
        string[] vppAgenceSelectionneesFormeJuridique = null;


        private ZenithWebServeur.BOJ.clsMicparametrelistevaleur pvpRecuperationParametreObjet(string[] vppGrille, string[] vppGrille1, string[] vppGrille2, int vppIndex, int vppIndex1, int vppIndex2, clsMicparametrelistevaleur clsMicparametrelistevaleurDTO)
        {
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
            clsMicparametrelistevaleur.PL_CODEPARAMETRELISTE = clsMicparametrelistevaleurDTO.PL_CODEPARAMETRELISTE;
            clsMicparametrelistevaleur.SE_CODESENS = clsMicparametrelistevaleurDTO.SE_CODESENS;
            clsMicparametrelistevaleur.NO_CODENATUREVIREMENT = clsMicparametrelistevaleurDTO.NO_CODENATUREVIREMENT;
            clsMicparametrelistevaleur.TM_CODEMEMBRE = vppAgenceSelectionneesTypeMembre[vppIndex2].ToString().Substring(0, 2);
            clsMicparametrelistevaleur.LG_CODEPARAMETRELIBELLEGROUPE = clsMicparametrelistevaleurDTO.LG_CODEPARAMETRELIBELLEGROUPE;
            clsMicparametrelistevaleur.LP_CODEPARAMETRELIBELLE = clsMicparametrelistevaleurDTO.LP_CODEPARAMETRELIBELLE;
            clsMicparametrelistevaleur.ZT_CODEZONE = clsMicparametrelistevaleurDTO.ZT_CODEZONE;
            clsMicparametrelistevaleur.EP_CODEEMPLACEMENT = clsMicparametrelistevaleurDTO.EP_CODEEMPLACEMENT;
            clsMicparametrelistevaleur.RE_CODERESEAU = clsMicparametrelistevaleurDTO.RE_CODERESEAU;
            clsMicparametrelistevaleur.TV_CODETYPEVIREMENT = clsMicparametrelistevaleurDTO.TV_CODETYPEVIREMENT;
            clsMicparametrelistevaleur.TO_CODETYPETRANSFERT = clsMicparametrelistevaleurDTO.TO_CODETYPETRANSFERT;
            clsMicparametrelistevaleur.BB_CODEBANCABLE = clsMicparametrelistevaleurDTO.BB_CODEBANCABLE;
            clsMicparametrelistevaleur.VP_MONTANTMINI = decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMINI);
            clsMicparametrelistevaleur.VP_MONTANTMAXI = decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMAXI);
            clsMicparametrelistevaleur.SX_CODESEXE = vppAgenceSelectionneesSexe[vppIndex].ToString().Substring(0, 2);
            if (vlpResultat == "F")
                clsMicparametrelistevaleur.FM_CODEFORMEJURIDIQUE = vppAgenceSelectionneesFormeJuridique[vppIndex1].ToString().Substring(0, 2);
            //clsMicparametrelistevaleur.VP_MONTANTMAXI = decimal.Parse(vppGrille.GetDataRow(vppIndex)["VP_MONTANTMAXI"].ToString());
            clsMicparametrelistevaleur.VP_TAUX = decimal.Parse(float.Parse(clsMicparametrelistevaleurDTO.VP_TAUX.Replace(".", ",").Replace(" %", "")).ToString());
            clsMicparametrelistevaleur.VP_MONTANT = decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANT);
            clsMicparametrelistevaleur.VP_VALEURMAXIMUM = decimal.Parse(clsMicparametrelistevaleurDTO.VP_VALEURMAXIMUM);
            clsMicparametrelistevaleur.VP_TYPEPARAMETRE = clsMicparametrelistevaleurDTO.VP_TYPEPARAMETRE;
            clsMicparametrelistevaleur.CO_CODECOMPTE = clsMicparametrelistevaleurDTO.CO_CODECOMPTE;

            //clsMicparametrelistevaleur.TYPEOPERATION = 0;

            return clsMicparametrelistevaleur;
        }

        //AJOUT
        public string pvgAjouterListe(List<clsMicparametrelistevaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur> clsMicparametrelistevaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur>();
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

                foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                {
                    clsObjetEnvoi.OE_PARAM = new string[] { clsMicparametrelistevaleurDTO.TM_CODEMEMBRE };
                    ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();

                    vppAgenceSelectionneesTypeMembre = clsMicparametrelistevaleurDTO.vppAgenceSelectionneesTypeMembre[0].ToString().Trim().Split(','); //
                    vppAgenceSelectionneesSexe = clsMicparametrelistevaleurDTO.vppAgenceSelectionneesSexe[0].ToString().Trim().Split(','); //
                    vppAgenceSelectionneesFormeJuridique = clsMicparametrelistevaleurDTO.vppAgenceSelectionneesFormeJuridique[0].ToString().Trim().Split(','); //


                    if (vppAgenceSelectionneesTypeMembre != null)
                        for (int l = 0; l < vppAgenceSelectionneesTypeMembre.Length; l++)
                        {
                            string vppRecupValeurTypeMembre = vppAgenceSelectionneesTypeMembre[l].Trim();
                            if (vppAgenceSelectionneesSexe != null)
                                for (int i = 0; i < vppAgenceSelectionneesSexe.Length; i++)
                                {
                                    vlpResultat = "S";
                                    string vppRecupValeur = vppAgenceSelectionneesSexe[i].Trim();
                                    //clsMicparametrelisteproduitsousproduitvaleur.SX_CODESEXE = vppAgenceSelectionneesSexe[i].Substring(0, 2);
                                    if (vppRecupValeur.Substring(0, 2) == "01" || vppRecupValeur.Substring(0, 2) == "02")
                                        clsMicparametrelistevaleurs.Add(pvpRecuperationParametreObjet(vppAgenceSelectionneesSexe, vppAgenceSelectionneesFormeJuridique, vppAgenceSelectionneesTypeMembre, i, 0, l, clsMicparametrelistevaleurDTO));
                                    if (vppRecupValeur.Substring(0, 2) == "03")
                                    {
                                        //clsMicparametrelistevaleurDTO.vppAgenceSelectionneesFormeJuridique = cpsDevCheckedComboBoxEdit2.Properties.GetCheckedItems().ToString().Split(',');
                                        if (vppAgenceSelectionneesFormeJuridique != null)
                                            for (int k = 0; k < vppAgenceSelectionneesFormeJuridique.Length; k++)
                                            {
                                                vlpResultat = "F";
                                                for (int j = 0; j < vppAgenceSelectionneesSexe.Length; j++)
                                                {
                                                    vppRecupValeur = vppAgenceSelectionneesSexe[j].Trim();
                                                    if (vppRecupValeur.Substring(0, 2) == "03" && (vppRecupValeurTypeMembre.Substring(0, 2) == "01" || vppRecupValeurTypeMembre.Substring(0, 2) == "98"))
                                                        clsMicparametrelistevaleurs.Add(pvpRecuperationParametreObjet(vppAgenceSelectionneesSexe, vppAgenceSelectionneesFormeJuridique, vppAgenceSelectionneesTypeMembre, j, k, l, clsMicparametrelistevaleurDTO));
                                                }
                                            }
                                    }



                                }
                        }
                    
                    clsObjetEnvoi.OE_A = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_Y;

                    //clsMicparametrelistevaleurs.Add(clsMicparametrelistevaleur);
                }

                clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgAjouterListe(clsDonnee, clsMicparametrelistevaleurs, clsObjetEnvoi));
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
        public string pvgAjouterListe1(List<clsMicparametrelistevaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur> clsMicparametrelistevaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur>();
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

                foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                {
                    clsObjetEnvoi.OE_PARAM = new string[] { };
                    ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();

                    clsMicparametrelistevaleur.LG_CODEPARAMETRELIBELLEGROUPE = clsMicparametrelistevaleurDTO.LG_CODEPARAMETRELIBELLEGROUPE.ToString();
                    clsMicparametrelistevaleur.LP_CODEPARAMETRELIBELLE = clsMicparametrelistevaleurDTO.LP_CODEPARAMETRELIBELLE.ToString();
                    clsMicparametrelistevaleur.VP_TYPEPARAMETRE = clsMicparametrelistevaleurDTO.VP_TYPEPARAMETRE.ToString();
                    clsMicparametrelistevaleur.CO_CODECOMPTE = clsMicparametrelistevaleurDTO.CO_CODECOMPTE.ToString();
                    clsMicparametrelistevaleur.ZT_CODEZONE = clsMicparametrelistevaleurDTO.ZT_CODEZONE.ToString();
                    clsMicparametrelistevaleur.EP_CODEEMPLACEMENT = clsMicparametrelistevaleurDTO.EP_CODEEMPLACEMENT.ToString();
                    clsMicparametrelistevaleur.BB_CODEBANCABLE = clsMicparametrelistevaleurDTO.BB_CODEBANCABLE.ToString();
                    clsMicparametrelistevaleur.VP_MONTANTMINI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMINI.ToString());
                    clsMicparametrelistevaleur.VP_MONTANTMAXI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMAXI.ToString());
                    clsMicparametrelistevaleur.PL_CODEPARAMETRELISTE = clsMicparametrelistevaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicparametrelistevaleur.SX_CODESEXE = clsMicparametrelistevaleurDTO.SX_CODESEXE.ToString();

                    clsMicparametrelistevaleur.VP_CODEPARAMETRELISTEVALEUR = clsMicparametrelistevaleurDTO.VP_CODEPARAMETRELISTEVALEUR.ToString();
                    clsMicparametrelistevaleur.TV_CODETYPEVIREMENT = clsMicparametrelistevaleurDTO.TV_CODETYPEVIREMENT.ToString();
                    clsMicparametrelistevaleur.TO_CODETYPETRANSFERT = clsMicparametrelistevaleurDTO.TO_CODETYPETRANSFERT.ToString();
                    clsMicparametrelistevaleur.RE_CODERESEAU = clsMicparametrelistevaleurDTO.RE_CODERESEAU.ToString();
                    clsMicparametrelistevaleur.VP_BORNEMIN = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_BORNEMIN.ToString());
                    clsMicparametrelistevaleur.VP_BORNEMAX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_BORNEMAX.ToString());
                    clsMicparametrelistevaleur.VP_TAUX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_TAUX.ToString());
                    clsMicparametrelistevaleur.VP_MONTANT = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANT.ToString());
                    clsMicparametrelistevaleur.VP_VALEURMAXIMUM = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_VALEURMAXIMUM.ToString());
                    clsMicparametrelistevaleur.TM_CODEMEMBRE = clsMicparametrelistevaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelistevaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelistevaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelistevaleur.SE_CODESENS = clsMicparametrelistevaleurDTO.SE_CODESENS.ToString();
                    clsMicparametrelistevaleur.NO_CODENATUREVIREMENT = clsMicparametrelistevaleurDTO.NO_CODENATUREVIREMENT.ToString();

                    clsObjetEnvoi.OE_A = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelistevaleurs.Add(clsMicparametrelistevaleur);
                }

                clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgAjouterListe(clsDonnee, clsMicparametrelistevaleurs, clsObjetEnvoi));
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
        public string pvgAjouterListeSpecifique1(List<clsMicparametrelistevaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur> clsMicparametrelistevaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur>();
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

                foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                {
                    clsObjetEnvoi.OE_PARAM = new string[] { };
                    ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();

                    /* clsMicparametrelistevaleur.LG_CODEPARAMETRELIBELLEGROUPE = clsMicparametrelistevaleurDTO.LG_CODEPARAMETRELIBELLEGROUPE.ToString();
                     clsMicparametrelistevaleur.VP_CODEPARAMETRELISTEVALEUR = clsMicparametrelistevaleurDTO.VP_CODEPARAMETRELISTEVALEUR.ToString();
                     clsMicparametrelistevaleur.PL_CODEPARAMETRELISTE = clsMicparametrelistevaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                     clsMicparametrelistevaleur.LP_CODEPARAMETRELIBELLE = clsMicparametrelistevaleurDTO.LP_CODEPARAMETRELIBELLE.ToString();
                     clsMicparametrelistevaleur.TV_CODETYPEVIREMENT = clsMicparametrelistevaleurDTO.TV_CODETYPEVIREMENT.ToString();
                     clsMicparametrelistevaleur.EP_CODEEMPLACEMENT = clsMicparametrelistevaleurDTO.EP_CODEEMPLACEMENT.ToString();
                     clsMicparametrelistevaleur.RE_CODERESEAU = clsMicparametrelistevaleurDTO.RE_CODERESEAU.ToString();
                     clsMicparametrelistevaleur.ZT_CODEZONE = clsMicparametrelistevaleurDTO.ZT_CODEZONE.ToString();
                     clsMicparametrelistevaleur.SV_CODESENSVIREMENT = clsMicparametrelistevaleurDTO.SV_CODESENSVIREMENT.ToString();
                     clsMicparametrelistevaleur.BB_CODEBANCABLE = clsMicparametrelistevaleurDTO.BB_CODEBANCABLE.ToString();
                     clsMicparametrelistevaleur.VP_BORNEMIN = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_BORNEMIN.ToString());
                     clsMicparametrelistevaleur.VP_BORNEMAX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_BORNEMAX.ToString());
                     clsMicparametrelistevaleur.VP_MONTANTMINI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMINI.ToString());
                     clsMicparametrelistevaleur.VP_MONTANTMAXI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMAXI.ToString());
                     clsMicparametrelistevaleur.VP_TAUX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_TAUX.ToString());
                     clsMicparametrelistevaleur.VP_MONTANT = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANT.ToString());
                     clsMicparametrelistevaleur.VP_VALEURMAXIMUM = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_VALEURMAXIMUM.ToString());
                     clsMicparametrelistevaleur.VP_TYPEPARAMETRE = clsMicparametrelistevaleurDTO.VP_TYPEPARAMETRE.ToString();
                     clsMicparametrelistevaleur.TM_CODEMEMBRE = clsMicparametrelistevaleurDTO.TM_CODEMEMBRE.ToString();
                     clsMicparametrelistevaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelistevaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                     clsMicparametrelistevaleur.SX_CODESEXE = clsMicparametrelistevaleurDTO.SX_CODESEXE.ToString();
                     clsMicparametrelistevaleur.SE_CODESENS = clsMicparametrelistevaleurDTO.SE_CODESENS.ToString();
                     clsMicparametrelistevaleur.NO_CODENATUREVIREMENT = clsMicparametrelistevaleurDTO.NO_CODENATUREVIREMENT.ToString();
                     clsMicparametrelistevaleur.TO_CODETYPETRANSFERT = clsMicparametrelistevaleurDTO.TO_CODETYPETRANSFERT.ToString();
                     clsMicparametrelistevaleur.CO_CODECOMPTE = clsMicparametrelistevaleurDTO.CO_CODECOMPTE.ToString();*/

                    clsMicparametrelistevaleur.LG_CODEPARAMETRELIBELLEGROUPE = clsMicparametrelistevaleurDTO.LG_CODEPARAMETRELIBELLEGROUPE.ToString();
                    clsMicparametrelistevaleur.LP_CODEPARAMETRELIBELLE = clsMicparametrelistevaleurDTO.LP_CODEPARAMETRELIBELLE.ToString();
                    clsMicparametrelistevaleur.ZT_CODEZONE = clsMicparametrelistevaleurDTO.ZT_CODEZONE.ToString();
                    clsMicparametrelistevaleur.EP_CODEEMPLACEMENT = clsMicparametrelistevaleurDTO.EP_CODEEMPLACEMENT.ToString();
                    clsMicparametrelistevaleur.RE_CODERESEAU = clsMicparametrelistevaleurDTO.RE_CODERESEAU.ToString();
                    clsMicparametrelistevaleur.TV_CODETYPEVIREMENT = clsMicparametrelistevaleurDTO.TV_CODETYPEVIREMENT.ToString();
                    clsMicparametrelistevaleur.TO_CODETYPETRANSFERT = clsMicparametrelistevaleurDTO.TO_CODETYPETRANSFERT.ToString();

                    clsMicparametrelistevaleur.BB_CODEBANCABLE = clsMicparametrelistevaleurDTO.BB_CODEBANCABLE.ToString();
                    clsMicparametrelistevaleur.VP_MONTANTMINI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMINI.ToString());
                    clsMicparametrelistevaleur.VP_MONTANTMAXI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMAXI.ToString());
                    clsMicparametrelistevaleur.VP_TAUX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_TAUX.ToString());
                    clsMicparametrelistevaleur.VP_MONTANT = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANT.ToString());
                    clsMicparametrelistevaleur.VP_VALEURMAXIMUM = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_VALEURMAXIMUM.ToString());
                    clsMicparametrelistevaleur.VP_TYPEPARAMETRE = "V";
                    clsMicparametrelistevaleur.TM_CODEMEMBRE = clsMicparametrelistevaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelistevaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelistevaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelistevaleur.SX_CODESEXE = clsMicparametrelistevaleurDTO.SX_CODESEXE.ToString();
                    clsMicparametrelistevaleur.SE_CODESENS = clsMicparametrelistevaleurDTO.SE_CODESENS.ToString();
                    clsMicparametrelistevaleur.NO_CODENATUREVIREMENT = clsMicparametrelistevaleurDTO.NO_CODENATUREVIREMENT.ToString();
                    clsMicparametrelistevaleur.CO_CODECOMPTE = clsMicparametrelistevaleurDTO.CO_CODECOMPTE.ToString();


                    clsObjetEnvoi.OE_A = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelistevaleurs.Add(clsMicparametrelistevaleur);
                }

                clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgAjouterListeSpecifique(clsDonnee, clsMicparametrelistevaleurs, clsObjetEnvoi));
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
        public string pvgAjouterListeSpecifique(List<clsMicparametrelistevaleur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur> clsMicparametrelistevaleurs = new List<ZenithWebServeur.BOJ.clsMicparametrelistevaleur>();
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

                foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                {
                    clsObjetEnvoi.OE_PARAM = new string[] { clsMicparametrelistevaleurDTO.TM_CODEMEMBRE };
                    ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();

                    clsMicparametrelistevaleur.LG_CODEPARAMETRELIBELLEGROUPE = clsMicparametrelistevaleurDTO.LG_CODEPARAMETRELIBELLEGROUPE.ToString();
                    clsMicparametrelistevaleur.VP_CODEPARAMETRELISTEVALEUR = clsMicparametrelistevaleurDTO.VP_CODEPARAMETRELISTEVALEUR.ToString();
                    clsMicparametrelistevaleur.PL_CODEPARAMETRELISTE = clsMicparametrelistevaleurDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicparametrelistevaleur.LP_CODEPARAMETRELIBELLE = clsMicparametrelistevaleurDTO.LP_CODEPARAMETRELIBELLE.ToString();
                    clsMicparametrelistevaleur.TV_CODETYPEVIREMENT = clsMicparametrelistevaleurDTO.TV_CODETYPEVIREMENT.ToString();
                    clsMicparametrelistevaleur.EP_CODEEMPLACEMENT = clsMicparametrelistevaleurDTO.EP_CODEEMPLACEMENT.ToString();
                    clsMicparametrelistevaleur.RE_CODERESEAU = clsMicparametrelistevaleurDTO.RE_CODERESEAU.ToString();
                    clsMicparametrelistevaleur.ZT_CODEZONE = clsMicparametrelistevaleurDTO.ZT_CODEZONE.ToString();
                    clsMicparametrelistevaleur.SV_CODESENSVIREMENT = clsMicparametrelistevaleurDTO.SV_CODESENSVIREMENT.ToString();
                    clsMicparametrelistevaleur.BB_CODEBANCABLE = clsMicparametrelistevaleurDTO.BB_CODEBANCABLE.ToString();
                    clsMicparametrelistevaleur.VP_BORNEMIN = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_BORNEMIN.ToString());
                    clsMicparametrelistevaleur.VP_BORNEMAX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_BORNEMAX.ToString());
                    clsMicparametrelistevaleur.VP_MONTANTMINI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMINI.ToString());
                    clsMicparametrelistevaleur.VP_MONTANTMAXI = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANTMAXI.ToString());
                    clsMicparametrelistevaleur.VP_TAUX = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_TAUX.ToString());
                    clsMicparametrelistevaleur.VP_MONTANT = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_MONTANT.ToString());
                    clsMicparametrelistevaleur.VP_VALEURMAXIMUM = Decimal.Parse(clsMicparametrelistevaleurDTO.VP_VALEURMAXIMUM.ToString());
                    clsMicparametrelistevaleur.VP_TYPEPARAMETRE = clsMicparametrelistevaleurDTO.VP_TYPEPARAMETRE.ToString();
                    clsMicparametrelistevaleur.TM_CODEMEMBRE = clsMicparametrelistevaleurDTO.TM_CODEMEMBRE.ToString();
                    clsMicparametrelistevaleur.FM_CODEFORMEJURIDIQUE = clsMicparametrelistevaleurDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicparametrelistevaleur.SX_CODESEXE = clsMicparametrelistevaleurDTO.SX_CODESEXE.ToString();
                    clsMicparametrelistevaleur.SE_CODESENS = clsMicparametrelistevaleurDTO.SE_CODESENS.ToString();
                    clsMicparametrelistevaleur.NO_CODENATUREVIREMENT = clsMicparametrelistevaleurDTO.NO_CODENATUREVIREMENT.ToString();
                    clsMicparametrelistevaleur.TO_CODETYPETRANSFERT = clsMicparametrelistevaleurDTO.TO_CODETYPETRANSFERT.ToString();
                    clsMicparametrelistevaleur.CO_CODECOMPTE = clsMicparametrelistevaleurDTO.CO_CODECOMPTE.ToString();

                    clsObjetEnvoi.OE_A = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicparametrelistevaleurDTO.clsObjetEnvoi.OE_Y;

                    clsMicparametrelistevaleurs.Add(clsMicparametrelistevaleur);
                }

                clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgAjouterListeSpecifique(clsDonnee, clsMicparametrelistevaleurs, clsObjetEnvoi));
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

        //COMBO
        public string pvgChargerDansDataSetPourCombo1(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.LG_CODEPARAMETRELIBELLEGROUPE };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSetPourCombo1(clsDonnee, clsObjetEnvoi);
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

        //COMBO
        public string pvgChargerDansDataSetPourComboMontantMinMax(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                    Objet.LP_CODEPARAMETRELIBELLE, Objet.SE_CODESENS, Objet.SX_CODESEXE,
                    Objet.ZT_CODEZONE, Objet.EP_CODEEMPLACEMENT, Objet.RE_CODERESEAU,
                    Objet.TV_CODETYPEVIREMENT, Objet.BB_CODEBANCABLE,
                    Objet.NO_CODENATUREVIREMENT, Objet.TO_CODETYPETRANSFERT
                };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSetPourComboMontantMinMax(clsDonnee, clsObjetEnvoi);
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

        //COMBO
        public string pvgChargerDansDataSetPourComboMontantMinMaxCompte(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                            Objet.LP_CODEPARAMETRELIBELLE,
                            Objet.SE_CODESENS,
                            Objet.SX_CODESEXE,
                            Objet.ZT_CODEZONE,
                            Objet.EP_CODEEMPLACEMENT,
                            Objet.RE_CODERESEAU,
                            Objet.TV_CODETYPEVIREMENT,
                            Objet.BB_CODEBANCABLE,
                            Objet.NO_CODENATUREVIREMENT,
                            Objet.TO_CODETYPETRANSFERT,
                            Objet.CO_CODECOMPTE
                };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSetPourComboMontantMinMaxCompte(clsDonnee, clsObjetEnvoi);
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

        //LISTE
        public string pvgChargerDansDataSetParametreSpecifique(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                    Objet.VP_TYPEPARAMETRE, Objet.SX_CODESEXE, Objet.SE_CODESENS,
                    Objet.NO_CODENATUREVIREMENT, Objet.VP_MONTANTMINI, Objet.VP_MONTANTMAXI,
                    Objet.LP_CODEPARAMETRELIBELLE, Objet.ZT_CODEZONE, Objet.EP_CODEEMPLACEMENT,
                    Objet.RE_CODERESEAU, Objet.TV_CODETYPEVIREMENT, Objet.TO_CODETYPETRANSFERT,
                    Objet.BB_CODEBANCABLE, Objet.CO_CODECOMPTE
                };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSetParametreSpecifique(clsDonnee, clsObjetEnvoi);
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

        //LISTE
        public string pvgChargerDansDataSetFonction(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                    Objet.AG_CODEAGENCE,
                    Objet.CODE,
                    Objet.TV_CODETYPEVIREMENT,
                    Objet.EP_CODEEMPLACEMENT,
                    Objet.RE_CODERESEAU,
                    Objet.ZT_CODEZONE,
                    Objet.BB_CODEBANCABLE,
                    Objet.TM_CODEMEMBRE,
                    Objet.FM_CODEFORMEJURIDIQUE,
                    Objet.SX_CODESEXE,
                    Objet.SE_CODESENS,
                    Objet.NO_CODENATUREVIREMENT,
                    Objet.O_CODETYPETRANSFERT,
                    Objet.CO_CODECOMPTE,
                    Objet.VP_MONTANT,
                    Objet.VP_TYPEPARAMETRE,
                    Objet.VP_DETAIL
                };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsMicparametrelistevaleurWSBLL.pvgChargerDansDataSetFonction(clsDonnee, clsObjetEnvoi);
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

        //SUPPRESSION
        public string pvgSupprimer(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                if (Objet.vlpTypeParametre == "V")
                {
                    clsObjetEnvoi.OE_PARAM = new string[] {
                    //"V",
                    Objet.vlpTypeParametre,
                    Objet.SE_CODESENS,
                    Objet.ZT_CODEZONE,
                    Objet.EP_CODEEMPLACEMENT,
                    Objet.RE_CODERESEAU,
                     Objet.TV_CODETYPEVIREMENT,
                    Objet.BB_CODEBANCABLE,
                    Objet.NO_CODENATUREVIREMENT,
                    Objet.PL_CODEPARAMETRELISTE,
                    Objet.TO_CODETYPETRANSFERT,
                    Objet.CO_CODECOMPTE
                };
                }
                else if (Objet.vlpTypeParametre == "T")
                {
                    clsObjetEnvoi.OE_PARAM = new string[] {
                       // "T",
                       Objet.vlpTypeParametre,
                    Objet.PL_CODEPARAMETRELISTE
                };
                }
                else
                {
                    clsObjetEnvoi.OE_PARAM = new string[] {
                        Objet.vlpTypeParametre,
                    Objet.ZT_CODEZONE,
                    Objet.EP_CODEEMPLACEMENT,
                    Objet.BB_CODEBANCABLE,
                    Objet.PL_CODEPARAMETRELISTE
                };
                }


                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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

        //SUPPRESSION TOTAL
        public string pvgSupprimerTous(clsMicparametrelistevaleur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicparametrelistevaleur clsMicparametrelistevaleur = new ZenithWebServeur.BOJ.clsMicparametrelistevaleur();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.VP_TYPEPARAMETRE, Objet.CO_CODECOMPTE };

                //foreach (ZenithWebServeur.DTO.clsMicparametrelistevaleur clsMicparametrelistevaleurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicparametrelistevaleurWSBLL.pvgSupprimerTous(clsDonnee, clsObjetEnvoi));
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
    }
}
