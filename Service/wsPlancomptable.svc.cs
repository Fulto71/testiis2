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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsPlancomptable" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsPlancomptable.svc ou wsPlancomptable.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsPlancomptable : IwsPlancomptable
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsPlancomptableWSBLL clsPlancomptableWSBLL = new clsPlancomptableWSBLL();

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

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        //public List<ZenithWebServeur.DTO.clsPlancomptable> pvgAjouter(List<ZenithWebServeur.DTO.clsPlancomptable> Objet)
        //{
        //    List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //    List<ZenithWebServeur.DTO.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    {
        //        //--TEST DES CHAMPS OBLIGATOIRES
        //        clsPlancomptables = TestChampObligatoireInsert(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsPlancomptables[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsPlancomptables;
        //        //--TEST CONTRAINTE
        //        clsPlancomptables = TestTestContrainteListe(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsPlancomptables[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsPlancomptables;
        //    }
        //    //clsObjetEnvoi.OE_PARAM = new string[] {};
        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        clsDonnee.pvgConnectionBase();
        //        foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
        //        {
        //            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
        //            clsPlancomptable.PL_CODENUMCOMPTE = clsPlancomptableDTO.PL_CODENUMCOMPTE.ToString();
        //            clsPlancomptable.PL_LIBELLE = clsPlancomptableDTO.PL_LIBELLE.ToString();
        //            clsPlancomptable.PL_NUMCOMPTE =clsPlancomptableDTO.PL_NUMCOMPTE.ToString();
        //            clsPlancomptable.PL_COMPTECOLLECTIF = clsPlancomptableDTO.PL_COMPTECOLLECTIF.ToString();
        //            clsObjetEnvoi.OE_A = clsPlancomptableDTO.clsObjetEnvoi.OE_A;
        //            clsObjetEnvoi.OE_Y = clsPlancomptableDTO.clsObjetEnvoi.OE_Y;
        //            clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgAjouter(clsDonnee, clsPlancomptable, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

        //        }
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //            clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "00";
        //            clsPlancomptable.clsObjetRetour.SL_RESULTAT = "TRUE";
        //            clsPlancomptable.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
        //            clsPlancomptables.Add(clsPlancomptable);
        //        }
        //        else
        //        {
        //            ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //            clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //            clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //            clsPlancomptable.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
        //            clsPlancomptables.Add(clsPlancomptable);
        //        }



        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //        clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsPlancomptable.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        clsPlancomptables.Add(clsPlancomptable);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //        clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsPlancomptable.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        clsPlancomptables.Add(clsPlancomptable);
        //    }

        //    finally
        //    {
        //        clsDonnee.pvgDeConnectionBase();
        //    }
        //    return clsPlancomptables;
        //}

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        //public List<ZenithWebServeur.DTO.clsPlancomptable> pvgModifier(List<ZenithWebServeur.DTO.clsPlancomptable> Objet)
        //{
        //    List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //    List<ZenithWebServeur.DTO.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    {
        //        //--TEST DES CHAMPS OBLIGATOIRES
        //        clsPlancomptables = TestChampObligatoireUpdate(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsPlancomptables[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsPlancomptables;
        //        //--TEST CONTRAINTE
        //        clsPlancomptables = TestTestContrainteListe(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsPlancomptables[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsPlancomptables;
        //    }

        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        clsDonnee.pvgConnectionBase();
        //        foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
        //        {
        //            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
        //            clsPlancomptable.PL_CODENUMCOMPTE = clsPlancomptableDTO.PL_CODENUMCOMPTE.ToString();
        //            clsPlancomptable.PL_LIBELLE = clsPlancomptableDTO.PL_LIBELLE.ToString();
        //            clsPlancomptable.PL_NUMCOMPTE = clsPlancomptableDTO.PL_NUMCOMPTE.ToString();
        //            clsPlancomptable.PL_COMPTECOLLECTIF = clsPlancomptableDTO.PL_COMPTECOLLECTIF.ToString();
        //            clsObjetEnvoi.OE_A = clsPlancomptableDTO.clsObjetEnvoi.OE_A;
        //            clsObjetEnvoi.OE_Y = clsPlancomptableDTO.clsObjetEnvoi.OE_Y;
        //            clsObjetEnvoi.OE_PARAM = new string[] { clsPlancomptableDTO.PL_CODENUMCOMPTE };
        //            clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgModifier(clsDonnee, clsPlancomptable, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

        //        }
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //            clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "00";
        //            clsPlancomptable.clsObjetRetour.SL_RESULTAT = "TRUE";
        //            clsPlancomptable.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
        //            clsPlancomptables.Add(clsPlancomptable);
        //        }
        //        else
        //        {
        //            ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //            clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //            clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //            clsPlancomptable.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
        //            clsPlancomptables.Add(clsPlancomptable);
        //        }



        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //        clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsPlancomptable.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        clsPlancomptables.Add(clsPlancomptable);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //        clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsPlancomptable.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        clsPlancomptables.Add(clsPlancomptable);
        //    }

        //    finally
        //    {
        //        clsDonnee.pvgDeConnectionBase();
        //    }
        //    return clsPlancomptables;
        //}


        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        //public List<ZenithWebServeur.DTO.clsPlancomptable> pvgSupprimer(List<ZenithWebServeur.DTO.clsPlancomptable> Objet)
        //{

        //    List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //    List<ZenithWebServeur.DTO.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


        //    for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    {
        //        //--TEST DES CHAMPS OBLIGATOIRES
        //        clsPlancomptables = TestChampObligatoireDelete(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsPlancomptables[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsPlancomptables;
        //        //--TEST CONTRAINTE
        //        clsPlancomptables = TestTestContrainteListe(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsPlancomptables[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsPlancomptables;
        //    }


        //    clsObjetEnvoi.OE_PARAM = new string[] { Objet[0].PL_CODENUMCOMPTE };
        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();

        //    try
        //    {
        //        clsDonnee.pvgConnectionBase();
        //        clsObjetEnvoi.OE_A = Objet[0].clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet[0].clsObjetEnvoi.OE_Y;
        //        clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "VIT0002").MS_LIBELLEMESSAGE);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //            clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "00";
        //            clsPlancomptable.clsObjetRetour.SL_RESULTAT = "TRUE";
        //            clsPlancomptable.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
        //            clsPlancomptables.Add(clsPlancomptable);
        //        }
        //        else
        //        {
        //            ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //            clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //            clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //            clsPlancomptable.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
        //            clsPlancomptables.Add(clsPlancomptable);
        //        }



        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //        clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsPlancomptable.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        clsPlancomptables.Add(clsPlancomptable);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsPlancomptable clsPlancomptable = new ZenithWebServeur.DTO.clsPlancomptable();
        //        clsPlancomptable.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsPlancomptable.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsPlancomptable.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsPlancomptable.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsPlancomptables = new List<ZenithWebServeur.DTO.clsPlancomptable>();
        //        clsPlancomptables.Add(clsPlancomptable);
        //    }


        //    finally
        //    {
        //        clsDonnee.pvgDeConnectionBase();
        //    }
        //    return clsPlancomptables;
        //}

        //LISTE
        public string pvgChargerDansDataSet(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.SO_CODESOCIETE, Objet.PL_NUMCOMPTE, Objet.PL_TYPECOMPTE };

                //foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                DataSet = clsPlancomptableWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
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
        public string pvgModifierListe(List<clsPlancomptable> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.BOJ.clsPlancomptable>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                DataSet = TestChampObligatoireUpdatepvgModifierListe(Objet[Idx]);
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

                    foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                    {
                        ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();

                        clsPlancomptable.SO_CODESOCIETE = clsPlancomptableDTO.SO_CODESOCIETE.ToString();
                        clsPlancomptable.AG_CODEAGENCE = clsPlancomptableDTO.AG_CODEAGENCE.ToString();
                        clsPlancomptable.PL_CODENUMCOMPTE = clsPlancomptableDTO.PL_CODENUMCOMPTE.ToString();
                        clsPlancomptable.PL_LIBELLE = clsPlancomptableDTO.PL_LIBELLE.ToString();
                        clsPlancomptable.PL_AUTORISEINVERSION = clsPlancomptableDTO.PL_AUTORISEINVERSION.ToString();

                        clsObjetEnvoi.OE_A = clsPlancomptableDTO.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = clsPlancomptableDTO.clsObjetEnvoi.OE_Y;

                        clsPlancomptables.Add(clsPlancomptable);
                    }
                    clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgModifierListe(clsDonnee, clsPlancomptables, clsObjetEnvoi));
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
        public string pvgTableLibelle(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));

            dt.Columns.Add(new DataColumn("PL_CODENUMCOMPTE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_LIBELLE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_COMPTECOLLECTIF", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_REPORTDEBIT", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_REPORTCREDIT", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_MONTANTPERIODEPRECEDENTDEBIT", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_MONTANTPERIODEPRECEDENTCREDIT", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_MONTANTPERIODEDEBITENCOURS", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_MONTANTPERIODECREDITENCOURS", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_MONTANTSOLDEFINALDEBIT", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_MONTANTSOLDEFINALCREDIT", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_SENS", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_TYPECOMPTE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_ACTIF", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_COMPTEREFERENTIELCOMPTABLE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.BOJ.clsPlancomptable>();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireUpdatepvgModifierListe(Objet[Idx]);
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.SO_CODESOCIETE, Objet.PL_NUMCOMPTE
                };
                
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsPlancomptable = clsPlancomptableWSBLL.pvgTableLibelle(clsDonnee, clsObjetEnvoi);
                if (clsPlancomptable != null)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();

                    dr["PL_CODENUMCOMPTE"] = clsPlancomptable.PL_CODENUMCOMPTE.ToString();
                    dr["PL_LIBELLE"] = clsPlancomptable.PL_LIBELLE.ToString();
                    dr["PL_COMPTECOLLECTIF"] = clsPlancomptable.PL_COMPTECOLLECTIF.ToString();
                    dr["PL_REPORTDEBIT"] = clsPlancomptable.PL_REPORTDEBIT.ToString();
                    dr["PL_REPORTCREDIT"] = clsPlancomptable.PL_REPORTCREDIT.ToString();
                    dr["PL_MONTANTPERIODEPRECEDENTDEBIT"] = clsPlancomptable.PL_MONTANTPERIODEPRECEDENTDEBIT.ToString();
                    dr["PL_MONTANTPERIODEPRECEDENTCREDIT"] = clsPlancomptable.PL_MONTANTPERIODEPRECEDENTCREDIT.ToString();
                    dr["PL_MONTANTPERIODEDEBITENCOURS"] = clsPlancomptable.PL_MONTANTPERIODEDEBITENCOURS.ToString();
                    dr["PL_MONTANTPERIODECREDITENCOURS"] = clsPlancomptable.PL_MONTANTPERIODECREDITENCOURS.ToString();
                    dr["PL_MONTANTSOLDEFINALDEBIT"] = clsPlancomptable.PL_MONTANTSOLDEFINALDEBIT.ToString();
                    dr["PL_MONTANTSOLDEFINALCREDIT"] = clsPlancomptable.PL_MONTANTSOLDEFINALCREDIT.ToString();
                    dr["PL_SENS"] = clsPlancomptable.PL_SENS.ToString();
                    dr["PL_TYPECOMPTE"] = clsPlancomptable.PL_TYPECOMPTE.ToString();
                    dr["PL_ACTIF"] = clsPlancomptable.PL_ACTIF.ToString();
                    dr["PL_COMPTEREFERENTIELCOMPTABLE"] = clsPlancomptable.PL_COMPTEREFERENTIELCOMPTABLE.ToString();

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
        public string pvgModifierListeDesactiverCompte(List<clsPlancomptable> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.BOJ.clsPlancomptable>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            for (int Idx = 0; Idx < Objet.Count; Idx++)
            {
                //--TEST DES CHAMPS OBLIGATOIRES
                DataSet = TestChampObligatoireUpdatepvgModifierListe(Objet[Idx]);
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

                foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();

                    clsPlancomptable.SO_CODESOCIETE = clsPlancomptableDTO.SO_CODESOCIETE.ToString();
                    clsPlancomptable.AG_CODEAGENCE = clsPlancomptableDTO.AG_CODEAGENCE.ToString();
                    clsPlancomptable.PL_CODENUMCOMPTE = clsPlancomptableDTO.PL_CODENUMCOMPTE.ToString();
                    clsPlancomptable.PL_ACTIF = clsPlancomptableDTO.PL_ACTIF.ToString();

                    clsObjetEnvoi.OE_A = clsPlancomptableDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsPlancomptableDTO.clsObjetEnvoi.OE_Y;

                    clsPlancomptables.Add(clsPlancomptable);
                }
                clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgModifierListeDesactiverCompte(clsDonnee, clsPlancomptables, clsObjetEnvoi));
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

        public string pvgTableLabelAvecSolde(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_CODENUMCOMPTE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_LIBELLE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_COMPTECOLLECTIF", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_TYPECOMPTE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_ACTIF", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_COMPTETIERS", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_SAISIE_ANALYTIQUE", typeof(string)));
            dt.Columns.Add(new DataColumn("PL_SOLDECOMPTE", typeof(string)));

            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.BOJ.clsPlancomptable>();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireUpdatepvgModifierListe(Objet[Idx]);
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
                clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.SO_CODESOCIETE,
                    Objet.AG_CODEAGENCE,
                    Objet.PL_NUMCOMPTE,
                    Objet.JT_DATEJOURNEETRAVAIL
                };
                
                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsPlancomptable = clsPlancomptableWSBLL.pvgTableLabelAvecSolde(clsDonnee, clsObjetEnvoi);
                if (clsPlancomptable != null)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    
                    dr["PL_CODENUMCOMPTE"] = clsPlancomptable.PL_CODENUMCOMPTE.ToString();
                    dr["PL_LIBELLE"] = clsPlancomptable.PL_LIBELLE.ToString();
                    dr["PL_COMPTECOLLECTIF"] = clsPlancomptable.PL_COMPTECOLLECTIF.ToString();
                    dr["PL_TYPECOMPTE"] = clsPlancomptable.PL_TYPECOMPTE.ToString();
                    dr["PL_ACTIF"] = clsPlancomptable.PL_ACTIF.ToString();
                    dr["PL_COMPTETIERS"] = clsPlancomptable.PL_COMPTETIERS.ToString();
                    dr["PL_SAISIE_ANALYTIQUE"] = clsPlancomptable.PL_SAISIE_ANALYTIQUE.ToString();
                    dr["PL_SOLDECOMPTE"] = clsPlancomptable.PL_SOLDECOMPTE.ToString();
                    
                    dr["SL_CODEMESSAGE"] = "00";
                    dr["SL_RESULTAT"] = "TRUE";
                    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                    dt.Rows.Add(dr);
                    DataSet.Tables.Add(dt);
                    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);

                }
                //clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgTableLabelAvecSolde(clsDonnee, clsObjetEnvoi));
                //if (clsObjetRetour.OR_BOOLEEN)
                //{
                //    DataSet = new DataSet();
                //    DataRow dr = dt.NewRow();
                //    dr["SL_CODEMESSAGE"] = "00";
                //    dr["SL_RESULTAT"] = "TRUE";
                //    dr["OR_OBJET"] = clsObjetRetour.OR_OBJET;
                //    dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                //    dt.Rows.Add(dr);
                //    DataSet.Tables.Add(dt);
                //    json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);
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
        public string pvgChargerDansDataSetCompteAutoriseEnODAvecProduit(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.SO_CODESOCIETE, Objet.PS_CODESOUSPRODUIT, Objet.PL_TYPECOMPTE };

                //foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsPlancomptableWSBLL.pvgChargerDansDataSetCompteAutoriseEnODAvecProduit(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSetListeComptePlanComptableCompteAmodiffier(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.SO_CODESOCIETE, Objet.PL_TYPECOMPTE, Objet.PL_CODENUMCOMPTE, Objet.TYPEECRAN };

                //foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsPlancomptableWSBLL.pvgChargerDansDataSetListeComptePlanComptableCompteAmodiffier(clsDonnee, clsObjetEnvoi);
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
        public string pvgChargerDansDataSetListeComptePlanComptable(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.SO_CODESOCIETE, Objet.PL_TYPECOMPTE, Objet.TYPEECRAN };

                //foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                //{

                //clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                //clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsPlancomptableWSBLL.pvgChargerDansDataSetListeComptePlanComptable(clsDonnee, clsObjetEnvoi);
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

        //NOMBRE DE COMPTE
        public string pvgValeurScalaireRequeteCount(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            List<ZenithWebServeur.BOJ.clsPlancomptable> clsPlancomptables = new List<ZenithWebServeur.BOJ.clsPlancomptable>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
            //    //--TEST DES CHAMPS OBLIGATOIRES
            //    DataSet = TestChampObligatoireUpdatepvgModifierListe(Objet[Idx]);
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

                if (Objet.PL_SENS == "1")
                {
                    clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.SO_CODESOCIETE,
                    Objet.PL_NUMCOMPTE
                };
                } else if (Objet.PL_SENS == "2") {
                    clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.SO_CODESOCIETE,
                    Objet.PL_NUMCOMPTE,
                    Objet.PL_TYPECOMPTE
                };
                } else
                {
                    clsObjetEnvoi.OE_PARAM = new string[] {
                    Objet.SO_CODESOCIETE,
                    Objet.PL_NUMCOMPTE,
                    Objet.PL_TYPECOMPTE,
                    Objet.PL_ACTIF
                };
                }
                

                
                //foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                //{

                    clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;
                
                //}
                clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgValeurScalaireRequeteCount(clsDonnee, clsObjetEnvoi));
                if (clsObjetRetour.OR_BOOLEEN)
                {
                    DataSet = new DataSet();
                    DataRow dr = dt.NewRow();
                    dr["SL_CODEMESSAGE"] = clsObjetRetour.OR_STRING;
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

        //COMBO
        public string pvgChargerDansDataSetPourCombo(clsPlancomptable Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsPlancomptable clsPlancomptable = new ZenithWebServeur.BOJ.clsPlancomptable();
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
                clsObjetEnvoi.OE_PARAM = new string[] { Objet.SO_CODESOCIETE, Objet.PL_NUMCOMPTE, Objet.PL_TYPECOMPTE };

                //foreach (ZenithWebServeur.DTO.clsPlancomptable clsPlancomptableDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsPlancomptableWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsPlancomptableWSBLL.pvgChargerDansDataSetPourCombo(clsDonnee, clsObjetEnvoi);
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
