using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.BL;
using TrainingTracker.Models;

namespace TrainingTracker.HelpingClasses
{
    public class Messages
    {
        #region Mail and Text Content
        public static string taskAward(User_Task task, int PushNoti = -1)
        {
            //string content = "Dear " + new UserBL().getUsersById(task.Task.UserId.Value).FirstName + ",\n You have been assigned a task " + new TaskBL().getCompanyTasksList(task.User1.CompanyID.Value).Where(x => x.Id == task.TaskID).FirstOrDefault().Name + " starting date of the task is " +
            //    task.StartDate.Value.ToString("dd/MM/yyyy") + "\n\n Thanks\n Team Zuptu";
            User u = new UserBL().getUsersById(task.UserID.Value);
            string UserName = u.FirstName + ' ' + u.LastName;
            Task t = new TaskBL().getTasksById(Convert.ToInt32(task.TaskID));
            string TaskName = t.Name;
            //new TaskBL().getCompanyTasksList(u.CompanyID.Value).Where(x => x.Id == task.TaskID)


            string link = General_Purpose.GetTaskLink(task.Id, (int)u.Role);

            string content = "";
            if (PushNoti == -1)
            {
                content = "Dear " + UserName + ",\n You have been assigned a task '" + t.Name + "' starting date of the task is " +
              task.StartDate.Value.ToString("MM/dd/yyyy") + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";
            }
            else
            {
                content = "Dear " + UserName + ",\n You have been assigned a task '" + t.Name + "' starting date of the task is " +
            task.StartDate.Value.ToString("MM/dd/yyyy") + "\n\n Thanks\n Team Zuptu";
            }





            return content;
        }

        //public static string taskAward(TaskManager task)
        //{
        //    string content = "Dear " + new ManagerBL().getManagersList().Where(x => x.Id == task.ManagerId).FirstOrDefault().FirstName + ",\n You have been assigned a task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskId).FirstOrDefault().Name + " starting date of the task is " +
        //        task.StartDate.Value.ToString("dd/MM/yyyy") + "\n\n Thanks\n Team Zuptu";
        //    return content;
        //}

        public static string taskStatusUpdate(User_Task task, int PushNoti = -1)
        {
            User u = new UserBL().getUsersById((int)task.UserID);
            string link = General_Purpose.GetTaskLink(task.Id, (int)u.Role);
            string content = "";
            if (PushNoti == -1)
            {
                content = "Dear " + new UserBL().getUsersById(task.UserID.Value).FirstName + ",\n You have updated status of a task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskID).FirstOrDefault().Name + "." + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";

            }
            else
            {
                content = "Dear " + new UserBL().getUsersById(task.UserID.Value).FirstName + ",\n You have updated status of a task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskID).FirstOrDefault().Name + "." + "\n\n Thanks\n Team Zuptu";

            }
            return content;

        }



        public static string taskComment(User_Task task, int PushNoti = -1)
        {
            User u = new UserBL().getUsersById((int)task.UserID);
            string link = General_Purpose.GetTaskLink(task.Id, (int)u.Role);
            string content = "";
            if (PushNoti == -1)
            {
                content = "Dear " + new UserBL().getUsersById(task.UserID.Value).FirstName + ",\n You have commented on a task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskID).FirstOrDefault().Name + ". Please visit Zuptu for further details." + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";

            }
            else
            {
                content = "Dear " + new UserBL().getUsersById(task.UserID.Value).FirstName + ",\n You have commented on a task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskID).FirstOrDefault().Name + ". Please visit Zuptu for further details." + "\n\n Thanks\n Team Zuptu";

            }

            return content;

        }




        public static string taskCompleted(User_Task task, int PushNoti = -1)
        {
            User u = new UserBL().getUsersById((int)task.UserID);
            string link = General_Purpose.GetTaskLink(task.Id, (int)u.Role);
            string content = "";
            if (PushNoti == -1)
            {
                content = "Dear " + new UserBL().getUsersById(task.UserID.Value).FirstName + ",\n Congratulations! You have completed your task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskID).FirstOrDefault().Name + ". Please visit Zuptu for further details." + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";

            }
            else
            {
                content = "Dear " + new UserBL().getUsersById(task.UserID.Value).FirstName + ",\n Congratulations! You have completed your task " + new TaskBL().getTasksList().Where(x => x.Id == task.TaskID).FirstOrDefault().Name + ". Please visit Zuptu for further details." + "\n\n Thanks\n Team Zuptu";

            }

            return content;

        }




