using GesitConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GesitConsole.Data
{
    public interface IRha : ICrud<Rha>
    {
        Task<IEnumerable<Rha>> CountExistingFileNameRha(string filename);
        IEnumerable<Rha> GetSubRHAByAssign(string assign);
    }
}
