using System;
using System.Threading.Tasks;

namespace ginger
{
  public interface DataView
  {
    Task UpdateAsync(Server server);
  }
}
