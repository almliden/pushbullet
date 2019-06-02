using System;
using System.Collections.Generic;
using System.Text;
using PushBullet.Common;

namespace PushBullet.Core.Entities
{
    public class Note : BulletCore, IBullet
    {
        public Note(string title, string body, string pushTarget) : base(title, body, BulletType.Note, pushTarget)
        {
        }

        public BulletType GetBulletType()
        {
            return BulletType.Note;
        }

        public string GetPushTarget()
        {
            return base.PushTarget;
        }
    }
}
