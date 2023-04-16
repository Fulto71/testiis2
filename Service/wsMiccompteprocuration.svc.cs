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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMiccompteprocuration" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMiccompteprocuration.svc ou wsMiccompteprocuration.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMiccompteprocuration : IwsMiccompteprocuration
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMiccompteprocurationWSBLL clsMiccompteprocurationWSBLL = new clsMiccompteprocurationWSBLL();

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
        public string pvgAjouterListe(List<clsMiccompteprocuration> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMiccompteprocuration> clsMiccompteprocurations = new List<ZenithWebServeur.BOJ.clsMiccompteprocuration>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
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
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();

                    clsMiccompteprocuration.AG_CODEAGENCE = clsMiccompteprocurationDTO.AG_CODEAGENCE.ToString();
                    clsMiccompteprocuration.CO_CODECOMPTE = clsMiccompteprocurationDTO.CO_CODECOMPTE.ToString();
                    clsMiccompteprocuration.PU_CODEPROCURATION = clsMiccompteprocurationDTO.PU_CODEPROCURATION.ToString();
                    clsMiccompteprocuration.PU_DATEMISEENPLACE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEMISEENPLACE.ToString());
                    clsMiccompteprocuration.PU_NOMBENEFICIAIRE = clsMiccompteprocurationDTO.PU_NOMBENEFICIAIRE.ToString();
                    clsMiccompteprocuration.PU_PRENOMBENEFICIAIRE = clsMiccompteprocurationDTO.PU_PRENOMBENEFICIAIRE.ToString();
                    clsMiccompteprocuration.PU_DATENAISSANCE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATENAISSANCE.ToString());
                    clsMiccompteprocuration.PU_LIEUNAISSANCE = clsMiccompteprocurationDTO.PU_LIEUNAISSANCE.ToString();
                    clsMiccompteprocuration.PF_CODEPROFESSION = clsMiccompteprocurationDTO.PF_CODEPROFESSION.ToString();
                    clsMiccompteprocuration.SM_CODESITUATIONMATRIMONIALE = clsMiccompteprocurationDTO.SM_CODESITUATIONMATRIMONIALE.ToString();
                    clsMiccompteprocuration.CL_IDCLIENT = clsMiccompteprocurationDTO.CL_IDCLIENT.ToString();
                    clsMiccompteprocuration.PI_CODEPIECE = clsMiccompteprocurationDTO.PI_CODEPIECE.ToString();
                    clsMiccompteprocuration.PU_NUMPIECE = clsMiccompteprocurationDTO.PU_NUMPIECE.ToString();
                    clsMiccompteprocuration.PU_DATEEXPIRATIONPIECE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEEXPIRATIONPIECE.ToString());
                    clsMiccompteprocuration.PU_TELEPHONE = clsMiccompteprocurationDTO.PU_TELEPHONE.ToString();
                    clsMiccompteprocuration.PU_BOITEPOSTALE = clsMiccompteprocurationDTO.PU_BOITEPOSTALE.ToString();
                    clsMiccompteprocuration.PU_ADRESSEGEOGRAPHIQUE = clsMiccompteprocurationDTO.PU_ADRESSEGEOGRAPHIQUE.ToString();
                    clsMiccompteprocuration.PU_EMAIL = clsMiccompteprocurationDTO.PU_EMAIL.ToString();
                    clsMiccompteprocuration.PU_DATEDEBUT = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEDEBUT.ToString());
                    clsMiccompteprocuration.PU_DATEFIN = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEFIN.ToString());
                    clsMiccompteprocuration.PU_DATECLOTURE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATECLOTURE.ToString());
                    clsMiccompteprocuration.PU_MONTANTMAXIMUMPARRETRAIT = Double.Parse(clsMiccompteprocurationDTO.PU_MONTANTMAXIMUMPARRETRAIT.ToString());
                    clsMiccompteprocuration.PU_MONTANTMAXIMUMRETIRE = Double.Parse(clsMiccompteprocurationDTO.PU_MONTANTMAXIMUMRETIRE.ToString());
                    clsMiccompteprocuration.PU_FREQUENCERETRAIT = int.Parse(clsMiccompteprocurationDTO.PU_FREQUENCERETRAIT.ToString());
                    clsMiccompteprocuration.OP_CODEOPERATEUR = clsMiccompteprocurationDTO.OP_CODEOPERATEUR.ToString();

                    /*   if (clsMiccompteprocuration.PU_PHOTO != null)
                           clsMiccompteprocuration.PU_PHOTO = System.Convert.FromBase64String(clsMiccompteprocurationDTO.PU_PHOTO.ToString());
                       if (clsMiccompteprocuration.PU_SIGNATURE != null)
                           clsMiccompteprocuration.PU_SIGNATURE = System.Convert.FromBase64String(clsMiccompteprocurationDTO.PU_SIGNATURE.ToString());*/

                    Byte[] PU_PHOTO = null;
                    Byte[] PU_SIGNATURE = null;
                    if (clsMiccompteprocurationDTO.PU_PHOTO != null)
                        PU_PHOTO = System.Convert.FromBase64String(clsMiccompteprocurationDTO.PU_PHOTO.ToString());
                    if (clsMiccompteprocurationDTO.PU_SIGNATURE != null)
                        PU_SIGNATURE = System.Convert.FromBase64String(clsMiccompteprocurationDTO.PU_SIGNATURE.ToString());

                    clsMiccompteprocuration.PU_PHOTO = PU_PHOTO;
                    clsMiccompteprocuration.PU_SIGNATURE = PU_SIGNATURE;

                    clsObjetEnvoi.OE_A = clsMiccompteprocurationDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMiccompteprocurationDTO.clsObjetEnvoi.OE_Y;

                    clsMiccompteprocurations.Add(clsMiccompteprocuration);
                }
                clsObjetRetour.SetValue(true, clsMiccompteprocurationWSBLL.pvgAjouterListe(clsDonnee, clsMiccompteprocurations, clsObjetEnvoi));
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


        public string pvgAjouterListe2(List<clsMiccompteprocuration> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMiccompteprocuration> clsMiccompteprocurations = new List<ZenithWebServeur.BOJ.clsMiccompteprocuration>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
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
            }

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();

                    /* clsMiccompteprocuration.AG_CODEAGENCE = clsMiccompteprocurationDTO.AG_CODEAGENCE.ToString();
                     clsMiccompteprocuration.CO_CODECOMPTE = clsMiccompteprocurationDTO.CO_CODECOMPTE.ToString();
                     clsMiccompteprocuration.PU_CODEPROCURATION = clsMiccompteprocurationDTO.PU_CODEPROCURATION.ToString();
                     clsMiccompteprocuration.PU_DATEMISEENPLACE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEMISEENPLACE.ToString());
                     clsMiccompteprocuration.PU_NOMBENEFICIAIRE = clsMiccompteprocurationDTO.PU_NOMBENEFICIAIRE.ToString();
                     clsMiccompteprocuration.PU_PRENOMBENEFICIAIRE = clsMiccompteprocurationDTO.PU_PRENOMBENEFICIAIRE.ToString();
                     clsMiccompteprocuration.PU_DATENAISSANCE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATENAISSANCE.ToString());
                     clsMiccompteprocuration.PU_LIEUNAISSANCE = clsMiccompteprocurationDTO.PU_LIEUNAISSANCE.ToString();
                     clsMiccompteprocuration.PF_CODEPROFESSION = clsMiccompteprocurationDTO.PF_CODEPROFESSION.ToString();
                     clsMiccompteprocuration.SM_CODESITUATIONMATRIMONIALE = clsMiccompteprocurationDTO.SM_CODESITUATIONMATRIMONIALE.ToString();
                     clsMiccompteprocuration.CL_IDCLIENT = clsMiccompteprocurationDTO.CL_IDCLIENT.ToString();
                     clsMiccompteprocuration.PI_CODEPIECE = clsMiccompteprocurationDTO.PI_CODEPIECE.ToString();
                     clsMiccompteprocuration.PU_NUMPIECE = clsMiccompteprocurationDTO.PU_NUMPIECE.ToString();
                     clsMiccompteprocuration.PU_DATEEXPIRATIONPIECE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEEXPIRATIONPIECE.ToString());
                     clsMiccompteprocuration.PU_TELEPHONE = clsMiccompteprocurationDTO.PU_TELEPHONE.ToString();
                     clsMiccompteprocuration.PU_BOITEPOSTALE = clsMiccompteprocurationDTO.PU_BOITEPOSTALE.ToString();
                     clsMiccompteprocuration.PU_ADRESSEGEOGRAPHIQUE = clsMiccompteprocurationDTO.PU_ADRESSEGEOGRAPHIQUE.ToString();
                     clsMiccompteprocuration.PU_EMAIL = clsMiccompteprocurationDTO.PU_EMAIL.ToString();
                     clsMiccompteprocuration.PU_DATEDEBUT = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEDEBUT.ToString());
                     clsMiccompteprocuration.PU_DATEFIN = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEFIN.ToString());
                     clsMiccompteprocuration.PU_DATECLOTURE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATECLOTURE.ToString());
                     clsMiccompteprocuration.PU_MONTANTMAXIMUMPARRETRAIT = Double.Parse(clsMiccompteprocurationDTO.PU_MONTANTMAXIMUMPARRETRAIT.ToString());
                     clsMiccompteprocuration.PU_MONTANTMAXIMUMRETIRE = Double.Parse(clsMiccompteprocurationDTO.PU_MONTANTMAXIMUMRETIRE.ToString());
                     clsMiccompteprocuration.PU_FREQUENCERETRAIT = int.Parse(clsMiccompteprocurationDTO.PU_FREQUENCERETRAIT.ToString());
                     clsMiccompteprocuration.OP_CODEOPERATEUR = clsMiccompteprocurationDTO.OP_CODEOPERATEUR.ToString();*/

                    clsMiccompteprocuration.CL_CODECLIENT = clsMiccompteprocurationDTO.CL_CODECLIENT;
                    clsMiccompteprocuration.AG_CODEAGENCE = clsMiccompteprocurationDTO.AG_CODEAGENCE;
                    clsMiccompteprocuration.PU_CODEPROCURATION = clsMiccompteprocurationDTO.PU_CODEPROCURATION;
                    clsMiccompteprocuration.CO_CODECOMPTE = clsMiccompteprocurationDTO.CO_CODECOMPTE;
                    clsMiccompteprocuration.PU_MONTANTMAXIMUMPARRETRAIT = double.Parse(clsMiccompteprocurationDTO.PU_MONTANTMAXIMUMPARRETRAIT);
                    clsMiccompteprocuration.PU_DATEMISEENPLACE = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEMISEENPLACE);
                    clsMiccompteprocuration.PU_DATEDEBUT = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEDEBUT);
                    clsMiccompteprocuration.PU_DATEFIN = DateTime.Parse(clsMiccompteprocurationDTO.PU_DATEFIN);
                    clsMiccompteprocuration.PU_MONTANTMAXIMUMRETIRE = double.Parse(clsMiccompteprocurationDTO.PU_MONTANTMAXIMUMRETIRE);
                    clsMiccompteprocuration.PU_FREQUENCERETRAIT = int.Parse(clsMiccompteprocurationDTO.PU_FREQUENCERETRAIT);
                    clsMiccompteprocuration.OP_CODEOPERATEUR = clsMiccompteprocurationDTO.OP_CODEOPERATEUR;

                    Byte[] PU_PHOTO = null;
                    Byte[] PU_SIGNATURE = null;
                    if (clsMiccompteprocurationDTO.PU_PHOTO != null)
                        PU_PHOTO = System.Convert.FromBase64String(clsMiccompteprocurationDTO.PU_PHOTO.ToString());
                    if (clsMiccompteprocurationDTO.PU_SIGNATURE != null)
                        PU_SIGNATURE = System.Convert.FromBase64String(clsMiccompteprocurationDTO.PU_SIGNATURE.ToString());

                    clsMiccompteprocuration.PU_PHOTO = PU_PHOTO;
                    clsMiccompteprocuration.PU_SIGNATURE = PU_SIGNATURE;

                    clsObjetEnvoi.OE_A = clsMiccompteprocurationDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMiccompteprocurationDTO.clsObjetEnvoi.OE_Y;

                    clsMiccompteprocurations.Add(clsMiccompteprocuration);
                }
                clsObjetRetour.SetValue(true, clsMiccompteprocurationWSBLL.pvgAjouterListe(clsDonnee, clsMiccompteprocurations, clsObjetEnvoi));
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
        public string pvgModifier(clsMiccompteprocuration Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.CO_CODECOMPTE, Objet.PU_CODEPROCURATION };

                //foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                //{

                clsMiccompteprocuration.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMiccompteprocuration.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMiccompteprocuration.PU_CODEPROCURATION = Objet.PU_CODEPROCURATION.ToString();
                clsMiccompteprocuration.PU_DATEMISEENPLACE = DateTime.Parse(Objet.PU_DATEMISEENPLACE.ToString());
                clsMiccompteprocuration.PU_NOMBENEFICIAIRE = Objet.PU_NOMBENEFICIAIRE.ToString();
                clsMiccompteprocuration.PU_PRENOMBENEFICIAIRE = Objet.PU_PRENOMBENEFICIAIRE.ToString();
                clsMiccompteprocuration.PU_DATENAISSANCE = DateTime.Parse(Objet.PU_DATENAISSANCE.ToString());
                clsMiccompteprocuration.PU_LIEUNAISSANCE = Objet.PU_LIEUNAISSANCE.ToString();
                clsMiccompteprocuration.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                clsMiccompteprocuration.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                clsMiccompteprocuration.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMiccompteprocuration.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMiccompteprocuration.PU_NUMPIECE = Objet.PU_NUMPIECE.ToString();
                clsMiccompteprocuration.PU_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.PU_DATEEXPIRATIONPIECE.ToString());
                clsMiccompteprocuration.PU_TELEPHONE = Objet.PU_TELEPHONE.ToString();
                clsMiccompteprocuration.PU_BOITEPOSTALE = Objet.PU_BOITEPOSTALE.ToString();
                clsMiccompteprocuration.PU_ADRESSEGEOGRAPHIQUE = Objet.PU_ADRESSEGEOGRAPHIQUE.ToString();
                clsMiccompteprocuration.PU_EMAIL = Objet.PU_EMAIL.ToString();
                clsMiccompteprocuration.PU_DATEDEBUT = DateTime.Parse(Objet.PU_DATEDEBUT.ToString());
                clsMiccompteprocuration.PU_DATEFIN = DateTime.Parse(Objet.PU_DATEFIN.ToString());
                clsMiccompteprocuration.PU_DATECLOTURE = DateTime.Parse(Objet.PU_DATECLOTURE.ToString());
                clsMiccompteprocuration.PU_MONTANTMAXIMUMPARRETRAIT = Double.Parse(Objet.PU_MONTANTMAXIMUMPARRETRAIT.ToString());
                clsMiccompteprocuration.PU_MONTANTMAXIMUMRETIRE = Double.Parse(Objet.PU_MONTANTMAXIMUMRETIRE.ToString());
                clsMiccompteprocuration.PU_FREQUENCERETRAIT = int.Parse(Objet.PU_FREQUENCERETRAIT.ToString());
                clsMiccompteprocuration.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();

                /*  if (clsMiccompteprocuration.PU_PHOTO != null)
                      clsMiccompteprocuration.PU_PHOTO = System.Convert.FromBase64String(Objet.PU_PHOTO.ToString());
                  if (clsMiccompteprocuration.PU_SIGNATURE != null)
                      clsMiccompteprocuration.PU_SIGNATURE = System.Convert.FromBase64String(Objet.PU_SIGNATURE.ToString());*/

                Byte[] PU_PHOTO = null;
                Byte[] PU_SIGNATURE = null;
                if (Objet.PU_PHOTO != null)
                    PU_PHOTO = System.Convert.FromBase64String(Objet.PU_PHOTO.ToString());
                if (Objet.PU_SIGNATURE != null)
                    PU_SIGNATURE = System.Convert.FromBase64String(Objet.PU_SIGNATURE.ToString());

                clsMiccompteprocuration.PU_PHOTO = PU_PHOTO;
                clsMiccompteprocuration.PU_SIGNATURE = PU_SIGNATURE;



                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiccompteprocurationWSBLL.pvgModifier(clsDonnee, clsMiccompteprocuration, clsObjetEnvoi));
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
        public string pvgModifier2(clsMiccompteprocuration Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.CO_CODECOMPTE, Objet.PU_CODEPROCURATION };

                //foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                //{

               /* clsMiccompteprocuration.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMiccompteprocuration.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMiccompteprocuration.PU_CODEPROCURATION = Objet.PU_CODEPROCURATION.ToString();
                clsMiccompteprocuration.PU_DATEMISEENPLACE = DateTime.Parse(Objet.PU_DATEMISEENPLACE.ToString());
                clsMiccompteprocuration.PU_NOMBENEFICIAIRE = Objet.PU_NOMBENEFICIAIRE.ToString();
                clsMiccompteprocuration.PU_PRENOMBENEFICIAIRE = Objet.PU_PRENOMBENEFICIAIRE.ToString();
                clsMiccompteprocuration.PU_DATENAISSANCE = DateTime.Parse(Objet.PU_DATENAISSANCE.ToString());
                clsMiccompteprocuration.PU_LIEUNAISSANCE = Objet.PU_LIEUNAISSANCE.ToString();
                clsMiccompteprocuration.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                clsMiccompteprocuration.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                clsMiccompteprocuration.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMiccompteprocuration.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMiccompteprocuration.PU_NUMPIECE = Objet.PU_NUMPIECE.ToString();
                clsMiccompteprocuration.PU_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.PU_DATEEXPIRATIONPIECE.ToString());
                clsMiccompteprocuration.PU_TELEPHONE = Objet.PU_TELEPHONE.ToString();
                clsMiccompteprocuration.PU_BOITEPOSTALE = Objet.PU_BOITEPOSTALE.ToString();
                clsMiccompteprocuration.PU_ADRESSEGEOGRAPHIQUE = Objet.PU_ADRESSEGEOGRAPHIQUE.ToString();
                clsMiccompteprocuration.PU_EMAIL = Objet.PU_EMAIL.ToString();
                clsMiccompteprocuration.PU_DATEDEBUT = DateTime.Parse(Objet.PU_DATEDEBUT.ToString());
                clsMiccompteprocuration.PU_DATEFIN = DateTime.Parse(Objet.PU_DATEFIN.ToString());
                clsMiccompteprocuration.PU_DATECLOTURE = DateTime.Parse(Objet.PU_DATECLOTURE.ToString());
                clsMiccompteprocuration.PU_MONTANTMAXIMUMPARRETRAIT = Double.Parse(Objet.PU_MONTANTMAXIMUMPARRETRAIT.ToString());
                clsMiccompteprocuration.PU_MONTANTMAXIMUMRETIRE = Double.Parse(Objet.PU_MONTANTMAXIMUMRETIRE.ToString());
                clsMiccompteprocuration.PU_FREQUENCERETRAIT = int.Parse(Objet.PU_FREQUENCERETRAIT.ToString());
                clsMiccompteprocuration.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();*/

                clsMiccompteprocuration.CL_CODECLIENT = Objet.CL_CODECLIENT;
                clsMiccompteprocuration.AG_CODEAGENCE = Objet.AG_CODEAGENCE;
                clsMiccompteprocuration.PU_CODEPROCURATION = Objet.PU_CODEPROCURATION;
                clsMiccompteprocuration.CO_CODECOMPTE = Objet.CO_CODECOMPTE;
                clsMiccompteprocuration.PU_MONTANTMAXIMUMPARRETRAIT = double.Parse(Objet.PU_MONTANTMAXIMUMPARRETRAIT);
                clsMiccompteprocuration.PU_DATEMISEENPLACE = DateTime.Parse(Objet.PU_DATEMISEENPLACE);
                clsMiccompteprocuration.PU_DATEDEBUT = DateTime.Parse(Objet.PU_DATEDEBUT);
                clsMiccompteprocuration.PU_DATEFIN = DateTime.Parse(Objet.PU_DATEFIN);
                clsMiccompteprocuration.PU_MONTANTMAXIMUMRETIRE = double.Parse(Objet.PU_MONTANTMAXIMUMRETIRE);
                clsMiccompteprocuration.PU_FREQUENCERETRAIT = int.Parse(Objet.PU_FREQUENCERETRAIT);
                clsMiccompteprocuration.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR;

                if (clsMiccompteprocuration.PU_PHOTO != null)
                    clsMiccompteprocuration.PU_PHOTO = System.Convert.FromBase64String(Objet.PU_PHOTO.ToString());
                if (clsMiccompteprocuration.PU_SIGNATURE != null)
                    clsMiccompteprocuration.PU_SIGNATURE = System.Convert.FromBase64String(Objet.PU_SIGNATURE.ToString());

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiccompteprocurationWSBLL.pvgModifier(clsDonnee, clsMiccompteprocuration, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMiccompteprocuration Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CO_CODECOMPTE, Objet.PU_CODEPROCURATION };

                //foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiccompteprocurationWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        public string pvgChargerDansDataSet2(clsMiccompteprocuration Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();
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
                    Objet.AG_CODEAGENCE, Objet.CO_CODECOMPTE, "",""
                };

                //foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMiccompteprocurationWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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

        public string pvgChargerDansDataSet3(clsMiccompteprocuration Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();
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
                    Objet.AG_CODEAGENCE, Objet.CO_CODECOMPTE
                };

                //foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMiccompteprocurationWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSet(clsMiccompteprocuration Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiccompteprocuration clsMiccompteprocuration = new ZenithWebServeur.BOJ.clsMiccompteprocuration();
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
                    Objet.AG_CODEAGENCE, Objet.CO_CODECOMPTE, Objet.PU_CODEPROCURATION
                };

                //foreach (ZenithWebServeur.DTO.clsMiccompteprocuration clsMiccompteprocurationDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsMiccompteprocurationWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
