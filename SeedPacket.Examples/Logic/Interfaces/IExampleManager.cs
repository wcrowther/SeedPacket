using SeedPacket.Examples.Logic.Models;
using System.Collections.Generic;

namespace SeedPacket.Examples.Logic.Interfaces;

public interface IExampleManager
{
	List<Company> GetExampleList(int? rows, int? seed);
}