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
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "wsMiceffetcheque" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez wsMiceffetcheque.svc ou wsMiceffetcheque.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public partial class wsMiceffetcheque : IwsMiceffetcheque
    {
        private clsDonnee _clsDonnee = new clsDonnee();
        private clsMessagesWSBLL clsMessagesWSBLL = new clsMessagesWSBLL();
        private clsMiceffetchequeWSBLL clsMiceffetchequeWSBLL = new clsMiceffetchequeWSBLL();

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
        public string pvgAjouterChequeEffet(List<clsMiceffetcheque> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
            List<ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique> clsMiceffetchequecaracteristiques = new List<ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
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
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique clsMiceffetchequecaracteristique = new ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique();

                    // objet
                    clsMiceffetcheque.EC_CODEEFFETCHEQUE = clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE.ToString();
                    clsMiceffetcheque.AG_CODEAGENCE = clsMiceffetchequeDTO.AG_CODEAGENCE.ToString();
                    clsMiceffetcheque.PV_CODEPOINTVENTE = clsMiceffetchequeDTO.PV_CODEPOINTVENTE.ToString();
                    clsMiceffetcheque.CO_CODECOMPTE = clsMiceffetchequeDTO.CO_CODECOMPTE.ToString();
                    clsMiceffetcheque.AB_CODEAGENCEBANCAIRE = clsMiceffetchequeDTO.AB_CODEAGENCEBANCAIRE.ToString();
                    clsMiceffetcheque.EC_BANQUEAGENCEDUTIREUR = clsMiceffetchequeDTO.EC_BANQUEAGENCEDUTIREUR.ToString();
                    clsMiceffetcheque.EC_COMPTEBANCAIRE = clsMiceffetchequeDTO.EC_COMPTEBANCAIRE.ToString();
                    clsMiceffetcheque.ET_CODETYPE = clsMiceffetchequeDTO.ET_CODETYPE.ToString();
                    clsMiceffetcheque.BB_CODEBANCABLE = clsMiceffetchequeDTO.BB_CODEBANCABLE.ToString();
                    clsMiceffetcheque.EP_CODEEMPLACEMENT = clsMiceffetchequeDTO.EP_CODEEMPLACEMENT.ToString();
                    clsMiceffetcheque.ZT_CODEZONE = clsMiceffetchequeDTO.ZT_CODEZONE.ToString();
                    clsMiceffetcheque.EC_CPTEBANQUE = clsMiceffetchequeDTO.EC_CPTEBANQUE.ToString();
                    clsMiceffetcheque.EC_MONTANT = Double.Parse(clsMiceffetchequeDTO.EC_MONTANT.ToString());
                    clsMiceffetcheque.EC_DATEEMISSIONCHEQUE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEEMISSIONCHEQUE.ToString());
                    clsMiceffetcheque.EC_DATERECEPTION = DateTime.Parse(clsMiceffetchequeDTO.EC_DATERECEPTION.ToString());
                    clsMiceffetcheque.EC_DATEDEPOTBANQUE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEPOTBANQUE.ToString());
                    clsMiceffetcheque.EC_DATEDEPOTBANQUECONFIRMATION = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEPOTBANQUECONFIRMATION.ToString());
                    clsMiceffetcheque.EC_DATEENCAISSEMENT = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEENCAISSEMENT.ToString());
                    clsMiceffetcheque.EC_DUREETHEORIQUEENCAISSEMENT = int.Parse(clsMiceffetchequeDTO.EC_DUREETHEORIQUEENCAISSEMENT.ToString());
                    clsMiceffetcheque.EC_DATEEIMPAYE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEEIMPAYE.ToString());
                    clsMiceffetcheque.EC_NUMEROVALEUR = clsMiceffetchequeDTO.EC_NUMEROVALEUR.ToString();
                    clsMiceffetcheque.EC_TIREUR = clsMiceffetchequeDTO.EC_TIREUR.ToString();
                    clsMiceffetcheque.EC_TIRE = clsMiceffetchequeDTO.EC_TIRE.ToString();
                    clsMiceffetcheque.EC_NOMDEPOSANT = clsMiceffetchequeDTO.EC_NOMDEPOSANT.ToString();
                    clsMiceffetcheque.EC_TELEPHONEDEPOSANT = clsMiceffetchequeDTO.EC_TELEPHONEDEPOSANT.ToString();
                    clsMiceffetcheque.EC_AUTRESINFORMATIONS = clsMiceffetchequeDTO.EC_AUTRESINFORMATIONS.ToString();
                    clsMiceffetcheque.EC_ESCOMPTE = clsMiceffetchequeDTO.EC_ESCOMPTE.ToString();
                    clsMiceffetcheque.EC_DATEDEBUTVALEUR = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEBUTVALEUR.ToString());
                    clsMiceffetcheque.EC_DATEFINVALEUR = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEFINVALEUR.ToString());
                    clsMiceffetcheque.OP_CODEOPERATEUR = clsMiceffetchequeDTO.OP_CODEOPERATEUR.ToString();
                    clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = clsMiceffetchequeDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMiceffetcheque.TYPEOPERATION = clsMiceffetchequeDTO.TYPEOPERATION.ToString();
                    clsMiceffetcheque.DATEJOURNEE = DateTime.Parse(clsMiceffetchequeDTO.DATEJOURNEE.ToString());
                    clsMiceffetcheque.MC_DATEPIECECONFIRMATION = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEPIECECONFIRMATION.ToString());
                    clsMiceffetcheque.MC_DATEPIECEENCAISSEMENT = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEPIECEENCAISSEMENT.ToString());
                    clsMiceffetcheque.MC_DATEANNULATIONENCAISSEMENT = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEANNULATIONENCAISSEMENT.ToString());
                    clsMiceffetcheque.MC_DATEANNULATIONCONFIRMATION = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEANNULATIONCONFIRMATION.ToString());
                    clsMiceffetcheque.MC_NUMPIECECONFIRMATION = clsMiceffetchequeDTO.MC_NUMPIECECONFIRMATION.ToString();
                    clsMiceffetcheque.MC_NUMPIECEENCAISSEMENT = clsMiceffetchequeDTO.MC_NUMPIECEENCAISSEMENT.ToString();

                    // liste
                    clsMiceffetchequecaracteristique.AG_CODEAGENCE = clsMiceffetchequeDTO.AG_CODEAGENCE;
                    clsMiceffetchequecaracteristique.EC_CODEEFFETCHEQUE = clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE;
                    clsMiceffetchequecaracteristique.PL_CODEPARAMETRELISTE = clsMiceffetchequeDTO.PL_CODEPARAMETRELISTE;
                    clsMiceffetchequecaracteristique.EE_VALEUR = Double.Parse(clsMiceffetchequeDTO.EE_VALEUR);

                    clsObjetEnvoi.OE_A = clsMiceffetchequeDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMiceffetchequeDTO.clsObjetEnvoi.OE_Y;

                    clsMiceffetchequecaracteristiques.Add(clsMiceffetchequecaracteristique);
                }
                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgAjouterChequeEffet(clsDonnee, clsMiceffetcheque, clsMiceffetchequecaracteristiques, clsObjetEnvoi));
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
        public string pvgAjouterChequeEffet1(List<clsMiceffetcheque> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
            List<ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique> clsMiceffetchequecaracteristiques = new List<ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
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
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();
                clsObjetEnvoi.OE_PARAM = new string[] { };

                foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                {
                    ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique clsMiceffetchequecaracteristique = new ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique();

                    // objet
                    clsMiceffetcheque.EC_CODEEFFETCHEQUE = clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE.ToString();//
                    clsMiceffetcheque.AG_CODEAGENCE = clsMiceffetchequeDTO.AG_CODEAGENCE.ToString();//
                    clsMiceffetcheque.PV_CODEPOINTVENTE = clsMiceffetchequeDTO.PV_CODEPOINTVENTE.ToString();//
                    clsMiceffetcheque.CO_CODECOMPTE = clsMiceffetchequeDTO.CO_CODECOMPTE.ToString();//
                    clsMiceffetcheque.AB_CODEAGENCEBANCAIRE = clsMiceffetchequeDTO.AB_CODEAGENCEBANCAIRE.ToString();//
                    clsMiceffetcheque.EC_COMPTEBANCAIRE = clsMiceffetchequeDTO.EC_COMPTEBANCAIRE.ToString();//
                    clsMiceffetcheque.ET_CODETYPE = clsMiceffetchequeDTO.ET_CODETYPE.ToString();//
                    clsMiceffetcheque.BB_CODEBANCABLE = clsMiceffetchequeDTO.BB_CODEBANCABLE.ToString();//
                    clsMiceffetcheque.EP_CODEEMPLACEMENT = clsMiceffetchequeDTO.EP_CODEEMPLACEMENT.ToString();//
                    clsMiceffetcheque.ZT_CODEZONE = clsMiceffetchequeDTO.ZT_CODEZONE.ToString();//
                    clsMiceffetcheque.EC_MONTANT = Double.Parse(clsMiceffetchequeDTO.EC_MONTANT.ToString());//
                    clsMiceffetcheque.EC_DATEEMISSIONCHEQUE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEEMISSIONCHEQUE.ToString());//
                    clsMiceffetcheque.EC_DATERECEPTION = DateTime.Parse(clsMiceffetchequeDTO.EC_DATERECEPTION.ToString());//
                    clsMiceffetcheque.EC_DUREETHEORIQUEENCAISSEMENT = int.Parse(clsMiceffetchequeDTO.EC_DUREETHEORIQUEENCAISSEMENT.ToString());//
                    clsMiceffetcheque.EC_NUMEROVALEUR = clsMiceffetchequeDTO.EC_NUMEROVALEUR.ToString();//
                    clsMiceffetcheque.EC_TIREUR = clsMiceffetchequeDTO.EC_TIREUR.ToString();//
                    clsMiceffetcheque.EC_TIRE = clsMiceffetchequeDTO.EC_TIRE.ToString();//
                    clsMiceffetcheque.EC_NOMDEPOSANT = clsMiceffetchequeDTO.EC_NOMDEPOSANT.ToString();//
                    clsMiceffetcheque.EC_TELEPHONEDEPOSANT = clsMiceffetchequeDTO.EC_TELEPHONEDEPOSANT.ToString();//
                    clsMiceffetcheque.EC_ESCOMPTE = clsMiceffetchequeDTO.EC_ESCOMPTE.ToString();//
                    clsMiceffetcheque.EC_DATEDEBUTVALEUR = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEBUTVALEUR.ToString());//
                    clsMiceffetcheque.EC_DATEFINVALEUR = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEFINVALEUR.ToString());//
                    clsMiceffetcheque.OP_CODEOPERATEUR = clsMiceffetchequeDTO.OP_CODEOPERATEUR.ToString();
                    clsMiceffetcheque.TYPEOPERATION = clsMiceffetchequeDTO.TYPEOPERATION.ToString();//
                    clsMiceffetcheque.EC_AUTRESINFORMATIONS = clsMiceffetchequeDTO.EC_AUTRESINFORMATIONS.ToString();//
                    
                    if (Double.Parse(clsMiceffetchequeDTO.EE_VALEUR) > 0)
                    {
                        clsMiceffetchequecaracteristique.AG_CODEAGENCE = clsMiceffetchequeDTO.AG_CODEAGENCE;
                        clsMiceffetchequecaracteristique.EC_CODEEFFETCHEQUE = clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE;
                        clsMiceffetchequecaracteristique.PL_CODEPARAMETRELISTE = clsMiceffetchequeDTO.PL_CODEPARAMETRELISTE;
                        clsMiceffetchequecaracteristique.EE_VALEUR = Double.Parse(clsMiceffetchequeDTO.EE_VALEUR);
                        clsMiceffetchequecaracteristiques.Add(clsMiceffetchequecaracteristique);
                    }


                    clsObjetEnvoi.OE_A = clsMiceffetchequeDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMiceffetchequeDTO.clsObjetEnvoi.OE_Y;


                }
                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgAjouterChequeEffet(clsDonnee, clsMiceffetcheque, clsMiceffetchequecaracteristiques, clsObjetEnvoi));
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
        public string pvgModifierChequeEffet(List<clsMiceffetcheque> Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
            List<ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique> clsMiceffetchequecaracteristiques = new List<ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique>();
            clsObjetEnvoi.OE_D = ConfigurationManager.AppSettings["OE_D"];
            clsObjetEnvoi.OE_X = ConfigurationManager.AppSettings["OE_X"];
            clsDonnee.vogCleCryptage = clsObjetEnvoi.OE_D;
            clsDonnee.vogUtilisateur = clsObjetEnvoi.OE_X;

            //for (int Idx = 0; Idx < Objet.Count; Idx++)
            //{
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
            //}

            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                //clsDonnee.pvgConnectionBase();
                clsDonnee.pvgDemarrerTransaction();

                foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                {
                    clsObjetEnvoi.OE_PARAM = new string[] { clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE };
                    ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique clsMiceffetchequecaracteristique = new ZenithWebServeur.BOJ.clsMiceffetchequecaracteristique();

                    // objet
                    clsMiceffetcheque.EC_CODEEFFETCHEQUE = clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE.ToString();
                    clsMiceffetcheque.AG_CODEAGENCE = clsMiceffetchequeDTO.AG_CODEAGENCE.ToString();
                    clsMiceffetcheque.PV_CODEPOINTVENTE = clsMiceffetchequeDTO.PV_CODEPOINTVENTE.ToString();
                    clsMiceffetcheque.CO_CODECOMPTE = clsMiceffetchequeDTO.CO_CODECOMPTE.ToString();
                    clsMiceffetcheque.AB_CODEAGENCEBANCAIRE = clsMiceffetchequeDTO.AB_CODEAGENCEBANCAIRE.ToString();
                    clsMiceffetcheque.EC_BANQUEAGENCEDUTIREUR = clsMiceffetchequeDTO.EC_BANQUEAGENCEDUTIREUR.ToString();
                    clsMiceffetcheque.EC_COMPTEBANCAIRE = clsMiceffetchequeDTO.EC_COMPTEBANCAIRE.ToString();
                    clsMiceffetcheque.ET_CODETYPE = clsMiceffetchequeDTO.ET_CODETYPE.ToString();
                    clsMiceffetcheque.BB_CODEBANCABLE = clsMiceffetchequeDTO.BB_CODEBANCABLE.ToString();
                    clsMiceffetcheque.EP_CODEEMPLACEMENT = clsMiceffetchequeDTO.EP_CODEEMPLACEMENT.ToString();
                    clsMiceffetcheque.ZT_CODEZONE = clsMiceffetchequeDTO.ZT_CODEZONE.ToString();
                    clsMiceffetcheque.EC_CPTEBANQUE = clsMiceffetchequeDTO.EC_CPTEBANQUE.ToString();
                    clsMiceffetcheque.EC_MONTANT = Double.Parse(clsMiceffetchequeDTO.EC_MONTANT.ToString());
                    clsMiceffetcheque.EC_DATEEMISSIONCHEQUE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEEMISSIONCHEQUE.ToString());
                    clsMiceffetcheque.EC_DATERECEPTION = DateTime.Parse(clsMiceffetchequeDTO.EC_DATERECEPTION.ToString());
                    clsMiceffetcheque.EC_DATEDEPOTBANQUE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEPOTBANQUE.ToString());
                    clsMiceffetcheque.EC_DATEDEPOTBANQUECONFIRMATION = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEPOTBANQUECONFIRMATION.ToString());
                    clsMiceffetcheque.EC_DATEENCAISSEMENT = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEENCAISSEMENT.ToString());
                    clsMiceffetcheque.EC_DUREETHEORIQUEENCAISSEMENT = int.Parse(clsMiceffetchequeDTO.EC_DUREETHEORIQUEENCAISSEMENT.ToString());
                    clsMiceffetcheque.EC_DATEEIMPAYE = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEEIMPAYE.ToString());
                    clsMiceffetcheque.EC_NUMEROVALEUR = clsMiceffetchequeDTO.EC_NUMEROVALEUR.ToString();
                    clsMiceffetcheque.EC_TIREUR = clsMiceffetchequeDTO.EC_TIREUR.ToString();
                    clsMiceffetcheque.EC_TIRE = clsMiceffetchequeDTO.EC_TIRE.ToString();
                    clsMiceffetcheque.EC_NOMDEPOSANT = clsMiceffetchequeDTO.EC_NOMDEPOSANT.ToString();
                    clsMiceffetcheque.EC_TELEPHONEDEPOSANT = clsMiceffetchequeDTO.EC_TELEPHONEDEPOSANT.ToString();
                    clsMiceffetcheque.EC_AUTRESINFORMATIONS = clsMiceffetchequeDTO.EC_AUTRESINFORMATIONS.ToString();
                    clsMiceffetcheque.EC_ESCOMPTE = clsMiceffetchequeDTO.EC_ESCOMPTE.ToString();
                    clsMiceffetcheque.EC_DATEDEBUTVALEUR = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEDEBUTVALEUR.ToString());
                    clsMiceffetcheque.EC_DATEFINVALEUR = DateTime.Parse(clsMiceffetchequeDTO.EC_DATEFINVALEUR.ToString());
                    clsMiceffetcheque.OP_CODEOPERATEUR = clsMiceffetchequeDTO.OP_CODEOPERATEUR.ToString();
                    clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = clsMiceffetchequeDTO.TS_CODETYPESCHEMACOMPTABLE.ToString();
                    clsMiceffetcheque.TYPEOPERATION = clsMiceffetchequeDTO.TYPEOPERATION.ToString();
                    clsMiceffetcheque.DATEJOURNEE = DateTime.Parse(clsMiceffetchequeDTO.DATEJOURNEE.ToString());
                    clsMiceffetcheque.MC_DATEPIECECONFIRMATION = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEPIECECONFIRMATION.ToString());
                    clsMiceffetcheque.MC_DATEPIECEENCAISSEMENT = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEPIECEENCAISSEMENT.ToString());
                    clsMiceffetcheque.MC_DATEANNULATIONENCAISSEMENT = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEANNULATIONENCAISSEMENT.ToString());
                    clsMiceffetcheque.MC_DATEANNULATIONCONFIRMATION = DateTime.Parse(clsMiceffetchequeDTO.MC_DATEANNULATIONCONFIRMATION.ToString());
                    clsMiceffetcheque.MC_NUMPIECECONFIRMATION = clsMiceffetchequeDTO.MC_NUMPIECECONFIRMATION.ToString();
                    clsMiceffetcheque.MC_NUMPIECEENCAISSEMENT = clsMiceffetchequeDTO.MC_NUMPIECEENCAISSEMENT.ToString();

                    // liste
                    clsMiceffetchequecaracteristique.AG_CODEAGENCE = clsMiceffetchequeDTO.AG_CODEAGENCE;
                    clsMiceffetchequecaracteristique.EC_CODEEFFETCHEQUE = clsMiceffetchequeDTO.EC_CODEEFFETCHEQUE;
                    clsMiceffetchequecaracteristique.PL_CODEPARAMETRELISTE = clsMiceffetchequeDTO.PL_CODEPARAMETRELISTE;
                    clsMiceffetchequecaracteristique.EE_VALEUR = Double.Parse(clsMiceffetchequeDTO.EE_VALEUR);

                    clsObjetEnvoi.OE_A = clsMiceffetchequeDTO.clsObjetEnvoi.OE_A;
                    clsObjetEnvoi.OE_Y = clsMiceffetchequeDTO.clsObjetEnvoi.OE_Y;

                    clsMiceffetchequecaracteristiques.Add(clsMiceffetchequecaracteristique);
                }
                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgModifierChequeEffet(clsDonnee, clsMiceffetcheque, clsMiceffetchequecaracteristiques, clsObjetEnvoi));
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

        //SUPPRESSION
        public string pvgExecuterOperation(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsMiceffetcheque.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();
                clsMiceffetcheque.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();
                clsMiceffetcheque.EC_CODEEFFETCHEQUE = Objet.EC_CODEEFFETCHEQUE.ToString();
                clsMiceffetcheque.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();
                clsMiceffetcheque.EC_NUMEROVALEUR = Objet.EC_NUMEROVALEUR.ToString();
                clsMiceffetcheque.AB_CODEAGENCEBANCAIRE = Objet.AB_CODEAGENCEBANCAIRE.ToString();
                clsMiceffetcheque.EC_COMPTEBANCAIRE = Objet.EC_COMPTEBANCAIRE.ToString();
                clsMiceffetcheque.EC_MONTANT = Double.Parse(Objet.EC_MONTANT.ToString());
                clsMiceffetcheque.EC_TIREUR = Objet.EC_TIREUR.ToString();
                clsMiceffetcheque.EC_TIRE = Objet.EC_TIRE.ToString();
                clsMiceffetcheque.EC_NOMDEPOSANT = Objet.EC_NOMDEPOSANT.ToString();
                clsMiceffetcheque.EC_TELEPHONEDEPOSANT = Objet.EC_TELEPHONEDEPOSANT.ToString();
                clsMiceffetcheque.EC_DATERECEPTION = DateTime.Parse(Objet.EC_DATERECEPTION.ToString());
                clsMiceffetcheque.EC_DATEEMISSIONCHEQUE = DateTime.Parse(Objet.EC_DATEEMISSIONCHEQUE.ToString());
                clsMiceffetcheque.EC_DATEDEBUTVALEUR = DateTime.Parse(Objet.EC_DATEDEBUTVALEUR.ToString());
                clsMiceffetcheque.EC_DATEFINVALEUR = DateTime.Parse(Objet.EC_DATEFINVALEUR.ToString());
                clsMiceffetcheque.EC_AUTRESINFORMATIONS = Objet.EC_AUTRESINFORMATIONS.ToString();
                clsMiceffetcheque.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMiceffetcheque.TYPEOPERATION = Objet.TYPEOPERATION.ToString();
                clsMiceffetcheque.EC_ESCOMPTE = Objet.EC_ESCOMPTE.ToString();
                clsMiceffetcheque.EP_CODEEMPLACEMENT = Objet.EP_CODEEMPLACEMENT.ToString();
                clsMiceffetcheque.ET_CODETYPE = Objet.ET_CODETYPE.ToString();
                clsMiceffetcheque.BB_CODEBANCABLE = Objet.BB_CODEBANCABLE.ToString();
                clsMiceffetcheque.EC_DUREETHEORIQUEENCAISSEMENT = int.Parse(Objet.EC_DUREETHEORIQUEENCAISSEMENT.ToString());
                clsMiceffetcheque.ZT_CODEZONE = Objet.ZT_CODEZONE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgExecuterOperation(clsDonnee, clsMiceffetcheque, clsObjetEnvoi));
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
        public string pvgExecuterOperation1(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsMiceffetcheque.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();//
                clsMiceffetcheque.EC_CODEEFFETCHEQUE = Objet.EC_CODEEFFETCHEQUE.ToString();//
                clsMiceffetcheque.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();//
                clsMiceffetcheque.TYPEOPERATION = "3";
                clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsMiceffetcheque.EC_BANQUEAGENCEDUTIREUR = Objet.EC_BANQUEAGENCEDUTIREUR.ToString();
                clsMiceffetcheque.EC_DATEDEPOTBANQUE = DateTime.Parse(Objet.EC_DATEDEPOTBANQUE.ToString());
                clsMiceffetcheque.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR;

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgExecuterOperation(clsDonnee, clsMiceffetcheque, clsObjetEnvoi));
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
        public string pvgExecuterOperation2(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsMiceffetcheque.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();//
                clsMiceffetcheque.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();//
                clsMiceffetcheque.EC_CODEEFFETCHEQUE = Objet.EC_CODEEFFETCHEQUE.ToString();//
                clsMiceffetcheque.TYPEOPERATION = "4";
                clsMiceffetcheque.EC_CPTEBANQUE = Objet.EC_CPTEBANQUE.ToString();//
                clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();
                clsMiceffetcheque.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();//
                clsMiceffetcheque.AB_CODEAGENCEBANCAIRE = Objet.AB_CODEAGENCEBANCAIRE.ToString();//
                clsMiceffetcheque.EC_ESCOMPTE = Objet.EC_ESCOMPTE.ToString();//
                clsMiceffetcheque.EC_DATEDEPOTBANQUECONFIRMATION = DateTime.Parse(Objet.EC_DATEDEPOTBANQUECONFIRMATION.ToString());
                clsMiceffetcheque.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR;
                clsMiceffetcheque.DATEJOURNEE = DateTime.Parse(Objet.DATEJOURNEE.ToString());

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgExecuterOperation(clsDonnee, clsMiceffetcheque, clsObjetEnvoi));
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
        public string pvgExecuterOperation3(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsMiceffetcheque.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();//
                clsMiceffetcheque.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();//
                clsMiceffetcheque.EC_CODEEFFETCHEQUE = Objet.EC_CODEEFFETCHEQUE.ToString();//
                clsMiceffetcheque.TYPEOPERATION = Objet.TYPEOPERATION.ToString();//
                clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();//
                clsMiceffetcheque.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();//
                clsMiceffetcheque.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();
                clsMiceffetcheque.DATEJOURNEE = DateTime.Parse(Objet.DATEJOURNEE.ToString());
                clsMiceffetcheque.EM_NOMOBJET = Objet.EM_NOMOBJET.ToString();
                clsMiceffetcheque.EC_DATEENCAISSEMENT = DateTime.Parse(Objet.EC_DATEENCAISSEMENT.ToString());
                clsMiceffetcheque.EC_DATEEIMPAYE = DateTime.Parse(Objet.EC_DATEEIMPAYE.ToString());
                clsMiceffetcheque.EC_MONTANT = double.Parse(Objet.EC_MONTANT.ToString());

                //Traitement des données de gestion des sms
                clsMiceffetcheque.CL_IDCLIENT = Objet.CL_IDCLIENT.ToString();
                clsMiceffetcheque.CL_TELEPHONE = Objet.CL_TELEPHONE.ToString();
                clsMiceffetcheque.PV_RAISONSOCIAL = Objet.PV_RAISONSOCIAL.ToString();
                clsMiceffetcheque.NUMEROCOMPTE = Objet.NUMEROCOMPTE.ToString();

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgExecuterOperation(clsDonnee, clsMiceffetcheque, clsObjetEnvoi));
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

        public string pvgExecuterOperation4(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsMiceffetcheque.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();//
                clsMiceffetcheque.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();//
                clsMiceffetcheque.EC_CODEEFFETCHEQUE = Objet.EC_CODEEFFETCHEQUE.ToString();//
                clsMiceffetcheque.TYPEOPERATION = Objet.TYPEOPERATION.ToString();//
                clsMiceffetcheque.EC_CPTEBANQUE = Objet.EC_CPTEBANQUE.ToString();//
                clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();//
                clsMiceffetcheque.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();//
                clsMiceffetcheque.EC_ESCOMPTE = Objet.EC_ESCOMPTE.ToString();//
                clsMiceffetcheque.EC_DATEDEPOTBANQUE = DateTime.Parse(Objet.EC_DATEDEPOTBANQUE.ToString());//
                clsMiceffetcheque.DATEJOURNEE = DateTime.Parse(Objet.DATEJOURNEE.ToString());//
                clsMiceffetcheque.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();//

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgExecuterOperation(clsDonnee, clsMiceffetcheque, clsObjetEnvoi));
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



        public string pvgExecuterOperation5(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                clsObjetEnvoi.OE_PARAM = new string[] { };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsMiceffetcheque.AG_CODEAGENCE = Objet.AG_CODEAGENCE.ToString();//
                clsMiceffetcheque.PV_CODEPOINTVENTE = Objet.PV_CODEPOINTVENTE.ToString();//
                clsMiceffetcheque.EC_CODEEFFETCHEQUE = Objet.EC_CODEEFFETCHEQUE.ToString();//
                clsMiceffetcheque.TYPEOPERATION = Objet.TYPEOPERATION.ToString();//
                clsMiceffetcheque.EC_CPTEBANQUE = Objet.EC_CPTEBANQUE.ToString();//
                clsMiceffetcheque.TS_CODETYPESCHEMACOMPTABLE = Objet.TS_CODETYPESCHEMACOMPTABLE.ToString();//
                clsMiceffetcheque.CO_CODECOMPTE = Objet.CO_CODECOMPTE.ToString();//
                clsMiceffetcheque.EC_ESCOMPTE = Objet.EC_ESCOMPTE.ToString();//
                clsMiceffetcheque.EC_DATEDEPOTBANQUE = DateTime.Parse(Objet.EC_DATEDEPOTBANQUE.ToString());//
                clsMiceffetcheque.DATEJOURNEE = DateTime.Parse(Objet.DATEJOURNEE.ToString());//
                clsMiceffetcheque.OP_CODEOPERATEUR = Objet.OP_CODEOPERATEUR.ToString();//

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgExecuterOperation(clsDonnee, clsMiceffetcheque, clsObjetEnvoi));
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
        public string pvgChargerDansDataSetDepot(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                        Objet.CL_CODECLIENT,
                        Objet.CL_NOMCLIENT,
                        Objet.CL_PRENOMCLIENT,
                        Objet.EC_NUMEROVALEUR,
                        Objet.EC_DATERECEPTION1,
                        Objet.EC_DATERECEPTION2,
                        Objet.EC_BANQUEDUTIREUR,
                        Objet.EC_BANQUEAGENCEDUTIREUR,
                        Objet.EC_TIREUR
                };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiceffetchequeWSBLL.pvgChargerDansDataSetDepot(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string)));
                    for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
                    {
                        DataSet.Tables[0].Rows[i]["SL_CODEMESSAGE"] = "00";
                        DataSet.Tables[0].Rows[i]["SL_RESULTAT"] = "TRUE";
                        DataSet.Tables[0].Rows[i]["CO_CODECOMPTE2"] = DataSet.Tables[0].Rows[i]["CO_CODECOMPTE"];
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
        public string pvgChargerDansDataSetDepotConfirmation(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                        Objet.CL_CODECLIENT,
                        Objet.CL_NOMCLIENT,
                        Objet.CL_PRENOMCLIENT,
                        Objet.EC_NUMEROVALEUR,
                        Objet.EC_DATERECEPTION1,
                        Objet.EC_DATERECEPTION2,
                        Objet.EC_BANQUEDUTIREUR,
                        Objet.EC_BANQUEAGENCEDUTIREUR,
                        Objet.EC_TIREUR
                };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiceffetchequeWSBLL.pvgChargerDansDataSetDepotConfirmation(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string)));
                    for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
                    {
                        DataSet.Tables[0].Rows[i]["SL_CODEMESSAGE"] = "00";
                        DataSet.Tables[0].Rows[i]["SL_RESULTAT"] = "TRUE";
                        DataSet.Tables[0].Rows[i]["CO_CODECOMPTE2"] = DataSet.Tables[0].Rows[i]["CO_CODECOMPTE"];
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
        public string pvgDatetheoriqueEcaissement(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("DATETHEORIQUE", typeof(string)));
            string json = "";



            ZenithWebServeur.DTO.clsObjetRetour clsObjetRetour = new ZenithWebServeur.DTO.clsObjetRetour();
            try
            {
                var DATE = "01/01/1900";
                DATE = DateTime.Parse(Objet.DATEENCAIS).AddDays(double.Parse(Objet.DUREE)).ToShortDateString();

                DataSet = new DataSet();
                DataRow dr = dt.NewRow();
                dr["SL_CODEMESSAGE"] = "00";
                dr["SL_RESULTAT"] = "TRUE";
                dr["SL_MESSAGE"] = "L'opération s'est réalisée avec succès";
                dr["DATETHEORIQUE"] = DATE;
                dt.Rows.Add(dr);
                DataSet.Tables.Add(dt);
                json = JsonConvert.SerializeObject(DataSet, Formatting.Indented);

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
        public string pvgChargerDansDataSetRemise(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                        Objet.CL_CODECLIENT,
                        Objet.CL_NOMCLIENT,
                        Objet.CL_PRENOMCLIENT,
                        Objet.EC_NUMEROVALEUR,
                        Objet.EC_DATEDEPOTBANQUE1,
                        Objet.EC_DATEDEPOTBANQUE2,
                        Objet.BQ_CODEBANQUE,
                        Objet.AB_CODEAGENCEBANCAIRE,
                        Objet.EC_TIREUR,
                };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiceffetchequeWSBLL.pvgChargerDansDataSetRemise(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    DataSet.Tables[0].Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string))); 
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
                    for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
                    {
                        DataSet.Tables[0].Rows[i]["CO_CODECOMPTE2"] = DataSet.Tables[0].Rows[i]["CO_CODECOMPTE"].ToString(); 
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
        public string pvgChargerDansDataSetConfirmationaAnnuler(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string))); 
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                        Objet.CL_CODECLIENT,
                        Objet.CL_NOMCLIENT,
                        Objet.CL_PRENOMCLIENT,
                        Objet.EC_NUMEROVALEUR,
                        Objet.EC_DATEDEPOTBANQUECONFIRMATION1,
                        Objet.EC_DATEDEPOTBANQUECONFIRMATION2,
                        Objet.BQ_CODEBANQUE,
                        Objet.AB_CODEAGENCEBANCAIRE,
                        Objet.EC_TIREUR
                };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                //clsObjetRetour.SetValue(true, clsMiceffetchequeWSBLL.pvgChargerDansDataSet1(clsDonnee, clsObjetEnvoi));
                DataSet = clsMiceffetchequeWSBLL.pvgChargerDansDataSetConfirmationaAnnuler(clsDonnee, clsObjetEnvoi);
                if (DataSet.Tables[0].Rows.Count > 0)
                {
                    DataSet.Tables[0].Columns.Add(new DataColumn("CO_CODECOMPTE2", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
                    DataSet.Tables[0].Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
                    for (int i = 0; i < DataSet.Tables[0].Rows.Count; i++)
                    {
                        DataSet.Tables[0].Rows[i]["CO_CODECOMPTE2"] = DataSet.Tables[0].Rows[i]["CO_CODECOMPTE"].ToString();
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
        public string pvgChargerDansDataSetEncaissementaAnnuler(clsMiceffetcheque Objet)
        {
            DataSet DataSet = new DataSet();
            DataTable dt = new DataTable("TABLE");
            dt.Columns.Add(new DataColumn("SL_CODEMESSAGE", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_RESULTAT", typeof(string)));
            dt.Columns.Add(new DataColumn("SL_MESSAGE", typeof(string)));
            string json = "";

            ZenithWebServeur.BOJ.clsObjetEnvoi clsObjetEnvoi = new ZenithWebServeur.BOJ.clsObjetEnvoi();
            ZenithWebServeur.BOJ.clsMiceffetcheque clsMiceffetcheque = new ZenithWebServeur.BOJ.clsMiceffetcheque();
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
                        Objet.CL_CODECLIENT,
                        Objet.CL_NOMCLIENT,
                        Objet.CL_PRENOMCLIENT,
                        Objet.EC_NUMEROVALEUR,
                        Objet.EC_DATEENCAISSEMENT1,
                        Objet.EC_DATEENCAISSEMENT2,
                        Objet.BQ_CODEBANQUE,
                        Objet.AB_CODEAGENCEBANCAIRE,
                        Objet.EC_TIREUR
                };

                //foreach (ZenithWebServeur.DTO.clsMiceffetcheque clsMiceffetchequeDTO in Objet)
                //{

                clsObjetEnvoi.OE_A = Objet.clsObjetEnvoi.OE_A;
                clsObjetEnvoi.OE_Y = Objet.clsObjetEnvoi.OE_Y;

                DataSet = clsMiceffetchequeWSBLL.pvgChargerDansDataSetEncaissementaAnnuler(clsDonnee, clsObjetEnvoi);
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
