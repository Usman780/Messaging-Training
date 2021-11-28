using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class CourseGroupStudyBL
    {
        #region CourseGroupStudy
        public List<CourseGroupStudy> getCourseGroupStudyList(DatabaseEntities de = null)
        {
            return new CourseGroupStudyDAL().getCourseGroupStudysList(de);
        }

        public CourseGroupStudy getCourseGroupStudyById(int _id, DatabaseEntities de = null)
        {
            return new CourseGroupStudyDAL().getCourseGroupStudyById(_id,de);
        }

        public CourseGroupStudy AddCourseGroupStudy(CourseGroupStudy oldpass, DatabaseEntities de = null)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;
            return new CourseGroupStudyDAL().AddCourseGroupStudy(oldpass,de);
        }

        public bool UpdateCourseGroupStudy(CourseGroupStudy oldpass, DatabaseEntities de = null)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;

            return new CourseGroupStudyDAL().UpdateCourseGroupStudy(oldpass,de);
        }

        public void DeleteCourseGroupStudy(int _id, DatabaseEntities de = null)
        {
            new CourseGroupStudyDAL().DeleteCourseGroupStudy(_id,de);
        }
        #endregion
    }
}