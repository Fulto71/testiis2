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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicordrevirement" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicordrevirement.svc ou wsMicordrevirement.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicordrevirement : IwsMicordrevirement
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicordrevirementWSBLL clsMicordrevirementWSBLL = new clsMicordrevirementWSBLL();

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
        public string pvgAjouter(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
            List<ZenithWebServeur.BOJ.clsMicordrevirementdetail> clsMicordrevirementdetails = new List<ZenithWebServeur.BOJ.clsMicordrevirementdetail>();
            List<ZenithWebServeur.BOJ.clsMicordrevirementfrais> clsMicordrevirementfraiss = new List<ZenithWebServeur.BOJ.clsMicordrevirementfrais>();
            List<ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais> clsMicordrevirementdetailfraiss = new List<ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais>();

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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                //MICORDREVIREMENTDETAIL
                foreach (ZenithWebServeur.DTO.clsMicordrevirementdetail clsMicordrevirementDTO in Objet.MICORDREVIREMENTDETAIL)
                {
                    ZenithWebServeur.BOJ.clsMicordrevirementdetail clsMicordrevirementdetail = new ZenithWebServeur.BOJ.clsMicordrevirementdetail();

                    //MICORDREVIREMENTDETAILFRAIS
                    foreach (ZenithWebServeur.DTO.clsMicordrevirementdetailfrais clsMicordrevirementdetailfraisDTO in clsMicordrevirementDTO.MICORDREVIREMENTDETAILFRAIS)
                    {
                        ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais clsMicordrevirementdetailfrais = new ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais();

                        //clsMicordrevirementdetails
                        clsMicordrevirementdetailfrais.AG_CODEAGENCE = clsMicordrevirementDTO.AG_CODEAGENCE.ToString();
                        clsMicordrevirementdetailfrais.VD_CODEORDREVIREMENTDETAIL = clsMicordrevirementDTO.VD_CODEORDREVIREMENTDETAIL.ToString();
                        clsMicordrevirementdetailfrais.PL_CODEPARAMETRELISTE = clsMicordrevirementDTO.OV_CODEORDREVIREMENT.ToString();
                        clsMicordrevirementdetailfrais.OF_MONTANT = Double.Parse(clsMicordrevirementDTO.CO_CODECOMPTE.ToString());

                        //clsObjetEnvoi.OE_A = clsMicordrevirementDTO.clsObjetEnvoi.OE_A;
                        //clsObjetEnvoi.OE_Y = clsMicordrevirementDTO.clsObjetEnvoi.OE_Y;

                        clsMicordrevirementdetailfraiss.Add(clsMicordrevirementdetailfrais);
                    }

                    //clsMicordrevirementdetails
                    clsMicordrevirementdetail.AG_CODEAGENCE = clsMicordrevirementDTO.AG_CODEAGENCE.ToString();
                    clsMicordrevirementdetail.VD_CODEORDREVIREMENTDETAIL = clsMicordrevirementDTO.VD_CODEORDREVIREMENTDETAIL.ToString();
                    clsMicordrevirementdetail.OV_CODEORDREVIREMENT = clsMicordrevirementDTO.OV_CODEORDREVIREMENT.ToString();
                    clsMicordrevirementdetail.CO_CODECOMPTE = clsMicordrevirementDTO.CO_CODECOMPTE.ToString();
                    clsMicordrevirementdetail.VD_LIBELLE = clsMicordrevirementDTO.VD_LIBELLE.ToString();
                    clsMicordrevirementdetail.VD_MONTANT = Double.Parse(clsMicordrevirementDTO.VD_MONTANT.ToString());
                    clsMicordrevirementdetail.VD_DATEPRELEVEMENT = DateTime.Parse(clsMicordrevirementDTO.VD_DATEPRELEVEMENT.ToString());
                    clsMicordrevirementdetail.VD_DATECLOTURE = DateTime.Parse(clsMicordrevirementDTO.VD_DATECLOTURE.ToString());
                    clsMicordrevirementdetail.VD_DATESUSPENSION = DateTime.Parse(clsMicordrevirementDTO.VD_DATESUSPENSION.ToString());
                    clsMicordrevirementdetail.VD_DATEFINSUSPENSION = DateTime.Parse(clsMicordrevirementDTO.VD_DATEFINSUSPENSION.ToString());
                    clsMicordrevirementdetail.VD_SAISIE = DateTime.Parse(clsMicordrevirementDTO.VD_SAISIE.ToString());
                    clsMicordrevirementdetail.MICORDREVIREMENTDETAILFRAIS = clsMicordrevirementdetailfraiss;

                    //clsObjetEnvoi.OE_A = clsMicordrevirementDTO.clsObjetEnvoi.OE_A;
                    //clsObjetEnvoi.OE_Y = clsMicordrevirementDTO.clsObjetEnvoi.OE_Y;

                    clsMicordrevirementdetails.Add(clsMicordrevirementdetail);
                }

                //MICORDREVIREMENTFRAIS
                foreach (ZenithWebServeur.DTO.clsMicordrevirementfrais clsMicordrevirementDTO in Objet.MICORDREVIREMENTFRAIS)
                {
                    ZenithWebServeur.BOJ.clsMicordrevirementfrais clsMicordrevirementfrais = new ZenithWebServeur.BOJ.clsMicordrevirementfrais();

                    //clsMicordrevirementfraiss
                    clsMicordrevirementfrais.AG_CODEAGENCE = clsMicordrevirementDTO.AG_CODEAGENCE.ToString();
                    clsMicordrevirementfrais.OV_CODEORDREVIREMENT = clsMicordrevirementDTO.OV_CODEORDREVIREMENT.ToString();
                    clsMicordrevirementfrais.PL_CODEPARAMETRELISTE = clsMicordrevirementDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicordrevirementfrais.OF_MONTANT = Double.Parse(clsMicordrevirementDTO.OF_MONTANT.ToString());

                    //clsObjetEnvoi.OE_A = clsMicordrevirementDTO.clsObjetEnvoi.OE_A;
                    //clsObjetEnvoi.OE_Y = clsMicordrevirementDTO.clsObjetEnvoi.OE_Y;

                    clsMicordrevirementfraiss.Add(clsMicordrevirementfrais);
                }

                clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMicordrevirement.TV_CODETYPEVIREMENT = Objet.TV_CODETYPEVIREMENT.ToString();
                clsMicordrevirement.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
                clsMicordrevirement.RE_CODERESEAU = Objet.RE_CODERESEAU.ToString();
                clsMicordrevirement.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();
                clsMicordrevirement.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
                clsMicordrevirement.PE_CODEPERIODICITE = Objet.PE_CODEPERIODICITE.ToString();
                clsMicordrevirement.TB_CODETOMBEE = Objet.TB_CODETOMBEE.ToString();
                clsMicordrevirement.OV_AVIS = Objet.OV_AVIS.ToString();
                clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
                clsMicordrevirement.OV_DUREE = int.Parse(Objet.OV_DUREE.ToString());
                clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
                clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
                clsMicordrevirement.OV_DATEPROCHAINEECHEANCE = DateTime.Parse(Objet.OV_DATEPROCHAINEECHEANCE.ToString());
                clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
                clsMicordrevirement.OV_LIBELLE = Objet.OV_LIBELLE.ToString();
                clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicordrevirement.MICORDREVIREMENTDETAIL = clsMicordrevirementdetails;
                clsMicordrevirement.MICORDREVIREMENTFRAIS = clsMicordrevirementfraiss;



                //clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                //clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                //clsMicordrevirement.TV_CODETYPEVIREMENT = Objet.TV_CODETYPEVIREMENT.ToString();
                //clsMicordrevirement.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
                //clsMicordrevirement.RE_CODERESEAU = Objet.RE_CODERESEAU.ToString();
                //clsMicordrevirement.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();
                //clsMicordrevirement.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
                //clsMicordrevirement.PE_CODEPERIODICITE = Objet.PE_CODEPERIODICITE.ToString();
                //clsMicordrevirement.TB_CODETOMBEE = Objet.TB_CODETOMBEE.ToString();
                //clsMicordrevirement.OV_AVIS = Objet.OV_AVIS.ToString();
                //clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
                //clsMicordrevirement.OV_DUREE = int.Parse(Objet.OV_DUREE.ToString());
                //clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
                //clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
                //clsMicordrevirement.OV_DATEPROCHAINEECHEANCE = DateTime.Parse(Objet.OV_DATEPROCHAINEECHEANCE.ToString());
                //clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
                //clsMicordrevirement.OV_LIBELLE = Objet.OV_LIBELLE.ToString();
                //clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                ////clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                ////clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                ////clsMicordrevirement.MO_DATEACTION = DateTime.Parse(Objet.MO_DATEACTION.ToString());
                ////clsMicordrevirement.MO_HEUREACTION = DateTime.Parse(Objet.MO_HEUREACTION.ToString());
                ////clsMicordrevirement.MO_ACTION = Objet.MO_ACTION.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgAjouter(clsDonnee, clsMicordrevirement, clsObjetEnvoi));
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
        public string pvgExecuterOrdreVirement(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicordrevirement.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
                clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
                clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
                clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicordrevirement.TESTSOLDE = Objet.TESTSOLDE.ToString();
                clsMicordrevirement.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsMicordrevirement.CODEPROCEDURE = Objet.CODEPROCEDURE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgExecuterOrdreVirement(clsDonnee, clsMicordrevirement, clsObjetEnvoi));
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
        public string pvgOperationSurOrdreVirement(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                clsMicordrevirement.OV_DATESUSPENSION = DateTime.Parse(Objet.OV_DATESUSPENSION.ToString());
                clsMicordrevirement.OV_DATECLOTURE = DateTime.Parse(Objet.OV_DATECLOTURE.ToString());
                clsMicordrevirement.TYPEOPERATION = int.Parse(Objet.TYPEOPERATION.ToString());

                //clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                //clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                //clsMicordrevirement.TV_CODETYPEVIREMENT = Objet.TV_CODETYPEVIREMENT.ToString();
                //clsMicordrevirement.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
                //clsMicordrevirement.RE_CODERESEAU = Objet.RE_CODERESEAU.ToString();
                //clsMicordrevirement.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();
                //clsMicordrevirement.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
                //clsMicordrevirement.PE_CODEPERIODICITE = Objet.PE_CODEPERIODICITE.ToString();
                //clsMicordrevirement.TB_CODETOMBEE = Objet.TB_CODETOMBEE.ToString();
                //clsMicordrevirement.OV_AVIS = Objet.OV_AVIS.ToString();
                //clsMicordrevirement.OV_DUREE = int.Parse(Objet.OV_DUREE.ToString());
                //clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
                //clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
                //clsMicordrevirement.OV_DATEFIN = DateTime.Parse(Objet.OV_DATEFIN.ToString());
                //clsMicordrevirement.OV_DATEPROCHAINEECHEANCE = DateTime.Parse(Objet.OV_DATEPROCHAINEECHEANCE.ToString());
                //clsMicordrevirement.OV_DATESUSPENSION = DateTime.Parse(Objet.OV_DATESUSPENSION.ToString());
                //clsMicordrevirement.OV_DATECLOTURE = DateTime.Parse(Objet.OV_DATECLOTURE.ToString());
                //clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
                //clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
                //clsMicordrevirement.OV_LIBELLE = Objet.OV_LIBELLE.ToString();
                //clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                //clsMicordrevirement.TYPEOPERATION = int.Parse(Objet.TYPEOPERATION.ToString());
                //clsMicordrevirement.OV_CODEORDREVIREMENTRETOUR = Objet.OV_CODEORDREVIREMENTRETOUR.ToString();

                //clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                //clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
                //clsMicordrevirement.OV_DATECLOTURE = DateTime.Parse(Objet.OV_DATECLOTURE.ToString());
                //clsMicordrevirement.OV_DATESUSPENSION = DateTime.Parse(Objet.OV_DATESUSPENSION.ToString());
                //clsMicordrevirement.TYPEOPERATION = int.Parse(Objet.TYPEOPERATION.ToString());

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgOperationSurOrdreVirement(clsDonnee, clsMicordrevirement, clsObjetEnvoi));
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
        //public string pvgModifier(clsMicordrevirement Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
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
        //        clsObjetEnvoi.OE_PARAM = new string[] { Objet.OV_CODEORDREVIREMENT };

        //        //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
        //        //{

        //        clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
        //        clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
        //        clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
        //        clsMicordrevirement.TV_CODETYPEVIREMENT = Objet.TV_CODETYPEVIREMENT.ToString();
        //        clsMicordrevirement.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
        //        clsMicordrevirement.RE_CODERESEAU = Objet.RE_CODERESEAU.ToString();
        //        clsMicordrevirement.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();
        //        clsMicordrevirement.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
        //        clsMicordrevirement.PE_CODEPERIODICITE = Objet.PE_CODEPERIODICITE.ToString();
        //        clsMicordrevirement.TB_CODETOMBEE = Objet.TB_CODETOMBEE.ToString();
        //        clsMicordrevirement.OV_AVIS = Objet.OV_AVIS.ToString();
        //        clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
        //        clsMicordrevirement.OV_DUREE = int.Parse(Objet.OV_DUREE.ToString());
        //        clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
        //        clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
        //        clsMicordrevirement.OV_DATEPROCHAINEECHEANCE = DateTime.Parse(Objet.OV_DATEPROCHAINEECHEANCE.ToString());
        //        clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
        //        clsMicordrevirement.OV_LIBELLE = Objet.OV_LIBELLE.ToString();
        //        clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgModifier(clsDonnee, clsMicordrevirement, clsObjetEnvoi));
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
        public string pvgModifier(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
            List<ZenithWebServeur.BOJ.clsMicordrevirementdetail> clsMicordrevirementdetails = new List<ZenithWebServeur.BOJ.clsMicordrevirementdetail>();
            List<ZenithWebServeur.BOJ.clsMicordrevirementfrais> clsMicordrevirementfraiss = new List<ZenithWebServeur.BOJ.clsMicordrevirementfrais>();
            List<ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais> clsMicordrevirementdetailfraiss = new List<ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais>();

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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                //MICORDREVIREMENTDETAIL
                foreach (ZenithWebServeur.DTO.clsMicordrevirementdetail clsMicordrevirementDTO in Objet.MICORDREVIREMENTDETAIL)
                {
                    ZenithWebServeur.BOJ.clsMicordrevirementdetail clsMicordrevirementdetail = new ZenithWebServeur.BOJ.clsMicordrevirementdetail();

                    //MICORDREVIREMENTDETAILFRAIS
                    foreach (ZenithWebServeur.DTO.clsMicordrevirementdetailfrais clsMicordrevirementdetailfraisDTO in clsMicordrevirementDTO.MICORDREVIREMENTDETAILFRAIS)
                    {
                        ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais clsMicordrevirementdetailfrais = new ZenithWebServeur.BOJ.clsMicordrevirementdetailfrais();

                        //clsMicordrevirementdetails
                        clsMicordrevirementdetailfrais.AG_CODEAGENCE = clsMicordrevirementDTO.AG_CODEAGENCE.ToString();
                        clsMicordrevirementdetailfrais.VD_CODEORDREVIREMENTDETAIL = clsMicordrevirementDTO.VD_CODEORDREVIREMENTDETAIL.ToString();
                        clsMicordrevirementdetailfrais.PL_CODEPARAMETRELISTE = clsMicordrevirementDTO.OV_CODEORDREVIREMENT.ToString();
                        clsMicordrevirementdetailfrais.OF_MONTANT = Double.Parse(clsMicordrevirementDTO.CO_CODECOMPTE.ToString());

                        //clsObjetEnvoi.OE_A = clsMicordrevirementDTO.clsObjetEnvoi.OE_A;
                        //clsObjetEnvoi.OE_Y = clsMicordrevirementDTO.clsObjetEnvoi.OE_Y;

                        clsMicordrevirementdetailfraiss.Add(clsMicordrevirementdetailfrais);
                    }

                    //clsMicordrevirementdetails
                    clsMicordrevirementdetail.AG_CODEAGENCE = clsMicordrevirementDTO.AG_CODEAGENCE.ToString();
                    clsMicordrevirementdetail.VD_CODEORDREVIREMENTDETAIL = clsMicordrevirementDTO.VD_CODEORDREVIREMENTDETAIL.ToString();
                    clsMicordrevirementdetail.OV_CODEORDREVIREMENT = clsMicordrevirementDTO.OV_CODEORDREVIREMENT.ToString();
                    clsMicordrevirementdetail.CO_CODECOMPTE = clsMicordrevirementDTO.CO_CODECOMPTE.ToString();
                    clsMicordrevirementdetail.VD_LIBELLE = clsMicordrevirementDTO.VD_LIBELLE.ToString();
                    clsMicordrevirementdetail.VD_MONTANT = Double.Parse(clsMicordrevirementDTO.VD_MONTANT.ToString());
                    clsMicordrevirementdetail.VD_DATEPRELEVEMENT = DateTime.Parse(clsMicordrevirementDTO.VD_DATEPRELEVEMENT.ToString());
                    clsMicordrevirementdetail.VD_DATECLOTURE = DateTime.Parse(clsMicordrevirementDTO.VD_DATECLOTURE.ToString());
                    clsMicordrevirementdetail.VD_DATESUSPENSION = DateTime.Parse(clsMicordrevirementDTO.VD_DATESUSPENSION.ToString());
                    clsMicordrevirementdetail.VD_DATEFINSUSPENSION = DateTime.Parse(clsMicordrevirementDTO.VD_DATEFINSUSPENSION.ToString());
                    clsMicordrevirementdetail.VD_SAISIE = DateTime.Parse(clsMicordrevirementDTO.VD_SAISIE.ToString());
                    clsMicordrevirementdetail.MICORDREVIREMENTDETAILFRAIS = clsMicordrevirementdetailfraiss;

                    //clsObjetEnvoi.OE_A = clsMicordrevirementDTO.clsObjetEnvoi.OE_A;
                    //clsObjetEnvoi.OE_Y = clsMicordrevirementDTO.clsObjetEnvoi.OE_Y;

                    clsMicordrevirementdetails.Add(clsMicordrevirementdetail);
                }

                //MICORDREVIREMENTFRAIS
                foreach (ZenithWebServeur.DTO.clsMicordrevirementfrais clsMicordrevirementDTO in Objet.MICORDREVIREMENTFRAIS)
                {
                    ZenithWebServeur.BOJ.clsMicordrevirementfrais clsMicordrevirementfrais = new ZenithWebServeur.BOJ.clsMicordrevirementfrais();

                    //clsMicordrevirementfraiss
                    clsMicordrevirementfrais.AG_CODEAGENCE = clsMicordrevirementDTO.AG_CODEAGENCE.ToString();
                    clsMicordrevirementfrais.OV_CODEORDREVIREMENT = clsMicordrevirementDTO.OV_CODEORDREVIREMENT.ToString();
                    clsMicordrevirementfrais.PL_CODEPARAMETRELISTE = clsMicordrevirementDTO.PL_CODEPARAMETRELISTE.ToString();
                    clsMicordrevirementfrais.OF_MONTANT = Double.Parse(clsMicordrevirementDTO.OF_MONTANT.ToString());

                    //clsObjetEnvoi.OE_A = clsMicordrevirementDTO.clsObjetEnvoi.OE_A;
                    //clsObjetEnvoi.OE_Y = clsMicordrevirementDTO.clsObjetEnvoi.OE_Y;

                    clsMicordrevirementfraiss.Add(clsMicordrevirementfrais);
                }

                clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMicordrevirement.TV_CODETYPEVIREMENT = Objet.TV_CODETYPEVIREMENT.ToString();
                clsMicordrevirement.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
                clsMicordrevirement.RE_CODERESEAU = Objet.RE_CODERESEAU.ToString();
                clsMicordrevirement.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();
                clsMicordrevirement.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
                clsMicordrevirement.PE_CODEPERIODICITE = Objet.PE_CODEPERIODICITE.ToString();
                clsMicordrevirement.TB_CODETOMBEE = Objet.TB_CODETOMBEE.ToString();
                clsMicordrevirement.OV_AVIS = Objet.OV_AVIS.ToString();
                clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
                clsMicordrevirement.OV_DUREE = int.Parse(Objet.OV_DUREE.ToString());
                clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
                clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
                clsMicordrevirement.OV_DATEPROCHAINEECHEANCE = DateTime.Parse(Objet.OV_DATEPROCHAINEECHEANCE.ToString());
                clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
                clsMicordrevirement.OV_LIBELLE = Objet.OV_LIBELLE.ToString();
                clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicordrevirement.MICORDREVIREMENTDETAIL = clsMicordrevirementdetails;
                clsMicordrevirement.MICORDREVIREMENTFRAIS = clsMicordrevirementfraiss;



                //clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsMicordrevirement.OV_CODEORDREVIREMENT = Objet.OV_CODEORDREVIREMENT.ToString();
                //clsMicordrevirement.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                //clsMicordrevirement.TV_CODETYPEVIREMENT = Objet.TV_CODETYPEVIREMENT.ToString();
                //clsMicordrevirement.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
                //clsMicordrevirement.RE_CODERESEAU = Objet.RE_CODERESEAU.ToString();
                //clsMicordrevirement.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();
                //clsMicordrevirement.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
                //clsMicordrevirement.PE_CODEPERIODICITE = Objet.PE_CODEPERIODICITE.ToString();
                //clsMicordrevirement.TB_CODETOMBEE = Objet.TB_CODETOMBEE.ToString();
                //clsMicordrevirement.OV_AVIS = Objet.OV_AVIS.ToString();
                //clsMicordrevirement.OV_MONTANT = Double.Parse(Objet.OV_MONTANT.ToString());
                //clsMicordrevirement.OV_DUREE = int.Parse(Objet.OV_DUREE.ToString());
                //clsMicordrevirement.OV_DATESAISIE = DateTime.Parse(Objet.OV_DATESAISIE.ToString());
                //clsMicordrevirement.OV_DATEDEBUT = DateTime.Parse(Objet.OV_DATEDEBUT.ToString());
                //clsMicordrevirement.OV_DATEPROCHAINEECHEANCE = DateTime.Parse(Objet.OV_DATEPROCHAINEECHEANCE.ToString());
                //clsMicordrevirement.OV_SENS = Objet.OV_SENS.ToString();
                //clsMicordrevirement.OV_LIBELLE = Objet.OV_LIBELLE.ToString();
                //clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                ////clsMicordrevirement.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                ////clsMicordrevirement.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                ////clsMicordrevirement.MO_DATEACTION = DateTime.Parse(Objet.MO_DATEACTION.ToString());
                ////clsMicordrevirement.MO_HEUREACTION = DateTime.Parse(Objet.MO_HEUREACTION.ToString());
                ////clsMicordrevirement.MO_ACTION = Objet.MO_ACTION.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgModifier(clsDonnee, clsMicordrevirement, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.OV_CODEORDREVIREMENT };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        public string pvgChargerDansDataSetRecherche(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
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
                    Objet.DATEJOURNEE1,
                    Objet.DATEJOURNEE2,
                };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicordrevirementWSBLL.pvgChargerDansDataSetRecherche(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSetRecherche1(clsMicordrevirement Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicordrevirement clsMicordrevirement = new ZenithWebServeur.BOJ.clsMicordrevirement();
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
                    Objet.DATEJOURNEE1,
                    Objet.DATEJOURNEE2
                };

                //foreach (ZenithWebServeur.DTO.clsMicordrevirement clsMicordrevirementDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicordrevirementWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicordrevirementWSBLL.pvgChargerDansDataSetRecherche1(clsDonnee, clsObjetEnvoi);
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
