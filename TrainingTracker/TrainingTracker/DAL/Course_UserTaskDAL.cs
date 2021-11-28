using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class Course_UserTaskDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region Course_UserTask
        public List<Course_UserTask> getCourse_UserTasksList(DatabaseEntities de=null)
        {
            DatabaseEntities db = new DatabaseEntities();
            
            List<Course_UserTask> Course_UserTask = new List<Course_UserTask>();
            if(de!=null)
            Course_UserTask= de.sp_GetCourse_UserTasks().Where(x => x.IsActive == 1).ToList();
            else
            Course_UserTask= db.sp_GetCourse_UserTasks().Where(x => x.IsActive == 1).ToList();
            //List<Course_UserTask> Course_UserTasklist = new List<Course_UserTask>();

            //foreach (Course_UserTask cou in Course_UserTask)
            //{
            //    Course_UserTask c = General_Purpose.ReturnCourse_UserTask(cou);
            //    Course_UserTasklist.Add(c);
            //}

            //return Course_UserTasklist;
            return Course_UserTask.Where(x=>x.CompanyID==Convert.ToInt32(logedinuser.Company)).ToList();
        }

        public Course_UserTask getCourse_UserTaskById(int _id, DatabaseEntities de = null)
        {
            //Course_UserTask pass;
            Course_UserTask foundedUser = new Course_UserTask();
            Course_UserTask cou = new Course_UserTask();

            if (de != null)
            {
                cou = de.sp_GetCourse_UserTaskById(_id).FirstOrDefault();
                return cou;
            }
            else
            {
            using (DatabaseEntities db = new DatabaseEntities())
            {
                cou = db.sp_GetCourse_UserTaskById(_id).FirstOrDefault();
                return cou;


                //  foundedUser = General_Purpose.ReturnCourse_UserTask(cou);
                // pass = db.Course_UserTasks.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            }
           }

        }

        public Course_UserTask AddCourse_UserTask(Course_UserTask user)
        {
            Course_UserTask Cut = new Course_UserTask();

            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.sp_Course_UserTaskAddUpdate("Insert", user.Id, user.IsActive, user.CourseID, user.User_TaskID, user.CreatedAt,user.UserId,user.TotalMarks,user.CompletedAt,user.ResultStatus,user.CompanyID,user.IsResultUpdated,user.ResultUpdatedBy, user.IsLead, user.CourseGroupStudyId);
                Cut = db.sp_GetCourse_UserTasks().LastOrDefault();
                db.SaveChanges();
               
            }

            
            return Cut;

        }

        public bool UpdateCourse_UserTask(Course_UserTask user, DatabaseEntities de = null)
        {
            // DatabaseEntities de = new DatabaseEntities();
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                db.sp_Course_UserTaskAddUpdate("Update", user.Id, user.IsActive, user.CourseID, user.User_TaskID, user.CreatedAt,user.UserId, user.TotalMarks, user.CompletedAt, user.ResultStatus,user.CompanyID,user.IsResultUpdated,user.ResultUpdatedBy,user.IsLead,user.CourseGroupStudyId);

                //db.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                de.sp_Course_UserTaskAddUpdate("Update", user.Id, user.IsActive, user.CourseID, user.User_TaskID, user.CreatedAt,user.UserId, user.TotalMarks, user.CompletedAt, user.ResultStatus,user.CompanyID, user.IsResultUpdated, user.ResultUpdatedBy, user.IsLead, user.CourseGroupStudyId);

                //  de.Entry(oldpass).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        //public void DeleteCourse_UserTask(int _id)
        //{
        //    DatabaseEntities db = new DatabaseEntities();

        //    bool varialble = true;
        //    if (db == null)
        //    {
        //        db = new DatabaseEntities();
        //        varialble = false;
        //    }

        //    db.Course_UserTasks.Remove(db.Course_UserTasks.FirstOrDefault(x => x.Id == _id));
        //    if (!varialble)
        //    {
        //        db.SaveChanges();
        //    }

        //}

        public bool DeleteCourseUser_Task(int id)
        {
            DatabaseEntities de = new DatabaseEntities();
            Course_UserTask Cut = new Course_UserTaskDAL().getCourse_UserTaskById((int)id,de);
            Cut.IsActive = 0;
        //    new User_TaskDL().DeleteUser_Task((int)Cut.User_TaskID);
            new Course_UserTaskDAL().UpdateCourse_UserTask(Cut,de);
            return true;

        }
        #endregion
    }
}