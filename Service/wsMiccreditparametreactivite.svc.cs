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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMiccreditparametreactivite" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMiccreditparametreactivite.svc ou wsMiccreditparametreactivite.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMiccreditparametreactivite : IwsMiccreditparametreactivite
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMiccreditparametreactiviteWSBLL clsMiccreditparametreactiviteWSBLL = new clsMiccreditparametreactiviteWSBLL();

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
       
        //AJOUT
        public string pvgAjouter(clsMiccreditparametreactivite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccreditparametreactivite clsMiccreditparametreactivite = new ZenithWebServeur.BOJ.clsMiccreditparametreactivite();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
          //DataSet = TestChampObligatoireInsert(Objet);
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

                //foreach (ZenithWebServeur.DTO.clsMiccreditparametreactivite clsMiccreditparametreactiviteDTO in Objet)
                //{
                
                clsMiccreditparametreactivite.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsMiccreditparametreactivite.TI_CODETYPEAMORTISSEMENT = Objet.TI_CODETYPEAMORTISSEMENT.ToString();
                clsMiccreditparametreactivite.AT_LIBELLE = Objet.AT_LIBELLE.ToString();
                clsMiccreditparametreactivite.AT_TAUXMINIMUM = Double.Parse(Objet.AT_TAUXMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXPARDEFAUT = Double.Parse(Objet.AT_TAUXPARDEFAUT.ToString());
                clsMiccreditparametreactivite.AT_TAUXMAXIMUM = Double.Parse(Objet.AT_TAUXMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMINIMUM = Double.Parse(Objet.AT_MONTANTMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMAXIMUM = Double.Parse(Objet.AT_MONTANTMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTEPARGNEMINIMUM = Double.Parse(Objet.AT_MONTANTEPARGNEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTEPARGNEOBLIGATOIRE = Double.Parse(Objet.AT_MONTANTEPARGNEOBLIGATOIRE.ToString());
                clsMiccreditparametreactivite.AT_MONTANTEPARGNEMAXIMUM = Double.Parse(Objet.AT_MONTANTEPARGNEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXASSURANCE = Double.Parse(Objet.AT_TAUXASSURANCE.ToString());
                clsMiccreditparametreactivite.AT_MONTANTASSURANCEMINIMUM = Double.Parse(Objet.AT_MONTANTASSURANCEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTASSURANCEMAXIMUM = Double.Parse(Objet.AT_MONTANTASSURANCEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_DUREEMINIMUM = int.Parse(Objet.AT_DUREEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_DUREEMAXIMUM = int.Parse(Objet.AT_DUREEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_DIFFEREMINIMUM = int.Parse(Objet.AT_DIFFEREMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_DIFFEREMAXIMUM = int.Parse(Objet.AT_DIFFEREMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMAXIMUMCREDITLIESALAIRENET = Double.Parse(Objet.AT_MONTANTMAXIMUMCREDITLIESALAIRENET.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMAXIMUMCREDITLIEALEPARGNE = Double.Parse(Objet.AT_MONTANTMAXIMUMCREDITLIEALEPARGNE.ToString());
                clsMiccreditparametreactivite.AT_DUREEMINIMUMANCIENNETE = int.Parse(Objet.AT_DUREEMINIMUMANCIENNETE.ToString());
                clsMiccreditparametreactivite.AT_NOMBREMINIMUMMEMBRE = int.Parse(Objet.AT_NOMBREMINIMUMMEMBRE.ToString());
                clsMiccreditparametreactivite.AT_NOMBREMAXIMUMMEMBRE = int.Parse(Objet.AT_NOMBREMAXIMUMMEMBRE.ToString());
                clsMiccreditparametreactivite.AT_TAUXPENALITEMINIMUM = Double.Parse(Objet.AT_TAUXPENALITEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXPENALITEMAXIMUM = Double.Parse(Objet.AT_TAUXPENALITEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_TABLEAUAMORTISSEMENTAUTOMATIQUE = Objet.AT_TABLEAUAMORTISSEMENTAUTOMATIQUE.ToString();
                clsMiccreditparametreactivite.AT_ACTIF = Objet.AT_ACTIF.ToString();
                clsMiccreditparametreactivite.AT_NUMEROORDRE = int.Parse(Objet.AT_NUMEROORDRE.ToString());
                clsMiccreditparametreactivite.AT_DATESAISIE = DateTime.Parse(Objet.AT_DATESAISIE.ToString());
                clsMiccreditparametreactivite.AT_TAUXDEPOTGARANTIEMINIMUM = Double.Parse(Objet.AT_TAUXDEPOTGARANTIEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXDEPOTGARANTIEPARDEFAUT = Double.Parse(Objet.AT_TAUXDEPOTGARANTIEPARDEFAUT.ToString());
                clsMiccreditparametreactivite.AT_TAUXDEPOTGARANTIEMAXIMUM = Double.Parse(Objet.AT_TAUXDEPOTGARANTIEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_AGEMINIMUM = Double.Parse(Objet.AT_AGEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_AGEMAXIMUM = Double.Parse(Objet.AT_AGEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_FRAISOBLIGATOIREAVANTDEBLOCAGE = Objet.AT_FRAISOBLIGATOIREAVANTDEBLOCAGE.ToString();
                clsMiccreditparametreactivite.AT_CODESOUSPRODUITEPARGNESURCREDIT = Objet.AT_CODESOUSPRODUITEPARGNESURCREDIT.ToString();
                clsMiccreditparametreactivite.AT_CODESOUSPRODUITFRAISFIXESURCREDIT = Objet.AT_CODESOUSPRODUITFRAISFIXESURCREDIT.ToString();
                clsMiccreditparametreactivite.AT_REGLERENTIEREMENTFRAISAVANTDEBLOCAGE = Objet.AT_REGLERENTIEREMENTFRAISAVANTDEBLOCAGE.ToString();
                clsMiccreditparametreactivite.AT_AUTORISATIONMODIFICATIONFRAISCREDITORDINAIRE = Objet.AT_AUTORISATIONMODIFICATIONFRAISCREDITORDINAIRE.ToString();
                clsMiccreditparametreactivite.AT_AUTORISATIONMODIFICATIONFRAISCREDITIMMOBILISE = Objet.AT_AUTORISATIONMODIFICATIONFRAISCREDITIMMOBILISE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiccreditparametreactiviteWSBLL.pvgAjouter(clsDonnee, clsMiccreditparametreactivite, clsObjetEnvoi));
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

        //MODIFICATION
        public string pvgModifier(clsMiccreditparametreactivite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccreditparametreactivite clsMiccreditparametreactivite = new ZenithWebServeur.BOJ.clsMiccreditparametreactivite();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AT_CODEACTIVITE };

                //foreach (ZenithWebServeur.DTO.clsMiccreditparametreactivite clsMiccreditparametreactiviteDTO in Objet)
                //{

                clsMiccreditparametreactivite.AT_CODEACTIVITE = Objet.AT_CODEACTIVITE.ToString();
                clsMiccreditparametreactivite.TA_CODETYPEACTIVITE = Objet.TA_CODETYPEACTIVITE.ToString();
                clsMiccreditparametreactivite.TI_CODETYPEAMORTISSEMENT = Objet.TI_CODETYPEAMORTISSEMENT.ToString();
                clsMiccreditparametreactivite.AT_LIBELLE = Objet.AT_LIBELLE.ToString();
                clsMiccreditparametreactivite.AT_TAUXMINIMUM = Double.Parse(Objet.AT_TAUXMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXPARDEFAUT = Double.Parse(Objet.AT_TAUXPARDEFAUT.ToString());
                clsMiccreditparametreactivite.AT_TAUXMAXIMUM = Double.Parse(Objet.AT_TAUXMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMINIMUM = Double.Parse(Objet.AT_MONTANTMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMAXIMUM = Double.Parse(Objet.AT_MONTANTMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTEPARGNEMINIMUM = Double.Parse(Objet.AT_MONTANTEPARGNEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTEPARGNEOBLIGATOIRE = Double.Parse(Objet.AT_MONTANTEPARGNEOBLIGATOIRE.ToString());
                clsMiccreditparametreactivite.AT_MONTANTEPARGNEMAXIMUM = Double.Parse(Objet.AT_MONTANTEPARGNEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXASSURANCE = Double.Parse(Objet.AT_TAUXASSURANCE.ToString());
                clsMiccreditparametreactivite.AT_MONTANTASSURANCEMINIMUM = Double.Parse(Objet.AT_MONTANTASSURANCEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTASSURANCEMAXIMUM = Double.Parse(Objet.AT_MONTANTASSURANCEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_DUREEMINIMUM = int.Parse(Objet.AT_DUREEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_DUREEMAXIMUM = int.Parse(Objet.AT_DUREEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_DIFFEREMINIMUM = int.Parse(Objet.AT_DIFFEREMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_DIFFEREMAXIMUM = int.Parse(Objet.AT_DIFFEREMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMAXIMUMCREDITLIESALAIRENET = Double.Parse(Objet.AT_MONTANTMAXIMUMCREDITLIESALAIRENET.ToString());
                clsMiccreditparametreactivite.AT_MONTANTMAXIMUMCREDITLIEALEPARGNE = Double.Parse(Objet.AT_MONTANTMAXIMUMCREDITLIEALEPARGNE.ToString());
                clsMiccreditparametreactivite.AT_DUREEMINIMUMANCIENNETE = int.Parse(Objet.AT_DUREEMINIMUMANCIENNETE.ToString());
                clsMiccreditparametreactivite.AT_NOMBREMINIMUMMEMBRE = int.Parse(Objet.AT_NOMBREMINIMUMMEMBRE.ToString());
                clsMiccreditparametreactivite.AT_NOMBREMAXIMUMMEMBRE = int.Parse(Objet.AT_NOMBREMAXIMUMMEMBRE.ToString());
                clsMiccreditparametreactivite.AT_TAUXPENALITEMINIMUM = Double.Parse(Objet.AT_TAUXPENALITEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXPENALITEMAXIMUM = Double.Parse(Objet.AT_TAUXPENALITEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_TABLEAUAMORTISSEMENTAUTOMATIQUE = Objet.AT_TABLEAUAMORTISSEMENTAUTOMATIQUE.ToString();
                clsMiccreditparametreactivite.AT_ACTIF = Objet.AT_ACTIF.ToString();
                clsMiccreditparametreactivite.AT_NUMEROORDRE = int.Parse(Objet.AT_NUMEROORDRE.ToString());
                clsMiccreditparametreactivite.AT_DATESAISIE = DateTime.Parse(Objet.AT_DATESAISIE.ToString());
                clsMiccreditparametreactivite.AT_TAUXDEPOTGARANTIEMINIMUM = Double.Parse(Objet.AT_TAUXDEPOTGARANTIEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_TAUXDEPOTGARANTIEPARDEFAUT = Double.Parse(Objet.AT_TAUXDEPOTGARANTIEPARDEFAUT.ToString());
                clsMiccreditparametreactivite.AT_TAUXDEPOTGARANTIEMAXIMUM = Double.Parse(Objet.AT_TAUXDEPOTGARANTIEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_AGEMINIMUM = Double.Parse(Objet.AT_AGEMINIMUM.ToString());
                clsMiccreditparametreactivite.AT_AGEMAXIMUM = Double.Parse(Objet.AT_AGEMAXIMUM.ToString());
                clsMiccreditparametreactivite.AT_FRAISOBLIGATOIREAVANTDEBLOCAGE = Objet.AT_FRAISOBLIGATOIREAVANTDEBLOCAGE.ToString();
                clsMiccreditparametreactivite.AT_CODESOUSPRODUITEPARGNESURCREDIT = Objet.AT_CODESOUSPRODUITEPARGNESURCREDIT.ToString();
                clsMiccreditparametreactivite.AT_CODESOUSPRODUITFRAISFIXESURCREDIT = Objet.AT_CODESOUSPRODUITFRAISFIXESURCREDIT.ToString();
                clsMiccreditparametreactivite.AT_REGLERENTIEREMENTFRAISAVANTDEBLOCAGE = Objet.AT_REGLERENTIEREMENTFRAISAVANTDEBLOCAGE.ToString();
                clsMiccreditparametreactivite.AT_AUTORISATIONMODIFICATIONFRAISCREDITORDINAIRE = Objet.AT_AUTORISATIONMODIFICATIONFRAISCREDITORDINAIRE.ToString();
                clsMiccreditparametreactivite.AT_AUTORISATIONMODIFICATIONFRAISCREDITIMMOBILISE = Objet.AT_AUTORISATIONMODIFICATIONFRAISCREDITIMMOBILISE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiccreditparametreactiviteWSBLL.pvgModifier(clsDonnee, clsMiccreditparametreactivite, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMiccreditparametreactivite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccreditparametreactivite clsMiccreditparametreactivite = new ZenithWebServeur.BOJ.clsMiccreditparametreactivite();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.TA_CODETYPEACTIVITE, Objet.AT_CODEACTIVITE };

                //foreach (ZenithWebServeur.DTO.clsMiccreditparametreactivite clsMiccreditparametreactiviteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiccreditparametreactiviteWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        public string pvgChargerDansDataSet2(clsMiccreditparametreactivite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccreditparametreactivite clsMiccreditparametreactivite = new ZenithWebServeur.BOJ.clsMiccreditparametreactivite();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.TA_CODETYPEACTIVITE };

                //foreach (ZenithWebServeur.DTO.clsMiccreditparametreactivite clsMiccreditparametreactiviteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiccreditparametreactiviteWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiccreditparametreactiviteWSBLL.pvgChargerDansDataSet2(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSetPourCombo(clsMiccreditparametreactivite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccreditparametreactivite clsMiccreditparametreactivite = new ZenithWebServeur.BOJ.clsMiccreditparametreactivite();
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
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsMiccreditparametreactivite clsMiccreditparametreactiviteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiccreditparametreactiviteWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiccreditparametreactiviteWSBLL.pvgChargerDansDataSetPourCombo(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSet(clsMiccreditparametreactivite Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccreditparametreactivite clsMiccreditparametreactivite = new ZenithWebServeur.BOJ.clsMiccreditparametreactivite();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.TA_CODETYPEACTIVITE };

                //foreach (ZenithWebServeur.DTO.clsMiccreditparametreactivite clsMiccreditparametreactiviteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiccreditparametreactiviteWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiccreditparametreactiviteWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
