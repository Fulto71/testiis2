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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsLogiciel" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsLogiciel.svc ou wsLogiciel.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsLogiciel : IwsLogiciel
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsLogicielWSBLL clsLogicielWSBLL = new clsLogicielWSBLL();

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
        //public List<ZenithWebServeur.DTO.clsLogiciel> pvgAjouter(List<ZenithWebServeur.DTO.clsLogiciel> Objet)
        //{
        //    List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //    List<ZenithWebServeur.DTO.clsLogiciel> clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    {
        //        //--TEST DES CHAMPS OBLIGATOIRES
        //        clsLogiciels = TestChampObligatoireInsert(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
        //        //--TEST CONTRAINTE
        //        clsLogiciels = TestTestContrainteListe(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
        //    }
        //    //clsObjetEnvoi.OE_PARAM = new string[] {};
        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        clsDonnee.pvgConnectionBase();
        //        foreach (ZenithWebServeur.DTO.clsLogiciel clsLogicielDTO in Objet)
        //        {
        //            ZenithWebServeur.BOJ.clsLogiciel clsLogiciel = new ZenithWebServeur.BOJ.clsLogiciel();
        //            clsLogiciel.JO_CODELogiciel = clsLogicielDTO.JO_CODELogiciel.ToString();
        //            clsLogiciel.JO_LIBELLE = clsLogicielDTO.JO_LIBELLE.ToString();
        //            clsLogiciel.JO_NUMEROORDRE =int.Parse(clsLogicielDTO.JO_NUMEROORDRE.ToString());
        //            clsObjetEnvoi.OE_A = clsLogicielDTO.clsObjetEnvoi.OE_A;
        //            clsObjetEnvoi.OE_Y = clsLogicielDTO.clsObjetEnvoi.OE_Y;
        //            clsObjetRetour.SetValue(true, clsLogicielWSBLL.pvgAjouter(clsDonnee, clsLogiciel, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

        //        }
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //            clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "00";
        //            clsLogiciel.clsObjetRetour.SL_RESULTAT = "TRUE";
        //            clsLogiciel.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
        //            clsLogiciels.Add(clsLogiciel);
        //        }
        //        else
        //        {
        //            ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //            clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //            clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //            clsLogiciel.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
        //            clsLogiciels.Add(clsLogiciel);
        //        }



        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        clsLogiciels.Add(clsLogiciel);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        clsLogiciels.Add(clsLogiciel);
        //    }

        //    finally
        //    {
        //        clsDonnee.pvgDeConnectionBase();
        //    }
        //    return clsLogiciels;
        //}

        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        //public List<ZenithWebServeur.DTO.clsLogiciel> pvgModifier(List<ZenithWebServeur.DTO.clsLogiciel> Objet)
        //{
        //    List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //    List<ZenithWebServeur.DTO.clsLogiciel> clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

        //    for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    {
        //        //--TEST DES CHAMPS OBLIGATOIRES
        //        clsLogiciels = TestChampObligatoireUpdate(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
        //        //--TEST CONTRAINTE
        //        clsLogiciels = TestTestContrainteListe(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
        //    }
            
        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
        //    try
        //    {
        //        clsDonnee.pvgConnectionBase();
        //        foreach (ZenithWebServeur.DTO.clsLogiciel clsLogicielDTO in Objet)
        //        {
        //            ZenithWebServeur.BOJ.clsLogiciel clsLogiciel = new ZenithWebServeur.BOJ.clsLogiciel();
        //            clsLogiciel.JO_CODELogiciel = clsLogicielDTO.JO_CODELogiciel.ToString();
        //            clsLogiciel.JO_LIBELLE = clsLogicielDTO.JO_LIBELLE.ToString();
        //            clsLogiciel.JO_NUMEROORDRE = int.Parse(clsLogicielDTO.JO_NUMEROORDRE.ToString());
        //            clsObjetEnvoi.OE_A = clsLogicielDTO.clsObjetEnvoi.OE_A;
        //            clsObjetEnvoi.OE_Y = clsLogicielDTO.clsObjetEnvoi.OE_Y;
        //            clsObjetEnvoi.OE_PARAM = new string[] { clsLogicielDTO.JO_CODELogiciel };
        //            clsObjetRetour.SetValue(true, clsLogicielWSBLL.pvgModifier(clsDonnee, clsLogiciel, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "GNE0069").MS_LIBELLEMESSAGE);

        //        }
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //            clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "00";
        //            clsLogiciel.clsObjetRetour.SL_RESULTAT = "TRUE";
        //            clsLogiciel.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
        //            clsLogiciels.Add(clsLogiciel);
        //        }
        //        else
        //        {
        //            ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //            clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //            clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //            clsLogiciel.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
        //            clsLogiciels.Add(clsLogiciel);
        //        }



        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        clsLogiciels.Add(clsLogiciel);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        clsLogiciels.Add(clsLogiciel);
        //    }

        //    finally
        //    {
        //        clsDonnee.pvgDeConnectionBase();
        //    }
        //    return clsLogiciels;
        //}


        ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
        ///<param name="Objet">Collection de clsInput </param>
        ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
        ///<author>Home Technology</author>
        //public List<ZenithWebServeur.DTO.clsLogiciel> pvgSupprimer(List<ZenithWebServeur.DTO.clsLogiciel> Objet)
        //{

        //    List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
        //    List<ZenithWebServeur.DTO.clsLogiciel> clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();

        //    ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
        //    clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
        //    clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
        //    clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
        //    clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


        //    for (int Idx = 0; Idx < Objet.Count; Idx++)
        //    {
        //        //--TEST DES CHAMPS OBLIGATOIRES
        //        clsLogiciels = TestChampObligatoireDelete(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
        //        //--TEST CONTRAINTE
        //        clsLogiciels = TestTestContrainteListe(Objet[Idx]);
        //        //--VERIFICATION DU RESULTAT DU TEST
        //        if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
        //    }


        //    clsObjetEnvoi.OE_PARAM = new string[] { Objet[0].JO_CODELogiciel };
        //    ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();

        //    try
        //    {
        //        clsDonnee.pvgConnectionBase();
        //        clsObjetEnvoi.OE_A = Objet[0].clsObjetEnvoi.OE_A;
        //        clsObjetEnvoi.OE_Y = Objet[0].clsObjetEnvoi.OE_Y;
        //        clsObjetRetour.SetValue(true, clsLogicielWSBLL.pvgSupprimer(clsDonnee, clsObjetEnvoi), clsMessagesWSBLL.pvgTableLibelle(clsDonnee, "VIT0002").MS_LIBELLEMESSAGE);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        if (clsObjetRetour.OR_BOOLEEN)
        //        {
        //            ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //            clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "00";
        //            clsLogiciel.clsObjetRetour.SL_RESULTAT = "TRUE";
        //            clsLogiciel.clsObjetRetour.SL_MESSAGE = "L'opération s'est réalisée avec succès";
        //            clsLogiciels.Add(clsLogiciel);
        //        }
        //        else
        //        {
        //            ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //            clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //            clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //            clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //            clsLogiciel.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé !!!";
        //            clsLogiciels.Add(clsLogiciel);
        //        }



        //    }
        //    catch (SqlException SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        clsLogiciels.Add(clsLogiciel);
        //    }
        //    catch (Exception SQLEx)
        //    {
        //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
        //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
        //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
        //        clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
        //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
        //        //Execution du log
        //        Log.Error(SQLEx.Message, null);
        //        clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
        //        clsLogiciels.Add(clsLogiciel);
        //    }


        //    finally
        //    {
        //        clsDonnee.pvgDeConnectionBase();
        //    }
        //    return clsLogiciels;
        //}


            ///<summary>Cette fonction permet de d'executer une requete SELECT dans la base de donnees </summary>
            ///<param name="Objet">Collection de clsInput </param>
            ///<returns>Une collection de clsInput valeur du résultat de la requete</returns>
            ///<author>Home Technology</author>
            //public List<ZenithWebServeur.DTO.clsLogiciel> pvgChargerDansDataSet(List<ZenithWebServeur.DTO.clsLogiciel> Objet)
            //{

            //List<ZenithWebServeur.DTO.clsObjetRetour> clsObjetRetourDTOs = new List<ZenithWebServeur.DTO.clsObjetRetour>();
            //List<ZenithWebServeur.DTO.clsLogiciel> clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
           
            //ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            //clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            //clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            //clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            //clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;


            ////for (int Idx = 0; Idx < Objet.Count; Idx++)
            ////{
            ////    //--TEST DES CHAMPS OBLIGATOIRES
            ////    clsLogiciels = TestChampObligatoireListe(Objet[Idx]);
            ////    //--VERIFICATION DU RESULTAT DU TEST
            ////    if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
            ////    //--TEST CONTRAINTE
            ////    clsLogiciels = TestTestContrainteListe(Objet[Idx]);
            ////    //--VERIFICATION DU RESULTAT DU TEST
            ////    if (clsLogiciels[0].clsObjetRetour.SL_RESULTAT == "FALSE") return clsLogiciels;
            ////}

      
            //clsObjetEnvoi.OE_PARAM= new string[] {};
            //DataSet DataSet = new DataSet();

            //try
            //{
            //clsDonnee.pvgConnectionBase();
            //DataSet = clsLogicielWSBLL.pvgChargerDansDataSet(clsDonnee, clsObjetEnvoi);
            //clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
            //if (DataSet.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow row in DataSet.Tables[0].Rows)
            //    {
            //        ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
            //        clsLogiciel.JO_CODELogiciel = row["JO_CODELogiciel"].ToString();
            //        clsLogiciel.JO_LIBELLE = row["JO_LIBELLE"].ToString();
            //        clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
            //        clsLogiciel.clsObjetRetour.SL_CODEMESSAGE ="00";
            //        clsLogiciel.clsObjetRetour.SL_RESULTAT = "TRUE";
            //        clsLogiciel.clsObjetRetour.SL_MESSAGE ="L'opération s'est réalisée avec succès";
            //        clsLogiciels.Add(clsLogiciel);
            //    }
            //}
            //else
            //{
            //    ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
            //    clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
            //    clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
            //    clsLogiciel.clsObjetRetour.SL_RESULTAT = "FALSE";
            //    clsLogiciel.clsObjetRetour.SL_MESSAGE = "Aucun enregistrement n'a été trouvé";
            //    clsLogiciels.Add(clsLogiciel);
            //}
                


            //}
            //catch (SqlException SQLEx)
            //{
            //ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
            //clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
            //clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
            //clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
            //clsLogiciel.clsObjetRetour.SL_RESULTAT ="FALSE";
            ////Execution du log
            //Log.Error(SQLEx.Message, null);
            //clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
            //clsLogiciels.Add(clsLogiciel);
            //}
            //catch (Exception SQLEx)
            //{
            //ZenithWebServeur.DTO.clsLogiciel clsLogiciel = new ZenithWebServeur.DTO.clsLogiciel();
            //clsLogiciel.clsObjetRetour = new ZenithWebServeur.Common.clsObjetRetour();
            //clsLogiciel.clsObjetRetour.SL_CODEMESSAGE = "99";
            //clsLogiciel.clsObjetRetour.SL_MESSAGE = SQLEx.Message;
            //clsLogiciel.clsObjetRetour.SL_RESULTAT ="FALSE";
            ////Execution du log
            //Log.Error(SQLEx.Message, null);
            //clsLogiciels = new List<ZenithWebServeur.DTO.clsLogiciel>();
            //clsLogiciels.Add(clsLogiciel);
            //}


            //finally
            //{
            //clsDonnee.pvgDeConnectionBase();
            //}
            //return clsLogiciels;
            //}
            
        //COMBO
        public string pvgChargerDansDataSetPourCombo(clsLogiciel Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsLogiciel clsLogiciel = new ZenithWebServeur.BOJ.clsLogiciel();
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

                //foreach (ZenithWebServeur.DTO.clsLogiciel clsLogicielDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsLogicielWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsLogicielWSBLL.pvgChargerDansDataSetPourCombo(clsDonnee, clsObjetEnvoi);
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
