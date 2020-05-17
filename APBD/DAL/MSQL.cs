
using APBD.DTO;
using APBD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APBD.DAL
{
    public class MSQL : IDbService
    {
        public void createStudent(Student student)
        {
            throw new NotImplementedException();
        }



        public bool addStudent(StudentDTO student)
        {

            initTable();
            using (var con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            using (var com = new SqlCommand())
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT IndexNumber FROM dbo.Student WHERE FirstName=" + student.FirstName;
                var dr = com.ExecuteReader();
                if (dr.HasRows) // Student istnieje
                {
                    return false;
                }


                com.CommandText = "SELECT * FROM dbo.Studies WHERE name=" + student.Studies;
                dr = com.ExecuteReader();
                if (!dr.HasRows) // Studia nie istnieje
                {
                    return false;
                }

                var tran = con.BeginTransaction();

                com.CommandText = $"SELECT top (1) IdEnrollment FROM dbo.Enrollment e INNER JOIN dbo.Studies s on e.IdStudy = s.IdStudy and s.Name = {student.Studies} ORDER BY Semester ASC)";
                dr = com.ExecuteReader();
                if (dr.HasRows) // Studia nie istnieją, tworzymy nowe studia
                {
                    com.CommandText = $"INSERT INTO dbo.Studies (Name) VALUE ({student.Studies})";
                    com.ExecuteNonQuery();
                    com.CommandText = $"INSERT INTO dbo.Enrollment (Semester, IdStudy, StartDate) VALUE (1, " +
                        $"(SELECT TOP (1) IdStudy FROM dbo.Studies WHERE name = {student.Studies})" +
                        $", {DateTime.Now})";
                    com.ExecuteNonQuery();
                }


                com.CommandText = $"INSERT INTO Student VALUES({student.IndexNumber},{student.FirstName},{student.LastName},{student.BirthDate}," +
                    $" (SELECT top (1) IdEnrollment FROM dbo.Enrollment e INNER JOIN dbo.Studies s on e.IdStudy = s.IdStudy and s.Name = {student.Studies} ORDER BY Semester ASC))";

                com.ExecuteNonQuery();
                tran.Commit();
                return true;
            }

            return false;
        }

        public bool promo(PromotionDTO promo)
        {
            initTable();
            using (var con = new SqlConnection(
      "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
      // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
      ))
            using (var com = new SqlCommand())
            {
                con.Open();
                com.Connection = con;
                com.CommandText = $"SELECT * FROM dbo.Enrollment e JOIN dbo.Studies s on e.IdStudy = s.IdStudy WHERE e.Semester = {promo.Semester} and s.name = {promo.Studies}";
                var dr = com.ExecuteReader();
                if (dr.HasRows) // Student istnieje
                {
                    initSP(); // utworzenie procedury
                    com.CommandText = "exec dbo.promo";
                    com.ExecuteNonQuery();
                    return true;
                }

            }

            return false;
        }


        public bool StudenExists(string index)
        {
            initTable();
            using (var con = new SqlConnection(
      "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
      // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
      ))
            using (var com = new SqlCommand())
            {
                con.Open();
                com.Connection = con;
                com.CommandText = $"SELECT * FROM dbo.Student WHERE IndexNumber = {index}";
                var dr = com.ExecuteReader();
                if (dr.HasRows) // Student istnieje
                {
                    return true;
                }

            }

            return false;
        }
        public IEnumerable<Student> GetStudents()
        {
            initTable();
            List<Student> students = null;
            using (var con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            using (var com = new SqlCommand())
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT IndexNumber, FirstName, LastName, BirthDate FROM dbo.Student";
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();


                    if (students == null)
                    {
                        students = new List<Student> { st };
                    }
                    else
                    {

                        students.Add(st);
                    }

                }

            }

            return students ?? new List<Student> { };
        }

        public IEnumerable<Enrollment> GetStudentsSemestr(string index)
        {
            initTable();
            List<Enrollment> enrollments = new List<Enrollment> { };
            using (var con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "SELECT e.* FROM dbo.Student s " +
                                        "INNER JOIN dbo.Enrollment e " +
                                        "ON s.IdEnrollment = e.IdEnrollment and s.IndexNumber = @id";
                com.Parameters.AddWithValue("id", index);
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Enrollment();
                    st.IdStudy = Int32.Parse(dr["IdStudy"].ToString());
                    st.Semester = Int32.Parse(dr["Semeester"].ToString());
                    st.IdEnrollment = Int32.Parse(dr["IdEnrollment"].ToString());
                    st.StartDate = dr["StartDate"].ToString();

                    enrollments.Add(st);
                }

            }
            return enrollments;
        }

        public void removeId(int id)
        {
            throw new NotImplementedException();
        }

        public void updateStudent(int id, Student student)
        {
            throw new NotImplementedException();
        }

        private static void initTable()
        {
            using (SqlConnection connection = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            {
                SqlCommand command = new SqlCommand(@"

                        CREATE TABLE IF NOT EXISTS[dbo].[Studies]
                        (

                            [IdStudy] INT NOT NULL PRIMARY KEY,
                            [Name] NVARCHAR(100) NOT NULL
                        )
                        CREATE TABLE IF NOT EXISTS[dbo].[Enrollment]
                        (

                            [IdEnrollment]   INT NOT NULL PRIMARY KEY, 
                            [Semester] int NOT NULL, 
                            [IdStudy] int NOT NULL, 
                            [StartDate] date NOT NULL
                            
                        )
                        CREATE TABLE IF NOT EXISTS[dbo].[Student]
                                (

                           [IndexNumber] NVARCHAR(100) NOT NULL PRIMARY KEY,

                           [FirstName] NVARCHAR(100) NOT NULL,

                           [LastName] NVARCHAR(100) NOT NULL,

                           [BirthDate] DATE NOT NULL, 
                            [IdEnrollment] INT NULL
                        )"
                    //FOREIGN KEY(IdEnrollment) REFERENCES Enrollment(IdEnrollment)
                    //FOREIGN KEY(IdStudy) REFERENCES Studies(IdStudy)

                    , connection);
                command.Connection.Open();
                try
                {

                    command.ExecuteNonQuery();
                }
                catch (Exception e) { }

            }
        }


        private static void initSP()
        {
            using (SqlConnection connection = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\cw3-master\\Cw3\\Cw3\\localDB.mdf;Integrated Security=True;Connect Timeout=30"
                // "Data Source=db-mssql;Initial Catalog=s16701;Integrated Security=True"
                ))
            {
                SqlCommand command = new SqlCommand(@"

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.promo') AND type in (N'P', N'PC'))
                                DROP PROCEDURE dbo.promo
                        
                        "
                    //FOREIGN KEY(IdEnrollment) REFERENCES Enrollment(IdEnrollment)
                    //FOREIGN KEY(IdStudy) REFERENCES Studies(IdStudy)

                    , connection);
                command.Connection.Open();
                try
                {

                    command.ExecuteNonQuery();
                }
                catch (Exception e) { }


                command = new SqlCommand(@"
                            
                        CREATE PROCEDURE dbo.promo
                            @studies NVARCHAR(100),
                            @Semester int
                        AS
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM dbo.Studies s INNER JOIN dbo Enrollment e on e.IdStudy = s.IdStudy and e.Semester = @Semester + 1 and s.Name = @studies)
                            BEGIN
                                INSERT INTO dbo.Enrollment
                                  ( Semester, 
                                    IdStudy, 
                                    StartDate) VALUE (@Semester +1, (SELECT top (1) IdStudy FROM dbo.Studies WHERE name = @Studies), getdate())
                            END

	                        UPDATE dbo.Student
                            SET IdEnrollment = (SELECT TOP (1) e.IdEnrollment FROM dbo.Studies s INNER JOIN dbo Enrollment e on e.IdStudy = s.IdStudy and e.Semester = @Semester + 1 and s.Name = @studies)
                            WHERE IdEnrollment = (SELECT TOP (1) e.IdEnrollment FROM dbo.Studies s INNER JOIN dbo Enrollment e on e.IdStudy = s.IdStudy and e.Semester = @Semester and s.Name = @studies)
                        END
                        
                        "
                   //FOREIGN KEY(IdEnrollment) REFERENCES Enrollment(IdEnrollment)
                   //FOREIGN KEY(IdStudy) REFERENCES Studies(IdStudy)

                   , connection);
                command.Connection.Open();
                try
                {

                    command.ExecuteNonQuery();
                }
                catch (Exception e) { }

            }
        }


    }
}
