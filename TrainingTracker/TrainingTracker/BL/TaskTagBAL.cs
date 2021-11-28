
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class TaskTagBL
    {
       

 #region TaskTags
        public List<TaskTag> getTaskTagsList()
        {
            return new TaskTagDL().getTaskTagsList();
        }
        public List<TaskTag> getAllTaskTagsList()
        {
            return new TaskTagDL().getAllTaskTagsList();
        }
        public TaskTag getTaskTagsById(int _id)
        {
            return new TaskTagDL().getTaskTagById(_id);
        }

        public bool AddTaskTags(TaskTag _TaskTags)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;
            return new TaskTagDL().AddTaskTag(_TaskTags);
        }

        public bool UpdateTaskTags(TaskTag _TaskTags)
        {
            //if (_TaskTags.Name == null || _TaskTags.Email == null || _TaskTags.Password == null || _TaskTags.Website_Address == null || _TaskTags.Phone == null)
            //    return false;

            return new TaskTagDL().UpdateTaskTag(_TaskTags);
        }

        public void DeleteTaskTags(int _id)
        {
            new TaskTagDL().DeleteTaskTag(_id);
        }
        #endregion

}
}