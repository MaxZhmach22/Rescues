using System;
using System.Collections.Generic;

namespace Rescues
{
    [Serializable]
    public sealed class NotepadEntry 
    {
        public string EntryName;
        public bool IsCrossedOut;
        public List<NotepadBulletpoint> BulletPoints;

        public void AddBulletpoint(int id)
        {
            var index = BulletPoints.FindIndex(x => x.Id.Equals(id));

            if (index < 0)
                BulletPoints.Add(new NotepadBulletpoint { Id = id});
        }

        public void CrossOutBulletpoint(int id)
        {
            var index = BulletPoints.FindIndex(x => x.Id.Equals(id));

            if (index >= 0)
                BulletPoints[index].IsCrossedOut = true;
        }

        public void RemoveBulletpoint(int id)
        {
            var index = BulletPoints.FindIndex(x => x.Id.Equals(id));
            if (index >= 0)
                BulletPoints.RemoveAt(index);
        }

        public void CrossOutEntry()
        {
            IsCrossedOut = true;
            BulletPoints.Clear();
        }
    }
}