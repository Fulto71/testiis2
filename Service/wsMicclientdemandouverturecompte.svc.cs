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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMicclientdemandouverturecompte" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMicclientdemandouverturecompte.svc ou wsMicclientdemandouverturecompte.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMicclientdemandouverturecompte : IwsMicclientdemandouverturecompte
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMicclientdemandouverturecompteWSBLL clsMicclientdemandouverturecompteWSBLL = new clsMicclientdemandouverturecompteWSBLL();

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
        public string pvgAjouter(clsMicclientdemandouverturecompte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompte = new ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte();
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

                //foreach (ZenithWebServeur.DTO.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompteDTO in Objet)
                //{
                
                clsMicclientdemandouverturecompte.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                clsMicclientdemandouverturecompte.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclientdemandouverturecompte.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMicclientdemandouverturecompte.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicclientdemandouverturecompte.OP_GESTIONNAIRE = Objet.OP_GESTIONNAIRE.ToString();
                clsMicclientdemandouverturecompte.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                clsMicclientdemandouverturecompte.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                clsMicclientdemandouverturecompte.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                clsMicclientdemandouverturecompte.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                clsMicclientdemandouverturecompte.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                clsMicclientdemandouverturecompte.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsMicclientdemandouverturecompte.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                clsMicclientdemandouverturecompte.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                clsMicclientdemandouverturecompte.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                clsMicclientdemandouverturecompte.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsMicclientdemandouverturecompte.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                clsMicclientdemandouverturecompte.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                clsMicclientdemandouverturecompte.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMicclientdemandouverturecompte.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsMicclientdemandouverturecompte.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsMicclientdemandouverturecompte.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsMicclientdemandouverturecompte.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                clsMicclientdemandouverturecompte.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                clsMicclientdemandouverturecompte.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                clsMicclientdemandouverturecompte.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                clsMicclientdemandouverturecompte.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                clsMicclientdemandouverturecompte.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                clsMicclientdemandouverturecompte.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                clsMicclientdemandouverturecompte.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                clsMicclientdemandouverturecompte.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                clsMicclientdemandouverturecompte.CL_FAX = Objet.CL_FAX.ToString();
                clsMicclientdemandouverturecompte.CL_EMAIL = Objet.CL_EMAIL.ToString();
                clsMicclientdemandouverturecompte.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                clsMicclientdemandouverturecompte.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                clsMicclientdemandouverturecompte.CL_DATEETABLISSEMENTPIECE = DateTime.Parse(Objet.CL_DATEETABLISSEMENTPIECE.ToString());
                clsMicclientdemandouverturecompte.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                clsMicclientdemandouverturecompte.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                clsMicclientdemandouverturecompte.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                clsMicclientdemandouverturecompte.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                clsMicclientdemandouverturecompte.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                clsMicclientdemandouverturecompte.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                clsMicclientdemandouverturecompte.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                clsMicclientdemandouverturecompte.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                clsMicclientdemandouverturecompte.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                clsMicclientdemandouverturecompte.CL_DATECREATIONCPTINTERNET = DateTime.Parse(Objet.CL_DATECREATIONCPTINTERNET.ToString());
                clsMicclientdemandouverturecompte.CL_DATEVALIDATIONCPTINTERNET = DateTime.Parse(Objet.CL_DATEVALIDATIONCPTINTERNET.ToString());
                clsMicclientdemandouverturecompte.CL_URLSITEMARCHAND = Objet.CL_URLSITEMARCHAND.ToString();
                clsMicclientdemandouverturecompte.CL_LIENPAGESUCCES = Objet.CL_LIENPAGESUCCES.ToString();
                clsMicclientdemandouverturecompte.CL_LIENPAGEECHEC = Objet.CL_LIENPAGEECHEC.ToString();
                clsMicclientdemandouverturecompte.CL_DATEREVERSEMENTCOMMISSIONCREATIONCLIENT = DateTime.Parse(Objet.CL_DATEREVERSEMENTCOMMISSIONCREATIONCLIENT.ToString());

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientdemandouverturecompteWSBLL.pvgAjouter(clsDonnee, clsMicclientdemandouverturecompte, clsObjetEnvoi));
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
        public string pvgModifier(clsMicclientdemandouverturecompte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompte = new ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.CL_IDCLIENT };

                //foreach (ZenithWebServeur.DTO.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompteDTO in Objet)
                //{

                clsMicclientdemandouverturecompte.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_CODECLIENT = Objet.CL_CODECLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_CODELIENTPROVISOIRE = Objet.CL_CODELIENTPROVISOIRE.ToString();
                clsMicclientdemandouverturecompte.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMicclientdemandouverturecompte.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMicclientdemandouverturecompte.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMicclientdemandouverturecompte.OP_GESTIONNAIRE = Objet.OP_GESTIONNAIRE.ToString();
                clsMicclientdemandouverturecompte.CO_CODECOMMUNE = Objet.CO_CODECOMMUNE.ToString();
                clsMicclientdemandouverturecompte.CL_ADRESSEGEOGRAPHIQUE = Objet.CL_ADRESSEGEOGRAPHIQUE.ToString();
                clsMicclientdemandouverturecompte.PY_CODEPAYSNATIONALITE = Objet.PY_CODEPAYSNATIONALITE.ToString();
                clsMicclientdemandouverturecompte.SM_CODESITUATIONMATRIMONIALE = Objet.SM_CODESITUATIONMATRIMONIALE.ToString();
                clsMicclientdemandouverturecompte.FM_CODEFORMEJURIDIQUE = Objet.FM_CODEFORMEJURIDIQUE.ToString();
                clsMicclientdemandouverturecompte.TM_CODEMEMBRE = Objet.TM_CODEMEMBRE.ToString();
                clsMicclientdemandouverturecompte.TT_CODETITREMEMBRE = Objet.TT_CODETITREMEMBRE.ToString();
                clsMicclientdemandouverturecompte.TP_CODETYPEPERSONNEL = Objet.TP_CODETYPEPERSONNEL.ToString();
                clsMicclientdemandouverturecompte.TC_CODETYPECONTRAT = Objet.TC_CODETYPECONTRAT.ToString();
                clsMicclientdemandouverturecompte.TM_CODEMEMBREPERSONNELIE = Objet.TM_CODEMEMBREPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.TT_CODETITREMEMBREPERSONNELIE = Objet.TT_CODETITREMEMBREPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.TP_CODETYPEPERSONNELPERSONNELIE = Objet.TP_CODETYPEPERSONNELPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.TC_CODETYPECONTRATPERSONNELIE = Objet.TC_CODETYPECONTRATPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.AC_CODEACTIVITE = Objet.AC_CODEACTIVITE.ToString();
                clsMicclientdemandouverturecompte.PF_CODEPROFESSION = Objet.PF_CODEPROFESSION.ToString();
                clsMicclientdemandouverturecompte.RC_CODERAISONDEPART = Objet.RC_CODERAISONDEPART.ToString();
                clsMicclientdemandouverturecompte.PI_CODEPIECE = Objet.PI_CODEPIECE.ToString();
                clsMicclientdemandouverturecompte.CM_IDCOMMERCIAL = Objet.CM_IDCOMMERCIAL.ToString();
                clsMicclientdemandouverturecompte.GR_CODEGROUPE = Objet.GR_CODEGROUPE.ToString();
                clsMicclientdemandouverturecompte.PS_CODESOUSPRODUIT = Objet.PS_CODESOUSPRODUIT.ToString();
                clsMicclientdemandouverturecompte.CL_IDCLIENTPERSONNELIE = Objet.CL_IDCLIENTPERSONNELIE.ToString();
                clsMicclientdemandouverturecompte.CL_DATECREATION = DateTime.Parse(Objet.CL_DATECREATION.ToString());
                clsMicclientdemandouverturecompte.CL_DATEDEPART = DateTime.Parse(Objet.CL_DATEDEPART.ToString());
                clsMicclientdemandouverturecompte.CL_DESCRIPTIONRAISONDEPART = Objet.CL_DESCRIPTIONRAISONDEPART.ToString();
                clsMicclientdemandouverturecompte.CL_BOITEPOSTALE = Objet.CL_BOITEPOSTALE.ToString();
                clsMicclientdemandouverturecompte.CL_REGISTRECOMMERCE = Objet.CL_REGISTRECOMMERCE.ToString();
                clsMicclientdemandouverturecompte.CL_NUMEROCOMPTECONTRIBUABLE = Objet.CL_NUMEROCOMPTECONTRIBUABLE.ToString();
                clsMicclientdemandouverturecompte.CL_NOMCLIENT = Objet.CL_NOMCLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_PRENOMCLIENT = Objet.CL_PRENOMCLIENT.ToString();
                clsMicclientdemandouverturecompte.CL_DATENAISSANCE = DateTime.Parse(Objet.CL_DATENAISSANCE.ToString());
                clsMicclientdemandouverturecompte.CL_LIEUNAISSANCE = Objet.CL_LIEUNAISSANCE.ToString();
                clsMicclientdemandouverturecompte.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                clsMicclientdemandouverturecompte.CL_FAX = Objet.CL_FAX.ToString();
                clsMicclientdemandouverturecompte.CL_EMAIL = Objet.CL_EMAIL.ToString();
                clsMicclientdemandouverturecompte.CL_SITEWEB = Objet.CL_SITEWEB.ToString();
                clsMicclientdemandouverturecompte.CL_NUMPIECE = Objet.CL_NUMPIECE.ToString();
                clsMicclientdemandouverturecompte.CL_DATEETABLISSEMENTPIECE = DateTime.Parse(Objet.CL_DATEETABLISSEMENTPIECE.ToString());
                clsMicclientdemandouverturecompte.CL_DATEEXPIRATIONPIECE = DateTime.Parse(Objet.CL_DATEEXPIRATIONPIECE.ToString());
                clsMicclientdemandouverturecompte.CL_REGIMEMATRIMONIALE = Objet.CL_REGIMEMATRIMONIALE.ToString();
                clsMicclientdemandouverturecompte.CL_NBENFANT = int.Parse(Objet.CL_NBENFANT.ToString());
                clsMicclientdemandouverturecompte.CL_CAPITAL = Double.Parse(Objet.CL_CAPITAL.ToString());
                clsMicclientdemandouverturecompte.CL_SALAIRENET = Double.Parse(Objet.CL_SALAIRENET.ToString());
                clsMicclientdemandouverturecompte.CL_DESCRIPTIONEMPLOYEUR = Objet.CL_DESCRIPTIONEMPLOYEUR.ToString();
                clsMicclientdemandouverturecompte.CL_BOITEPOSTALEEMPLOYEUR = Objet.CL_BOITEPOSTALEEMPLOYEUR.ToString();
                clsMicclientdemandouverturecompte.CL_TELEMPLOYEUR = Objet.CL_TELEMPLOYEUR.ToString();
                clsMicclientdemandouverturecompte.CL_MATRICULEEMPLOYE = Objet.CL_MATRICULEEMPLOYE.ToString();
                clsMicclientdemandouverturecompte.CL_DATECREATIONCPTINTERNET = DateTime.Parse(Objet.CL_DATECREATIONCPTINTERNET.ToString());
                clsMicclientdemandouverturecompte.CL_DATEVALIDATIONCPTINTERNET = DateTime.Parse(Objet.CL_DATEVALIDATIONCPTINTERNET.ToString());
                clsMicclientdemandouverturecompte.CL_URLSITEMARCHAND = Objet.CL_URLSITEMARCHAND.ToString();
                clsMicclientdemandouverturecompte.CL_LIENPAGESUCCES = Objet.CL_LIENPAGESUCCES.ToString();
                clsMicclientdemandouverturecompte.CL_LIENPAGEECHEC = Objet.CL_LIENPAGEECHEC.ToString();
                clsMicclientdemandouverturecompte.CL_DATEREVERSEMENTCOMMISSIONCREATIONCLIENT = DateTime.Parse(Objet.CL_DATEREVERSEMENTCOMMISSIONCREATIONCLIENT.ToString());

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientdemandouverturecompteWSBLL.pvgModifier(clsDonnee, clsMicclientdemandouverturecompte, clsObjetEnvoi));
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
        public string pvgSupprimer(clsMicclientdemandouverturecompte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompte = new ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.AG_CODEAGENCE, Objet.CL_IDCLIENT };

                //foreach (ZenithWebServeur.DTO.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMicclientdemandouverturecompteWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi));
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
        public string pvgChargerDansDataSet(clsMicclientdemandouverturecompte Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompte = new ZenithWebServeur.BOJ.clsMicclientdemandouverturecompte();
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
                    Objet.CL_DATECREATION1,
                    Objet.CL_DATECREATION2,
                    Objet.CL_CODECLIENT,
                    Objet.CL_NOMPRENOMCLIENT,
                     Objet.TYPEOPERATION
                };

                //foreach (ZenithWebServeur.DTO.clsMicclientdemandouverturecompte clsMicclientdemandouverturecompteDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsMicclientdemandouverturecompteWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
