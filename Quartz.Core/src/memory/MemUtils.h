#ifndef QUARTZ_CORE_MEMUTILS_H
#define QUARTZ_CORE_MEMUTILS_H

#include <cstddef>
#include <cinttypes>
#include <vector>
#include <unordered_map>
#include <iostream>
#include <cstring>

namespace MemUtils {
    inline void MemCpy(void* dest, void* src, uint32_t bytes) { memcpy(dest, src, bytes); } 
    inline void MemSet(void* dest, int val, uint32_t bytes) { memset(dest, val, bytes); }
    inline int MemCmp(void* dest, void* src, uint32_t bytes) { return memcmp(dest, src, bytes); } 
};


#endif //QUARTZ_CORE_MEMUTILS_H
