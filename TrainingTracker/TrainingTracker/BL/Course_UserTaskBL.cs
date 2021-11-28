using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class Course_UserTaskBL
    {
        #region Course_UserTask
        public List<Course_UserTask> getCourse_UserTaskList(DatabaseEntities de= null)
        {
            return new Course_UserTaskDAL().getCourse_UserTasksList( de );
        }

        public Course_UserTask getCourse_UserTaskById(int _id)
        {
            return new Course_UserTaskDAL().getCourse_UserTaskById(_id);
        }

        public Course_UserTask AddCourse_UserTask(Course_UserTask oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;
            return new Course_UserTaskDAL().AddCourse_UserTask(oldpass);
        }

        public bool UpdateCourse_UserTask(Course_UserTask oldpass, DatabaseEntities de = null)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;

            return new Course_UserTaskDAL().UpdateCourse_UserTask(oldpass,de);
        }

        //public void DeleteCourse_UserTask(int _id)
        //{
        //    new Course_UserTaskDAL().DeleteCourse_UserTask(_id);
        //}
        #endregion
    }
}