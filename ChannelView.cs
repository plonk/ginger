using System;
using System.Threading.Tasks;

namespace ginger
{
  public interface ChannelView
  {
    Task UpdateAsync(Server server, string channelId);
  }
}

