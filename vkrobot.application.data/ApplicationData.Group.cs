﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 31.01.2024 05:59:08
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

#nullable enable annotations
#nullable disable warnings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace vkrobot.application.data
{
    public partial class Group {

        public Group()
        {
            OnCreated();
        }

        public Guid Id { get; set; }

        public string GroupId { get; set; }

        public string? User { get; set; }

        public string? Password { get; set; }

        public DateTime? LastScan { get; set; }

        public string? ErrorText { get; set; }

        public string? GroupName { get; set; }

        public bool? Private { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
