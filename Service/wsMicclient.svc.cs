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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicclient" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicclient.svc ou wsMicclient.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicclient : IwsMicclient
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicclientWSBLL clsMicclientWSBLL = new clsMicclientWSBLL();
        private clsJourneetravailWSBLL clsJourneetravailWSBLL = new clsJourneetravailWSBLL();

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
        public string pvgAjouter(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };
                if (Objet.clsObjetEnvoi != null)
                {
                    clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    if (Objet.clsObjetEnvoi.OE_J != "")
                        clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_Y = clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_U = clsObjetEnvoi.OE_U;
                    if (Objet.clsObjetEnvoi.OE_G != "")
                        clsObjetEnvoi.OE_G = DateTime.Parse(Objet.clsObjetEnvoi.OE_G);
                    clsObjetEnvoi.OE_F = Objet.clsObjetEnvoi.OE_F;
                    clsObjetEnvoi.OE_T = Objet.clsObjetEnvoi.OE_T;
                }

                if (clsJourneetravailWSBLL.pvgValeurScalaireRequeteCount2(clsDonnee, clsObjetEnvoi) == "0")
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Cette journée a été déjà fermée ou non encore ouverte !!!";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                else
                {
                    //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                    //{

                    clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                    clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                    clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                    clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                    clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                    clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                    clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                    clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                    clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                    clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                    clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                    clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                    clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                    clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                    clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                    clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                    clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                    clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                    clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                    clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                    clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                    clsMicclient.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                    clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                    clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                    clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                    clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                    clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                    clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                    clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                    clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                    clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                    clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                    clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                    clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                    clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                    clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                    clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                    clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                    clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                    clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                    clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                    clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                    clsMicclient.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                    clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                    clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                    clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                    clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                    clsMicclient.CL_CHIFFREAFFAIRE = Double.Parse(Objet.CL_CHIFFREAFFAIRE.ToString());
                    clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();
                    clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                    clsMicclient.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                    clsMicclient.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                    clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgAjouter(clsDonnee, clsMicclient, clsObjetEnvoi));
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

        public string pvgChargerDansDataSetPourComboEdition(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.TP_CODETYPETIERS };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMicclientWSBLL.pvgChargerDansDataSetPourComboEdition(clsDonnee, clsObjetEnvoi);
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

        //AJOUT
        public string pvgValeurScalaireRequeteCount(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("VALEUR", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT, Objet.TM_CODEMEMBRE };

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgValeurScalaireRequeteCount(clsDonnee, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["VALEUR"] = clsObjetRetour.OR_STRING;
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
        public string pvgValeurScalaireRequeteCount_new(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("VALEUR", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT };

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgValeurScalaireRequeteCount(clsDonnee, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["VALEUR"] = clsObjetRetour.OR_STRING;
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
        public string pvgAjouterValidation(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            ZenithWebServeur.BOJ.clsMicclientphoto clsMicclientphoto = new ZenithWebServeur.BOJ.clsMicclientphoto();
            ZenithWebServeur.DTO.clsMicclientphoto clsMicclientphotodto = new ZenithWebServeur.DTO.clsMicclientphoto();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgAjouterValidation(Objet);
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

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsMicclient.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.OP_GESTIONNAIRE = Objet.OP_GESTIONNAIRE.ToString();
                clsMicclient.CL_IDCLIENTDEMANDEUR = Objet.CL_IDCLIENTDEMANDEUR.ToString();
                clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();

                //clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                //clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                //clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                //clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                //clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                //clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                //clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                //clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                //clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                //clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                //clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                //clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                //clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                //clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                //clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                //clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                //clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                //clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                //clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                //clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                //clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                //clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                //clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                //clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                //clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                //clsMicclient.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                //clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                //clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                //clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                //clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                //clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                //clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                //clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                //clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                //clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                //clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                //clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                //clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                //clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                //clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                //clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                //clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                //clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                //clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                //clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                //clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                //clsMicclient.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                //clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                //clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                //clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                //clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                //clsMicclient.CL_CHIFFREAFFAIRE = Double.Parse(Objet.CL_CHIFFREAFFAIRE.ToString());
                //clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();
                //clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                //clsMicclient.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                //clsMicclient.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                //clsMicclient.CH_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO.ToString());
                //clsMicclient.CH_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE.ToString());

                //objet photo
                clsMicclientphoto.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclientphoto.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                // clsMicclientphoto.CH_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO.ToString());
                // clsMicclientphoto.CH_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE.ToString());
                Byte[] CM_PHOTO = null;
                Byte[] CM_SIGNATURE = null;
                if (Objet.CH_PHOTO != "")
                    CM_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO);
                if (Objet.CH_SIGNATURE != "")
                    CM_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE);

                clsMicclientphoto.CH_PHOTO = CM_PHOTO;
                clsMicclientphoto.CH_SIGNATURE = CM_SIGNATURE;

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgAjouterValidation(clsDonnee, clsMicclient, clsMicclientphoto, clsObjetEnvoi));
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
        public string pvgModifierCL_CODECLIENT(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgModifierCL_CODECLIENT(Objet);
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
                //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclient.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierCL_CODECLIENT(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgModifierSX_CODESEXE(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgModifierSX_CODESEXE(Objet);
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
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierSX_CODESEXE(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgModifierCM_IDCOMMERCIAL(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgModifierCM_IDCOMMERCIAL(Objet);
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

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierCM_IDCOMMERCIAL(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgModifierAgentCollecteEtDeCredit(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgModifierAgentCollecteEtDeCredit(Objet);
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

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierAgentCollecteEtDeCredit(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgAjouterListe(List<clsMicclient> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMicclient> clsMicclients = new List<ZenithWebServeur.BOJ.clsMicclient>();
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

                foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();

                    clsMicclient.AG_CODEAGENCE = clsMicclientDTO.AG_CODEAGENCE.ToString();
                    clsMicclient.PV_CODEPOINTVENTE = clsMicclientDTO.PV_CODEPOINTVENTE.ToString();
                    clsMicclient.CL_CODECLIENT = clsMicclientDTO.CL_CODECLIENT.ToString();
                    clsMicclient.CL_IDCLIENT = clsMicclientDTO.CL_IDCLIENT.ToString();
                    clsMicclient.CL_CODELIENTPROVISOIRE = clsMicclientDTO.CL_CODELIENTPROVISOIRE.ToString();
                    clsMicclient.OP_CODEOPERATEUR = clsMicclientDTO.OP_CODEOPERATEUR.ToString();
                    clsMicclient.CO_CODECOMMUNE = clsMicclientDTO.CO_CODECOMMUNE.ToString();
                    clsMicclient.PY_CODEPAYSNATIONALITE = clsMicclientDTO.PY_CODEPAYSNATIONALITE.ToString();
                    clsMicclient.CL_ADRESSEGEOGRAPHIQUE = clsMicclientDTO.CL_ADRESSEGEOGRAPHIQUE.ToString();
                    clsMicclient.SM_CODESITUATIONMATRIMONIALE = clsMicclientDTO.SM_CODESITUATIONMATRIMONIALE.ToString();
                    clsMicclient.FM_CODEFORMEJURIDIQUE = clsMicclientDTO.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicclient.TM_CODEMEMBRE = clsMicclientDTO.TM_CODEMEMBRE.ToString();
                    clsMicclient.TT_CODETITREMEMBRE = clsMicclientDTO.TT_CODETITREMEMBRE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNEL = clsMicclientDTO.TP_CODETYPEPERSONNEL.ToString();
                    clsMicclient.TC_CODETYPECONTRAT = clsMicclientDTO.TC_CODETYPECONTRAT.ToString();
                    clsMicclient.TM_CODEMEMBREPERSONNELIE = clsMicclientDTO.TM_CODEMEMBREPERSONNELIE.ToString();
                    clsMicclient.TT_CODETITREMEMBREPERSONNELIE = clsMicclientDTO.TT_CODETITREMEMBREPERSONNELIE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = clsMicclientDTO.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                    clsMicclient.TC_CODETYPECONTRATPERSONNELIE = clsMicclientDTO.TC_CODETYPECONTRATPERSONNELIE.ToString();
                    clsMicclient.AC_CODEACTIVITE = clsMicclientDTO.AC_CODEACTIVITE.ToString();
                    clsMicclient.PF_CODEPROFESSION = clsMicclientDTO.PF_CODEPROFESSION.ToString();
                    clsMicclient.RC_CODERAISONDEPART = clsMicclientDTO.RC_CODERAISONDEPART.ToString();
                    clsMicclient.PI_CODEPIECE = clsMicclientDTO.PI_CODEPIECE.ToString();
                    clsMicclient.GR_CODEGROUPE = clsMicclientDTO.GR_CODEGROUPE.ToString();
                    clsMicclient.PS_CODESOUSPRODUIT = clsMicclientDTO.PS_CODESOUSPRODUIT.ToString();
                    clsMicclient.CL_IDCLIENTPERSONNELIE = clsMicclientDTO.CL_IDCLIENTPERSONNELIE.ToString();
                    clsMicclient.CL_DATECREATION = DateTime.Parse(clsMicclientDTO.CL_DATECREATION.ToString());
                    clsMicclient.CL_DATEDEPART = DateTime.Parse(clsMicclientDTO.CL_DATEDEPART.ToString());
                    clsMicclient.CL_DESCRIPTIONRAISONDEPART = clsMicclientDTO.CL_DESCRIPTIONRAISONDEPART.ToString();
                    clsMicclient.CL_BOITEPOSTALE = clsMicclientDTO.CL_BOITEPOSTALE.ToString();
                    clsMicclient.CL_REGISTRECOMMERCE = clsMicclientDTO.CL_REGISTRECOMMERCE.ToString();
                    clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = clsMicclientDTO.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                    clsMicclient.CL_NOMCLIENT = clsMicclientDTO.CL_NOMCLIENT.ToString();
                    clsMicclient.CL_PRENOMCLIENT = clsMicclientDTO.CL_PRENOMCLIENT.ToString();
                    clsMicclient.CL_DATENAISSANCE = DateTime.Parse(clsMicclientDTO.CL_DATENAISSANCE.ToString());
                    clsMicclient.CL_LIEUNAISSANCE = clsMicclientDTO.CL_LIEUNAISSANCE.ToString();
                    clsMicclient.CL_TELEPHONE = clsMicclientDTO.CL_TELEPHONE.ToString();
                    clsMicclient.CL_FAX = clsMicclientDTO.CL_FAX.ToString();
                    clsMicclient.CL_EMAIL = clsMicclientDTO.CL_EMAIL.ToString();
                    clsMicclient.CL_SITEWEB = clsMicclientDTO.CL_SITEWEB.ToString();
                    clsMicclient.CL_NUMPIECE = clsMicclientDTO.CL_NUMPIECE.ToString();
                    clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(clsMicclientDTO.CL_DATEEXPIRATIONPIECE.ToString());
                    clsMicclient.CL_REGIMEMATRIMONIALE = clsMicclientDTO.CL_REGIMEMATRIMONIALE.ToString();
                    clsMicclient.CL_NBENFANT = int.Parse(clsMicclientDTO.CL_NBENFANT.ToString());
                    clsMicclient.CL_DESCRIPTIONEMPLOYEUR = clsMicclientDTO.CL_DESCRIPTIONEMPLOYEUR.ToString();
                    clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = clsMicclientDTO.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                    clsMicclient.CL_TELEMPLOYEUR = clsMicclientDTO.CL_TELEMPLOYEUR.ToString();
                    clsMicclient.CL_MATRICULEEMPLOYE = clsMicclientDTO.CL_MATRICULEEMPLOYE.ToString();
                    clsMicclient.OP_GESTIONNAIRECOMPTE = clsMicclientDTO.OP_GESTIONNAIRECOMPTE.ToString();
                    clsMicclient.CM_IDCOMMERCIAL = clsMicclientDTO.CM_IDCOMMERCIAL.ToString();
                    clsMicclient.CL_CAPITAL = Double.Parse(clsMicclientDTO.CL_CAPITAL.ToString());
                    clsMicclient.CL_SALAIRENET = Double.Parse(clsMicclientDTO.CL_SALAIRENET.ToString());
                    clsMicclient.CL_TAUXREMUNERATION = Double.Parse(clsMicclientDTO.CL_TAUXREMUNERATION.ToString());
                    clsMicclient.CL_CHIFFREAFFAIRE = Double.Parse(clsMicclientDTO.CL_CHIFFREAFFAIRE.ToString());
                    clsMicclient.OB_NOMOBJET = clsMicclientDTO.OB_NOMOBJET.ToString();
                    clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = clsMicclientDTO.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                    clsMicclient.GM_CODESEGMENT = clsMicclientDTO.GM_CODESEGMENT.ToString();
                    clsMicclient.GT_CODETYPECLIENT = clsMicclientDTO.GT_CODETYPECLIENT.ToString();

                    clsObjetEnvoi.OE_A = clsMicclientDTO.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = clsMicclientDTO.clsObjetEnvoi.OE_Y;
                    clsMicclients.Add(clsMicclient);
                }
                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgAjouterListe(clsDonnee, clsMicclients, clsObjetEnvoi));
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

        //MODIFIER
        public string pvgModifier(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.CL_CODECLIENT };
                if (Objet.clsObjetEnvoi != null)
                {
                    clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    if (Objet.clsObjetEnvoi.OE_J != "")
                        clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_Y = clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_U = clsObjetEnvoi.OE_U;
                    if (Objet.clsObjetEnvoi.OE_G != "")
                        clsObjetEnvoi.OE_G = DateTime.Parse(Objet.clsObjetEnvoi.OE_G);
                    clsObjetEnvoi.OE_F = Objet.clsObjetEnvoi.OE_F;
                    clsObjetEnvoi.OE_T = Objet.clsObjetEnvoi.OE_T;
                }

                if (clsJourneetravailWSBLL.pvgValeurScalaireRequeteCount2(clsDonnee, clsObjetEnvoi) == "0")
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Cette journée a été déjà fermée ou non encore ouverte !!!";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                else
                {
                    //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                    //{

                    clsMicclient.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                    clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                    clsMicclient.OP_GESTIONNAIRE = Objet.OP_GESTIONNAIRE.ToString();
                    clsMicclient.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                    clsMicclient.CL_IDCLIENTDEMANDEUR = Objet.CL_IDCLIENTDEMANDEUR.ToString();
                    clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                    clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                    clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                    clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                    clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                    clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                    clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                    clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                    clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                    clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                    clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                    clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                    clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                    clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                    clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                    clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                    clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                    clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                    clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                    clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                    clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                    clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                    clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                    clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                    clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                    clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                    clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                    clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                    clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                    clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                    clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                    clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                    clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                    clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                    clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                    clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                    clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                    clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                    clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                    clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                    clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                    clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                    clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                    clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                    clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                    clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();
                    clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                    clsMicclient.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                    clsMicclient.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                    //clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    //clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                    //clsMicclient.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                    //clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                    //clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                    //clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                    //clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                    //clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                    //clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                    //clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                    //clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                    //clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                    //clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                    //clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                    //clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                    //clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                    //clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                    //clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                    //clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                    //clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                    //clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                    //clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                    //clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                    //clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                    //clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                    //clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                    //clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                    //clsMicclient.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                    //clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                    //clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                    //clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                    //clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                    //clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                    //clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                    //clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                    //clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                    //clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                    //clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                    //clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                    //clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                    //clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                    //clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                    //clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                    //clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                    //clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                    //clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                    //clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                    //clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                    //clsMicclient.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                    //clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                    //clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                    //clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                    //clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                    //clsMicclient.CL_CHIFFREAFFAIRE = Double.Parse(Objet.CL_CHIFFREAFFAIRE.ToString());
                    //clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();
                    //clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                    //clsMicclient.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                    //clsMicclient.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();
                    //if (clsMicclient.CH_PHOTO != null)
                    //    clsMicclient.CH_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO.ToString());
                    //if (clsMicclient.CH_SIGNATURE != null)
                    //    clsMicclient.CH_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE.ToString());

                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                    clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifier(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgAjouterClientPhoto(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            ZenithWebServeur.BOJ.clsMicclientphoto clsMicclientphoto = new ZenithWebServeur.BOJ.clsMicclientphoto();
            ZenithWebServeur.DTO.clsMicclientphoto clsMicclientphotodto = new ZenithWebServeur.DTO.clsMicclientphoto();
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
                //clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE };
                if (Objet.clsObjetEnvoi != null)
                {
                    clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    if (Objet.clsObjetEnvoi.OE_J != "")
                        clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_Y = clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_U = clsObjetEnvoi.OE_U;
                    if (Objet.clsObjetEnvoi.OE_G != "")
                        clsObjetEnvoi.OE_G = DateTime.Parse(Objet.clsObjetEnvoi.OE_G);
                    clsObjetEnvoi.OE_F = Objet.clsObjetEnvoi.OE_F;
                    clsObjetEnvoi.OE_T = Objet.clsObjetEnvoi.OE_T;
                }

                if (clsJourneetravailWSBLL.pvgValeurScalaireRequeteCount2(clsDonnee, clsObjetEnvoi) == "0")
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Cette journée a été déjà fermée ou non encore ouverte !!!";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                else
                {
                    //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                    //{

                    // objet principal
                    clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                    clsMicclient.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                    clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                    clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                    clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                    clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                    clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                    clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                    clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                    clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                    clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                    clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                    clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                    clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                    clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                    clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                    clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                    clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                    clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                    clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                    clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                    clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                    clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                    clsMicclient.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                    clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                    clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                    clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                    clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                    clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                    clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                    clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                    clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                    clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                    clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                    clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                    clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                    clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                    clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                    clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                    clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                    clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                    clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                    clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                    clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                    clsMicclient.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                    clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                    clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                    clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                    clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                    clsMicclient.CL_CHIFFREAFFAIRE = Double.Parse(Objet.CL_CHIFFREAFFAIRE.ToString());
                    clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();
                    clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                    clsMicclient.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                    clsMicclient.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                    //objet photo
                    clsMicclientphoto.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMicclientphoto.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                    // clsMicclientphoto.CH_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO.ToString());
                    // clsMicclientphoto.CH_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE.ToString());
                    Byte[] CM_PHOTO = null;
                    Byte[] CM_SIGNATURE = null;
                    if (Objet.CH_PHOTO != "")
                        CM_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO);
                    if (Objet.CH_SIGNATURE != "")
                        CM_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE);

                    clsMicclientphoto.CH_PHOTO = CM_PHOTO;
                    clsMicclientphoto.CH_SIGNATURE = CM_SIGNATURE;

                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                    clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgAjouterClientPhoto(clsDonnee, clsMicclient, clsMicclientphoto, clsObjetEnvoi));
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
        public string pvgModifierTypeMembre(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            ZenithWebServeur.BOJ.clsMicclientphoto clsMicclientphoto = new ZenithWebServeur.BOJ.clsMicclientphoto();

            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgModifierTypeMembre(Objet);
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
                clsObjetEnvoi.OE_PARAM = new string[] {  };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                // objet principal
                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierTypeMembre(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgModifierTelePhone(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            ZenithWebServeur.BOJ.clsMicclientphoto clsMicclientphoto = new ZenithWebServeur.BOJ.clsMicclientphoto();

            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            DataSet = TestChampObligatoireInsertpvgModifierTelePhone(Objet);
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

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                // objet principal
                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierTelePhone(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgModifierBIC(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            ZenithWebServeur.BOJ.clsMicclientphoto clsMicclientphoto = new ZenithWebServeur.BOJ.clsMicclientphoto();

            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //--TEST DES CHAMPS OBLIGATOIRES
            //DataSet = TestChampObligatoireInsertpvgModifierTelePhone(Objet);
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

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                // objet principal
                clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclient.CL_NOMPRENOMMERE = Objet.CL_NOMPRENOMMERE.ToString();
                clsMicclient.CL_NBREEPOUSE = int.Parse(Objet.CL_NBREEPOUSE.ToString());
                clsMicclient.CL_DATEETABLISSEMENTPIECE = DateTime.Parse(Objet.CL_DATEETABLISSEMENTPIECE.ToString());
                clsMicclient.PY_CODEPAYSCNI = Objet.PY_CODEPAYSCNI.ToString();
                clsMicclient.CL_NBREEMPLOYE = int.Parse(Objet.CL_NBREEMPLOYE.ToString());
                clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                clsMicclient.CL_NUMEROFISCAL = Objet.CL_NUMEROFISCAL.ToString();
                clsMicclient.PY_CODEPAYSFISCAL = Objet.PY_CODEPAYSFISCAL.ToString();
                clsMicclient.CL_NUMEROBCEAO = Objet.CL_NUMEROBCEAO.ToString();
                clsMicclient.PY_CODEPAYSDELIVRANCENUMEROBCEAO = Objet.PY_CODEPAYSDELIVRANCENUMEROBCEAO.ToString();
                clsMicclient.CL_NOMCOMMERCIAL = Objet.CL_NOMCOMMERCIAL.ToString();
                clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicclient.SA_CODESITUATIONAFFAIRE = Objet.SA_CODESITUATIONAFFAIRE.ToString();
                clsMicclient.AE_CODEAGENTECONOMIQUE = Objet.AE_CODEAGENTECONOMIQUE.ToString();
                clsMicclient.TB_CODE = Objet.TB_CODE.ToString();
                clsMicclient.CP_CODE = Objet.CP_CODE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierBIC(clsDonnee, clsMicclient, clsObjetEnvoi));
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
        public string pvgModifierClientPhoto(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
            ZenithWebServeur.BOJ.clsMicclientphoto clsMicclientphoto = new ZenithWebServeur.BOJ.clsMicclientphoto();

            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
                //--TEST DES CHAMPS OBLIGATOIRES
                DataSet = TestChampObligatoireUpdate(Objet);
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.CL_IDCLIENT };

                if (Objet.clsObjetEnvoi != null)
                {
                    clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    if (Objet.clsObjetEnvoi.OE_J != "")
                        clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_Y = clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_U = clsObjetEnvoi.OE_U;
                    if (Objet.clsObjetEnvoi.OE_G != "")
                        clsObjetEnvoi.OE_G = DateTime.Parse(Objet.clsObjetEnvoi.OE_G);
                    clsObjetEnvoi.OE_F = Objet.clsObjetEnvoi.OE_F;
                    clsObjetEnvoi.OE_T = Objet.clsObjetEnvoi.OE_T;
                }

                if (clsJourneetravailWSBLL.pvgValeurScalaireRequeteCount2(clsDonnee, clsObjetEnvoi) == "0")
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Cette journée a été déjà fermée ou non encore ouverte !!!";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                else
                {
                    //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                    //{

                    // objet principal
                    clsMicclient.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMicclient.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                    clsMicclient.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                    clsMicclient.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                    clsMicclient.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                    clsMicclient.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                    clsMicclient.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                    clsMicclient.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                    clsMicclient.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                    clsMicclient.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                    clsMicclient.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                    clsMicclient.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                    clsMicclient.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                    clsMicclient.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                    clsMicclient.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                    clsMicclient.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                    clsMicclient.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                    clsMicclient.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                    clsMicclient.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                    clsMicclient.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                    clsMicclient.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                    clsMicclient.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                    clsMicclient.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                    clsMicclient.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                    clsMicclient.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                    clsMicclient.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                    clsMicclient.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                    clsMicclient.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                    clsMicclient.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                    clsMicclient.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                    clsMicclient.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                    clsMicclient.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                    clsMicclient.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                    clsMicclient.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                    clsMicclient.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                    clsMicclient.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                    clsMicclient.CL_FAX = Objet.CL_FAX.ToString();
                    clsMicclient.CL_EMAIL = Objet.CL_EMAIL.ToString();
                    clsMicclient.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                    clsMicclient.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                    clsMicclient.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                    clsMicclient.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                    clsMicclient.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                    clsMicclient.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                    clsMicclient.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                    clsMicclient.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                    clsMicclient.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                    clsMicclient.OP_GESTIONNAIRECOMPTE = Objet.OP_GESTIONNAIRECOMPTE.ToString();
                    clsMicclient.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                    clsMicclient.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                    clsMicclient.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                    clsMicclient.CL_TAUXREMUNERATION = Double.Parse(Objet.CL_TAUXREMUNERATION.ToString());
                    clsMicclient.CL_CHIFFREAFFAIRE = Double.Parse(Objet.CL_CHIFFREAFFAIRE.ToString());
                    clsMicclient.OB_NOMOBJET = Objet.OB_NOMOBJET.ToString();
                    clsMicclient.OP_AGENTDECOLLECTEETDECREDIT = Objet.OP_AGENTDECOLLECTEETDECREDIT.ToString();
                    clsMicclient.GM_CODESEGMENT = Objet.GM_CODESEGMENT.ToString();
                    clsMicclient.GT_CODETYPECLIENT = Objet.GT_CODETYPECLIENT.ToString();

                    //objet photo
                    clsMicclientphoto.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                    clsMicclientphoto.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                    //clsMicclientphoto.CH_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO.ToString());
                    //clsMicclientphoto.CH_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE.ToString());
                    Byte[] CM_PHOTO = null;
                    Byte[] CM_SIGNATURE = null;
                    if (Objet.CH_PHOTO != "")
                        CM_PHOTO = System.Convert.FromBase64String(Objet.CH_PHOTO);
                    if (Objet.CH_SIGNATURE != "")
                        CM_SIGNATURE = System.Convert.FromBase64String(Objet.CH_SIGNATURE);

                    clsMicclientphoto.CH_PHOTO = CM_PHOTO;
                    clsMicclientphoto.CH_SIGNATURE = CM_SIGNATURE;

                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                    clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgModifierClientPhoto(clsDonnee, clsMicclient, clsMicclientphoto, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                if (Objet.clsObjetEnvoi != null)
                {
                    clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    if (Objet.clsObjetEnvoi.OE_J != "")
                        clsObjetEnvoi.OE_J = DateTime.Parse(Objet.clsObjetEnvoi.OE_J);
                    clsObjetEnvoi.OE_Y = clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_U = clsObjetEnvoi.OE_U;
                    if (Objet.clsObjetEnvoi.OE_G != "")
                        clsObjetEnvoi.OE_G = DateTime.Parse(Objet.clsObjetEnvoi.OE_G);
                    clsObjetEnvoi.OE_F = Objet.clsObjetEnvoi.OE_F;
                    clsObjetEnvoi.OE_T = Objet.clsObjetEnvoi.OE_T;
                }

                if (clsJourneetravailWSBLL.pvgValeurScalaireRequeteCount2(clsDonnee, clsObjetEnvoi) == "0")
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = "99";
                    dr["SL_RESULTAT"] = "FALSE";
                    dr["SL_MESSAGE"] = "Cette journée a été déjà fermée ou non encore ouverte !!!";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
                }
                else
                {
                    //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                    //{

                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                    clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        public string pvgChargerDansDataSet(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicclientWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerRechercheClientparNomEcranClientDiffere(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT, Objet.CL_NOMCLIENT, Objet.CL_PRENOMCLIENT, Objet.TM_CODEMEMBRE, Objet.CL_DATECREATION1, Objet.CL_DATECREATION2, Objet.CL_TELEPHONE, Objet.CL_MATRICULEEMPLOYE, Objet.SX_CODESEXE };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicclientWSBLL.pvgChargerRechercheClientparNomEcranClientDiffere(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerRechercheClientparNomEcranClient(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                    Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT, Objet.CL_NOMCLIENT, Objet.CL_PRENOMCLIENT,
                    Objet.TM_CODEMEMBRE, Objet.CL_DATECREATION1, Objet.CL_DATECREATION2,
                    Objet.CL_TELEPHONE, Objet.CL_MATRICULEEMPLOYE, Objet.SX_CODESEXE
                };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsMicclientWSBLL.pvgChargerRechercheClientparNomEcranClient(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerListeCommercialparNom(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                    Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT, Objet.CL_NOMCLIENT, Objet.CL_PRENOMCLIENT,
                    Objet.TM_CODEMEMBRE, Objet.CL_DATECREATION1, Objet.CL_DATECREATION2,
                    Objet.CL_TELEPHONE, Objet.CL_MATRICULEEMPLOYE, Objet.SX_CODESEXE
                };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMicclientWSBLL.pvgChargerRechercheClientparNomEcranClient(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerRechercheClientparNom(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                    Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT, Objet.CL_NOMCLIENT,
                    Objet.CL_PRENOMCLIENT, Objet.TM_CODEMEMBRE, Objet.CL_DATECREATION1,
                    Objet.CL_DATECREATION2, Objet.CL_TELEPHONE, Objet.CL_MATRICULEEMPLOYE,
                    Objet.SX_CODESEXE
                };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicclientWSBLL.pvgChargerRechercheClientparNom(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerRechercheClientparNom1(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                    Objet.AG_CODEAGENCE, Objet.CL_CODECLIENT, Objet.CL_NOMCLIENT,
                    Objet.CL_PRENOMCLIENT, "", Objet.CL_DATECREATION1,
                    Objet.CL_DATECREATION2, Objet.CL_TELEPHONE, "","03"
                };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicclientWSBLL.pvgChargerRechercheClientparNom(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerListeClientGestionnaire(clsMicclient Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclient clsMicclient = new ZenithWebServeur.BOJ.clsMicclient();
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
                    Objet.AG_CODEAGENCE, Objet.OP_GESTIONNAIRE
                };

                //foreach (ZenithWebServeur.DTO.clsMicclient clsMicclientDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMicclientWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMicclientWSBLL.pvgChargerListeClientGestionnaire(clsDonnee, clsObjetEnvoi);
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
