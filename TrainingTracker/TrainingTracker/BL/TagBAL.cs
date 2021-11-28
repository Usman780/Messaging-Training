
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class TagBL
    {
       

 #region Tags
        public List<Tag> getTagsList()
        {
            return new TagDL().getTagsList();
        }
        public List<Tag> getAllTagsList()
        {
            return new TagDL().getAllTagsList();
        }
        public Tag getTagsById(int _id)
        {
            return new TagDL().getTagById(_id);
        }

        public bool AddTags(Tag _Tags)
        {
            //if (_Tags.Name == null || _Tags.Email == null || _Tags.Password == null || _Tags.Website_Address == null || _Tags.Phone == null)
            //    return false;
            return new TagDL().AddTag(_Tags);
        }

        public bool UpdateTags(Tag _Tags)
        {
            //if (_Tags.Name == null || _Tags.Email == null || _Tags.Password == null || _Tags.Website_Address == null || _Tags.Phone == null)
            //    return false;

            return new TagDL().UpdateTag(_Tags);
        }

        public void DeleteTags(int _id)
        {
            new TagDL().DeleteTag(_id);
        }
        #endregion

}
}