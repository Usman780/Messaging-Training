using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using TrainingTracker.BL;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;
using TrainingTracker.Controllers;

namespace TrainingTracker.HelpingClasses.Outlook
{
    public class SessionTokenCache 
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
      
        private static ReaderWriterLockSlim sessionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        string userId = string.Empty;
        string cacheId = string.Empty;
        HttpContextBase httpContext = null;
        User user = new User();
      

        TokenCache tokenCache = new TokenCache();

        public SessionTokenCache(string userId, string httpContext)
        {
           
            //string xx =HttpContext.Current.Session["LoggedInUserd"].ToString();
            this.userId = userId;
            cacheId = userId + "_TokenCache";
            //  this.httpContext = httpContext;

            int login = 0;
            if (logedinuser.Id == 0)
            {
                var response = HttpContext.Current.Request;
                try
                {
                    string idZuptu = response.Cookies["Zuptu_Id"].Value;
                    login = Convert.ToInt32(idZuptu);

                }
                catch (Exception ex)
                {
                    string idZuptu2 = response.Cookies["Zuptu_Id2"].Value;
                    login = Convert.ToInt32(idZuptu2);

                }
            }
            else
            {
                login = logedinuser.Id;
            }

            user = new UserBL().getUsersById(login);
            Load();
        }

        public TokenCache GetMsalCacheInstance()
        {
            tokenCache.SetBeforeAccess(BeforeAccessNotification);
            tokenCache.SetAfterAccess(AfterAccessNotification);
            Load();
            return tokenCache;
        }

        public bool HasData()
        {
           // User user = new UserBL().getUsersById(logedinuser.Id);
           // bool check = (user.OutlookToken != null);
          //  check = (user.OutlookToken != null && (Encoding.UTF8.GetBytes(user.OutlookToken)).Length > 0);
            return (user.OutlookToken != null && (Encoding.UTF8.GetBytes(user.OutlookToken)).Length > 0); ;
        }

        public void Clear()
        {
            DatabaseEntities de = new DatabaseEntities();
            UserBL userBL = new UserBL();
           //User user = userBL.getUsersById(logedinuser.Id);
            user.OutlookToken = null;
            Models.User newUser = new Models.User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HomeNumber = user.HomeNumber,
                Notes = user.Notes,
                IsActive = user.IsActive,
                AcessLevel = user.AcessLevel,
                Image = user.Image,
                Password = user.Password,
                isMail = user.isMail,
                isSlack = user.isSlack,
                isSMS = user.isSMS,
                SlackAddress = user.SlackAddress,
                GoogleKeyLength = user.GoogleKeyLength,
                GoggleTaskColor = user.GoggleTaskColor,
                ShowTasks = user.ShowTasks,
                ShowUrgent = user.ShowUrgent,
                CompanyID = user.CompanyID,
                DivisionId = user.DivisionId,
                SearchByDepartment = user.SearchByDepartment,
                SearchByDivision = user.SearchByDivision,
                SearchByPriority = user.SearchByPriority,
                SearchByPrivate = user.SearchByPrivate,
                SearchByUserType = user.SearchByUserType,
                LowPriorityColor = user.LowPriorityColor,
                HighPriorityColor = user.HighPriorityColor,
                MediumPriorityColor = user.MediumPriorityColor,
                Role = user.Role,
                IsMasterAdmin = user.IsMasterAdmin,
                OutlookToken = user.OutlookToken,
                DepartmentId = user.DepartmentId,
                CanvasLoginId=user.CanvasLoginId,
                CreatedAt = user.CreatedAt,
                JoiningDate = user.JoiningDate,
                DeletionDate = user.DeletionDate,
                Player_Id = user.Player_Id,
                IsPrimary = user.IsPrimary,
                IsDelegate = user.IsDelegate,
                IsZuptuSuperAdminUser = user.IsZuptuSuperAdminUser,
                SignatureImage = user.SignatureImage


            };

            new UserBL().UpdateUsers(newUser);
          
         
        }

        private void Load()
        {

           

            //User user = new UserBL().getUsersById(logedinuser.Id);

            sessionLock.EnterReadLock();
           
            tokenCache.Deserialize(user.OutlookToken==null?null:Encoding.UTF8.GetBytes(user.OutlookToken));
            sessionLock.ExitReadLock();
        }

        private void Persist()
        {
            sessionLock.EnterReadLock();
           
            // Optimistically set HasStateChanged to false.
            // We need to do it early to avoid losing changes made by a concurrent thread.
            tokenCache.HasStateChanged = false;

            //User user = new UserBL().getUsersById(logedinuser.Id);
            user.OutlookToken = Encoding.UTF8.GetString(tokenCache.Serialize());
            Models.User newUser = new Models.User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HomeNumber = user.HomeNumber,
                Notes = user.Notes,
                IsActive = user.IsActive,
                AcessLevel = user.AcessLevel,
                Image = user.Image,
                Password = user.Password,
                isMail = user.isMail,
                isSlack = user.isSlack,
                isSMS = user.isSMS,
                SlackAddress = user.SlackAddress,
                GoogleKeyLength = user.GoogleKeyLength,
                GoggleTaskColor = user.GoggleTaskColor,
                ShowTasks = user.ShowTasks,
                ShowUrgent = user.ShowUrgent,
                CompanyID = user.CompanyID,
                DivisionId = user.DivisionId,
                SearchByDepartment = user.SearchByDepartment,
                SearchByDivision = user.SearchByDivision,
                SearchByPriority = user.SearchByPriority,
                SearchByPrivate = user.SearchByPrivate,
                SearchByUserType = user.SearchByUserType,
                LowPriorityColor = user.LowPriorityColor,
                HighPriorityColor = user.HighPriorityColor,
                MediumPriorityColor = user.MediumPriorityColor,
                Role = user.Role,
                IsMasterAdmin = user.IsMasterAdmin,
                OutlookToken = user.OutlookToken,
                DepartmentId = user.DepartmentId,
                CanvasLoginId=user.CanvasLoginId,
                CreatedAt = user.CreatedAt,
                JoiningDate = user.JoiningDate,
                DeletionDate = user.DeletionDate,
                Player_Id = user.Player_Id,
                IsPrimary = user.IsPrimary,
                IsDelegate= user.IsDelegate,
                IsZuptuSuperAdminUser =user.IsZuptuSuperAdminUser,
                SignatureImage = user.SignatureImage

            };

            new UserBL().UpdateUsers(newUser);

            


           

           // httpContext.Session[cacheId] = tokenCache.Serialize();
            sessionLock.ExitReadLock();
        }

        // Triggered right before ADAL needs to access the cache.
        private void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            // Reload the cache from the persistent store in case it changed since the last access.
            Load();
        }

        // Triggered right after ADAL accessed the cache.
        private void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (tokenCache.HasStateChanged)
            {
                Persist();
            }
        }
    }
}