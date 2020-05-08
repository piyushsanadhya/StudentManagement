using StudentManagementProject.Areas.Admin.Models.Student;
using StudentManagementProject.Areas.Admin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Areas.Admin.BusinessModule
{
    public class BlStudent
    {
        public async Task<List<StudentDetailsViewModel>> GetStudentDetailsAsync (int skip,int take,string orderby,string searchby)
        {
            StudentRepository studentRepository = new StudentRepository();
            return await studentRepository.GetStudents(skip,take,orderby,searchby);
        }

        public async Task<int> GetStudentTotalDetailsCountAsync()
        {
            StudentRepository studentRepository = new StudentRepository();
            return await studentRepository.GetStudentsTotal();
        }
    }
}
