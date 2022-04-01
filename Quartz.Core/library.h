#ifndef QUARTZ_CORE_LIBRARY_H
#define QUARTZ_CORE_LIBRARY_H

#include "src/memory/MemoryAllocator.h"
#include "src/memory/EcsArray.h"
#include "src/memory/MemUtils.h"
#include "src/collections/IntMap.h"

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
    
    IntMap* IntMap_Create() { return new IntMap();}
    void IntMap_Destroy(IntMap* ptr) { delete ptr; }
    void IntMap_Set(IntMap* ptr, IntInt element) { ptr->set(element); }
    void IntMap_Remove(IntMap* ptr, uint32_t key) { ptr->remove(key); }
    uint32_t IntMap_TryGetValue(IntMap* ptr, uint32_t key) { return ptr->tryGetValue(key); }
    int IntMap_Count(IntMap* ptr) { return (int) ptr->count(); }
    void IntMap_Clear(IntMap* ptr) { ptr->clear();}
}

#endif //QUARTZ_CORE_LIBRARY_H
