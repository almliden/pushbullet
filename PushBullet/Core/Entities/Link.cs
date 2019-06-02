using System;
using System.Collections.Generic;
using System.Text;
using PushBullet.Common;

namespace PushBullet.Core.Entities
{
    public class Link: BulletCore, IBullet
    {
        public string Url { get; private set; }
        public Link(string title, string body, string url, string pushTarget) : base(title, body, BulletType.Link, pushTarget)
        {
            this.Url = url;
        }

        public BulletType GetBulletType()
        {
            return BulletType.Link;
        }

        public string GetPushTarget()
        {
            return base.PushTarget;
        }
    }
}
