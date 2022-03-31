#include "EcsArray.h"

EcsArray::~EcsArray() {
    MemoryAllocator::freeGlobal(allocation.allocationPtr); 
}

void EcsArray::resize(uint32_t newSize) {
    if (allocation.allocatedBytes == 0) {
        uint32_t s = std::max(newSize, (uint32_t) BASIC_ECS_ARRAY_SIZE);
        allocation = Allocation(MemoryAllocator::allocateGlobal(s), s);
        return;
    }
    allocation = Allocation(MemoryAllocator::resizeGlobal(allocation.allocationPtr, newSize), newSize);
}

void EcsArray::expand(uint32_t addBytes) {
    resize(allocation.allocatedBytes + addBytes);
}

void EcsArray::ensureFreeSpace(uint32_t bytes) {
    int freeSpace = getFreeSpace();
    if (freeSpace >= bytes) return;
    expand(std::max(bytes - freeSpace, allocation.allocatedBytes));
}

int EcsArray::getFreeSpace() const {
    return (int) allocation.allocatedBytes - (int) bytesUsed;
}