        public static string groupTaskComment(GroupTasks_Details task, User user, int PushNoti = -1)
        {
            string link = General_Purpose.GetGroupTaskLink(task.Id);
            string content = "";
            if (PushNoti == -1)
            {
                content = "Dear " + user.FirstName + ",\n There is a comment on your Group Task " + new GroupTaskBL().getAllGroupTasksList().Where(x => x.Id == task.GroupTaskId).FirstOrDefault().Name + ". Please visit Zuptu for further details." + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";

            }
            else
            {
                content = "Dear " + user.FirstName + ",\n There is a comment on your Group Task " + new GroupTaskBL().getAllGroupTasksList().Where(x => x.Id == task.GroupTaskId).FirstOrDefault().Name + ". Please visit Zuptu for further details." + "\n\n Thanks\n Team Zuptu";

            }

            return content;

        }


        public static string groupTaskAddition(GroupTasks_Details task, User user, int PushNoti = -1)
        {
            string link = General_Purpose.GetGroupTaskLink(task.Id);
            string content = "";
            if (PushNoti == -1)
            {
                string grouptaskname = new GroupTaskBL().getGroupTasksById(Convert.ToInt32(task.GroupTaskId)).Name;

                content = "Dear " + user.FirstName + ",\n You have been added in Group Task '" + grouptaskname + "'. Please visit Zuptu for further details." + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";

            }
            else
            {
                string grouptaskname = new GroupTaskBL().getGroupTasksById(Convert.ToInt32(task.GroupTaskId)).Name;

                content = "Dear " + user.FirstName + ",\n You have been added in Group Task '" + grouptaskname + "'. Please visit Zuptu for further details." + "\n\n Thanks\n Team Zuptu";

            }


            return content;

        }

        public static string groupTaskCompletion(GroupTasks_Details task, User user, int PushNoti = -1)
        {
            string link = General_Purpose.GetGroupTaskLink(task.Id);
            string content = "";
            if (PushNoti == -1)
            {

                content = "Dear " + user.FirstName + ",\n Your Group Task '" + task.GroupTask.Name + "' has been completed. Please visit Zuptu for further details." + "\n\nTo visit the task please click this link: " + link + "\n\n Thanks\n Team Zuptu";

            }
            else
            {

                content = "Dear " + user.FirstName + ",\n Your Group Task '" + task.GroupTask.Name + "' has been completed. Please visit Zuptu for further details." + "\n\n Thanks\n Team Zuptu";
            }
            return content;

        }
        public static string groupTaskStatusUpdation(GroupTasks_Details task, User user, int PushNoti = -1)
        {
            string link = General_Purpose.GetGroupTaskLink(task.Id);
            string content = "";
            if (PushNoti == -1)
            {

                content = "Dear " + user.FirstName + ",\n Status of the Group Task you have been assigned '" + task.GroupTask.Name + "' is changed . Please visit Zuptu for further details." + "\n\nTo visit the task please click this link: " + link + "\n\nThanks\n Team Zuptu";

            }
            else
            {

                content = "Dear " + user.FirstName + ",\n Status of the Group Task you have been assigned '" + task.GroupTask.Name + "' is changed . Please visit Zuptu for further details." + "\n\nThanks\n Team Zuptu";
            }

            return content;

        }


        public static string additionInTheSystem(User user)
        {
            string content = "Dear " + user.FirstName + ",\n Congraulations ! You have been added to Zuptu System. Please contact your Manager for your login details";
            return content;

        }

        public static string groupTaskSlackMessage(string groupTaskName)
        {
            return "There is an important update about your group task " + groupTaskName + ". Please visit Zuptu." + "\n\nThanks\n Team Zuptu";
        }


        #endregion






    }
}