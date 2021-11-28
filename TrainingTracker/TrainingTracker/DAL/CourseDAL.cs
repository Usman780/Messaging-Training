using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class CourseDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region Course
        public List<Course> getCoursesList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int com = Convert.ToInt32(logedinuser.Company);
            List<Course> pass = db.sp_GetCourses().Where(x => x.IsActive == 1 && x.CompanyID==com && x.IsPublished==1).ToList();
            List<Course> courselist = new List<Course>();

            foreach (Course cou in pass)
            {
                Course c = General_Purpose.ReturnCourse(cou);
                courselist.Add(c);
            }
            return pass;
        }
     
        public Course getCourseById(int _id)
        {
            //Course pass;
            Course foundedUser = new Course();

            using (DatabaseEntities db = new DatabaseEntities())
            {
                Course cou = new Course();
                cou = db.sp_GetCourseById(_id).FirstOrDefault();


                foundedUser = General_Purpose.ReturnCourse(cou);
               // pass = db.Courses.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            }

            return foundedUser;
        }

        public bool AddCourse(Course cou)
        {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.sp_CourseAddUpdate("Insert", cou.Id, cou.Name, cou.IsPublished, cou.Description, cou.CompanyID, cou.NoFile, cou.FileSize, cou.IsActive, cou.CreatedAt, cou.CreatedBy, cou.ResultAnnoucement,cou.TitleImage,cou.CreateCertificate, cou.IsScorm, cou.ScormCourseFile, cou.SignatureImage, cou.CertificateValidity, cou.Citation, cou.SignatureText);

                //db.Courses.Add(oldpass);
                //db.SaveChanges();
            }
            return true;

        }

        public bool UpdateCourse(Course user,DatabaseEntities de=null)
        {
           // DatabaseEntities de = new DatabaseEntities();
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                db.sp_CourseAddUpdate("Update", user.Id, user.Name, user.IsPublished, user.Description, user.CompanyID, user.NoFile, user.FileSize, user.IsActive, user.CreatedAt, user.CreatedBy, user.ResultAnnoucement,user.TitleImage,user.CreateCertificate, user.IsScorm, user.ScormCourseFile,user.SignatureImage,user.CertificateValidity,user.Citation,user.SignatureText);

                //db.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
            }
            else
            {
                de.sp_CourseAddUpdate("Update", user.Id, user.Name, user.IsPublished, user.Description, user.CompanyID, user.NoFile, user.FileSize, user.IsActive, user.CreatedAt, user.CreatedBy, user.ResultAnnoucement,user.TitleImage,user.CreateCertificate, user.IsScorm, user.ScormCourseFile, user.SignatureImage, user.CertificateValidity, user.Citation, user.SignatureText);

                //  de.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;

            }
            return true;
        }

        public void DeleteCourse(int _id)
        {
            DatabaseEntities db = new DatabaseEntities();

            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }

            db.Courses.Remove(db.Courses.FirstOrDefault(x => x.Id == _id));
            if (!varialble)
            {
                db.SaveChanges();
            }

        }

        #endregion
    }
}