RED=$(tput setaf 1)
YELLOW=$(tput setaf 3)
GREEN=$(tput setaf 2)
NoColor=$(tput sgr0)

LocalPath="$(dirname "$0")"
cd "$LocalPath" || exit


echo "$YELLOW start building Quartz.Core for linux... $NoColor"
echo "$YELLOW target: $LocalPath/Quartz.Core/bin/libQuartz_Core.so $NoColor"
cd ../Quartz.Core || exit

cmake . || (printf "\n%s error building c++ project: cmake error %s\n" "$RED" "$NoColor" && exit )
make || (printf "\n%s error building c++ project: make error %s\n" "$RED" "$NoColor" && exit )
echo "$GREEN finish building Quartz.Core for linux... $NoColor"

echo "$YELLOW moving files $NoColor"
cp bin/libQuartz_Core.so ../Quartz/libs/libQuartz_Core.so

echo "$GREEN libQuartz_Core.so copied to destination folder $NoColor"