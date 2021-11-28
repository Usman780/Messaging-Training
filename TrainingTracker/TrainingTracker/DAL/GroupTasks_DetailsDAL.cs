
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.BL;
using RusticiSoftware.HostedEngine.Client;

namespace TrainingTracker.DAL
{

  public class GroupTasks_DetailsDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        #region GroupTasks_Details
        public List<GroupTasks_Details> getGroupTasks_DetailssList(DatabaseEntities de=null)
        {


            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            if (de != null)
            {
                //List<GroupTasks_Details> GroupTasks_Details = de.GroupTasks_Details.Where(x => x.IsActive == 1 && x.GroupTask.User.CompanyID == Companyid).ToList();
                List<GroupTasks_Details> GroupTasks_Details = de.GroupTasks_Details.Where(x => x.IsActive == 1 && x.CompanyId == Companyid).ToList();
                return GroupTasks_Details;

            }
            List<GroupTasks_Details> GroupTasks_Detailss = db.GroupTasks_Details.Where(x=>x.IsActive==1 && x.CompanyId==Companyid).ToList();

            return GroupTasks_Detailss;
        }
        public List<GroupTasks_Details> getAllGroupTasks_DetailssList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int Companyid = Convert.ToInt32(logedinuser.Company);
            //List<GroupTasks_Details> GroupTasks_Detailss = db.GroupTasks_Details.Where(x => x.GroupTask.User.CompanyID == Companyid).ToList(); ;
            List<GroupTasks_Details> GroupTasks_Detailss = db.GroupTasks_Details.Where(x => x.CompanyId == Companyid).ToList(); ;

            return GroupTasks_Detailss;
        }
        
        public GroupTasks_Details getGroupTasks_DetailsById(int _Id)
        {
            GroupTasks_Details _GroupTasks_Details;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                _GroupTasks_Details = db.GroupTasks_Details.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            }

            return _GroupTasks_Details;
        }

        public GroupTasks_Details getGroupTasks_DetailsByIdWrapper(int _Id,DatabaseEntities de=null)
        {
            if (de == null)
            {
                GroupTasks_Details _GroupTasks_Details;
                DatabaseEntities db = new DatabaseEntities();

                _GroupTasks_Details = db.GroupTasks_Details.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


                return _GroupTasks_Details;
            }else
            {
                GroupTasks_Details _GroupTasks_Details;
                
                _GroupTasks_Details = de.GroupTasks_Details.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


                return _GroupTasks_Details;
            }
        }
        public int AddGroupTasks_Details(GroupTasks_Details _GroupTasks_Details)
        {

            _GroupTasks_Details.CompanyId = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTasks_Details.Add(_GroupTasks_Details);
                db.SaveChanges();
                return _GroupTasks_Details.Id;
            }
         
        }

        public bool UpdateGroupTasks_Details(GroupTasks_Details _GroupTasks_Details,DatabaseEntities de=null)
        {
            if (_GroupTasks_Details.CompanyId == 0 || _GroupTasks_Details.CompanyId == null)
                _GroupTasks_Details.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de != null)
            {
                de.Entry(_GroupTasks_Details).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            
            return true;
        }
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Entry(_GroupTasks_Details).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTasks_DetailsWrapper(GroupTasks_Details _GroupTasks_Details,DatabaseEntities de=null)
        {
            if (_GroupTasks_Details.CompanyId == 0 || _GroupTasks_Details.CompanyId == null)
                _GroupTasks_Details.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_GroupTasks_Details).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }
            else
            {
                de.Entry(_GroupTasks_Details).State= System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
      
        }




        public void DeleteGroupTasks_Details(int _id)
        {
            ///////////////////////////////////////Delete PArent Frequency task 
            DatabaseEntities dc = new DatabaseEntities();
            GroupTasks_Details utask = new GroupTasks_DetailsDL().getGroupTasks_DetailsByIdWrapper(_id, dc);
            GroupTasks_Details childutask = new GroupTasks_DetailsDL().getGroupTasks_DetailssList(dc).Where(x => x.ParentID == utask.Id).FirstOrDefault();
            if (utask.ParentID == null && childutask != null)
            {
                List<GroupTasks_Details> list = new GroupTasks_DetailsDL().getGroupTasks_DetailssList(dc).Where(x => x.ParentID == utask.Id).ToList();
                foreach (GroupTasks_Details i in list)
                {
                    i.ParentID = childutask.Id;
                    new GroupTasks_DetailsDL().UpdateGroupTasks_DetailsWrapper(i, dc);
                    dc.SaveChanges();
                }
                GroupTasks_Details child1 = new GroupTasks_DetailsDL().getGroupTasks_DetailsByIdWrapper(childutask.Id, dc);
                child1.ParentID = null;
                new GroupTasks_DetailsDL().UpdateGroupTasks_DetailsWrapper(child1, dc);
                dc.SaveChanges();


            }

            ///////////////////////////////////////////
            if(utask.CourseId!= null)
            {
                User_Task ut = new User_TaskDL().GetGroupStudyUser_Tasks(Convert.ToInt32(logedinuser.Company)).Where(x => x.GroupTaskDetailId == utask.Id).FirstOrDefault();
                
                if(ut!= null)
                {
                    Course_UserTask Cut = new Course_UserTaskDAL().getCourse_UserTasksList().Where(x => x.CourseID == utask.CourseId && x.User_TaskID == ut.Id && x.CourseGroupStudyId!=null).FirstOrDefault();
                    if (Cut != null)
                    {
                        if (Cut.CourseGroupStudyId != null)
                        {
                            List<Course_UserTask> CuList = new Course_UserTaskDAL().getCourse_UserTasksList().Where(x => x.CourseGroupStudyId == Cut.CourseGroupStudyId).ToList();
                            foreach (Course_UserTask item in CuList)
                            {
                                CourseGroupStudy Cgst = new CourseGroupStudyDAL().getCourseGroupStudyById((int)item.CourseGroupStudyId);
                                User_Task ut2 = new User_TaskDL().getUser_TaskById((int)item.User_TaskID);
                                new User_TaskDL().DeleteUser_Task(ut2.Id);
                                if (item.IsLead == 1)
                                {
                                    Course course = new CourseBL().getCourseById((int)item.CourseID);
                                    if (course != null && course.IsScorm == 1)
                                    {
                                        ScormCloud.Configuration = new RusticiSoftware.HostedEngine.Client.Configuration(
                        "https://cloud.scorm.com/EngineWebServices",
                        ProjectVaraiables.ScormAppId,
                        ProjectVaraiables.ScormSecretKey,
                           ProjectVaraiables.ScormOrigin);
                                        try
                                        {
                                            ScormCloud.RegistrationService.DeleteRegistration(item.Id.ToString(), false);

                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                new Course_UserTaskDAL().DeleteCourseUser_Task(item.Id);
                                new CourseGroupStudyDAL().DeleteCourseGroupStudy(Cgst.Id);
                            }
                        }
                       
                    }
                }
            }
            //////////////////////////////////////////


            DatabaseEntities de = new DatabaseEntities();
            GroupTasks_Details gt = de.GroupTasks_Details.ToList().Where(x => x.Id == _id).FirstOrDefault();
            if (gt != null)
            {
                GroupTask_UserDL gtmdl = new GroupTask_UserDL();
       
                gt.IsActive = 0;
                foreach (var item in gt.GroupTask_User)
                {
                    gtmdl.DeleteGroupTask_User(item.Id, de);
                }

             


                UpdateGroupTasks_DetailsWrapper(gt, de);
                de.SaveChanges();
            }

        }
#endregion

}
}