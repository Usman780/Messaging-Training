using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Json;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using TrainingTracker.BL;
using TrainingTracker.Models;

namespace TrainingTracker.HelpingClasses.GoogleCalendar
{
    public class DataStore : IDataStore
    {
        public System.Threading.Tasks.Task ClearAsync()
        {

            return System.Threading.Tasks.Task.Delay(0);
        }

        public System.Threading.Tasks.Task DeleteAsync<T>(string key)
        {

            string idString = key.Split('>')[0];
            string roleString = key.Split('>')[1];

            idString = Regex.Replace(idString, "[^0-9.]", "");
            roleString = Regex.Replace(roleString,"[^0-9.]", "");
            int id = Convert.ToInt32(idString);
            int role = Convert.ToInt32(roleString);
            DatabaseEntities de = new DatabaseEntities();
            User user = null;
              user = new UserBL().getUsersById(id, de);
                if (user != null)
                {
                    user.GoogleKeyLength = null;
                    de.Entry(user).State = EntityState.Modified;
                    de.SaveChanges();
                
            }
            
          

            return System.Threading.Tasks.Task.Delay(0);
        }

        public Task<T> GetAsync<T>(string key)
        {

            string idString = key.Split('>')[0];
            string roleString = key.Split('>')[1];

            idString = Regex.Replace(idString, "[^0-9.]", "");
            roleString = Regex.Replace(roleString, "[^0-9.]", "");
            int id = Convert.ToInt32(idString);
            int role = Convert.ToInt32(roleString);
            DatabaseEntities de = new DatabaseEntities();
            User user = null;
              user = new UserBL().getUsersById(id, de);
               
           
            


            
            var value = user.GoogleKeyLength == null ? default(T) : NewtonsoftJsonSerializer.Instance.Deserialize<T>(user.GoogleKeyLength);
            return System.Threading.Tasks.Task.FromResult<T>(value);
        }

        public System.Threading.Tasks.Task StoreAsync<T>(string key, T value)
        {
            string idString = key.Split('>')[0];
            string roleString = key.Split('>')[1];

            idString = Regex.Replace(idString, "[^0-9.]", "");
            roleString = Regex.Replace(roleString, "[^0-9.]", "");
            int id = Convert.ToInt32(idString);
            int role = Convert.ToInt32(roleString);
            DatabaseEntities de = new DatabaseEntities();
            User user = null;
         
            
            User admin = new UserBL().getUsersById(id, de);
            if (admin != null)
            {
              
                    string test = JsonConvert.SerializeObject(value);
                    if (!test.ToUpper().Contains("ZUPTU.COM"))
                    {
                        admin.GoogleKeyLength = test;
                        de.Entry(admin).State = EntityState.Modified;
                        de.SaveChanges();
                    }

                
            }
            
          
          



          
           
           

            return System.Threading.Tasks.Task.Delay(0);
        }
    }
}