using System;
using System.Collections.Generic;
using System.Text;

namespace PushBullet.Common
{
    public static class Helper
    {
        public static Dictionary<BulletType, string> BulletTypeUrl = new Dictionary<BulletType, string>()
        {
            { BulletType.Note, "pushes"},
            { BulletType.Link, "pushes"},
            { BulletType.File, "pushes"}
        };

    }
}
