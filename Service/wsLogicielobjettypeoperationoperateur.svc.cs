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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsLogicielobjettypeoperationoperateur" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsLogicielobjettypeoperationoperateur.svc ou wsLogicielobjettypeoperationoperateur.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsLogicielobjettypeoperationoperateur : IwsLogicielobjettypeoperationoperateur
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsLogicielobjettypeoperationoperateurWSBLL clsLogicielobjettypeoperationoperateurWSBLL = new clsLogicielobjettypeoperationoperateurWSBLL();

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
        public string pvgAjouterListe(List<clsLogicielobjettypeoperationoperateur> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur> clsLogicielobjettypeoperationoperateurs = new List<ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur>();
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

                foreach (ZenithWebServeur.DTO.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateurDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateur = new ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur();

                    clsLogicielobjettypeoperationoperateur.AG_CODEAGENCE = clsLogicielobjettypeoperationoperateurDTO.AG_CODEAGENCE.ToString();
                    clsLogicielobjettypeoperationoperateur.OB_CODEOBJET = clsLogicielobjettypeoperationoperateurDTO.OB_CODEOBJET.ToString();
                    clsLogicielobjettypeoperationoperateur.OP_CODEOPERATEUR = clsLogicielobjettypeoperationoperateurDTO.OP_CODEOPERATEUR.ToString();
                    clsLogicielobjettypeoperationoperateur.TO_CODETYPEOPERATION = clsLogicielobjettypeoperationoperateurDTO.TO_CODETYPEOPERATION.ToString();
                    clsLogicielobjettypeoperationoperateur.LO_ACTIF = clsLogicielobjettypeoperationoperateurDTO.LO_ACTIF.ToString();
                    clsLogicielobjettypeoperationoperateur.COCHER = clsLogicielobjettypeoperationoperateurDTO.COCHER.ToString();

                    clsObjetEnvoi.OE_A = clsLogicielobjettypeoperationoperateurDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsLogicielobjettypeoperationoperateurDTO.clsObjetEnvoi.OE_Y;

                    clsLogicielobjettypeoperationoperateurs.Add(clsLogicielobjettypeoperationoperateur);
                }
                clsObjetRetour.SetValue(true, clsLogicielobjettypeoperationoperateurWSBLL.pvgAjouterListe(clsDonnee, clsLogicielobjettypeoperationoperateurs, clsObjetEnvoi));
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
        public string pvgModifier(clsLogicielobjettypeoperationoperateur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateur = new ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur();
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
                //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };

                //foreach (ZenithWebServeur.DTO.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateurDTO in Objet)
                //{

                clsLogicielobjettypeoperationoperateur.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsLogicielobjettypeoperationoperateur.VL_CODEVILLE = Objet.VL_CODEVILLE.ToString();
                ////clsLogicielobjettypeoperationoperateur.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_RAISONSOCIAL = Objet.PV_RAISONSOCIAL.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_BOITEPOSTAL = Objet.PV_BOITEPOSTAL.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_ADRESSEGEOGRAPHIQUE = Objet.PV_ADRESSEGEOGRAPHIQUE.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_TELEPHONE = Objet.PV_TELEPHONE.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_FAX = Objet.PV_FAX.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_EMAIL = Objet.PV_EMAIL.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_POINTVENTECODE = Objet.PV_POINTVENTECODE.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_NUMEROCOMPTE = Objet.PV_NUMEROCOMPTE.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_REFERENCE = Objet.PV_REFERENCE.ToString();
                //clsLogicielobjettypeoperationoperateur.PV_DATECREATION = DateTime.Parse(Objet.PV_DATECREATION.ToString());
                //clsLogicielobjettypeoperationoperateur.PV_ACTIF = Objet.PV_ACTIF.ToString();
                //clsLogicielobjettypeoperationoperateur.OP_GERANT = Objet.OP_GERANT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsLogicielobjettypeoperationoperateurWSBLL.pvgModifier(clsDonnee, clsLogicielobjettypeoperationoperateur, clsObjetEnvoi));
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
        public string pvgSupprimer(clsLogicielobjettypeoperationoperateur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateur = new ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur();
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

                //foreach (ZenithWebServeur.DTO.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsLogicielobjettypeoperationoperateurWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        //public string pvgChargerDansDataSetOperateurDroitTypeSchemaComptable(clsLogicielobjettypeoperationoperateur Objet)
        //{
        //    DataSet DataSet = new DataSet();
        //    DataTable dt = new DataTable("TABLE");
        //    dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
        //    dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
        //    string json = "";

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateur = new ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur();
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
        //        clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.OP_CODEOPERATEUR, Objet.TS_CODETYPESCHEMACOMPTABLE };

        //        //foreach (ZenithWebServeur.DTO.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateurDTO in Objet)
        //        //{

        //        clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

        //        //clsObjetRetour.SetValue(true, clsLogicielobjettypeoperationoperateurWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
        //        DataSet = clsLogicielobjettypeoperationoperateurWSBLL.pvgChargerDansDataSetOperateurDroitTypeSchemaComptable(clsDonnee, clsObjetEnvoi);
        //        if (DataSet.Tables[0].Rows.Count > 0)
        //        {
        //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);

        //            DataSet = new DataSet();
        //            DataRow dr = dt.NewRow();
        //            dr["SL_CODEMESSAGE"] = "00";
        //            dr["SL_RESULTAT"] = "TRUE";
        //            dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
        //            dt.Rows.Add(dr);
        //            DataSet.Tables.Add(dt);
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
        public string pvgChargerDansDataSetOperateurDroit(clsLogicielobjettypeoperationoperateur Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateur = new ZenithWebServeur.BOJ.clsLogicielobjettypeoperationoperateur();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.OP_CODEOPERATEUR };

                //foreach (ZenithWebServeur.DTO.clsLogicielobjettypeoperationoperateur clsLogicielobjettypeoperationoperateurDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsLogicielobjettypeoperationoperateurWSBLL.pvgChargerDansDataSetOperateurDroit(clsDonnee, clsObjetEnvoi);
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
