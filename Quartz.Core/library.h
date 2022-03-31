#ifndef QUARTZ_CORE_LIBRARY_H
#define QUARTZ_CORE_LIBRARY_H

#include "src/memory/MemoryAllocator.h"
#include "src/memory/EcsArray.h"
#include "src/memory/MemUtils.h"

int main();

extern "C" {
    uint8_t* Allocate(uint32_t bytes) {MemoryAllocator::allocateGlobal(bytes);}
    void Free(uint8_t* ptr) {MemoryAllocator::freeGlobal(ptr);}
    uint8_t* Resize(uint8_t* ptr, uint32_t newSizeBytes) {return MemoryAllocator::resizeGlobal(ptr, newSizeBytes);}
    void CleanupMemoryAllocator() {MemoryAllocator::cleanupGlobal();}
    
    uint64_t GetCurrentAllocatedBytes() { return MemoryAllocator::getInstance()->currentAllocated;}
    uint64_t GetTotalAllocatedBytes() { return MemoryAllocator::getInstance()->totalAllocated;}
    uint64_t GetAllocatedBytesSinceLastCleanup() { return MemoryAllocator::getInstance()->allocatedSinceLastCleanup;}
    
    void MemCpy(void* dest, void* src, uint32_t bytes) { MemUtils::MemCpy(dest, src, bytes); }
    void MemSet(void* dest, int val, uint32_t bytes) { MemUtils::MemSet(dest, val, bytes); }
    int MemCmp(void* dest, void* src, uint32_t bytes) { return MemUtils::MemCmp(dest, src, bytes); }
}

#endif //QUARTZ_CORE_LIBRARY_H
