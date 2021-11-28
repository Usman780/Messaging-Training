using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class CourseBL
    {
        #region Course
        public List<Course> getCourseList()
        {
            return new CourseDAL().getCoursesList();
        }
     
        public Course getCourseById(int _id)
        {
            return new CourseDAL().getCourseById(_id);
        }

        public bool AddCourse(Course oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;
            return new CourseDAL().AddCourse(oldpass);
        }

        public bool UpdateCourse(Course oldpass)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;

            return new CourseDAL().UpdateCourse(oldpass);
        }

        public void DeleteCourse(int _id)
        {
            new CourseDAL().DeleteCourse(_id);
        }
        #endregion
    }
}