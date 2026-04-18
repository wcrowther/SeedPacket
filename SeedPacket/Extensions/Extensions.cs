using System;

namespace SeedPacket.Extensions
{
	public static class Extensions
	{
		// WILL TO DO - Move to WildHare Extensions
		public static bool HasEmptyConstructor(this Type type)
		{
			return type.GetConstructor(Type.EmptyTypes) != null;
		}
	}
}
