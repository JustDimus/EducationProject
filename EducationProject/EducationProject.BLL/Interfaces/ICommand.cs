﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface ICommand
    {
        public string Name { get; }

        public object Handle(object[] Params);
    }
}
