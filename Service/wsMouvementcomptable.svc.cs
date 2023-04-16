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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMouvementcomptable" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMouvementcomptable.svc ou wsMouvementcomptable.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMouvementcomptable : IwsMouvementcomptable
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMouvementcomptableWSBLL clsMouvementcomptableWSBLL = new clsMouvementcomptableWSBLL();

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
        public string pvgAjouterComptabilisation(List<clsMouvementcomptable> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();
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

                foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                    ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();

                    // clsEtatmouvementacomptabilisers
                    clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsMouvementcomptableDTO.AG_CODEAGENCE.ToString();
                    clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsMouvementcomptableDTO.EM_DATEPIECE.ToString());
                    clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsMouvementcomptableDTO.CO_CODECOMPTE.ToString();
                    clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsMouvementcomptableDTO.EM_NOMOBJET.ToString();
                    clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsMouvementcomptableDTO.EM_SCHEMACOMPTABLECODE.ToString();
                    clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsMouvementcomptableDTO.EM_NUMEROSEQUENCE.ToString();
                    clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsMouvementcomptableDTO.EM_REFERENCEPIECE.ToString();
                    clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsMouvementcomptableDTO.EM_LIBELLEOPERATION.ToString();
                    clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsMouvementcomptableDTO.EM_NOMTIERS.ToString();
                    clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsMouvementcomptableDTO.PI_CODEPIECE.ToString();
                    clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsMouvementcomptableDTO.EM_NUMPIECETIERS.ToString();
                    clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsMouvementcomptableDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsEtatmouvementacomptabiliser.EM_MONTANT = Double.Parse(clsMouvementcomptableDTO.EM_MONTANT.ToString());
                    clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsMouvementcomptableDTO.OP_CODEOPERATEUR.ToString();
                    clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsMouvementcomptableDTO.SC_LIGNECACHEE.ToString();
                    clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsMouvementcomptableDTO.EM_SENSBILLETAGE.ToString();
                    clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsMouvementcomptableDTO.PL_CODENUMCOMPTE.ToString();
                    clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsMouvementcomptableDTO.PV_CODEPOINTVENTE.ToString();

                    clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    clsEtatmouvementacomptabilisers1.Add(clsEtatmouvementacomptabiliser);
                }
                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi));
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
        public string pvgLettrage(List<clsMouvementcomptable> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMouvementcomptable> clsMouvementcomptables = new List<ZenithWebServeur.BOJ.clsMouvementcomptable>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                DataSet = TestChampObligatoireInsertLettrage(Objet[Idx]);
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

                foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                    
                    clsMouvementcomptable.AG_CODEAGENCE = clsMouvementcomptableDTO.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.PV_CODEPOINTVENTE = clsMouvementcomptableDTO.PV_CODEPOINTVENTE.ToString();
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(clsMouvementcomptableDTO.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.MC_NUMPIECE = clsMouvementcomptableDTO.MC_NUMPIECE.ToString();
                    clsMouvementcomptable.LT_CODELETTRAGE = int.Parse(clsMouvementcomptableDTO.LT_CODELETTRAGE.ToString());
                    clsMouvementcomptable.OP_CODEOPERATEUR = clsMouvementcomptableDTO.OP_CODEOPERATEUR.ToString();
                    clsMouvementcomptable.TYPEOPERATION = clsMouvementcomptableDTO.TYPEOPERATION.ToString();


                    clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    clsMouvementcomptables.Add(clsMouvementcomptable);
                }
                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgLettrage(clsDonnee, clsMouvementcomptables, clsObjetEnvoi));
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
        public string pvgComptabilisationTontine(List<clsMouvementcomptable> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMouvementcomptable> clsMouvementcomptables = new List<ZenithWebServeur.BOJ.clsMouvementcomptable>();
            List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

           /*  for (int Idx = 0; Idx < Objet.Count; Idx++)
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
            } */

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                    ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();

                    // clsEtatmouvementacomptabilisers
                    clsMouvementcomptable.AG_CODEAGENCE = clsMouvementcomptableDTO.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = clsMouvementcomptableDTO.TI_IDTIERS.ToString();
                    clsMouvementcomptable.MI_CODEMISE = clsMouvementcomptableDTO.MI_CODEMISE.ToString();
                    clsMouvementcomptable.ST_CODESTICKER1 = clsMouvementcomptableDTO.ST_CODESTICKER1.ToString();
                    clsMouvementcomptable.ST_CODESTICKER2 = clsMouvementcomptableDTO.ST_CODESTICKER2.ToString();
                    clsMouvementcomptable.STICKER = clsMouvementcomptableDTO.STICKER.ToString();
                    clsMouvementcomptable.MC_NUMPIECE = clsMouvementcomptableDTO.MC_NUMPIECE.ToString();
                    clsMouvementcomptable.MC_REFERENCEPIECE = clsMouvementcomptableDTO.MC_REFERENCEPIECE.ToString();
                    clsMouvementcomptable.MC_LIBELLEOPERATION = clsMouvementcomptableDTO.MC_LIBELLEOPERATION.ToString();
                    clsMouvementcomptable.PL_CODENUMCOMPTE = clsMouvementcomptableDTO.PL_CODENUMCOMPTE.ToString();
                    clsMouvementcomptable.MC_NOMTIERS = clsMouvementcomptableDTO.MC_NOMTIERS.ToString();
                    clsMouvementcomptable.PI_CODEPIECE = clsMouvementcomptableDTO.PI_CODEPIECE.ToString();
                    clsMouvementcomptable.MC_NUMPIECETIERS = clsMouvementcomptableDTO.MC_NUMPIECETIERS.ToString();
                    clsMouvementcomptable.OP_CODEOPERATEUR = clsMouvementcomptableDTO.OP_CODEOPERATEUR.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = clsMouvementcomptableDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();

                    // clsBilletages
                    clsBilletage.AG_CODEAGENCE = clsMouvementcomptableDTO.AG_CODEAGENCE.ToString();
                    clsBilletage.MC_NUMSEQUENCE = clsMouvementcomptableDTO.MC_NUMSEQUENCE.ToString();
                    clsBilletage.CB_CODECOUPURE = clsMouvementcomptableDTO.CB_CODECOUPURE.ToString();
                    clsBilletage.MC_NUMPIECE = clsMouvementcomptableDTO.MC_NUMPIECE.ToString();
                    clsBilletage.PL_CODENUMCOMPTE = clsMouvementcomptableDTO.PL_CODENUMCOMPTE.ToString();
                    clsBilletage.MC_DATEPIECE = DateTime.Parse(clsMouvementcomptableDTO.MC_DATEPIECE.ToString());
                    clsBilletage.TYPEOPERATION = clsMouvementcomptableDTO.TYPEOPERATION.ToString();

                    clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    clsMouvementcomptables.Add(clsMouvementcomptable);
                    clsBilletages.Add(clsBilletage);
                }
                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgComptabilisationTontine(clsDonnee, clsMouvementcomptables, clsBilletages, clsObjetEnvoi));
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

        public string pvgAjouterComptabilisation2(List<clsMouvementcomptableOperationdeCaisse> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("NUMEROBORDEREAU", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable2 = new ZenithWebServeur.BOJ.clsMouvementcomptable();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                //  DataSet = TestChampObligatoireInsert2(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST DES TYPES DE DONNEES
                // DataSet = TestTypeDonnee(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //  if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST CONTRAINTE
                // DataSet = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMouvementcomptableOperationdeCaisse clsMouvementcomptableDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();


                    //  clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    //  clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();

                    if (clsMouvementcomptableDTO.clsMouvementcomptable1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable1)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE;
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION;
                            clsEtatmouvementacomptabiliser.EM_MONTANT = double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT);
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET;
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS;
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE;
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS;
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.MC_REFERENCEPIECE;
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE;
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE;
                            //  clsEtatmouvementacomptabiliser.MB_IDTIERS = clsEtatmouvementacomptabiliserDTO.MB_IDTIERS;
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE;
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsEtatmouvementacomptabilisers1.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsMouvementcomptable2 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable2)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE;
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION;
                            clsEtatmouvementacomptabiliser.EM_MONTANT = double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT);
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET;
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS;
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE;
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS;
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.MC_REFERENCEPIECE;
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE;
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE;
                            //  clsEtatmouvementacomptabiliser.MB_IDTIERS = clsEtatmouvementacomptabiliserDTO.MB_IDTIERS;
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE;
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsEtatmouvementacomptabilisers2.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsMouvementcomptable3 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable3)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE;
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION;
                            clsEtatmouvementacomptabiliser.EM_MONTANT = double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT);
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET;
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS;
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE;
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS;
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.MC_REFERENCEPIECE;
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE;
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE;
                            //  clsEtatmouvementacomptabiliser.MB_IDTIERS = clsEtatmouvementacomptabiliserDTO.MB_IDTIERS;
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE;
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsEtatmouvementacomptabilisers3.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsBilletages1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsBilletageDTO in clsMouvementcomptableDTO.clsBilletages1)
                        {
                            ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();
                            clsBilletage.AG_CODEAGENCE = clsBilletageDTO.AG_CODEAGENCE;
                            clsBilletage.BI_NUMPIECE = clsBilletageDTO.BI_NUMPIECE;
                            clsBilletage.BI_NUMSEQUENCE = clsBilletageDTO.BI_NUMSEQUENCE;
                            clsBilletage.BI_QUANTITEENTREE = int.Parse(clsBilletageDTO.BI_QUANTITEENTREE);
                            clsBilletage.BI_QUANTITESORTIE = int.Parse(clsBilletageDTO.BI_QUANTITESORTIE);
                            clsBilletage.CB_CODECOUPURE = clsBilletageDTO.CB_CODECOUPURE;
                            clsBilletage.MC_DATEPIECE = DateTime.Parse(clsBilletageDTO.MC_DATEPIECE);
                            clsBilletage.MC_NUMPIECE = clsBilletageDTO.MC_NUMPIECE;
                            clsBilletage.MC_NUMSEQUENCE = clsBilletageDTO.MC_NUMSEQUENCE;
                            clsBilletage.PL_CODENUMCOMPTE = clsBilletageDTO.PL_CODENUMCOMPTE;
                            clsBilletage.TYPEOPERATION = clsBilletageDTO.TYPEOPERATION;

                            clsBilletages.Add(clsBilletage);
                        }

                    // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);
                    clsMouvementcomptable2 = clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi);
                }


                // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi));
                if (clsMouvementcomptable2 != null)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["NUMEROBORDEREAU"] = clsMouvementcomptable2.NUMEROBORDEREAU;
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

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public string pvgChargerDansDataSet3(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
            //--TEST CONTRAINTE MC_DATEPIECE=@MC_DATEPIECE AND NUMEROBORDEREAU=@NUMEROBORDEREAU
            //DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.MC_DATEPIECE, Objet.NUMEROBORDEREAU };

            try
            {
                clsDonnee.pvgConnectionBase();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMouvementcomptableWSBLL.pvgChargerDansDataSet3(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    string exportFilename = "";
                    string URL_ETAT = "";

                    // URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


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
                clsDonnee.pvgDeConnectionBase();
            }
            return json;
        }

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        public string pvgEditionRecu(clsMouvementcomptable Objet)
        {


            DataSet DataSet1 = new DataSet();
            DataSet DataSet2 = new DataSet();
            DataSet DataSet3 = new DataSet();


            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("URL_ETAT", typeof(string)));
            string json = "";

            List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
            //--TEST CONTRAINTE MC_DATEPIECE=@MC_DATEPIECE AND NUMEROBORDEREAU=@NUMEROBORDEREAU
            //DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.MC_DATEPIECE, Objet.NUMEROBORDEREAU };

            try
            {
                clsDonnee.pvgConnectionBase();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsBilletageWSBLL clsBilletageWSBLL = new clsBilletageWSBLL();

                DataSet1 = clsBilletageWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
                DataSet2 = clsMouvementcomptableWSBLL.pvgChargerDansDataSet3(clsDonnee, clsObjetEnvoi);

                if (DataSet2.Tables[0].Rows.Count > 0)
                {
                    string vlpNumeroCompte = ""; string vlpIntituleCompte = "";
                    //Nous voulons mettre à jour la colone NUMEROCOMPTE avec les valeur non vide de NUMEROCOMPTE
                    //pour permettre au reçu de caisse de faire apparaître ce numero sur le reçu,malgré
                    //que la ligne selection pour les reçus est pris sur le compte caisse.
                    if (DataSet2.Tables.Count > 0)
                    {
                        for (int Idx = 0; Idx < DataSet2.Tables[0].Rows.Count; Idx++)
                        {
                            //Recupération du compte de tiers
                            if (DataSet2.Tables[0].Rows[Idx]["NUMEROCOMPTE"].ToString() != "")
                            {
                                vlpNumeroCompte = DataSet2.Tables[0].Rows[Idx]["NUMEROCOMPTE"].ToString();
                                vlpIntituleCompte = DataSet2.Tables[0].Rows[Idx]["CO_INTITULECOMPTERECU"].ToString();
                                break;
                            }
                        }
                        //mise à jour du compte de tiers sur les champs avec compte de tiers vide
                        for (int Idx = 0; Idx < DataSet2.Tables[0].Rows.Count; Idx++)
                        {
                            if (DataSet2.Tables[0].Rows[Idx]["NUMEROCOMPTE"].ToString() == "" && vlpNumeroCompte != "")
                            {
                                DataSet2.Tables[0].Rows[Idx]["NUMEROCOMPTE"] = vlpNumeroCompte;
                                DataSet2.Tables[0].Rows[Idx]["CO_INTITULECOMPTERECU"] = vlpIntituleCompte;
                            }
                        }
                        //Pour éviter que le recu affiche deux lignes,pour une ligne d'opération il faut filstrer les opérations uniquement sur ceux qui exige un billetage 'E' ou 'S'.
                        DataTable table = new DataTable();
                        table = DataSet2.Tables[0].Select("MC_SENSBILLETAGE IN('E','S')").CopyToDataTable();
                        DataSet3.Tables.Add(table);
                    }

                    //
                    //if (vppCritere[2] == "Guichet")
                    //{
                    //    if(Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_MONTANTRPEN)//avec papier entete
                    //        clsReporting.Instance.pvpApercuEtat(new FrmVisionneuse(), Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC1, DataSet3, new string[] { Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC2, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC3, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC4, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC5 }, new DataSet[] { DataSet3, DataSet1, DataSet3, DataSet1 }, new string[] { "Entete1", "Entete2", "Entete3", "Entete4", "DateDebut", "Option" }, new string[] { "", "", "", "", "", "" }, "", "", vppCritere[1] == "false" ? false : true, true, false, 130);
                    //    else //sans papier entete
                    //        clsReporting.Instance.pvpApercuEtat(new FrmVisionneuse(), Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC1, DataSet3, new string[] { Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC2, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC3, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC4, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC5 }, new DataSet[] { DataSet3, DataSet1, DataSet3, DataSet1 }, new string[] { "Entete1", "Entete2", "Entete3", "Entete4", "DateDebut", "Option" }, new string[] { Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT1, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT2, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT3, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT4, "", "" }, "", "", vppCritere[1] == "false" ? false : true, true, false, 130);
                    //}
                    //else
                    //{
                    //    if (Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_MONTANTRPEN)//avec papier entete
                    //        clsReporting.Instance.pvpApercuEtat(new FrmVisionneuse(), Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC1, DataSet3, new string[] { Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC2, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC3, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC4, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC5 }, new DataSet[] { DataSet3, DataSet1, DataSet3, DataSet1 }, new string[] { "Entete1", "Entete2", "Entete3", "Entete4", "DateDebut", "Option" }, new string[] { "", "", "", "", "", "" }, "", "", vppCritere[1] == "false" ? false : true, true, false, 130);
                    //    else//sans papier entete
                    //        clsReporting.Instance.pvpApercuEtat(new FrmVisionneuse(), Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC1, DataSet3, new string[] { Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC2, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC3, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC4, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURREC5 }, new DataSet[] { DataSet3, DataSet1, DataSet3, DataSet1 }, new string[] { "Entete1", "Entete2", "Entete3", "Entete4", "DateDebut", "Option" }, new string[] { Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT1, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT2, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT3, Zenith.TOOLS.clsDeclaration.vagParametreGlobal.PP_VALEURENT4, "", "" }, "", "", vppCritere[1] == "false" ? false : true, true, false, 130);
                    //}
                    //

                    ////
                    //string CheminImages = "";
                    //CheminImages = "";
                    //if (DataSet3.Tables.Count > 0)
                    //{
                    //    CheminImages = HostingEnvironment.MapPath("~/IMAGES/");
                    //    CheminImages = CheminImages + DataSet3.Tables["TABLE1"].Rows[0]["LOGO"].ToString(); //CheminImages + "\\IMAGES\\" + DataSet3.Tables["TABLE1"].Rows[0]["LOGO"].ToString();
                    //}
                    //if (!File.Exists(CheminImages)) CheminImages = "";
                    //if (DataSet3.Tables.Count > 0)
                    //    DataSet3.Tables["TABLE1"].Rows[0]["LOGO"] = CheminImages;
                    ////
                    ////

                    string PP_VALEURREC1 = "";
                    string PP_VALEURREC2 = "";
                    string PP_VALEURREC3 = "";
                    string PP_VALEURREC4 = "";
                    string PP_VALEURREC5 = "";

                    clsParametreWSBLL clsParametreWSBLL = new clsParametreWSBLL();
                    ZenithWebServeur.BOJ.clsParametre clsParametre = new ZenithWebServeur.BOJ.clsParametre();
                    clsObjetEnvoi.OE_PARAM = new string[] { "REC1" };
                    clsParametre = (ZenithWebServeur.BOJ.clsParametre)clsParametreWSBLL.pvgTableLibelle(clsDonnee, clsObjetEnvoi);
                    PP_VALEURREC1 = clsParametre.PP_VALEUR.ToString();
                    if (PP_VALEURREC1 == "") PP_VALEURREC1 = "RecuCaisse.rpt";
                    //
                    clsObjetEnvoi.OE_PARAM = new string[] { "REC2" };
                    clsParametre = (ZenithWebServeur.BOJ.clsParametre)clsParametreWSBLL.pvgTableLibelle(clsDonnee, clsObjetEnvoi);
                    PP_VALEURREC2 = clsParametre.PP_VALEUR.ToString();
                    if (PP_VALEURREC2 == "") PP_VALEURREC2 = "RecuCaisseDetail1.rpt";
                    //
                    clsObjetEnvoi.OE_PARAM = new string[] { "REC3" };
                    clsParametre = (ZenithWebServeur.BOJ.clsParametre)clsParametreWSBLL.pvgTableLibelle(clsDonnee, clsObjetEnvoi);
                    PP_VALEURREC3 = clsParametre.PP_VALEUR.ToString();
                    if (PP_VALEURREC3 == "") PP_VALEURREC3 = "RecuCaisseBilletage1.rpt";
                    //
                    clsObjetEnvoi.OE_PARAM = new string[] { "REC4" };
                    clsParametre = (ZenithWebServeur.BOJ.clsParametre)clsParametreWSBLL.pvgTableLibelle(clsDonnee, clsObjetEnvoi);
                    PP_VALEURREC4 = clsParametre.PP_VALEUR.ToString();
                    if (PP_VALEURREC4 == "") PP_VALEURREC4 = "RecuCaisseDetail2.rpt";
                    //

                    clsObjetEnvoi.OE_PARAM = new string[] { "REC5" };
                    clsParametre = (ZenithWebServeur.BOJ.clsParametre)clsParametreWSBLL.pvgTableLibelle(clsDonnee, clsObjetEnvoi);
                    PP_VALEURREC5 = clsParametre.PP_VALEUR.ToString();
                    if (PP_VALEURREC5 == "") PP_VALEURREC5 = "RecuCaisseBilletage2.rpt";





                    string reportPath = "~/Etats/";// + Objet.ET_DOSSIER;
                    string reportFileName = "RecuCaisse_mini.rpt";// PP_VALEURREC1;// Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    string exportFilename = "";
                    string URL_ETAT = "";


                    string[] vppFichierSousEtat = new string[] { "RecuCaisseDetail_mini.rpt", "RecuCaisseBilletage_mini.rpt", "RecuCaisseDetail_mini.rpt - 01", "RecuCaisseBilletage_mini.rpt - 01" };
                    DataSet[] vppDataSetSousEtat = new DataSet[] { DataSet3, DataSet1, DataSet3, DataSet1 };


                    //if (Objet.TYPEOPERATION == "01")//++Sans entete 
                    //{

                    //}


                    if (Objet.TYPEOPERATION == "Guichet")
                    {
                        URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet3, vppFichierSousEtat, vppDataSetSousEtat, Objet.vappNomFormule, Objet.vappValeurFormule);
                    }
                    else
                    {
                        URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet3, vppFichierSousEtat, vppDataSetSousEtat, Objet.vappNomFormule, Objet.vappValeurFormule);
                    }










                    // json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                    //string reportPath = "~/Etats/" + Objet.ET_DOSSIER;
                    //string reportFileName = Objet.ET_NOMETAT;// "YTDVarianceCrossTab.rpt";
                    //string exportFilename = "";
                    //string URL_ETAT = "";

                    // URL_ETAT = Stock.WCF.Utilities.CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, DataSet, Objet.vappNomFormule, Objet.vappValeurFormule, Objet.FORMEETAT);


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
                clsDonnee.pvgDeConnectionBase();
            }
            return json;
        }

        //AJOUT
        public string pvgAjouterSaisieEcriture(List<clsMouvementcomptable> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMouvementcomptable> clsMouvementcomptables = new List<ZenithWebServeur.BOJ.clsMouvementcomptable>();
            List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            /*  for (int Idx = 0; Idx < Objet.Count; Idx++)
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
             } */

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                    ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();

                    // clsEtatmouvementacomptabilisers
                    clsMouvementcomptable.AG_CODEAGENCE = clsMouvementcomptableDTO.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(clsMouvementcomptableDTO.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.TABLE_NAME = clsMouvementcomptableDTO.TABLE_NAME.ToString();
                    clsMouvementcomptable.IN_VALEURID = clsMouvementcomptableDTO.IN_VALEURID.ToString();
                    clsMouvementcomptable.NB_ID = clsMouvementcomptableDTO.NB_ID.ToString();
                    clsMouvementcomptable.MC_NUMPIECE = clsMouvementcomptableDTO.MC_NUMPIECE.ToString();
                    clsMouvementcomptable.MC_REFERENCEPIECE = clsMouvementcomptableDTO.MC_REFERENCEPIECE.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = clsMouvementcomptableDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.OP_CODEOPERATEUR = clsMouvementcomptableDTO.OP_CODEOPERATEUR.ToString();
                    clsMouvementcomptable.MR_CODEMODEREGLEMENT = clsMouvementcomptableDTO.MR_CODEMODEREGLEMENT.ToString();
                    clsMouvementcomptable.JO_CODEJOURNAL = clsMouvementcomptableDTO.JO_CODEJOURNAL.ToString();
                    clsMouvementcomptable.CO_CODECOMPTE1 = clsMouvementcomptableDTO.CO_CODECOMPTE1.ToString();
                    clsMouvementcomptable.PL_CODENUMCOMPTE = clsMouvementcomptableDTO.PL_CODENUMCOMPTE.ToString();
                    clsMouvementcomptable.MC_LIBELLEOPERATION = clsMouvementcomptableDTO.MC_LIBELLEOPERATION.ToString();
                    clsMouvementcomptable.PI_CODEPIECE = clsMouvementcomptableDTO.PI_CODEPIECE.ToString();
                    clsMouvementcomptable.MC_NUMPIECETIERS = clsMouvementcomptableDTO.MC_NUMPIECETIERS.ToString();
                    clsMouvementcomptable.MC_NOMTIERS = clsMouvementcomptableDTO.MC_NOMTIERS.ToString();
                    clsMouvementcomptable.MC_SENSBILLETAGE = clsMouvementcomptableDTO.MC_SENSBILLETAGE.ToString();
                    clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(clsMouvementcomptableDTO.MC_MONTANTDEBIT.ToString());
                    clsMouvementcomptable.MC_MONTANTCREDIT = Double.Parse(clsMouvementcomptableDTO.MC_MONTANTCREDIT.ToString());
                    clsMouvementcomptable.MC_ANNULATION = clsMouvementcomptableDTO.MC_ANNULATION.ToString();

                    clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    clsMouvementcomptables.Add(clsMouvementcomptable);
                }
                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterSaisieEcriture(clsDonnee, clsMouvementcomptables, clsObjetEnvoi));
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
        public string pvgComptabilisationImmo(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                if (Objet.EM_NOMOBJET == "FrmImmobilisationCession")
                {
                    clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                    clsMouvementcomptable.SO_CODESOCIETE = Objet.SO_CODESOCIETE.ToString();
                    clsMouvementcomptable.PL_NUMCOMPTE = Objet.PL_NUMCOMPTE.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(Objet.MC_MONTANTDEBIT.ToString());
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                }
                else if (Objet.EM_NOMOBJET == "FrmImmobilisationRebut")
                {
                    clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                }
                else if (Objet.EM_NOMOBJET == "FrmImmobilisationModificationDuree")
                {
                    clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                    clsMouvementcomptable.IM_DUREE = Double.Parse(Objet.IM_DUREE.ToString());
                }
                else if (Objet.EM_NOMOBJET == "FrmImmobilisationFactureAvoir")
                {
                    clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(Objet.MC_MONTANTDEBIT.ToString());
                    clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                }
                else if (Objet.EM_NOMOBJET == "FrmImmobilisationReglement")
                {
                    clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(Objet.MC_MONTANTDEBIT.ToString());
                    clsMouvementcomptable.MC_REFERENCEPIECE = Objet.MC_REFERENCEPIECE.ToString();
                    clsMouvementcomptable.PL_NUMCOMPTE = Objet.PL_NUMCOMPTE.ToString();
                    clsMouvementcomptable.SO_CODESOCIETE = Objet.SO_CODESOCIETE.ToString();
                    clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                }
                else
                {
                    clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMouvementcomptable.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                    clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                    clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(Objet.MC_MONTANTDEBIT.ToString());
                    clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                    clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                }

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgComptabilisationImmo(clsDonnee, clsMouvementcomptable, clsObjetEnvoi));
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
        public string pvgComptabilisationImmo1(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMouvementcomptable.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMouvementcomptable.SO_CODESOCIETE = Objet.SO_CODESOCIETE;
                clsMouvementcomptable.PL_NUMCOMPTE = Objet.PL_NUMCOMPTE;
                clsMouvementcomptable.TI_IDTIERS = Objet.TI_IDTIERS.ToString();
                clsMouvementcomptable.MC_REFERENCEPIECE = Objet.MC_REFERENCEPIECE.ToString();
                clsMouvementcomptable.PL_CODENUMCOMPTE = Objet.PL_CODENUMCOMPTE.ToString();
                clsMouvementcomptable.MONTANT = Double.Parse(Objet.MONTANT.ToString());
                clsMouvementcomptable.DATEJOURNEE = DateTime.Parse(Objet.DATEJOURNEE.ToString());
                clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsMouvementcomptable.IM_DUREE = Double.Parse(Objet.IM_DUREE.ToString());
                clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.DATEJOURNEE.ToString());
                clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(Objet.MONTANT.ToString());
                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgComptabilisationImmo(clsDonnee, clsMouvementcomptable, clsObjetEnvoi));
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
        public string pvgChargerDansDataSetRecuOuvertureCpte(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.AG_CODEAGENCE, Objet.CO_CODECOMPTE, Objet.ET_TYPEETAT
                };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgChargerDansDataSetRecuOuvertureCpte(clsDonnee, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
                //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.PV_CODEPOINTVENTE };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        public string pvgChargerDansDataSet(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMouvementcomptableWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSetMouvementMois(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.EX_EXERCICE, Objet.MO_CODEMOIS, Objet.PL_NUMCOMPTE };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMouvementcomptableWSBLL.pvgChargerDansDataSetMouvementMois(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSetRecherche(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
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
                    Objet.NUMEROBORDEREAU,
                    Objet.PI_CODEPIECE,
                    Objet.MC_REFERENCEPIECE,
                    Objet.MC_LIBELLEOPERATION,
                    Objet.MC_NUMPIECETIERS,
                    Objet.MC_NOMTIERS,
                    Objet.MC_DATEPIECE1,
                    Objet.MC_DATEPIECE2,
                    Objet.MC_MONTANTDEBIT1,
                    Objet.MC_MONTANTDEBIT2,
                    Objet.MC_MONTANTCREDIT1,
                    Objet.MC_MONTANTCREDIT2,
                    Objet.OV_CODEORDREVIREMENT,
                    Objet.EC_CODEEFFETCHEQUE,
                    Objet.PU_CODEPROCURATION,
                };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMouvementcomptableWSBLL.pvgChargerDansDataSetRecherche(clsDonnee, clsObjetEnvoi);
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
        public string pvgSoldeComptePrecedent(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_SOLDE", typeof(string)));
            
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            // DataSet = TestChampObligatoireUpdate(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            // DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            // DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                clsDonnee.pvgConnectionBase();
                //clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CO_CODECOMPTE, Objet.DATEDEBUT };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{


                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgSoldeComptePrecedent(clsDonnee, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["SL_SOLDE"] = clsObjetRetour.OR_STRING;
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
                //bool OR_BOOLEEN = true;
                //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE")
                //{
                //    OR_BOOLEEN = false;
                //}
                //clsDonnee.pvgTerminerTransaction(!clsObjetRetour.OR_BOOLEEN);
                clsDonnee.pvgDeConnectionBase();
            }

            return json;
        }

        //LISTE
        public string pvgSoldeCompte(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_SOLDE", typeof(string)));

            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            // DataSet = TestChampObligatoireUpdate(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            // DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            // DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                clsDonnee.pvgConnectionBase();
                //clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { "", "" };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{


                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgSoldeCompte(clsDonnee, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dr["SL_SOLDE"] = clsObjetRetour.OR_STRING;
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
                //bool OR_BOOLEEN = true;
                //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE")
                //{
                //    OR_BOOLEEN = false;
                //}
                //clsDonnee.pvgTerminerTransaction(!clsObjetRetour.OR_BOOLEEN);
                clsDonnee.pvgDeConnectionBase();
            }

            return json;
        }

        //AJOUT
        public string pvgAjouterCompteaCompte(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            // DataSet = TestChampObligatoireUpdate(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            // DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            // DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMouvementcomptable.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                clsMouvementcomptable.CO_CODECOMPTE1 = Objet.CO_CODECOMPTE1.ToString();
                clsMouvementcomptable.CO_CODECOMPTE2 = Objet.CO_CODECOMPTE2.ToString();
                clsMouvementcomptable.MC_LIBELLEOPERATION = Objet.MC_LIBELLEOPERATION.ToString();
                clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsMouvementcomptable.NO_CODENATUREVIREMENT = Objet.NO_CODENATUREVIREMENT.ToString();
                clsMouvementcomptable.MC_MONTANTDEBIT = Double.Parse(Objet.MC_MONTANTDEBIT.ToString());
                clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterCompteaCompte(clsDonnee, clsMouvementcomptable, clsObjetEnvoi));
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
        public string pvgExtourne(clsMouvementcomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            // DataSet = TestChampObligatoireUpdate(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST DES TYPES DE DONNEES
            // DataSet = TestTypeDonnee(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //--TEST CONTRAINTE
            // DataSet = TestTestContrainteListe(Objet);
            //--VERIFICATION DU RESULTAT DU TEST
            // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsMouvementcomptableDTO in Objet)
                //{

                clsMouvementcomptable.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMouvementcomptable.MC_DATEPIECECOMPTABILISATION = DateTime.Parse(Objet.MC_DATEPIECECOMPTABILISATION.ToString());
                clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(Objet.MC_DATEPIECE.ToString());
                clsMouvementcomptable.MC_NUMPIECE = Objet.MC_NUMPIECE.ToString();
                clsMouvementcomptable.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgExtourne(clsDonnee, clsMouvementcomptable, clsObjetEnvoi));
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


        public string pvgAjouterComptabilisation1(List<clsMouvementcomptableOperationdeCaisse> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("NUMEROBORDEREAU", typeof(string)));
            dt.Columns.Add(new DataColumn("SOLDECOMPTE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                //  DataSet = TestChampObligatoireInsert2(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST DES TYPES DE DONNEES
                // DataSet = TestTypeDonnee(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //  if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST CONTRAINTE
                // DataSet = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };
                ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                foreach (ZenithWebServeur.DTO.clsMouvementcomptableOperationdeCaisse clsMouvementcomptableDTO in Objet)
                {



                    //  clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    //  clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();

                    if (clsMouvementcomptableDTO.clsMouvementcomptable1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable1)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE;
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION;
                            clsEtatmouvementacomptabiliser.EM_MONTANT = double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT);
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET;
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS;
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE;
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS;
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.MC_REFERENCEPIECE;
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE;
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE;
                            //  clsEtatmouvementacomptabiliser.MB_IDTIERS = clsEtatmouvementacomptabiliserDTO.MB_IDTIERS;
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE;
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsEtatmouvementacomptabilisers1.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsMouvementcomptable2 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable2)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE;
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION;
                            clsEtatmouvementacomptabiliser.EM_MONTANT = double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT);
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET;
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS;
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE;
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS;
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.MC_REFERENCEPIECE;
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE;
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE;
                            //  clsEtatmouvementacomptabiliser.MB_IDTIERS = clsEtatmouvementacomptabiliserDTO.MB_IDTIERS;
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE;
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsEtatmouvementacomptabilisers2.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsMouvementcomptable3 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable3)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE;
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION;
                            clsEtatmouvementacomptabiliser.EM_MONTANT = double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT);
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET;
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS;
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE;
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS;
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.MC_REFERENCEPIECE;
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE;
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE;
                            //  clsEtatmouvementacomptabiliser.MB_IDTIERS = clsEtatmouvementacomptabiliserDTO.MB_IDTIERS;
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsEtatmouvementacomptabiliser.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE;
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsEtatmouvementacomptabilisers3.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsBilletages1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsBilletageDTO in clsMouvementcomptableDTO.clsBilletages1)
                        {
                            ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();
                            clsBilletage.AG_CODEAGENCE = clsBilletageDTO.AG_CODEAGENCE;
                            clsBilletage.BI_NUMPIECE = clsBilletageDTO.BI_NUMPIECE;
                            clsBilletage.BI_NUMSEQUENCE = clsBilletageDTO.BI_NUMSEQUENCE;
                            clsBilletage.BI_QUANTITEENTREE = int.Parse(clsBilletageDTO.BI_QUANTITEENTREE);
                            clsBilletage.BI_QUANTITESORTIE = int.Parse(clsBilletageDTO.BI_QUANTITESORTIE);
                            clsBilletage.CB_CODECOUPURE = clsBilletageDTO.CB_CODECOUPURE;
                            clsBilletage.MC_DATEPIECE = DateTime.Parse(clsBilletageDTO.MC_DATEPIECE);
                            clsBilletage.MC_NUMPIECE = clsBilletageDTO.MC_NUMPIECE;
                            clsBilletage.MC_NUMSEQUENCE = clsBilletageDTO.MC_NUMSEQUENCE;
                            clsBilletage.PL_CODENUMCOMPTE = clsBilletageDTO.PL_CODENUMCOMPTE;
                            clsBilletage.TYPEOPERATION = clsBilletageDTO.TYPEOPERATION;

                            clsBilletages.Add(clsBilletage);
                        }

                    // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);
                    clsMouvementcomptable = clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi);
                }


                // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi));
                if (clsMouvementcomptable != null)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["NUMEROBORDEREAU"] = clsMouvementcomptable.NUMEROBORDEREAU;
                    dr["SOLDECOMPTE"] = clsMouvementcomptable.SOLDECOMPTE;
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

        public string pvgAjouterComptabilisation_second(List<clsMouvementcomptableOperationdeCaisse> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("NUMEROBORDEREAU", typeof(string)));
            dt.Columns.Add(new DataColumn("SOLDECOMPTE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
            //List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                //  DataSet = TestChampObligatoireInsert2(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST DES TYPES DE DONNEES
                // DataSet = TestTypeDonnee(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //  if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST CONTRAINTE
                // DataSet = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };
                ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                foreach (ZenithWebServeur.DTO.clsMouvementcomptableOperationdeCaisse clsMouvementcomptableDTO in Objet)
                {
                    
                    //  clsObjetEnvoi.OE_A = clsMouvementcomptableDTO.clsObjetEnvoi.OE_A;
                    //  clsObjetEnvoi.OE_Y = clsMouvementcomptableDTO.clsObjetEnvoi.OE_Y;

                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers2 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers3 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();

                    if (clsMouvementcomptableDTO.clsMouvementcomptable1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable1)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE.ToString();
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.EM_DATEPIECE.ToString());
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE.ToString();
                            clsEtatmouvementacomptabiliser.EM_NOMOBJET = clsEtatmouvementacomptabiliserDTO.EM_NOMOBJET.ToString();
                            clsEtatmouvementacomptabiliser.EM_SCHEMACOMPTABLECODE = clsEtatmouvementacomptabiliserDTO.EM_SCHEMACOMPTABLECODE.ToString();
                            clsEtatmouvementacomptabiliser.EM_NUMEROSEQUENCE = clsEtatmouvementacomptabiliserDTO.EM_NUMEROSEQUENCE.ToString();
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.EM_REFERENCEPIECE.ToString();
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION.ToString();
                            clsEtatmouvementacomptabiliser.EM_NOMTIERS = clsEtatmouvementacomptabiliserDTO.EM_NOMTIERS.ToString();
                            clsEtatmouvementacomptabiliser.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE.ToString();
                            clsEtatmouvementacomptabiliser.EM_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.EM_NUMPIECETIERS.ToString();
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                            clsEtatmouvementacomptabiliser.EM_MONTANT = Double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT.ToString());
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR.ToString();
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE.ToString();
                            clsEtatmouvementacomptabiliser.EM_SENSBILLETAGE = clsEtatmouvementacomptabiliserDTO.EM_SENSBILLETAGE.ToString();
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE.ToString();

                            clsEtatmouvementacomptabilisers1.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsMouvementcomptable2 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable2)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE.ToString();
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.EM_DATEPIECE.ToString());
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE.ToString();
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE.ToString();
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.EM_REFERENCEPIECE.ToString();
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION.ToString();
                            clsEtatmouvementacomptabiliser.EM_MONTANT = Double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT.ToString());
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR.ToString();
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE.ToString();

                            clsEtatmouvementacomptabilisers2.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsMouvementcomptable3 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable3)
                        {
                            ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser clsEtatmouvementacomptabiliser = new ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser();
                            clsEtatmouvementacomptabiliser.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                            clsEtatmouvementacomptabiliser.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE.ToString();
                            clsEtatmouvementacomptabiliser.EM_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.EM_DATEPIECE.ToString());
                            clsEtatmouvementacomptabiliser.CO_CODECOMPTE = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE.ToString();
                            clsEtatmouvementacomptabiliser.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE.ToString();
                            clsEtatmouvementacomptabiliser.EM_REFERENCEPIECE = clsEtatmouvementacomptabiliserDTO.EM_REFERENCEPIECE.ToString();
                            clsEtatmouvementacomptabiliser.EM_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.EM_LIBELLEOPERATION.ToString();
                            clsEtatmouvementacomptabiliser.EM_MONTANT = Double.Parse(clsEtatmouvementacomptabiliserDTO.EM_MONTANT.ToString());
                            clsEtatmouvementacomptabiliser.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR.ToString();
                            clsEtatmouvementacomptabiliser.SC_LIGNECACHEE = clsEtatmouvementacomptabiliserDTO.SC_LIGNECACHEE.ToString();

                            clsEtatmouvementacomptabilisers3.Add(clsEtatmouvementacomptabiliser);
                        }

                    if (clsMouvementcomptableDTO.clsBilletages1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsBilletageDTO in clsMouvementcomptableDTO.clsBilletages1)
                        {
                            ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();
                            clsBilletage.AG_CODEAGENCE = clsBilletageDTO.AG_CODEAGENCE;
                            clsBilletage.CB_CODECOUPURE = clsBilletageDTO.CB_CODECOUPURE;
                            clsBilletage.BI_QUANTITEENTREE = int.Parse(clsBilletageDTO.BI_QUANTITEENTREE);
                            clsBilletage.BI_QUANTITESORTIE = int.Parse(clsBilletageDTO.BI_QUANTITESORTIE);

                            clsBilletages.Add(clsBilletage);
                        }

                    // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);
                    clsMouvementcomptable = clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi);
                }


                // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi));
                if (clsMouvementcomptable != null)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["NUMEROBORDEREAU"] = clsMouvementcomptable.NUMEROBORDEREAU;
                    dr["SOLDECOMPTE"] = clsMouvementcomptable.SOLDECOMPTE;
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

        public string pvgAjouterComptabilisationTontine1(List<clsMouvementcomptableOperationdeCaisse> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("NUMEROBORDEREAU", typeof(string)));
            // dt.Columns.Add(new DataColumn("SOLDECOMPTE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                //  DataSet = TestChampObligatoireInsert2(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //    if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST DES TYPES DE DONNEES
                // DataSet = TestTypeDonnee(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                //  if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
                //--TEST CONTRAINTE
                // DataSet = TestTestContrainteListe(Objet[Idx]);
                //--VERIFICATION DU RESULTAT DU TEST
                // if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };
                ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                foreach (ZenithWebServeur.DTO.clsMouvementcomptableOperationdeCaisse clsMouvementcomptableDTO in Objet)
                {

                    // ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                    List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser> clsEtatmouvementacomptabilisers1 = new List<ZenithWebServeur.BOJ.clsEtatmouvementacomptabiliser>();
                    List<ZenithWebServeur.BOJ.clsMouvementcomptable> clsMouvementcomptables = new List<ZenithWebServeur.BOJ.clsMouvementcomptable>();
                    List<ZenithWebServeur.BOJ.clsBilletage> clsBilletages = new List<ZenithWebServeur.BOJ.clsBilletage>();

                    if (clsMouvementcomptableDTO.clsMouvementcomptable1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsEtatmouvementacomptabiliserDTO in clsMouvementcomptableDTO.clsMouvementcomptable1)
                        {
                            ZenithWebServeur.BOJ.clsMouvementcomptable clsMouvementcomptable1 = new ZenithWebServeur.BOJ.clsMouvementcomptable();
                            clsMouvementcomptable.AG_CODEAGENCE = clsEtatmouvementacomptabiliserDTO.AG_CODEAGENCE;
                            clsMouvementcomptable.PV_CODEPOINTVENTE = clsEtatmouvementacomptabiliserDTO.PV_CODEPOINTVENTE;
                            clsMouvementcomptable.MC_DATEPIECE = DateTime.Parse(clsEtatmouvementacomptabiliserDTO.MC_DATEPIECE);
                            clsMouvementcomptable.TI_IDTIERS = clsEtatmouvementacomptabiliserDTO.TI_IDTIERS;
                            clsMouvementcomptable.CO_CODECOMPTE1 = clsEtatmouvementacomptabiliserDTO.CO_CODECOMPTE1;
                            clsMouvementcomptable.PL_CODENUMCOMPTE = clsEtatmouvementacomptabiliserDTO.PL_CODENUMCOMPTE;
                            clsMouvementcomptable.MC_MONTANTDEBIT = double.Parse(clsEtatmouvementacomptabiliserDTO.MC_MONTANTDEBIT);
                            clsMouvementcomptable.MC_NOMTIERS = clsEtatmouvementacomptabiliserDTO.MC_NOMTIERS;
                            clsMouvementcomptable.PI_CODEPIECE = clsEtatmouvementacomptabiliserDTO.PI_CODEPIECE;
                            clsMouvementcomptable.MC_NUMPIECETIERS = clsEtatmouvementacomptabiliserDTO.MC_NUMPIECETIERS;
                            clsMouvementcomptable.OP_CODEOPERATEUR = clsEtatmouvementacomptabiliserDTO.OP_CODEOPERATEUR;
                            clsMouvementcomptable.MC_LIBELLEOPERATION = clsEtatmouvementacomptabiliserDTO.MC_LIBELLEOPERATION;
                            clsMouvementcomptable.TS_CODETYPESCHEMACOMPTABLE = clsEtatmouvementacomptabiliserDTO.TS_CODETYPESCHEMACOMPTABLE;

                            clsMouvementcomptables.Add(clsMouvementcomptable);
                        }





                    if (clsMouvementcomptableDTO.clsBilletages1 != null)
                        foreach (ZenithWebServeur.DTO.clsMouvementcomptable clsBilletageDTO in clsMouvementcomptableDTO.clsBilletages1)
                        {
                            ZenithWebServeur.BOJ.clsBilletage clsBilletage = new ZenithWebServeur.BOJ.clsBilletage();
                            clsBilletage.AG_CODEAGENCE = clsBilletageDTO.AG_CODEAGENCE;
                            clsBilletage.BI_NUMPIECE = clsBilletageDTO.BI_NUMPIECE;
                            clsBilletage.BI_NUMSEQUENCE = clsBilletageDTO.BI_NUMSEQUENCE;
                            clsBilletage.BI_QUANTITEENTREE = int.Parse(clsBilletageDTO.BI_QUANTITEENTREE);
                            clsBilletage.BI_QUANTITESORTIE = int.Parse(clsBilletageDTO.BI_QUANTITESORTIE);
                            clsBilletage.CB_CODECOUPURE = clsBilletageDTO.CB_CODECOUPURE;
                            clsBilletage.MC_DATEPIECE = DateTime.Parse(clsBilletageDTO.MC_DATEPIECE);
                            clsBilletage.MC_NUMPIECE = clsBilletageDTO.MC_NUMPIECE;
                            clsBilletage.MC_NUMSEQUENCE = clsBilletageDTO.MC_NUMSEQUENCE;
                            clsBilletage.PL_CODENUMCOMPTE = clsBilletageDTO.PL_CODENUMCOMPTE;
                            clsBilletage.TYPEOPERATION = clsBilletageDTO.TYPEOPERATION;

                            clsBilletages.Add(clsBilletage);
                        }

                    clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgComptabilisationTontine(clsDonnee, clsMouvementcomptables, clsBilletages, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

                }


                // clsObjetRetour.SetValue(true, clsMouvementcomptableWSBLL.pvgAjouterComptabilisation(clsDonnee, clsEtatmouvementacomptabilisers1, clsEtatmouvementacomptabilisers2, clsEtatmouvementacomptabilisers3, clsBilletages, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["NUMEROBORDEREAU"] = clsObjetRetour.OR_STRING;
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
