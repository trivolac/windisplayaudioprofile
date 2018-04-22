using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsDisplayAudioProfile.Entities
{
    [Serializable]
    public class ProfileCollection
    {
        private List<Profile> profileList;

        public ProfileCollection()
        {
            profileList = new List<Profile>();
        }

        public List<Profile> ProfileList { get => profileList; set => profileList = value; }

        public void AddProfile(Profile profile)
        {
            ProfileList.Add(profile);
        }
    }
}
