﻿using SPLUS.Shared.Commons.BaseEntities;

namespace SPLUS.Identity.Domain.Entities
{
    public class UserRole : AbstractEntity
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

        //public virtual User User { get; set; }
    }
}
