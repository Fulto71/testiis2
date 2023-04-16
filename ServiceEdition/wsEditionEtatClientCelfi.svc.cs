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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsEditionEtatClientCelfi" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsEditionEtatClientCelfi.svc ou wsEditionEtatClientCelfi.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    //public partial class wsEditionEtatClientCelfi : IwsEditionEtatClientCelfi
    //{
    //    private clsDonnee _clsDonnee = new clsDonnee();
    //    private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
    //    private clsEditionEtatClientCelfiWSBLL clsEditionEtatClientCelfiWSBLL = new clsEditionEtatClientCelfiWSBLL();

    //    public clsDonnee clsDonnee
    //    {
    //        get { return _clsDonnee; }
    //        set { _clsDonnee = value; }
    //    }

    //    //Déclaration du log
    //    log4net.ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    //    public string Base64Encode(string plainText)
    //    {
    //        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
    //        return System.Convert.ToBase64String(plainTextBytes);
    //    }



    //    ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
    //    ///<param name="Objet">Collection de clsInput </param>
    //    ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
    //    ///<author>Home Technology</author>
    //    public string pvgChargerDansDataSet(ZenithWebServeur.DTO.clsEditionEtatClientCelfi Objet)
    //    {
    //        DataSet DataSet = new DataSet();
    //        DataTable dt = new DataTable("TABLE");
    //        dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
    //        dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
    //        dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
    //        string json = "";
            
    //        List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
    //        List<ZenithWebServeur.DTO.clsCommune> clsCommunes = new List<ZenithWebServeur.DTO.clsCommune>();

    //        ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
    //        clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
    //        clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
    //        clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
    //        clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


    //        //for (int Idx = 0; Idx < Objet.Count; Idx++)
    //        //{
    //            //--TEST DES CHAMPS OBLIGATOIRES
    //            DataSet = TestChampObligatoireListe(Objet);
    //            //--VERIFICATION DU RESULTAT DU TEST
    //            if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
    //            //--TEST CONTRAINTE
    //            //DataSet = TestTestContrainteListe(Objet);
    //            //--VERIFICATION DU RESULTAT DU TEST
    //            //if (DataSet.Tables[0].Rows[0]["SL_RESULTAT"].ToString() == "FALSE") { json = JsonConvert.SerializeObject(DataSet, Formatting.Indented); return json; }
    //        //}

    //    clsObjetEnvoi.OE_PARAM = new string[] { Objet.ET_NOMGROUPE, Objet.OP_CODEOPERATEUR, Objet.ET_AFFICHER, Objet.OD_APERCU };

    //        try
    //        {
    //            clsDonnee.pvgDemarrerTransaction();
    //            DataSet = clsEditionEtatClientCelfiWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
    //             if (DataSet.Tables[0].Rows.Count > 0)
    //            {
    //                DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
    //                DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
    //                DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
    //                for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
    //                {
    //                    DataSet.Tables[0].Rows[i]["SL_CODEMESSAGE"] = "00";
    //                    DataSet.Tables[0].Rows[i]["SL_RESULTAT"] = "TRUE";
    //                    DataSet.Tables[0].Rows[i]["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
    //                }

    //                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
    //            }
    //            else
    //            {

    //                DataSet = new DataSet();
    //                DataRow dr = dt.NewRow();
    //                dr["SL_CODEMESSAGE"] = "99";
    //                dr["SL_RESULTAT"] = "FALSE";
    //                dr["SL_MESSAGE"] = "Aucun enregistrement n'a été trouvé";
    //                dt.Rows.Add(dr);
    //                DataSet.Tables.Add(dt);
    //                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
    //            }
    //        }
    //         catch (SqlException SQLEx)
    //        {
    //            DataSet = new DataSet();
    //            DataRow dr = dt.NewRow();
    //            dr["SL_CODEMESSAGE"] = "99";
    //            dr["SL_RESULTAT"] = "FALSE";
    //            dr["SL_MESSAGE"] = (SQLEx.Number == 2601 || SQLEx.Number == 2627) ? clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0003").MS_LIBELLEMESSAGE : SQLEx.Message;
    //            dt.Rows.Add(dr);
    //            DataSet.Tables.Add(dt);
    //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
    //            //Execution du log
    //            Log.Error(SQLEx.Message, null);
    //        }
    //        catch (Exception SQLEx)
    //        {
    //            DataSet = new DataSet();
    //            DataRow dr = dt.NewRow();
    //            dr["SL_CODEMESSAGE"] = "99";
    //            dr["SL_RESULTAT"] = "FALSE";
    //            dr["SL_MESSAGE"] = SQLEx.Message;
    //            dt.Rows.Add(dr);
    //            DataSet.Tables.Add(dt);
    //            json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
    //            //Execution du log
    //            Log.Error(SQLEx.Message, null);

    //        }


    //        finally
    //        {
    //            clsDonnee.pvgTerminerTransaction(true);
    //        }
    //        return json;
    //    }
        
    //}
}
