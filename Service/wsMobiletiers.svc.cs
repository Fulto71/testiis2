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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMobiletiers" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMobiletiers.svc ou wsMobiletiers.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMobiletiers : IwsMobiletiers
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMobiletiersWSBLL clsMobiletiersWSBLL = new clsMobiletiersWSBLL();

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
        public string pvgAjouter(clsMobiletiers Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgAjouter(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
                //{

                //clsMobiletiers.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsMobiletiers.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsMobiletiers.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                //clsMobiletiers.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                //clsMobiletiers.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                //clsMobiletiers.CL_IDCLIENTAGENTAGREE = Objet.CL_IDCLIENTAGENTAGREE.ToString();
                //clsMobiletiers.MB_DATECREATION = DateTime.Parse(Objet.MB_DATECREATION.ToString());
                //clsMobiletiers.MB_DATESAISIE = DateTime.Parse(Objet.MB_DATESAISIE.ToString());
                //clsMobiletiers.MB_NOMTIERS = Objet.MB_NOMTIERS.ToString();
                //clsMobiletiers.MB_PRENOMTIERS = Objet.MB_PRENOMTIERS.ToString();
                //clsMobiletiers.MB_DATENAISSANCE = DateTime.Parse(Objet.MB_DATENAISSANCE.ToString());
                //clsMobiletiers.MB_LIEUNAISSANCE = Objet.MB_LIEUNAISSANCE.ToString();
                //clsMobiletiers.MB_TELEPHONE = Objet.MB_TELEPHONE.ToString();
                //clsMobiletiers.MB_EMAIL = Objet.MB_EMAIL.ToString();
                //clsMobiletiers.MB_NUMPIECE = Objet.MB_NUMPIECE.ToString();
                //clsMobiletiers.MB_INDICATIF = Objet.MB_INDICATIF.ToString();
                //clsMobiletiers.MB_PHOTO = System.Convert.FromBase64String(Objet.MB_PHOTO.ToString());
                //clsMobiletiers.MB_SIGNATURE = System.Convert.FromBase64String(Objet.MB_SIGNATURE.ToString());
                //clsMobiletiers.RP_DATEVALIDATION = DateTime.Parse(Objet.RP_DATEVALIDATION.ToString());
                //clsMobiletiers.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                //clsMobiletiers.SL_LOGIN = Objet.SL_LOGIN.ToString();
                //clsMobiletiers.RP_HEURE = DateTime.Parse(Objet.RP_HEURE.ToString());
                //clsMobiletiers.RP_DATE = DateTime.Parse(Objet.RP_DATE.ToString());
                //clsMobiletiers.RP_CODEVALIDATION = Objet.RP_CODEVALIDATION.ToString();
                //clsMobiletiers.SL_MOTPASSE = Objet.SL_MOTPASSE.ToString();
                //clsMobiletiers.COCHER = Objet.COCHER.ToString();

                clsMobiletiers.MB_IDTIERS = Objet.MB_IDTIERS.ToString();
                clsMobiletiers.MB_CODETIERS = Objet.MB_CODETIERS.ToString();
                clsMobiletiers.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMobiletiers.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMobiletiers.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMobiletiers.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMobiletiers.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMobiletiers.CL_IDCLIENTAGENTAGREE = Objet.CL_IDCLIENTAGENTAGREE.ToString();
                clsMobiletiers.MB_DATECREATION = DateTime.Parse(Objet.MB_DATECREATION.ToString());
                clsMobiletiers.MB_NOMTIERS = Objet.MB_NOMTIERS.ToString();
                clsMobiletiers.MB_PRENOMTIERS = Objet.MB_PRENOMTIERS.ToString();
                clsMobiletiers.MB_DATENAISSANCE = DateTime.Parse(Objet.MB_DATENAISSANCE.ToString());
                clsMobiletiers.MB_LIEUNAISSANCE = Objet.MB_LIEUNAISSANCE.ToString();
                clsMobiletiers.MB_TELEPHONE = Objet.MB_TELEPHONE.ToString();
                clsMobiletiers.MB_EMAIL = Objet.MB_EMAIL.ToString();
                clsMobiletiers.MB_NUMPIECE = Objet.MB_NUMPIECE.ToString();
                clsMobiletiers.MB_INDICATIF = Objet.MB_INDICATIF.ToString();
                //clsMobiletiers.MB_PHOTO = System.Convert.FromBase64String(Objet.MB_PHOTO.ToString());
                // clsMobiletiers.MB_SIGNATURE = System.Convert.FromBase64String(Objet.MB_SIGNATURE.ToString());
                Byte[] MB_PHOTO = null;
                Byte[] MB_SIGNATURE = null;
                if (Objet.MB_PHOTO != "")
                    MB_PHOTO = System.Convert.FromBase64String(Objet.MB_PHOTO);
                if (Objet.MB_SIGNATURE != "")
                    MB_SIGNATURE = System.Convert.FromBase64String(Objet.MB_SIGNATURE);

                clsMobiletiers.MB_PHOTO = MB_PHOTO;
                clsMobiletiers.MB_SIGNATURE = MB_SIGNATURE;

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMobiletiersWSBLL.pvgAjouter(clsDonnee, clsMobiletiers, clsObjetEnvoi));
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

        //AJOUT
        public string pvgValidationDemandedeModificationMotdePasse(List<clsMobiletiers> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMobiletiers> clsMobiletierss = new List<ZenithWebServeur.BOJ.clsMobiletiers>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireInsert(Objet[Idx]);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST DES TYPES DE DONNEES
            //DataSet = TestTypeDonnee(Objet[Idx]);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            ////--TEST CONTRAINTE
            //DataSet = TestTestContrainteListe(Objet[Idx]);
            ////--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();

                    clsMobiletiers.SL_MOTPASSE = clsMobiletiersDTO.SL_MOTPASSE.ToString();
                    clsMobiletiers.RP_CODEVALIDATION = clsMobiletiersDTO.RP_CODEVALIDATION.ToString();
                    clsMobiletiers.RP_DATE = DateTime.Parse(clsMobiletiersDTO.RP_DATE.ToString());
                    clsMobiletiers.RP_HEURE = DateTime.Parse(clsMobiletiersDTO.RP_HEURE.ToString());
                    clsMobiletiers.SL_LOGIN = clsMobiletiersDTO.SL_LOGIN.ToString();
                    clsMobiletiers.CL_IDCLIENT = clsMobiletiersDTO.CL_IDCLIENT.ToString();
                    clsMobiletiers.MB_IDTIERS = clsMobiletiersDTO.MB_IDTIERS.ToString();
                    clsMobiletiers.RP_DATEVALIDATION = DateTime.Parse(clsMobiletiersDTO.RP_DATEVALIDATION.ToString());
                    clsMobiletiers.COCHER = clsMobiletiersDTO.COCHER.ToString();

                    clsObjetEnvoi.OE_A = clsMobiletiersDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMobiletiersDTO.clsObjetEnvoi.OE_Y;
                    clsMobiletierss.Add(clsMobiletiers);
                }
                clsObjetRetour.SetValue(true, clsMobiletiersWSBLL.pvgValidationDemandedeModificationMotdePasse(clsDonnee, clsMobiletierss, clsObjetEnvoi));
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
        public string pvgModifier(clsMobiletiers Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgpvgModifier(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.CO_CODECOMPTE, Objet.MB_CODETIERS };

                //foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
                //{

                //clsMobiletiers.MB_IDTIERS = Objet.MB_IDTIERS.ToString();
                //clsMobiletiers.MB_CODETIERS = Objet.MB_CODETIERS.ToString();
                //clsMobiletiers.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsMobiletiers.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsMobiletiers.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                //clsMobiletiers.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                //clsMobiletiers.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                //clsMobiletiers.CL_IDCLIENTAGENTAGREE = Objet.CL_IDCLIENTAGENTAGREE.ToString();
                //clsMobiletiers.MB_DATECREATION = DateTime.Parse(Objet.MB_DATECREATION.ToString());
                //clsMobiletiers.MB_DATESAISIE = DateTime.Parse(Objet.MB_DATESAISIE.ToString());
                //clsMobiletiers.MB_NOMTIERS = Objet.MB_NOMTIERS.ToString();
                //clsMobiletiers.MB_PRENOMTIERS = Objet.MB_PRENOMTIERS.ToString();
                //clsMobiletiers.MB_DATENAISSANCE = DateTime.Parse(Objet.MB_DATENAISSANCE.ToString());
                //clsMobiletiers.MB_LIEUNAISSANCE = Objet.MB_LIEUNAISSANCE.ToString();
                //clsMobiletiers.MB_TELEPHONE = Objet.MB_TELEPHONE.ToString();
                //clsMobiletiers.MB_EMAIL = Objet.MB_EMAIL.ToString();
                //clsMobiletiers.MB_NUMPIECE = Objet.MB_NUMPIECE.ToString();
                //clsMobiletiers.MB_INDICATIF = Objet.MB_INDICATIF.ToString();
                //clsMobiletiers.MB_PHOTO = System.Convert.FromBase64String(Objet.MB_PHOTO.ToString());
                //clsMobiletiers.MB_SIGNATURE = System.Convert.FromBase64String(Objet.MB_SIGNATURE.ToString());
                //clsMobiletiers.RP_DATEVALIDATION = DateTime.Parse(Objet.RP_DATEVALIDATION.ToString());
                //clsMobiletiers.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                //clsMobiletiers.SL_LOGIN = Objet.SL_LOGIN.ToString();
                //clsMobiletiers.RP_HEURE = DateTime.Parse(Objet.RP_HEURE.ToString());
                //clsMobiletiers.RP_DATE = DateTime.Parse(Objet.RP_DATE.ToString());
                //clsMobiletiers.RP_CODEVALIDATION = Objet.RP_CODEVALIDATION.ToString();
                //clsMobiletiers.SL_MOTPASSE = Objet.SL_MOTPASSE.ToString();
                //clsMobiletiers.COCHER = Objet.COCHER.ToString();

                clsMobiletiers.MB_IDTIERS = Objet.MB_IDTIERS.ToString();
                clsMobiletiers.MB_CODETIERS = Objet.MB_CODETIERS.ToString();
                clsMobiletiers.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMobiletiers.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMobiletiers.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMobiletiers.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMobiletiers.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMobiletiers.CL_IDCLIENTAGENTAGREE = Objet.CL_IDCLIENTAGENTAGREE.ToString();
                clsMobiletiers.MB_DATECREATION = DateTime.Parse(Objet.MB_DATECREATION.ToString());
                clsMobiletiers.MB_NOMTIERS = Objet.MB_NOMTIERS.ToString();
                clsMobiletiers.MB_PRENOMTIERS = Objet.MB_PRENOMTIERS.ToString();
                clsMobiletiers.MB_DATENAISSANCE = DateTime.Parse(Objet.MB_DATENAISSANCE.ToString());
                clsMobiletiers.MB_LIEUNAISSANCE = Objet.MB_LIEUNAISSANCE.ToString();
                clsMobiletiers.MB_TELEPHONE = Objet.MB_TELEPHONE.ToString();
                clsMobiletiers.MB_EMAIL = Objet.MB_EMAIL.ToString();
                clsMobiletiers.MB_NUMPIECE = Objet.MB_NUMPIECE.ToString();
                clsMobiletiers.MB_INDICATIF = Objet.MB_INDICATIF.ToString();
                // clsMobiletiers.MB_PHOTO = System.Convert.FromBase64String(Objet.MB_PHOTO.ToString());
                // clsMobiletiers.MB_SIGNATURE = System.Convert.FromBase64String(Objet.MB_SIGNATURE.ToString());
                Byte[] MB_PHOTO = null;
                Byte[] MB_SIGNATURE = null;
                if (Objet.MB_PHOTO != "")
                    MB_PHOTO = System.Convert.FromBase64String(Objet.MB_PHOTO);
                if (Objet.MB_SIGNATURE != "")
                    MB_SIGNATURE = System.Convert.FromBase64String(Objet.MB_SIGNATURE);

                clsMobiletiers.MB_PHOTO = MB_PHOTO;
                clsMobiletiers.MB_SIGNATURE = MB_SIGNATURE;

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMobiletiersWSBLL.pvgModifier(clsDonnee, clsMobiletiers, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMobiletiers Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireDeletepvgSupprimer(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.MB_IDTIERS };

                //foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMobiletiersWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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

        //LISTE
        //public string pvgChargerDansDataSet(clsMobiletiers Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    //for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    //{
        //    //--TEST DES CHAMPS OBLIGATOIRES
        //    //DataSet = TestChampObligatoireListe(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //--TEST DES TYPES DE DONNEES
        //    //DataSet = TestTypeDonnee(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //--TEST CONTRAINTE
        //    //DataSet = TestTestContrainteListe(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //}

        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        //clsDonnee.pvgConnectionBase();
        //        clsDonnee.pvgDemarrerTransaction();
        //        clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.MB_CODETIERS, Objet.MB_TELEPHONE, Objet.MB_NOMTIERS, Objet.MB_PRENOMTIERS, Objet.MB_DATECREATION1, Objet.MB_DATECREATION2 };

        //        //foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
        //        //{

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        //clsObjetRetour.SetValue(true, clsMobiletiersWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
        //        DataSet = clsMobiletiersWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
        //         if (DataSet.Tables[0].Rows.Count > 0)
        //        {
        //            DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //            DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //            DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //            for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
        //            {
        //                DataSet.Tables[0].Rows[i]["SL_CODEMESSAGE"] = "00";
        //                DataSet.Tables[0].Rows[i]["SL_RESULTAT"] = "TRUE";
        //                DataSet.Tables[0].Rows[i]["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
        //            }

        //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //        }
        //        else
        //        {
        //            DataSet = new DataSet();
        //            DataRow dr = dt.NewRow();
        //            dr["SL_CODEMESSAGE"] = "99";
        //            dr["SL_RESULTAT"] = "FALSE";
        //            dr["SL_MESSAGE"] = "Aucun enregistrement n'a été trouvé";
        //            dt.Rows.Add(dr);
        //            DataSet.Tables.Add(dt);
        //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
        //        }
        //        //}
        //    }
        //     catch (SqlException SQLEx)
        //    {
        //        DataSet = new DataSet();
        //        DataRow dr = dt.NewRow();
        //        dr["SL_CODEMESSAGE"] = "99";
        //        dr["SL_RESULTAT"] = "FALSE";
        //        dr["SL_MESSAGE"] = (SQLEx.Number == 2601 || SQLEx.Number == 2627) ? clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0003").MS_LIBELLEMESSAGE : SQLEx.Message;
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
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public List<ZenithWebServeur.DTO.clsMobiletiers> pvgChargerDansDataSet(List<ZenithWebServeur.DTO.clsMobiletiers> Objet)
        {

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsMobiletiers> clsMobiletierss = new List<ZenithWebServeur.DTO.clsMobiletiers>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    clsMobiletierss = TestChampObligatoireListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsMobiletierss[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsMobiletierss;
            //    //--TEST CONTRAINTE
            //    clsMobiletierss = TestTestContrainteListe(Objet[Idx]);
            //    //--VERIFICATION DU RESULTAT DU TEST
            //    if (clsMobiletierss[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsMobiletierss;
            //}


            clsObjetEnvoi.OE_PARAM = new string[] { };
            DataSet DataSet = new DataSet();

            try
            {
                clsDonnee.pvgConnectionBase();

                foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();
                    clsObjetEnvoi.OE_PARAM = new string[] {
                        clsMobiletiersDTO.AG_CODEAGENCE,
                        clsMobiletiersDTO.MB_CODETIERS,
                        clsMobiletiersDTO.MB_TELEPHONE,
                        clsMobiletiersDTO.MB_NOMTIERS,
                        clsMobiletiersDTO.MB_PRENOMTIERS,
                        clsMobiletiersDTO.MB_DATECREATION1.ToString(),
                        clsMobiletiersDTO.MB_DATECREATION2.ToString()
                    };

                    clsObjetEnvoi.OE_A = clsMobiletiersDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMobiletiersDTO.clsObjetEnvoi.OE_Y;
                }

                DataSet = clsMobiletiersWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
                clsMobiletierss = new List<ZenithWebServeur.DTO.clsMobiletiers>();
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in DataSet.Tables[0].Rows)
                    {
                        ZenithWebServeur.DTO.clsMobiletiers clsMobiletiers = new ZenithWebServeur.DTO.clsMobiletiers();
                        clsMobiletiers.MB_IDTIERS = row["MB_IDTIERS"].ToString();
                        clsMobiletiers.MB_CODETIERS = row["MB_CODETIERS"].ToString();
                        clsMobiletiers.AG_CODEAGENCE = row["AG_CODEAGENCE"].ToString();
                        clsMobiletiers.PV_CODEPOINTVENTE = row["PV_CODEPOINTVENTE"].ToString();
                        clsMobiletiers.OP_CODEOPERATEUR = row["OP_CODEOPERATEUR"].ToString();
                        clsMobiletiers.PI_CODEPIECE = row["PI_CODEPIECE"].ToString();
                        clsMobiletiers.CO_CODECOMPTE = row["CO_CODECOMPTE"].ToString();
                        clsMobiletiers.CL_IDCLIENTAGENTAGREE = row["CL_IDCLIENTAGENTAGREE"].ToString();
                        clsMobiletiers.MB_DATECREATION = row["MB_DATECREATION"].ToString();
                        clsMobiletiers.MB_DATESAISIE = row["MB_DATESAISIE"].ToString();
                        clsMobiletiers.MB_NOMTIERS = row["MB_NOMTIERS"].ToString();
                        clsMobiletiers.MB_PRENOMTIERS = row["MB_PRENOMTIERS"].ToString();
                        clsMobiletiers.MB_DATENAISSANCE = row["MB_DATENAISSANCE"].ToString();
                        clsMobiletiers.MB_LIEUNAISSANCE = row["MB_LIEUNAISSANCE"].ToString();
                        clsMobiletiers.MB_TELEPHONE = row["MB_TELEPHONE"].ToString();
                        clsMobiletiers.MB_EMAIL = row["MB_EMAIL"].ToString();
                        clsMobiletiers.MB_NUMPIECE = row["MB_NUMPIECE"].ToString();
                        clsMobiletiers.MB_INDICATIF = row["MB_INDICATIF"].ToString();
                        clsMobiletiers.MB_NOMTIERS = row["MB_NOMTIERS"].ToString();
                        clsMobiletiers.MB_PRENOMTIERS = row["MB_PRENOMTIERS"].ToString();
                        clsMobiletiers.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                        clsMobiletiers.clsObjetRetour.SL_CODEMESSAGE = "00";
                        clsMobiletiers.clsObjetRetour.SL_RESULTAT = "TRUE";
                        clsMobiletiers.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
                        clsMobiletierss.Add(clsMobiletiers);
                    }
                }
                else
                {
                    ZenithWebServeur.DTO.clsMobiletiers clsMobiletiers = new ZenithWebServeur.DTO.clsMobiletiers();
                    clsMobiletiers.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                    clsMobiletiers.clsObjetRetour.SL_CODEMESSAGE = "99";
                    clsMobiletiers.clsObjetRetour.SL_RESULTAT = "FALSE";
                    clsMobiletiers.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé";
                    clsMobiletierss.Add(clsMobiletiers);
                }
            }
            catch (SqlException SQLEx)
            {
                ZenithWebServeur.DTO.clsMobiletiers clsMobiletiers = new ZenithWebServeur.DTO.clsMobiletiers();
                clsMobiletiers.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsMobiletiers.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsMobiletiers.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsMobiletiers.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsMobiletierss = new List<ZenithWebServeur.DTO.clsMobiletiers>();
                clsMobiletierss.Add(clsMobiletiers);
            }
            catch (Exception SQLEx)
            {
                ZenithWebServeur.DTO.clsMobiletiers clsMobiletiers = new ZenithWebServeur.DTO.clsMobiletiers();
                clsMobiletiers.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
                clsMobiletiers.clsObjetRetour.SL_CODEMESSAGE = "99";
                clsMobiletiers.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
                clsMobiletiers.clsObjetRetour.SL_RESULTAT = "FALSE";
                //Execution du log
                Log.Error(SQLEx.Message, null);
                clsMobiletierss = new List<ZenithWebServeur.DTO.clsMobiletiers>();
                clsMobiletierss.Add(clsMobiletiers);
            }

            finally
            {
                clsDonnee.pvgDeConnectionBase();
            }
            return clsMobiletierss;
        }

        //LISTE
        public string pvgChargerDansDataSetListeDeMotdePasseaModifier(clsMobiletiers Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("MB_IDTIERS_2", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMobiletiers clsMobiletiers = new ZenithWebServeur.BOJ.clsMobiletiers();
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
                    Objet.CL_CODECLIENT,
                    Objet.CL_NOMCLIENT,
                    Objet.CL_PRENOMCLIENT,
                    Objet.CL_TELEPHONE,
                    Objet.OP_CODEOPERATEUR,
                    Objet.DATEDEBUT,
                    Objet.DATEFIN,
                    Objet.TYPEOPERATION
                };

                //foreach (ZenithWebServeur.DTO.clsMobiletiers clsMobiletiersDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMobiletiersWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMobiletiersWSBLL.pvgChargerDansDataSetListeDeMotdePasseaModifier(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("MB_IDTIERS_2", typeof(string)));
                    
                    for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
                    {
                        DataSet.Tables[0].Rows[i]["MB_IDTIERS_2"] = DataSet.Tables[0].Rows[i]["MB_IDTIERS"].ToString();
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
