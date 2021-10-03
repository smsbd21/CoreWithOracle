using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Microsoft.Extensions.Configuration;

namespace OraCoreCrud.Services
{
    public class DbConService
    {
        private readonly string strConn;
        public static OracleConnection oCon;
        public DbConService(IConfiguration _Config)
        {
            strConn = _Config.GetConnectionString("OracleDbConnection");
        }
        public DbConService()
        {
            this.InitClsDbCon();
        }
        private void InitClsDbCon()
        {
            oCon = new OracleConnection(strConn);
            //oCon = new OracleConnection("USER ID=NG_" + sUsr + "; PASSWORD=HR$#_" + sPwd + "; DATA SOURCE=" + sHost + "/ORCL");
        }

        #region ========================== [Connection Method] ==========================
        public bool OpenCon()
        {
            try
            {
                if (oCon.State.Equals(ConnectionState.Closed)) oCon.Open();
                return true;
            }
            catch (OracleException)
            {
                return false;
            }
        }
        public bool CloseCon()
        {
            try
            {
                oCon.Close();
                return true;
            }
            catch (OracleException)
            {
                return false;
            }
        }
        public bool RunDmlQuery(string strSql)
        {
            try
            {
                this.OpenCon();
                OracleCommand cmd = new OracleCommand(strSql, oCon);
                cmd.ExecuteNonQuery(); this.CloseCon(); return true;
            }
            catch (OracleException ex)
            {
                if (ex.ErrorCode == 1830)
                {
                    strSql = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD-MON-YYYY HH24:MI:SS'";
                    this.RunDmlQuery(strSql);
                }
                return false;
            }
        }
        public DataSet GetDataSet(string strSql)
        {
            DataSet dtSet = null;
            try
            {
                this.OpenCon();
                OracleDataAdapter oAdp = new OracleDataAdapter(strSql, oCon);
                dtSet = new DataSet(); oAdp.Fill(dtSet); this.CloseCon();
            }
            catch (OracleException)
            {
                throw;
            }
            return dtSet;
        }
        public DataTable GetDataTable(string strSql)
        {
            DataTable dTab = null;
            try
            {
                this.OpenCon();
                OracleDataAdapter oAdp = new OracleDataAdapter(strSql, oCon);
                dTab = new DataTable(); oAdp.Fill(dTab); this.CloseCon();
            }
            catch (OracleException)
            {
                throw;
            }
            return dTab;
        }
        public bool RunDmlBlob(byte[] bLob, string str)
        {
            try
            {
                this.OpenCon();
                OracleParameter oPr = new OracleParameter();
                oPr.Value = bLob;
                oPr.OracleDbType = OracleDbType.Blob;
                oPr.ParameterName = "pBlob";
                OracleCommand oCmd = new OracleCommand(str, oCon);
                oCmd.Parameters.Clear();
                oCmd.Parameters.Add(oPr);
                oCmd.ExecuteNonQuery();
                oCmd.Dispose();
                this.CloseCon();
                return true;
            }
            catch (OracleException)
            {
                return false;
            }
        }
        public bool RunDmlClob(byte[] bLob, string str)
        {
            try
            {
                this.OpenCon(); OracleBlob ol;
                OracleDataReader dr; OracleTransaction tr;
                OracleCommand oCmd = new OracleCommand(str, oCon);
                dr = oCmd.ExecuteReader(); dr.Read();
                tr = oCon.BeginTransaction();
                ol = dr.GetOracleBlob(0);
                ol.Write(bLob, 0, bLob.Length);
                tr.Commit(); oCmd.Dispose();
                this.CloseCon(); return true;
            }
            catch (OracleException)
            {
                return false;
            }
        }
        public OracleDataReader GetExecuteReader(string strSql)
        {
            OracleDataReader dr = null;
            try
            {
                OpenCon();
                OracleCommand oCmd = new OracleCommand(strSql, oCon);
                dr = oCmd.ExecuteReader();
            }
            catch (OracleException)
            {
                throw;
            }
            return dr;
        }
        #endregion
    }
}
