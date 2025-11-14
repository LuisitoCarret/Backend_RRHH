using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ==========================
// Modulo.Reclutamiento.Infrastructure/SqlOptions.cs
// ==========================
namespace Modulo.Reclutamiento.Infrastructure;

public sealed class SqlOptions
{
    public string ConnectionString { get; }
    public SqlOptions(string cs) => ConnectionString = cs;
}
