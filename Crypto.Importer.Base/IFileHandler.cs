using CryptoRobert.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoRobert.Importer.Base
{
    public interface IFileHandler
    {
        void SaveKlineToFile(List<Kline> klines);
    }
}
