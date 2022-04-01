cmake_minimum_required(VERSION 3.21)
project(Quartz_Core)

set(CMAKE_CXX_STANDARD 23)

set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
add_library(Quartz_Core SHARED library.cpp src/memory/MemoryAllocator.cpp src/memory/MemoryAllocator.h src/memory/EcsArray.cpp src/memory/EcsArray.h src/memory/MemUtils.cpp src/memory/MemUtils.h src/collections/IntMap.cpp src/collections/IntMap.h src/collections/SortedIntIntList.cpp src/collections/SortedIntIntList.h)
add_executable(Quartz_Core_Test library.cpp src/memory/MemoryAllocator.cpp src/memory/MemoryAllocator.h src/memory/EcsArray.cpp src/memory/EcsArray.h src/memory/MemUtils.cpp src/memory/MemUtils.h src/collections/IntMap.cpp src/collections/IntMap.h src/collections/SortedIntIntList.cpp src/collections/SortedIntIntList.h)
