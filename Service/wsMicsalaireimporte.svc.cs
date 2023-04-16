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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicsalaireimporte" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicsalaireimporte.svc ou wsMicsalaireimporte.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicsalaireimporte : IwsMicsalaireimporte
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicsalaireimporteWSBLL clsMicsalaireimporteWSBLL = new clsMicsalaireimporteWSBLL();

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
        public string pvgAjouterListe(List<clsMicsalaireimporte> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicsalaireimporte> clsMicsalaireimportes = new List<ZenithWebServeur.BOJ.clsMicsalaireimporte>();
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

                foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();

                    clsMicsalaireimporte.AG_CODEAGENCE = clsMicsalaireimporteDTO.AG_CODEAGENCE.ToString();
                    clsMicsalaireimporte.PV_CODEPOINTVENTE = clsMicsalaireimporteDTO.PV_CODEPOINTVENTE.ToString();
                    clsMicsalaireimporte.OP_CODEOPERATEUR = clsMicsalaireimporteDTO.OP_CODEOPERATEUR.ToString();
                    clsMicsalaireimporte.SL_IDINDEX = clsMicsalaireimporteDTO.SL_IDINDEX.ToString();
                    clsMicsalaireimporte.CL_NUMEROCOMPTE = clsMicsalaireimporteDTO.CL_NUMEROCOMPTE.ToString();
                    clsMicsalaireimporte.CO_CODECOMPTE = clsMicsalaireimporteDTO.CO_CODECOMPTE.ToString();
                    clsMicsalaireimporte.PS_CODESOUSPRODUIT = clsMicsalaireimporteDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicsalaireimporte.PL_CODENUMCOMPTE = clsMicsalaireimporteDTO.PL_CODENUMCOMPTE.ToString();
                    clsMicsalaireimporte.CODEPARAMETRE = clsMicsalaireimporteDTO.CODEPARAMETRE.ToString();
                    clsMicsalaireimporte.CL_NOMCLIENT = clsMicsalaireimporteDTO.CL_NOMCLIENT.ToString();
                    clsMicsalaireimporte.CL_PRENOMCLIENT = clsMicsalaireimporteDTO.CL_PRENOMCLIENT.ToString();
                    clsMicsalaireimporte.CL_SALAIREDEBIT = Double.Parse(clsMicsalaireimporteDTO.CL_SALAIREDEBIT.ToString());
                    clsMicsalaireimporte.CL_SALAIRECREDIT = Double.Parse(clsMicsalaireimporteDTO.CL_SALAIRECREDIT.ToString());
                    clsMicsalaireimporte.CL_FRAISPRELEVEMENT = Double.Parse(clsMicsalaireimporteDTO.CL_FRAISPRELEVEMENT.ToString());
                    clsMicsalaireimporte.CL_AUTREFRAIS = Double.Parse(clsMicsalaireimporteDTO.CL_AUTREFRAIS.ToString());
                    clsMicsalaireimporte.DATEJOURNEE = DateTime.Parse(clsMicsalaireimporteDTO.DATEJOURNEE.ToString());
                    clsMicsalaireimporte.LIBELLEOPERATION = clsMicsalaireimporteDTO.LIBELLEOPERATION.ToString();
                    clsMicsalaireimporte.TS_CODETYPESCHEMACOMPTABLE = clsMicsalaireimporteDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMicsalaireimporte.TYPEOPERATION = clsMicsalaireimporteDTO.TYPEOPERATION.ToString();
                    clsMicsalaireimporte.MC_NUMPIECE = clsMicsalaireimporteDTO.MC_NUMPIECE.ToString();
                    clsMicsalaireimporte.CL_NOUVEAUCLIENT = clsMicsalaireimporteDTO.CL_NOUVEAUCLIENT.ToString();
                    clsMicsalaireimporte.NO_CODENATUREVIREMENT = clsMicsalaireimporteDTO.NO_CODENATUREVIREMENT.ToString();
                    clsMicsalaireimporte.SL_NOMTIERS = clsMicsalaireimporteDTO.SL_NOMTIERS.ToString();
                    clsMicsalaireimporte.AT_CODEACTIVITE = clsMicsalaireimporteDTO.AT_CODEACTIVITE.ToString();
                    clsMicsalaireimporte.SL_TYPEOPERATION = clsMicsalaireimporteDTO.SL_TYPEOPERATION.ToString();
                    //clsMicsalaireimporte.PS_ABREVIATION = clsMicsalaireimporteDTO.PS_ABREVIATION.ToString();

                    clsObjetEnvoi.OE_A = clsMicsalaireimporteDTO.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = clsMicsalaireimporteDTO.clsObjetEnvoi.OE_Y;

                    clsMicsalaireimportes.Add(clsMicsalaireimporte);
                }
                clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgAjouterListe(clsDonnee, clsMicsalaireimportes, clsObjetEnvoi));
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
        public string pvgAjouterListeValidationVirementSalaire(List<clsMicsalaireimporte> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicsalaireimporte> clsMicsalaireimportes = new List<ZenithWebServeur.BOJ.clsMicsalaireimporte>();
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

                foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();

                    clsMicsalaireimporte.AG_CODEAGENCE = clsMicsalaireimporteDTO.AG_CODEAGENCE.ToString();
                    clsMicsalaireimporte.PV_CODEPOINTVENTE = clsMicsalaireimporteDTO.PV_CODEPOINTVENTE.ToString();
                    clsMicsalaireimporte.OP_CODEOPERATEUR = clsMicsalaireimporteDTO.OP_CODEOPERATEUR.ToString();
                    clsMicsalaireimporte.SL_IDINDEX = clsMicsalaireimporteDTO.SL_IDINDEX.ToString();
                    clsMicsalaireimporte.CL_NUMEROCOMPTE = clsMicsalaireimporteDTO.CL_NUMEROCOMPTE.ToString();
                    clsMicsalaireimporte.CO_CODECOMPTE = clsMicsalaireimporteDTO.CO_CODECOMPTE.ToString();
                    clsMicsalaireimporte.PS_CODESOUSPRODUIT = clsMicsalaireimporteDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicsalaireimporte.PL_CODENUMCOMPTE = clsMicsalaireimporteDTO.PL_CODENUMCOMPTE.ToString();
                    clsMicsalaireimporte.CODEPARAMETRE = clsMicsalaireimporteDTO.CODEPARAMETRE.ToString();
                    clsMicsalaireimporte.CL_NOMCLIENT = clsMicsalaireimporteDTO.CL_NOMCLIENT.ToString();
                    clsMicsalaireimporte.CL_PRENOMCLIENT = clsMicsalaireimporteDTO.CL_PRENOMCLIENT.ToString();
                    clsMicsalaireimporte.CL_SALAIREDEBIT = Double.Parse(clsMicsalaireimporteDTO.CL_SALAIREDEBIT.ToString());
                    clsMicsalaireimporte.CL_SALAIRECREDIT = Double.Parse(clsMicsalaireimporteDTO.CL_SALAIRECREDIT.ToString());
                    clsMicsalaireimporte.CL_FRAISPRELEVEMENT = Double.Parse(clsMicsalaireimporteDTO.CL_FRAISPRELEVEMENT.ToString());
                    clsMicsalaireimporte.CL_AUTREFRAIS = Double.Parse(clsMicsalaireimporteDTO.CL_AUTREFRAIS.ToString());
                    clsMicsalaireimporte.DATEJOURNEE = DateTime.Parse(clsMicsalaireimporteDTO.DATEJOURNEE.ToString());
                    clsMicsalaireimporte.LIBELLEOPERATION = clsMicsalaireimporteDTO.LIBELLEOPERATION.ToString();
                    clsMicsalaireimporte.TS_CODETYPESCHEMACOMPTABLE = clsMicsalaireimporteDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMicsalaireimporte.TYPEOPERATION = clsMicsalaireimporteDTO.TYPEOPERATION.ToString();
                    clsMicsalaireimporte.MC_NUMPIECE = clsMicsalaireimporteDTO.MC_NUMPIECE.ToString();
                    clsMicsalaireimporte.CL_NOUVEAUCLIENT = clsMicsalaireimporteDTO.CL_NOUVEAUCLIENT.ToString();
                    clsMicsalaireimporte.NO_CODENATUREVIREMENT = clsMicsalaireimporteDTO.NO_CODENATUREVIREMENT.ToString();
                    clsMicsalaireimporte.SL_NOMTIERS = clsMicsalaireimporteDTO.SL_NOMTIERS.ToString();
                    clsMicsalaireimporte.AT_CODEACTIVITE = clsMicsalaireimporteDTO.AT_CODEACTIVITE.ToString();
                    clsMicsalaireimporte.SL_TYPEOPERATION = clsMicsalaireimporteDTO.SL_TYPEOPERATION.ToString();
                    clsMicsalaireimporte.PS_ABREVIATION = clsMicsalaireimporteDTO.PS_ABREVIATION.ToString();

                    clsObjetEnvoi.OE_A = clsMicsalaireimporteDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicsalaireimporteDTO.clsObjetEnvoi.OE_Y;

                    clsMicsalaireimportes.Add(clsMicsalaireimporte);
                }
                clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgAjouterListeValidationVirementSalaire(clsDonnee, clsMicsalaireimportes, clsObjetEnvoi));
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
        //public string pvgAjouter(clsMicsalaireimporte Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    //for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    //{
        //    //--TEST DES CHAMPS OBLIGATOIRES
        //    DataSet = TestChampObligatoireInsert(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //--TEST DES TYPES DE DONNEES
        //    DataSet = TestTypeDonnee(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //--TEST CONTRAINTE
        //    DataSet = TestTestContrainteListe(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //}

        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        //clsDonnee.pvgConnectionBase();
        //        clsDonnee.pvgDemarrerTransaction();
        //        //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };

        //        //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
        //        //{

        //        clsMicsalaireimporte.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
        //        clsMicsalaireimporte.VL_CODEVILLE = Objet.VL_CODEVILLE.ToString();
        //        //clsMicsalaireimporte.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
        //        clsMicsalaireimporte.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
        //        clsMicsalaireimporte.PV_RAISONSOCIAL = Objet.PV_RAISONSOCIAL.ToString();
        //        clsMicsalaireimporte.PV_BOITEPOSTAL = Objet.PV_BOITEPOSTAL.ToString();
        //        clsMicsalaireimporte.PV_ADRESSEGEOGRAPHIQUE = Objet.PV_ADRESSEGEOGRAPHIQUE.ToString();
        //        clsMicsalaireimporte.PV_TELEPHONE = Objet.PV_TELEPHONE.ToString();
        //        clsMicsalaireimporte.PV_FAX = Objet.PV_FAX.ToString();
        //        clsMicsalaireimporte.PV_EMAIL = Objet.PV_EMAIL.ToString();
        //        clsMicsalaireimporte.PV_POINTVENTECODE = Objet.PV_POINTVENTECODE.ToString();
        //        clsMicsalaireimporte.PV_NUMEROCOMPTE = Objet.PV_NUMEROCOMPTE.ToString();
        //        clsMicsalaireimporte.PV_REFERENCE = Objet.PV_REFERENCE.ToString();
        //        clsMicsalaireimporte.PV_DATECREATION = DateTime.Parse(Objet.PV_DATECREATION.ToString());
        //        clsMicsalaireimporte.PV_ACTIF = Objet.PV_ACTIF.ToString();
        //        clsMicsalaireimporte.OP_GERANT = Objet.OP_GERANT.ToString();

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgAjouter(clsDonnee, clsMicsalaireimporte, clsObjetEnvoi));
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
        //        //}
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

        //MODIFICATION
        //public string pvgModifier(clsMicsalaireimporte Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    //for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    //{
        //    //--TEST DES CHAMPS OBLIGATOIRES
        //    DataSet = TestChampObligatoireUpdate(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //--TEST DES TYPES DE DONNEES
        //    DataSet = TestTypeDonnee(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //--TEST CONTRAINTE
        //    DataSet = TestTestContrainteListe(Objet);
        //    //--VERIFICATION DU RESULTAT DU TEST
        //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
        //    //}

        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        //clsDonnee.pvgConnectionBase();
        //        clsDonnee.pvgDemarrerTransaction();
        //        //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };

        //        //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
        //        //{

        //        clsMicsalaireimporte.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
        //        clsMicsalaireimporte.VL_CODEVILLE = Objet.VL_CODEVILLE.ToString();
        //        //clsMicsalaireimporte.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
        //        clsMicsalaireimporte.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
        //        clsMicsalaireimporte.PV_RAISONSOCIAL = Objet.PV_RAISONSOCIAL.ToString();
        //        clsMicsalaireimporte.PV_BOITEPOSTAL = Objet.PV_BOITEPOSTAL.ToString();
        //        clsMicsalaireimporte.PV_ADRESSEGEOGRAPHIQUE = Objet.PV_ADRESSEGEOGRAPHIQUE.ToString();
        //        clsMicsalaireimporte.PV_TELEPHONE = Objet.PV_TELEPHONE.ToString();
        //        clsMicsalaireimporte.PV_FAX = Objet.PV_FAX.ToString();
        //        clsMicsalaireimporte.PV_EMAIL = Objet.PV_EMAIL.ToString();
        //        clsMicsalaireimporte.PV_POINTVENTECODE = Objet.PV_POINTVENTECODE.ToString();
        //        clsMicsalaireimporte.PV_NUMEROCOMPTE = Objet.PV_NUMEROCOMPTE.ToString();
        //        clsMicsalaireimporte.PV_REFERENCE = Objet.PV_REFERENCE.ToString();
        //        clsMicsalaireimporte.PV_DATECREATION = DateTime.Parse(Objet.PV_DATECREATION.ToString());
        //        clsMicsalaireimporte.PV_ACTIF = Objet.PV_ACTIF.ToString();
        //        clsMicsalaireimporte.OP_GERANT = Objet.OP_GERANT.ToString();

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgModifier(clsDonnee, clsMicsalaireimporte, clsObjetEnvoi));
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
        //        //}
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

        //SUPPRESSION
        public string pvgSupprimerVersementACC(clsMicsalaireimporte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
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
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                //{

                clsMicsalaireimporte.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_CODEOPERATEUR;

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgSupprimerVersementACC(clsDonnee, clsObjetEnvoi, clsMicsalaireimporte));
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

        public string pvgChargerDansDataSetPourCombo2(clsMicsalaireimporte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_MATRICULEEMPLOYE };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                //{
                
                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgChargerDansDataSetPourCombo2(clsDonnee, clsObjetEnvoi));
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

        public string pvgChargerDansDataSetPourCombo3(clsMicsalaireimporte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgChargerDansDataSetPourCombo3(clsDonnee, clsObjetEnvoi));
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

        public string pvgChargerDansDataSetPourCombo4(clsMicsalaireimporte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_MATRICULEEMPLOYE, Objet.PS_ABREVIATION };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgChargerDansDataSetPourCombo4(clsDonnee, clsObjetEnvoi));
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

        public string pvgChargerDansDataSetPourCombo1(clsMicsalaireimporte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_NUMEROCOMPTE };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMicsalaireimporteWSBLL.pvgChargerDansDataSetPourCombo1(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSet(clsMicsalaireimporte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimporte clsMicsalaireimporte = new ZenithWebServeur.BOJ.clsMicsalaireimporte();
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
                    Objet.OP_CODEOPERATEUR,
                    Objet.OP_NOMPRENOM,
                    Objet.PL_CODENUMCOMPTE,
                    Objet.PL_NUMCOMPTE,
                    Objet.PL_LIBELLE,
                    Objet.OP_CAISSIER,
                    Objet.CODEPARAMETRE,
                    Objet.OP_AGENTDECOLLECTEETDECREDIT
                };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimporte clsMicsalaireimporteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicsalaireimporteWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicsalaireimporteWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
