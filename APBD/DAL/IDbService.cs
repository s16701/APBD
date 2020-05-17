using APBD.DTO;
using APBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();

        public void removeId(int id);
        public void createStudent(Student student);

        public void updateStudent(int id, Student student);
        public IEnumerable<Enrollment> GetStudentsSemestr(string id);
        bool addStudent(StudentDTO student);
        bool promo(PromotionDTO promo);
        bool StudenExists(string index);
    }
}
