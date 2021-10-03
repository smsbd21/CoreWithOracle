using System;
using System.Data;
using OraCoreCrud.Models;
using OraCoreCrud.Interface;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;


namespace OraCoreCrud.Services
{
    public class StudentService: IStudentService
    {
        private readonly string strConn;
        public StudentService(IConfiguration _Config)
        {
            strConn = _Config.GetConnectionString("OracleDbConnection");
        }
        public Student GetStudentById(int id)
        {
            Student oStud = new Student();
            using (OracleConnection oCon = new OracleConnection(strConn))
            {
                using (OracleCommand oCmd = new OracleCommand())
                {
                    oCon.Open(); oCmd.BindByName = true;
                    oCmd.CommandText = "SELECT ID, NAME, EMAIL FROM APICOREDB.STUDENT WHERE ID='" + id + "'";
                    OracleDataReader oDr = oCmd.ExecuteReader();
                    while (oDr.Read())
                    {
                        oStud.Id = Convert.ToInt32(oDr["ID"]);
                        oStud.Name = oDr["NAME"].ToString();
                        oStud.Email = oDr["EMAIL"].ToString();
                    }
                    oDr.Close();
                }
            }
            return oStud;
        }
        public void AddStudent(Student oStud)
        {
            try
            {
                using (OracleConnection oCon = new OracleConnection(strConn))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCon.Open();
                        //string strSql = "INSERT INTO APICOREDB.STUDENT(ID, NAME, EMAIL) VALUES('" + oStud.Id + "','" + oStud.Name + "','" + oStud.Email + "')";
                        oCmd.CommandText = "INSERT INTO APICOREDB.STUDENT(ID, NAME, EMAIL) VALUES('" + oStud.Id + "','" + oStud.Name + "','" + oStud.Email + "')";
                        oCmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }            
        }
        public void EditStudent(Student oStud)
        {
            try
            {
                using (OracleConnection oCon = new OracleConnection(strConn))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCon.Open();
                        oCmd.CommandText = "UPDATE APICOREDB.STUDENT SET NAME='" + oStud.Name + "',EMAIL='" + oStud.Email + "' WHERE ID='" + oStud.Id + "'";
                        oCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteStudent(Student oStud)
        {
            try
            {
                using (OracleConnection oCon = new OracleConnection(strConn))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCon.Open();
                        oCmd.CommandText = "DELETE FROM APICOREDB.STUDENT WHERE ID='" + oStud.Id + "'";
                        oCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<Student> GetStudents()
        {
            List<Student> oStudList = new List<Student>();
            using(OracleConnection oCon=new OracleConnection(strConn))
            {
                oCon.Open();
                string strSql = "SELECT ID, NAME, EMAIL FROM APICOREDB.STUDENT";
                OracleDataAdapter oAdp = new OracleDataAdapter(strSql, oCon);
                DataSet dtSet = new DataSet(); oAdp.Fill(dtSet); //oCon.Close();
                foreach (DataRow dr in dtSet.Tables[0].Rows)
                {
                    Student stud = new Student()
                    {
                        Id = Convert.ToInt32(dr["ID"]),
                        Name = dr["NAME"].ToString(),
                        Email = dr["EMAIL"].ToString()
                    };
                    oStudList.Add(stud);
                }
                /*
                using (OracleCommand oCmd=new OracleCommand())
                {                    
                    oCon.Open();
                    oCmd.BindByName = true;
                    oCmd.CommandText = "SELECT ID, NAME, EMAIL FROM APICOREDB.STUDENT";
                    OracleDataReader oDr = oCmd.ExecuteReader();
                    while (oDr.Read())
                    {
                        Student stud = new Student()
                        {
                            Id = Convert.ToInt32(oDr["ID"]),
                            Name = oDr["NAME"].ToString(),
                            Email = oDr["EMAIL"].ToString()
                        };
                        oStudList.Add(stud);
                    }
                    oDr.Close();
                }
                */
            }
            return oStudList;
        }
        public Student GetStudentDataById(int id)
        {
            Student oStud = new Student();
            DbConService db = new DbConService();
            string strSql = "SELECT ID, NAME, EMAIL FROM APICOREDB.STUDENT WHERE ID='" + id + "'";
            DataSet ds = db.GetDataSet(strSql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                oStud.Id = Convert.ToInt32(dr["ID"]);
                oStud.Name = dr["NAME"].ToString();
                oStud.Email = dr["EMAIL"].ToString();
            }
            return oStud;
        }
    }
}
