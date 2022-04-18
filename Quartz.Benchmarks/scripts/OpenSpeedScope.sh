LocalPath="$(dirname "$0")"
echo "opening speedscope"
find "$LocalPath"/../bin/Release/net6.0/BenchmarkDotNet.Artifacts/*.speedscope.json -exec speedscope "{}" \;