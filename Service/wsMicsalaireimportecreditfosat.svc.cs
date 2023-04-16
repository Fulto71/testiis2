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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicsalaireimportecreditfosat" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicsalaireimportecreditfosat.svc ou wsMicsalaireimportecreditfosat.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicsalaireimportecreditfosat : IwsMicsalaireimportecreditfosat
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicsalaireimportecreditfosatWSBLL clsMicsalaireimportecreditfosatWSBLL = new clsMicsalaireimportecreditfosatWSBLL();
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
        public string pvgAjouterListe(List<clsMicsalaireimportecreditfosat> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat> clsMicsalaireimportecreditfosats = new List<ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat>();
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

                foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();

                    clsMicsalaireimportecreditfosat.SL_IDINDEX = clsMicsalaireimportecreditfosatDTO.SL_IDINDEX.ToString();
                    clsMicsalaireimportecreditfosat.AG_CODEAGENCE = clsMicsalaireimportecreditfosatDTO.AG_CODEAGENCE.ToString();
                    clsMicsalaireimportecreditfosat.OP_CODEOPERATEUR = clsMicsalaireimportecreditfosatDTO.OP_CODEOPERATEUR.ToString();
                    clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE = clsMicsalaireimportecreditfosatDTO.CL_NUMEROCOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.CO_CODECOMPTE = clsMicsalaireimportecreditfosatDTO.CO_CODECOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.CL_IDCLIENT = clsMicsalaireimportecreditfosatDTO.CL_IDCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.PL_CODENUMCOMPTE = clsMicsalaireimportecreditfosatDTO.PL_CODENUMCOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.PS_CODESOUSPRODUIT = clsMicsalaireimportecreditfosatDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicsalaireimportecreditfosat.PV_CODEPOINTVENTE = clsMicsalaireimportecreditfosatDTO.PV_CODEPOINTVENTE.ToString();
                    clsMicsalaireimportecreditfosat.PV_RAISONSOCIAL = clsMicsalaireimportecreditfosatDTO.PV_RAISONSOCIAL.ToString();
                    clsMicsalaireimportecreditfosat.TS_CODETYPESCHEMACOMPTABLE = clsMicsalaireimportecreditfosatDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMicsalaireimportecreditfosat.CODEPARAMETRE = clsMicsalaireimportecreditfosatDTO.CODEPARAMETRE.ToString();
                    clsMicsalaireimportecreditfosat.AT_CODEACTIVITE = clsMicsalaireimportecreditfosatDTO.AT_CODEACTIVITE.ToString();
                    clsMicsalaireimportecreditfosat.TI_CODETYPEAMORTISSEMENT = clsMicsalaireimportecreditfosatDTO.TI_CODETYPEAMORTISSEMENT.ToString();
                    clsMicsalaireimportecreditfosat.PE_CODEPERIODICITE = clsMicsalaireimportecreditfosatDTO.PE_CODEPERIODICITE.ToString();
                    clsMicsalaireimportecreditfosat.CR_NUMERODOSSIER = clsMicsalaireimportecreditfosatDTO.CR_NUMERODOSSIER.ToString();
                    clsMicsalaireimportecreditfosat.CR_DATEDEBLOCAGE = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEDEBLOCAGE.ToString());
                    clsMicsalaireimportecreditfosat.CR_DATEPECHEANCE = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEPECHEANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_NOMBREECHEANCE = int.Parse(clsMicsalaireimportecreditfosatDTO.CR_NOMBREECHEANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_TAUX = Decimal.Parse(clsMicsalaireimportecreditfosatDTO.CR_TAUX.ToString());
                    clsMicsalaireimportecreditfosat.CR_DUREE = int.Parse(clsMicsalaireimportecreditfosatDTO.CR_DUREE.ToString());
                    clsMicsalaireimportecreditfosat.CR_MONTCREDITACCORDE = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_MONTCREDITACCORDE.ToString());
                    clsMicsalaireimportecreditfosat.CR_MONTANTINTERETATTENDU = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_MONTANTINTERETATTENDU.ToString());
                    clsMicsalaireimportecreditfosat.CR_FRAISDOSSIER = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_FRAISDOSSIER.ToString());
                    clsMicsalaireimportecreditfosat.CR_FONDASSURANCE = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_FONDASSURANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_LIBELLEOPERATION = clsMicsalaireimportecreditfosatDTO.CR_LIBELLEOPERATION.ToString();
                    clsMicsalaireimportecreditfosat.CR_NOMCLIENT = clsMicsalaireimportecreditfosatDTO.CR_NOMCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.CR_PRENOMCLIENT = clsMicsalaireimportecreditfosatDTO.CR_PRENOMCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.CR_TELEPHONE = clsMicsalaireimportecreditfosatDTO.CR_TELEPHONE.ToString();
                    clsMicsalaireimportecreditfosat.SL_TYPEOPERATION = clsMicsalaireimportecreditfosatDTO.SL_TYPEOPERATION.ToString();
                    clsMicsalaireimportecreditfosat.CR_DATEOPERATION = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEOPERATION.ToString());

                    clsObjetEnvoi.OE_A = clsMicsalaireimportecreditfosatDTO.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = clsMicsalaireimportecreditfosatDTO.clsObjetEnvoi.OE_Y;

                    clsMicsalaireimportecreditfosats.Add(clsMicsalaireimportecreditfosat);
            }
                clsObjetRetour.SetValue(true, clsMicsalaireimportecreditfosatWSBLL.pvgAjouterListe(clsDonnee, clsMicsalaireimportecreditfosats, clsObjetEnvoi));
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
        public string pvgAjouterListe2(List<clsMicsalaireimportecreditfosat> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat> clsMicsalaireimportecreditfosats = new List<ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

           /* for (int Idx = 0; Idx < Objet.Count; Idx++)
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
            }*/

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();

                    clsMicsalaireimportecreditfosat.SL_IDINDEX = clsMicsalaireimportecreditfosatDTO.SL_IDINDEX.ToString();
                    clsMicsalaireimportecreditfosat.AG_CODEAGENCE = clsMicsalaireimportecreditfosatDTO.AG_CODEAGENCE.ToString();
                    clsMicsalaireimportecreditfosat.OP_CODEOPERATEUR = clsMicsalaireimportecreditfosatDTO.OP_CODEOPERATEUR.ToString();
                    clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE = clsMicsalaireimportecreditfosatDTO.CL_NUMEROCOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.CO_CODECOMPTE = clsMicsalaireimportecreditfosatDTO.CO_CODECOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.CL_IDCLIENT = clsMicsalaireimportecreditfosatDTO.CL_IDCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.PL_CODENUMCOMPTE = clsMicsalaireimportecreditfosatDTO.PL_CODENUMCOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.PS_CODESOUSPRODUIT = clsMicsalaireimportecreditfosatDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicsalaireimportecreditfosat.PV_CODEPOINTVENTE = clsMicsalaireimportecreditfosatDTO.PV_CODEPOINTVENTE.ToString();
                    clsMicsalaireimportecreditfosat.PV_RAISONSOCIAL = clsMicsalaireimportecreditfosatDTO.PV_RAISONSOCIAL.ToString();
                    clsMicsalaireimportecreditfosat.TS_CODETYPESCHEMACOMPTABLE = clsMicsalaireimportecreditfosatDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMicsalaireimportecreditfosat.CODEPARAMETRE = clsMicsalaireimportecreditfosatDTO.CODEPARAMETRE.ToString();
                    clsMicsalaireimportecreditfosat.AT_CODEACTIVITE = clsMicsalaireimportecreditfosatDTO.AT_CODEACTIVITE.ToString();
                    clsMicsalaireimportecreditfosat.TI_CODETYPEAMORTISSEMENT = clsMicsalaireimportecreditfosatDTO.TI_CODETYPEAMORTISSEMENT.ToString();
                    clsMicsalaireimportecreditfosat.PE_CODEPERIODICITE = clsMicsalaireimportecreditfosatDTO.PE_CODEPERIODICITE.ToString();
                    clsMicsalaireimportecreditfosat.CR_NUMERODOSSIER = clsMicsalaireimportecreditfosatDTO.CR_NUMERODOSSIER.ToString();
                    clsMicsalaireimportecreditfosat.CR_DATEDEBLOCAGE = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEDEBLOCAGE.ToString());
                    clsMicsalaireimportecreditfosat.CR_DATEPECHEANCE = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEPECHEANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_NOMBREECHEANCE = int.Parse(clsMicsalaireimportecreditfosatDTO.CR_NOMBREECHEANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_TAUX = Decimal.Parse(clsMicsalaireimportecreditfosatDTO.CR_TAUX.ToString());
                    clsMicsalaireimportecreditfosat.CR_DUREE = int.Parse(clsMicsalaireimportecreditfosatDTO.CR_DUREE.ToString());
                    clsMicsalaireimportecreditfosat.CR_MONTCREDITACCORDE = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_MONTCREDITACCORDE.ToString());
                    clsMicsalaireimportecreditfosat.CR_MONTANTINTERETATTENDU = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_MONTANTINTERETATTENDU.ToString());
                    clsMicsalaireimportecreditfosat.CR_FRAISDOSSIER = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_FRAISDOSSIER.ToString());
                    clsMicsalaireimportecreditfosat.CR_FONDASSURANCE = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_FONDASSURANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_LIBELLEOPERATION = clsMicsalaireimportecreditfosatDTO.CR_LIBELLEOPERATION.ToString();
                    clsMicsalaireimportecreditfosat.CR_NOMCLIENT = clsMicsalaireimportecreditfosatDTO.CR_NOMCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.CR_PRENOMCLIENT = clsMicsalaireimportecreditfosatDTO.CR_PRENOMCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.CR_TELEPHONE = clsMicsalaireimportecreditfosatDTO.CR_TELEPHONE.ToString();
                    clsMicsalaireimportecreditfosat.SL_TYPEOPERATION = clsMicsalaireimportecreditfosatDTO.SL_TYPEOPERATION.ToString();
                    clsMicsalaireimportecreditfosat.CR_DATEOPERATION = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEOPERATION.ToString());
                   // clsMicsalaireimportecreditfosat.LIBELLEOPERATIONTABLE = clsMicsalaireimportecreditfosatDTO.LIBELLEOPERATIONTABLE.ToString();


                    if (clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE != "" )
                    {

                        DataSet vlpDataSet = new DataSet();
                        if (clsMicsalaireimportecreditfosat.PP_MONTANTNCOM == 2 )
                        {
                            clsObjetEnvoi.OE_PARAM = new string[] { clsMicsalaireimportecreditfosat.AG_CODEAGENCE, clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE, clsMicsalaireimportecreditfosat.TY_CODETYPEINSTITUTION };
                            vlpDataSet = clsMicsalaireimporteWSBLL.pvgChargerDansDataSetPourCombo2(clsDonnee, clsObjetEnvoi);
                        }

                        else
                        {
                            clsObjetEnvoi.OE_PARAM = new string[] { clsMicsalaireimportecreditfosat.AG_CODEAGENCE, string.Format("{0:00000000}", double.Parse(clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE)), clsMicsalaireimportecreditfosat.TY_CODETYPEINSTITUTION };
                            vlpDataSet = clsMicsalaireimporteWSBLL.pvgChargerDansDataSetPourCombo3(clsDonnee, clsObjetEnvoi);
                        }

                        //
                        if (vlpDataSet.Tables[0].Rows.Count > 0)
                        {
                            clsMicsalaireimportecreditfosat.CO_CODECOMPTE = vlpDataSet.Tables["TABLE"].Rows[0]["CO_CODECOMPTE"].ToString();
                            clsMicsalaireimportecreditfosat.PS_CODESOUSPRODUIT = vlpDataSet.Tables["TABLE"].Rows[0]["PS_CODESOUSPRODUIT"].ToString();
                        }
                        else 
                        {
                                DataSet = new DataSet();
                                DataRow dr = dt.NewRow();
                                dr["SL_CODEMESSAGE"] = "99";
                                dr["SL_RESULTAT"] = "FALSE";
                                dr["SL_MESSAGE"] = "Le matricule " + clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE + " n''existe pas !!!";
                                dt.Rows.Add(dr);
                                DataSet.Tables.Add(dt);
                                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                                return json;
                            
                        }
                    }

                    clsObjetEnvoi.OE_A = clsMicsalaireimportecreditfosatDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicsalaireimportecreditfosatDTO.clsObjetEnvoi.OE_Y;

                    clsMicsalaireimportecreditfosats.Add(clsMicsalaireimportecreditfosat);
                }
                clsObjetRetour.SetValue(true, clsMicsalaireimportecreditfosatWSBLL.pvgAjouterListe(clsDonnee, clsMicsalaireimportecreditfosats, clsObjetEnvoi));
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
        public string pvgAjouterListeValidation(List<clsMicsalaireimportecreditfosat> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat> clsMicsalaireimportecreditfosats = new List<ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat>();
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

                foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();

                    clsMicsalaireimportecreditfosat.SL_IDINDEX = clsMicsalaireimportecreditfosatDTO.SL_IDINDEX.ToString();
                    clsMicsalaireimportecreditfosat.AG_CODEAGENCE = clsMicsalaireimportecreditfosatDTO.AG_CODEAGENCE.ToString();
                    clsMicsalaireimportecreditfosat.OP_CODEOPERATEUR = clsMicsalaireimportecreditfosatDTO.OP_CODEOPERATEUR.ToString();
                    clsMicsalaireimportecreditfosat.CL_NUMEROCOMPTE = clsMicsalaireimportecreditfosatDTO.CL_NUMEROCOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.CO_CODECOMPTE = clsMicsalaireimportecreditfosatDTO.CO_CODECOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.CL_IDCLIENT = clsMicsalaireimportecreditfosatDTO.CL_IDCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.PL_CODENUMCOMPTE = clsMicsalaireimportecreditfosatDTO.PL_CODENUMCOMPTE.ToString();
                    clsMicsalaireimportecreditfosat.PS_CODESOUSPRODUIT = clsMicsalaireimportecreditfosatDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicsalaireimportecreditfosat.PV_CODEPOINTVENTE = clsMicsalaireimportecreditfosatDTO.PV_CODEPOINTVENTE.ToString();
                    clsMicsalaireimportecreditfosat.PV_RAISONSOCIAL = clsMicsalaireimportecreditfosatDTO.PV_RAISONSOCIAL.ToString();
                    clsMicsalaireimportecreditfosat.TS_CODETYPESCHEMACOMPTABLE = clsMicsalaireimportecreditfosatDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMicsalaireimportecreditfosat.CODEPARAMETRE = clsMicsalaireimportecreditfosatDTO.CODEPARAMETRE.ToString();
                    clsMicsalaireimportecreditfosat.AT_CODEACTIVITE = clsMicsalaireimportecreditfosatDTO.AT_CODEACTIVITE.ToString();
                    clsMicsalaireimportecreditfosat.TI_CODETYPEAMORTISSEMENT = clsMicsalaireimportecreditfosatDTO.TI_CODETYPEAMORTISSEMENT.ToString();
                    clsMicsalaireimportecreditfosat.PE_CODEPERIODICITE = clsMicsalaireimportecreditfosatDTO.PE_CODEPERIODICITE.ToString();
                    clsMicsalaireimportecreditfosat.CR_NUMERODOSSIER = clsMicsalaireimportecreditfosatDTO.CR_NUMERODOSSIER.ToString();
                    clsMicsalaireimportecreditfosat.CR_DATEDEBLOCAGE = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEDEBLOCAGE.ToString());
                    clsMicsalaireimportecreditfosat.CR_DATEPECHEANCE = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEPECHEANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_NOMBREECHEANCE = int.Parse(clsMicsalaireimportecreditfosatDTO.CR_NOMBREECHEANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_TAUX = Decimal.Parse(clsMicsalaireimportecreditfosatDTO.CR_TAUX.ToString());
                    clsMicsalaireimportecreditfosat.CR_DUREE = int.Parse(clsMicsalaireimportecreditfosatDTO.CR_DUREE.ToString());
                    clsMicsalaireimportecreditfosat.CR_MONTCREDITACCORDE = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_MONTCREDITACCORDE.ToString());
                    clsMicsalaireimportecreditfosat.CR_MONTANTINTERETATTENDU = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_MONTANTINTERETATTENDU.ToString());
                    clsMicsalaireimportecreditfosat.CR_FRAISDOSSIER = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_FRAISDOSSIER.ToString());
                    clsMicsalaireimportecreditfosat.CR_FONDASSURANCE = Double.Parse(clsMicsalaireimportecreditfosatDTO.CR_FONDASSURANCE.ToString());
                    clsMicsalaireimportecreditfosat.CR_LIBELLEOPERATION = clsMicsalaireimportecreditfosatDTO.CR_LIBELLEOPERATION.ToString();
                    clsMicsalaireimportecreditfosat.CR_NOMCLIENT = clsMicsalaireimportecreditfosatDTO.CR_NOMCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.CR_PRENOMCLIENT = clsMicsalaireimportecreditfosatDTO.CR_PRENOMCLIENT.ToString();
                    clsMicsalaireimportecreditfosat.CR_TELEPHONE = clsMicsalaireimportecreditfosatDTO.CR_TELEPHONE.ToString();
                    clsMicsalaireimportecreditfosat.SL_TYPEOPERATION = clsMicsalaireimportecreditfosatDTO.SL_TYPEOPERATION.ToString();
                    clsMicsalaireimportecreditfosat.CR_DATEOPERATION = DateTime.Parse(clsMicsalaireimportecreditfosatDTO.CR_DATEOPERATION.ToString());

                    clsObjetEnvoi.OE_A = clsMicsalaireimportecreditfosatDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMicsalaireimportecreditfosatDTO.clsObjetEnvoi.OE_Y;

                    clsMicsalaireimportecreditfosats.Add(clsMicsalaireimportecreditfosat);
                }
                clsObjetRetour.SetValue(true, clsMicsalaireimportecreditfosatWSBLL.pvgAjouterListeValidation(clsDonnee, clsMicsalaireimportecreditfosats, clsObjetEnvoi));
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
        //public string pvgAjouter(clsMicsalaireimportecreditfosat Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();
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

        //        //foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
        //        //{

        //        clsMicsalaireimportecreditfosat.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
        //        clsMicsalaireimportecreditfosat.VL_CODEVILLE = Objet.VL_CODEVILLE.ToString();
        //        //clsMicsalaireimportecreditfosat.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
        //        clsMicsalaireimportecreditfosat.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_RAISONSOCIAL = Objet.PV_RAISONSOCIAL.ToString();
        //        clsMicsalaireimportecreditfosat.PV_BOITEPOSTAL = Objet.PV_BOITEPOSTAL.ToString();
        //        clsMicsalaireimportecreditfosat.PV_ADRESSEGEOGRAPHIQUE = Objet.PV_ADRESSEGEOGRAPHIQUE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_TELEPHONE = Objet.PV_TELEPHONE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_FAX = Objet.PV_FAX.ToString();
        //        clsMicsalaireimportecreditfosat.PV_EMAIL = Objet.PV_EMAIL.ToString();
        //        clsMicsalaireimportecreditfosat.PV_POINTVENTECODE = Objet.PV_POINTVENTECODE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_NUMEROCOMPTE = Objet.PV_NUMEROCOMPTE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_REFERENCE = Objet.PV_REFERENCE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_DATECREATION = DateTime.Parse(Objet.PV_DATECREATION.ToString());
        //        clsMicsalaireimportecreditfosat.PV_ACTIF = Objet.PV_ACTIF.ToString();
        //        clsMicsalaireimportecreditfosat.OP_GERANT = Objet.OP_GERANT.ToString();

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        clsObjetRetour.SetValue(true, clsMicsalaireimportecreditfosatWSBLL.pvgAjouter(clsDonnee, clsMicsalaireimportecreditfosat, clsObjetEnvoi));
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
        //public string pvgModifier(clsMicsalaireimportecreditfosat Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();
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

        //        //foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
        //        //{

        //        clsMicsalaireimportecreditfosat.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
        //        clsMicsalaireimportecreditfosat.VL_CODEVILLE = Objet.VL_CODEVILLE.ToString();
        //        //clsMicsalaireimportecreditfosat.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
        //        clsMicsalaireimportecreditfosat.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_RAISONSOCIAL = Objet.PV_RAISONSOCIAL.ToString();
        //        clsMicsalaireimportecreditfosat.PV_BOITEPOSTAL = Objet.PV_BOITEPOSTAL.ToString();
        //        clsMicsalaireimportecreditfosat.PV_ADRESSEGEOGRAPHIQUE = Objet.PV_ADRESSEGEOGRAPHIQUE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_TELEPHONE = Objet.PV_TELEPHONE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_FAX = Objet.PV_FAX.ToString();
        //        clsMicsalaireimportecreditfosat.PV_EMAIL = Objet.PV_EMAIL.ToString();
        //        clsMicsalaireimportecreditfosat.PV_POINTVENTECODE = Objet.PV_POINTVENTECODE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_NUMEROCOMPTE = Objet.PV_NUMEROCOMPTE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_REFERENCE = Objet.PV_REFERENCE.ToString();
        //        clsMicsalaireimportecreditfosat.PV_DATECREATION = DateTime.Parse(Objet.PV_DATECREATION.ToString());
        //        clsMicsalaireimportecreditfosat.PV_ACTIF = Objet.PV_ACTIF.ToString();
        //        clsMicsalaireimportecreditfosat.OP_GERANT = Objet.OP_GERANT.ToString();

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        clsObjetRetour.SetValue(true, clsMicsalaireimportecreditfosatWSBLL.pvgModifier(clsDonnee, clsMicsalaireimportecreditfosat, clsObjetEnvoi));
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
        //public string pvgSupprimer(clsMicsalaireimportecreditfosat Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    //for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    //{
        //    //--TEST DES CHAMPS OBLIGATOIRES
        //    DataSet = TestChampObligatoireDelete(Objet);
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
        //        //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.PV_CODEPOINTVENTE };

        //        //foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
        //        //{

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        clsObjetRetour.SetValue(true, clsMicsalaireimportecreditfosatWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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

        //LISTE
        public string pvgChargerDansDataSet(clsMicsalaireimportecreditfosat Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosat = new ZenithWebServeur.BOJ.clsMicsalaireimportecreditfosat();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.OP_CODEOPERATEUR, Objet.CODEPARAMETRE };

                //foreach (ZenithWebServeur.DTO.clsMicsalaireimportecreditfosat clsMicsalaireimportecreditfosatDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsMicsalaireimportecreditfosatWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
