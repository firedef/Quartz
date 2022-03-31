namespace Quartz.other; 

public static class StringFormat {
	public static string ToStringData(this ulong bytes) {
		const double kb = 1024;
		const double mb = kb * 1024;
		const double gb = mb * 1024;
		const double tb = gb * 1024;

		if (bytes < kb) return $"{bytes}b";
		if (bytes < mb) return $"{(bytes / kb):0.0}kb";
		if (bytes < gb) return $"{(bytes / mb):0.0}mb";
		if (bytes < tb) return $"{(bytes / gb):0.0}gb";
		return $"{(bytes / tb):0.0}tb";
	}
}