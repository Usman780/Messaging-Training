
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class TaskFileBL
    {
       

 #region TaskFile
        public List<TaskFile> getTaskFileList()
        {
            return new TaskFileDL().getTaskFileList();
        }
        public List<TaskFile> getAllTaskFileList()
        {
            return new TaskFileDL().getAllTaskFileList();
        }
        public TaskFile getTaskFileById(int _id)
        {
            return new TaskFileDL().getTaskFileById(_id);
        }

        public bool AddTaskFile(TaskFile _TaskFile)
        {
            //if (_TaskFile.Name == null || _TaskFile.Email == null || _TaskFile.Password == null || _TaskFile.Website_Address == null || _TaskFile.Phone == null)
            //    return false;
            return new TaskFileDL().AddTaskFile(_TaskFile);
        }

        public bool UpdateTaskFile(TaskFile _TaskFile)
        {
            //if (_TaskFile.Name == null || _TaskFile.Email == null || _TaskFile.Password == null || _TaskFile.Website_Address == null || _TaskFile.Phone == null)
            //    return false;

            return new TaskFileDL().UpdateTaskFile(_TaskFile);
        }

        public void DeleteTaskFile(int _id)
        {
            new TaskFileDL().DeleteTaskFile(_id);
        }
        #endregion

}
}