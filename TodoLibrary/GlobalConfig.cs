using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.DataAccess;

namespace TodoLibrary;

public static class GlobalConfig
{
    public static IDataConnection Connection { get; private set; }

    public static void InitializeConnections()
    {
        TextConnector text = new TextConnector();
        Connection = text;
    }
}
