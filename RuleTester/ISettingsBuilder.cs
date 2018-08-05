using CryptoRobert.Infra.Patterns;
using RuleTester.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleTester
{
    public interface ISettingsBuilder
    {
        Dictionary<string, IPattern> GenerateSettings(TesterOutput output);
    }
}
