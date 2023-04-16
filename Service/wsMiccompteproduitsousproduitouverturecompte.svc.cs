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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMiccompteproduitsousproduitouverturecompte" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMiccompteproduitsousproduitouverturecompte.svc ou wsMiccompteproduitsousproduitouverturecompte.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMiccompteproduitsousproduitouverturecompte : IwsMiccompteproduitsousproduitouverturecompte
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMiccompteproduitsousproduitouverturecompteWSBLL clsMiccompteproduitsousproduitouverturecompteWSBLL = new clsMiccompteproduitsousproduitouverturecompteWSBLL();

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
        public string pvgAjouterListe(List<clsMiccompteproduitsousproduitouverturecompte> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsMiccompteproduitsousproduitouverturecompte> clsMiccompteproduitsousproduitouverturecomptes = new List<ZenithWebServeur.BOJ.clsMiccompteproduitsousproduitouverturecompte>();
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

                foreach (ZenithWebServeur.DTO.clsMiccompteproduitsousproduitouverturecompte clsMiccompteproduitsousproduitouverturecompteDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMiccompteproduitsousproduitouverturecompte clsMiccompteproduitsousproduitouverturecompte = new ZenithWebServeur.BOJ.clsMiccompteproduitsousproduitouverturecompte();
                clsMiccompteproduitsousproduitouverturecompte.PS_CODESOUSPRODUIT1 = clsMiccompteproduitsousproduitouverturecompteDTO.PS_CODESOUSPRODUIT1.ToString();
                clsMiccompteproduitsousproduitouverturecompte.PS_CODESOUSPRODUIT2 = clsMiccompteproduitsousproduitouverturecompteDTO.PS_CODESOUSPRODUIT2.ToString();
                    clsMiccompteproduitsousproduitouverturecompte.PL_NUMCOMPTE = clsMiccompteproduitsousproduitouverturecompteDTO.PL_NUMCOMPTE.ToString();
                    clsMiccompteproduitsousproduitouverturecompte.PS_LIBELLE = clsMiccompteproduitsousproduitouverturecompteDTO.PS_LIBELLE.ToString();
                    clsMiccompteproduitsousproduitouverturecompte.PS_ABREVIATION = clsMiccompteproduitsousproduitouverturecompteDTO.PS_ABREVIATION.ToString();
                    clsMiccompteproduitsousproduitouverturecompte.COCHER = clsMiccompteproduitsousproduitouverturecompteDTO.COCHER.ToString();
                    
                clsObjetEnvoi.OE_A = clsMiccompteproduitsousproduitouverturecompteDTO.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = clsMiccompteproduitsousproduitouverturecompteDTO.clsObjetEnvoi.OE_Y;
                    clsObjetEnvoi.OE_O = clsMiccompteproduitsousproduitouverturecompteDTO.SO_CODESOCIETE;

                    clsMiccompteproduitsousproduitouverturecomptes.Add(clsMiccompteproduitsousproduitouverturecompte);
                }
                clsObjetRetour.SetValue(true, clsMiccompteproduitsousproduitouverturecompteWSBLL.pvgAjouterListe(clsDonnee, clsMiccompteproduitsousproduitouverturecomptes, clsObjetEnvoi));
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
        
    }
}
