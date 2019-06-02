using PushBullet.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushBullet.Core.Entities
{
    public class BulletCore
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public DateTime Created { get; private set; }

        public BulletType BulletType { get; private set; }
        public string PushTarget { get; private set; }

        public BulletCore(string title, string body, BulletType bulletType, string pushTarget)
        {
            this.Title = title;
            this.Body = body;
            this.BulletType = bulletType;
            this.PushTarget = pushTarget;
        }
    }
}
