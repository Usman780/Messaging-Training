using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class CourseGroupStudyDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region CourseGroupStudy
        public List<CourseGroupStudy> getCourseGroupStudysList(DatabaseEntities de = null)
        {
            DatabaseEntities db = new DatabaseEntities();
            int com = Convert.ToInt32(logedinuser.Company);
            List<CourseGroupStudy> pass = new List<CourseGroupStudy>();
            if(de!=null)
                pass = de.sp_GetCourseGroupStudies().Where(x => x.IsActive == 1 && x.CompanyId == com).ToList();
            else
            pass = db.sp_GetCourseGroupStudies().Where(x => x.IsActive == 1 && x.CompanyId == com).ToList();
           // List<CourseGroupStudy> courselist = new List<CourseGroupStudy>();

            
            return pass;
        }

        public CourseGroupStudy getCourseGroupStudyById(int _id, DatabaseEntities de = null)
        {
            //CourseGroupStudy pass;
            CourseGroupStudy foundedUser = new CourseGroupStudy();
            CourseGroupStudy cou = new CourseGroupStudy();
            if (de != null)
            {
                cou = de.sp_GetCourseGroupStudyById(_id).FirstOrDefault();
            }
            else
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {

                    cou = db.sp_GetCourseGroupStudyById(_id).FirstOrDefault();


                    // foundedUser = General_Purpose.ReturnCourseGroupStudy(cou);
                    // pass = db.CourseGroupStudys.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
                }
            }
           

            return cou;
        }

        public CourseGroupStudy AddCourseGroupStudy(CourseGroupStudy cou, DatabaseEntities de = null)
        {
            CourseGroupStudy cgst = new CourseGroupStudy();
            if (de != null)
            {

                    cgst = de.sp_CourseGroupStudyAddUpdate("Insert", cou.Id, cou.Name, cou.Description, cou.CourseID, cou.User_TaskId, cou.LeadBy, cou.Status, cou.IsActive, cou.CreatedBy, cou.CreatedAt, cou.CompanyId).FirstOrDefault();
                de.SaveChanges();
            }
            else
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    cgst = db.sp_CourseGroupStudyAddUpdate("Insert", cou.Id, cou.Name, cou.Description, cou.CourseID, cou.User_TaskId, cou.LeadBy, cou.Status, cou.IsActive, cou.CreatedBy, cou.CreatedAt, cou.CompanyId).FirstOrDefault();
                    db.SaveChanges();
                    //db.CourseGroupStudys.Add(oldpass);
                    //db.SaveChanges();
                }
            }
          
            return cgst;

        }

        public bool UpdateCourseGroupStudy(CourseGroupStudy user, DatabaseEntities de = null)
        {
            // DatabaseEntities db = new DatabaseEntities();
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                db.sp_CourseGroupStudyAddUpdate("Update", user.Id, user.Name, user.Description, user.CourseID, user.User_TaskId, user.LeadBy, user.Status, user.IsActive, user.CreatedBy, user.CreatedAt, user.CompanyId);

                //db.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                de.sp_CourseGroupStudyAddUpdate("Update", user.Id, user.Name, user.Description, user.CourseID, user.User_TaskId, user.LeadBy, user.Status, user.IsActive, user.CreatedBy, user.CreatedAt, user.CompanyId);
                de.SaveChanges();
                //  de.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;

            }
            return true;
        }

        public void DeleteCourseGroupStudy(int _id, DatabaseEntities de = null)
        {
            DatabaseEntities db = new DatabaseEntities();
            if (de != null)
            {
                
                de.CourseGroupStudies.Remove(de.CourseGroupStudies.FirstOrDefault(x => x.Id == _id));
               
                    de.SaveChanges();
                
            }
            else
            {
                bool varialble = true;
                if (db == null)
                {
                    db = new DatabaseEntities();
                    varialble = false;
                }

                db.CourseGroupStudies.Remove(db.CourseGroupStudies.FirstOrDefault(x => x.Id == _id));
                if (!varialble)
                {
                    db.SaveChanges();
                }
            }
           

        }

        #endregion
    }
}