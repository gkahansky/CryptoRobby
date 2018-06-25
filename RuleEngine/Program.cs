﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;
using Crypto.RuleEngine;

namespace Crypto.Importer.Bnb
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RuleEngineService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}