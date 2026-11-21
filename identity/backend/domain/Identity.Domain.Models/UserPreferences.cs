using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Domain.Models
{
    public class UserPreferences
    {
        public string Theme { get; set; } = "system";
        public bool SecurityAlertsEnabled { get; set; } = true;
        public bool MarketingEmailsEnabled { get; set; } = false;
    }
}