using PushBullet.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushBullet.Core.Entities
{
    public interface IBullet
    {
        BulletType GetBulletType();
        string GetPushTarget();
    }
}
