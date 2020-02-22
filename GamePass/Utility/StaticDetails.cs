﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Utility
{
    public static class StaticDetails
    {
        // stored procedures commands
        public const string Proc_Platform_Create = "usp_CreatePlatform";
        public const string Proc_Platform_Get = "usp_GetPlatform";
        public const string Proc_Platform_GetAll = "usp_GetPlatforms";
        public const string Proc_Platform_Update = "usp_UpdatePlatform";
        public const string Proc_Platform_Delete = "usp_DeletePlatform";

        // roles
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";
    }
}