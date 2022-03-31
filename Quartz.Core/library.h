#ifndef QUARTZ_CORE_LIBRARY_H
#define QUARTZ_CORE_LIBRARY_H

#include "src/memory/MemoryAllocator.h"

int main();

extern "C" {
    uint8_t* Allocate(uint32_t bytes) {MemoryAllocator::allocateGlobal(bytes);}
    void Free(uint8_t* ptr) {MemoryAllocator::freeGlobal(ptr);}
    void CleanupMemoryAllocator() {MemoryAllocator::cleanupGlobal();}
}

#endif //QUARTZ_CORE_LIBRARY_H
