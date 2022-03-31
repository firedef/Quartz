#ifndef QUARTZ_CORE_ECSARRAY_H
#define QUARTZ_CORE_ECSARRAY_H

#include "MemoryAllocator.h"
#define BASIC_ECS_ARRAY_SIZE 1<<24

class EcsArray {
public:
    Allocation allocation;
    uint32_t bytesUsed;
    
    inline int getFreeSpace() const;
    
    void ensureFreeSpace(uint32_t bytes);
    void expand(uint32_t addBytes);
    void resize(uint32_t newSize);

    virtual ~EcsArray();
};


#endif //QUARTZ_CORE_ECSARRAY_H
