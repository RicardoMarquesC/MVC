using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace eBillingSuite.Models.DigitalModel
{
    public class NomeTemplate
    {
        private Guid pkid;

        public Guid Pkid
        {
            get { return pkid; }
            set { pkid = value; }
        }
        private string nomeTempl;

        public string NomeTempl
        {
            get { return nomeTempl; }
            set { nomeTempl = value; }
        }

        private string tipoXml;

        public string TipoXml
        {
            get { return tipoXml; }
            set { tipoXml = value; }
        }

        private Guid fkforn;

        public Guid Fkforn
        {
            get { return fkforn; }
            set { fkforn = value; }
        }

        private Guid fktipodoc;

        public Guid Fktipodoc
        {
            get { return fktipodoc; }
            set { fktipodoc = value; }
        }

        private Boolean validado;

        public Boolean Validado
        {
            get { return validado; }
            set { validado = value; }
        }

        private bool tipoDocGenerico;
        public bool TipoDocGenerico
        {
            get { return tipoDocGenerico; }
            set { tipoDocGenerico = value; }
        }

        public NomeTemplate()
        {
        }

        public NomeTemplate(Guid id, string nome, string txml, Guid forn, Guid tipodoc, Boolean valid, bool tipoDocGenerico)
        {
            this.pkid = id;
            this.nomeTempl = nome;
            this.tipoXml = txml;
            this.fkforn = forn;
            this.fktipodoc = tipodoc;
            this.validado = valid;
            this.TipoDocGenerico = tipoDocGenerico;
        }

        //11-03-2015
        public static List<Guid> GetNomeTemplatePkidByTipoDoc(string connStr, string tipoDoc, bool masterizated = false)
        {
            List<Guid> nomeTemplateIds = new List<Guid>();
            int flag = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        string query = "SELECT NomeTemplate.pkid FROM NomeTemplate, TipoFacturas" +
                            " WHERE NomeTemplate.fktipofact = TipoFacturas.pkid and TipoFacturas.nome = @tipoDoc and Masterizado=@masterizated";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("@tipoDoc", SqlDbType.NVarChar).Value = tipoDoc;
                            cmd.Parameters.Add("@masterizated", SqlDbType.Bit).Value = masterizated;

                            using (SqlDataReader thisReader = cmd.ExecuteReader())
                            {
                                while (thisReader.Read())
                                    nomeTemplateIds.Add(thisReader.GetGuid(0));
                            }
                        }
                        query = null;
                    }
                    catch (Exception)
                    {
                        flag = 1;
                        scope.Dispose();

                        if (conn.State == ConnectionState.Open)
                            conn.Close();

                        throw;
                    }
                    finally
                    {
                        if (flag != 1)
                            scope.Complete();

                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            return nomeTemplateIds;
        }

    }
}